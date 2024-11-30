using System;
using System.Collections.Generic;

namespace Language.Lua
{
    public partial class Assignment : Statement
    {

        // [PixelCrushers] Supports monitoring of variable changes:
        // Local variable assignments take the form: x = 3
        public static HashSet<string> MonitoredLocalVariables = new HashSet<string>();
        public static System.Action<string, object> LocalVariableChanged = null;
        // Variable assignments are in the Variable[] table: Variable["x"] = 3
        public static HashSet<string> MonitoredVariables = new HashSet<string>();
        public static System.Action<string, object> VariableChanged = null;
        private static LuaValue VariableTableToMonitor = null;

        public static void InitializeVariableMonitoring()
        {
            MonitoredLocalVariables = new HashSet<string>();
            LocalVariableChanged = null;
            MonitoredVariables = new HashSet<string>();
            VariableChanged = null;
            VariableTableToMonitor = null;
        }

        public static void InvokeVariableChanged(string variable, object value)
        {
            VariableChanged?.Invoke(variable, value);
        }

        private static bool AreValuesEqual(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;
            var type1 = obj1.GetType();
            var type2 = obj2.GetType();
            if (type1 != type2) return false;
            if (type1 == typeof(bool))
            {
                return (bool)obj1 == (bool)obj2;
            }
            else if (type1 == typeof(double))
            {
                return UnityEngine.Mathf.Approximately(0, (float)((double)obj1 - (double)obj2));
            }
            else if (type1 == typeof(string))
            {
                return string.Equals(obj1.ToString(), obj2.ToString());
            }
            else
            {
                return obj1 == obj2;
            }
        }

        public override LuaValue Execute(LuaTable environment, out bool isBreak)
        {
            //[PixelCrushers] LuaValue[] values = this.ExprList.ConvertAll(expr => expr.Evaluate(enviroment)).ToArray();
            LuaValue[] values = LuaInterpreterExtensions.EvaluateAll(this.ExprList, environment).ToArray();

            LuaValue[] neatValues = LuaMultiValue.UnWrapLuaValues(values);

            for (int i = 0; i < Math.Min(this.VarList.Count, neatValues.Length); i++)
            {
                Var var = this.VarList[i];

                if (var.Accesses.Count == 0)
                {
                    VarName varName = var.Base as VarName;

                    if (varName != null)
                    {
                        //[PixelCrushers]
                        SetKeyValue(environment, new LuaString(varName.Name), values[i]);
                        if (varName.Name == "Variable")
                        {
                            VariableTableToMonitor = values[0];
                        }
                        var isMonitored = MonitoredLocalVariables.Contains(varName.Name) && values.Length >= 1;
                        if (isMonitored)
                        {
                            var newValue = values[0].Value;
                            try
                            {
                                LocalVariableChanged?.Invoke(varName.Name, newValue);
                            }
                            catch (Exception e)
                            {
                                UnityEngine.Debug.LogException(e);
                            }
                        }
                        continue;
                    }
                }
                else
                {
                    LuaValue baseValue = var.Base.Evaluate(environment);

                    for (int j = 0; j < var.Accesses.Count - 1; j++)
                    {
                        Access access = var.Accesses[j];

                        baseValue = access.Evaluate(baseValue, environment);
                    }

                    Access lastAccess = var.Accesses[var.Accesses.Count - 1];

                    NameAccess nameAccess = lastAccess as NameAccess;
                    if (nameAccess != null)
                    {
                        if (baseValue == null || (baseValue is LuaNil))
                        {
                            throw new System.NullReferenceException("Cannot assign to a null value. Are you trying to assign to a nonexistent table element?.");
                        }
                        SetKeyValue(baseValue, new LuaString(nameAccess.Name), values[i]);
                        continue;
                    }

                    KeyAccess keyAccess = lastAccess as KeyAccess;
                    if (lastAccess != null)
                    {
                        SetKeyValue(baseValue, keyAccess.Key.Evaluate(environment), values[i]);
                    }
                }
            }

            isBreak = false;
            return null;
        }

        private static void SetKeyValue(LuaValue baseValue, LuaValue key, LuaValue value)
        {
            LuaValue newIndex = LuaNil.Nil;
            LuaTable table = baseValue as LuaTable;
            if (table != null)
            {
                //[PixelCrushers]
                var isMonitored = baseValue == VariableTableToMonitor &&
                    key != null &&
                    MonitoredVariables.Contains(key.ToString());
                object originalValue = null;
                if (isMonitored)
                {
                    var originalLuaValue = table.GetValue(key);
                    if (originalLuaValue != null) originalValue = originalLuaValue.Value;
                }
                try
                {
                    if (table.ContainsKey(key))
                    {
                        table.SetKeyValue(key, value);
                        return;
                    }
                    else
                    {
                        if (table.MetaTable != null)
                        {
                            newIndex = table.MetaTable.GetValue("__newindex");
                        }

                        if (newIndex == LuaNil.Nil)
                        {
                            table.SetKeyValue(key, value);
                            return;
                        }
                    }
                }
                finally
                {
                    //[PixelCrushers]
                    if (baseValue == VariableTableToMonitor && key != null && value != null)
                    {
                        if (isMonitored && !AreValuesEqual(value.Value, originalValue))
                        {
                            try
                            {
                                VariableChanged?.Invoke(key.ToString(), value.Value);
                            }
                            catch (Exception e)
                            {
                                UnityEngine.Debug.LogException(e);
                            }
                        }
                    }
                }
            }
            else
            {
                LuaUserdata userdata = baseValue as LuaUserdata;
                if (userdata != null)
                {
                    if (userdata.MetaTable != null)
                    {
                        newIndex = userdata.MetaTable.GetValue("__newindex");
                    }

                    if (newIndex == LuaNil.Nil)
                    {
                        throw new Exception("Assign field of userdata without __newindex defined.");
                    }
                }
            }

            LuaFunction func = newIndex as LuaFunction;
            if (func != null)
            {
                func.Invoke(new LuaValue[] { baseValue, key, value });
            }
            else
            {
                SetKeyValue(newIndex, key, value);
            }
        }
    }
}
