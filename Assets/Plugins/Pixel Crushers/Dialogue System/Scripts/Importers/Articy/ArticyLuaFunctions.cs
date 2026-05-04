#if USE_ARTICY
// Copyright (c) Pixel Crushers. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using Language.Lua;
using System.Text;

namespace PixelCrushers.DialogueSystem.Articy
{

    /// <summary>
    /// Implements articy:expresso functions. You'll typically add this to the 
    /// Dialogue Manager GameObject.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class ArticyLuaFunctions : Saver
    {
        private static bool s_registered = false;
        private static ArticyLuaFunctions s_instance = null;

        [Tooltip("Tick to enable tracking of seen counters for dialogue entries.")]
        [SerializeField] private bool useSeenCounters = false;

        private Dictionary<string, int> seenCounters = new Dictionary<string, int>(); // < [convID:entryID], count >

        protected const string ArticyIdFieldTitle = "Articy Id";
        protected const string ArticyTechnicalNameFieldTitle = "Technical Name";

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitStaticVariables()
        {
            s_registered = false;
        }
#endif

        public override void Reset()
        {
            base.Reset();
            key = "articy";
        }

        public override void OnEnable()
        {
            base.OnEnable();
            RegisterLuaFunctions();
        }

        private void RegisterLuaFunctions()
        { 
            if (s_registered) return;
            s_registered = true;
            s_instance = this;
            Lua.RegisterFunction(nameof(getObj), this, SymbolExtensions.GetMethodInfo(() => getObj(string.Empty)));
            Lua.RegisterFunction("getObject", this, SymbolExtensions.GetMethodInfo(() => getObj(string.Empty)));
            Lua.RegisterFunction(nameof(getProp), this, SymbolExtensions.GetMethodInfo(() => getProp(string.Empty, string.Empty)));
            Lua.RegisterFunction(nameof(setProp), this, SymbolExtensions.GetMethodInfo(() => setProp(string.Empty, string.Empty, default(object))));
            Lua.RegisterFunction(nameof(incrementProp), this, SymbolExtensions.GetMethodInfo(() => incrementProp(string.Empty, string.Empty, default(object))));
            Lua.RegisterFunction(nameof(isPropInRange), this, SymbolExtensions.GetMethodInfo(() => isPropInRange(string.Empty, string.Empty, 0, 0)));
            Lua.RegisterFunction(nameof(getSeenCounter), this, SymbolExtensions.GetMethodInfo(() => getSeenCounter(string.Empty)));
            Lua.RegisterFunction(nameof(setSeenCounter), this, SymbolExtensions.GetMethodInfo(() => setSeenCounter(string.Empty, 0)));
            Lua.RegisterFunction(nameof(resetAllSeenCounters), this, SymbolExtensions.GetMethodInfo(() => resetAllSeenCounters()));
            Lua.RegisterFunction(nameof(random), this, SymbolExtensions.GetMethodInfo(() => random(0, 0)));
            Lua.RegisterFunction(nameof(isInRange), this, SymbolExtensions.GetMethodInfo(() => isInRange(0, 0, 0)));
            Lua.Environment.Register(nameof(fallback), fallback);
            DialogueLua.includeSimStatus = true;
        }

        protected virtual void OnConversationLine(Subtitle subtitle)
        {
            // Set the Lua variables 'speaker' and 'self':
            var speaker = "\"Actor[\\\"" + DialogueLua.StringToTableIndex(subtitle.speakerInfo.nameInDatabase) + "\\\"]\"";
            var self = "\"Dialog[" + subtitle.dialogueEntry.id + "]\""; // Note that Dialog[#] only has SimStatus to conserve memory. getProp() uses special case to get entry fields.
            Lua.Run("speaker = " + speaker + "; self = " + self, DialogueDebug.logInfo);

            // Update seen counter:
            if (useSeenCounters)
            {
                var key = $"{subtitle.dialogueEntry.conversationID}:{subtitle.dialogueEntry.id}";
                if (!seenCounters.TryGetValue(key, out var currentValue)) currentValue = 0;
                seenCounters[key] = currentValue + 1;
            }
        }

        // Returns an object identifier string such as Actor["Player"] that getProp/setProp/getSeenCounter can use.
        public static string getObj(string objectName)
        {
            var db = DialogueManager.MasterDatabase;

            // Does objectName match an actor's Name, Technical Name, or Articy Id?
            var actor = db.actors.Find(x => string.Equals(objectName, x.Name) || string.Equals(objectName, x.LookupValue(ArticyTechnicalNameFieldTitle)) || string.Equals(objectName, x.LookupValue(ArticyIdFieldTitle)));
            if (actor != null) return "Actor[\"" + DialogueLua.StringToTableIndex(actor.Name) + "\"]";

            // Does objectName match an item/quest's Name, Technical Name, or Articy Id?
            var item = db.items.Find(x => string.Equals(objectName, x.Name) || string.Equals(objectName, x.LookupValue(ArticyTechnicalNameFieldTitle)) || string.Equals(objectName, x.LookupValue(ArticyIdFieldTitle)));
            if (item != null) return "Item[\"" + DialogueLua.StringToTableIndex(item.Name) + "\"]";

            // Does objectName match a location's Name, Technical Name, or Articy Id?
            var location = db.locations.Find(x => string.Equals(objectName, x.Name) || string.Equals(objectName, x.LookupValue(ArticyTechnicalNameFieldTitle)) || string.Equals(objectName, x.LookupValue(ArticyIdFieldTitle)));
            if (location != null) return "Location[\"" + DialogueLua.StringToTableIndex(location.Name) + "\"]";

            // Does objectName match an conversation's Title, Technical Name, or Articy Id?
            var conversation = db.conversations.Find(x => string.Equals(objectName, x.Title) || string.Equals(objectName, x.LookupValue(ArticyTechnicalNameFieldTitle)) || string.Equals(objectName, x.LookupValue(ArticyIdFieldTitle)));
            if (conversation != null) return "Conversation[\"" + conversation.id + "\"]";

            // Does objectName match a dialogue entry's id, Title, Technical Name, or Articy Id?
            if (objectName.StartsWith("Dialog[")) return objectName;
            return null;
        }

        public static object getProp(string objectIdentifier, string propertyName)
        {
            if (string.IsNullOrEmpty(objectIdentifier) || string.IsNullOrEmpty(propertyName)) return string.Empty;

            // If identifier is for a dialogue entry, handle it specially:
            if (objectIdentifier.StartsWith("Dialog[") && DialogueManager.isConversationActive)
            {
                // Handle Dialog[#] specially:
                var entryID = Tools.StringToInt(objectIdentifier.Substring(7, objectIdentifier.Length - 8));
                var conversationID = DialogueManager.currentConversationState.subtitle.dialogueEntry.conversationID;
                if (string.Equals("SimStatus", propertyName)) return DialogueLua.GetSimStatus(conversationID, entryID);
                var entry = DialogueManager.masterDatabase.GetDialogueEntry(conversationID, entryID);
                if (entry == null) return string.Empty;
                var field = Field.Lookup(entry.fields, propertyName);
                if (field == null) return string.Empty;
                if (field.type == FieldType.Number) return Tools.StringToFloat(field.value);
                else if (field.type == FieldType.Boolean) return Tools.StringToBool(field.value);
                else return field.value;
            }

            // Otherwise get Lua element's property value:
            var result = Lua.Run("return " + objectIdentifier + "." + DialogueLua.StringToTableIndex(GetShortPropertyName(propertyName)), DialogueDebug.logInfo);
            if (result.isBool)
            {
                return result.asBool;
            }
            else if (result.isNumber)
            {
                return result.asInt;
            }
            else
            {
                return result.asString;
            }
        }

        public static void setProp(string objectIdentifier, string propertyName, object value)
        {
            string rightSide = GetRightSide(value); var fullIdentifier = objectIdentifier + "." + GetShortPropertyName(propertyName);
            Lua.Run($"{fullIdentifier} = {rightSide}", DialogueDebug.logInfo);
        }

        public static void incrementProp(string objectIdentifier, string propertyName, object value)
        {
            string rightSide = GetRightSide(value);
            var fullIdentifier = objectIdentifier + "." + GetShortPropertyName(propertyName);
            Lua.Run($"{fullIdentifier} = {fullIdentifier} + {rightSide}", DialogueDebug.logInfo);
        }

        private static string GetRightSide(object value)
        {
            if (value == null)
            {
                return "nil";
            }
            else if (value.GetType() == typeof(string))
            {
                return "\"" + value.ToString() + "\"";
            }
            else if (value.GetType() == typeof(bool))
            {
                return value.ToString().ToLower();
            }
            else
            {
                return value.ToString();
            }
        }

        public static bool isPropInRange(string objectIdentifier, string propertyName, double lowerBound, double upperBound)
        {
            var value = getProp(objectIdentifier, propertyName);
            if (value != null &&
                value.GetType() == typeof(int))
            {
                var intValue = (int)value;
                return (int)lowerBound <= intValue && intValue <= (int)upperBound;
            }
            else
            {
                return false;
            }
        }

        public static LuaValue fallback(LuaValue[] values)
        {
            // Although articy's fallback() returns true only if no sibling dialogue entries'
            // conditions are true, we always return true here so it will be used if entries'
            // conditions are false.
            return LuaBoolean.True;
        }

        public static double random(double lowerBound, double upperBound)
        {
            return UnityEngine.Random.Range((int)lowerBound, (int)upperBound + 1);
        }

        public static bool isInRange(double value, double lowerBound, double upperBound)
        {
            return (int)lowerBound <= (int)value && (int)value <= (int)upperBound;
        }

        private static string GetShortPropertyName(string propertyName)
        {
            // In articy, custom feature properties include the feature name.
            // In DS, they don't. Remove the feature name if present.
            if (propertyName.Contains("."))
            {
                var lastIndex = propertyName.LastIndexOf('.');
                return propertyName.Substring(lastIndex + 1);
            }
            else
            {
                return propertyName;
            }
        }

        public static double getSeenCounter(string objectIdentifier)
        {
            if (s_instance == null) return 0;
            if (string.IsNullOrEmpty(objectIdentifier)) return 0;
            var key = string.Empty;
            if (objectIdentifier.StartsWith("Dialog["))
            {
                // Seen counter for entry in current conversation:
                var conversationID = DialogueManager.currentConversationState.subtitle.dialogueEntry.conversationID;
                var entryID = Tools.StringToInt(objectIdentifier.Substring(7, objectIdentifier.Length - 8));
                key = $"{conversationID}:{entryID}";
            }
            else if (objectIdentifier.StartsWith("Conversation["))
            {
                var firstOpenBracket = objectIdentifier.IndexOf('[');
                var firstCloseBracket = objectIdentifier.IndexOf("]");
                var conversationID = objectIdentifier.Substring(firstOpenBracket + 1, firstCloseBracket - firstOpenBracket - 1);
                var secondOpenBracket = objectIdentifier.LastIndexOf("[");
                var secondCloseBracket = objectIdentifier.LastIndexOf("]");
                var entryID = objectIdentifier.Substring(secondOpenBracket + 1, secondCloseBracket - secondOpenBracket - 1);
                key = $"{conversationID}:{entryID}";
            }
            else
            {
                var entry = FindDialogueEntry(objectIdentifier);
                if (entry != null)
                {
                    key = $"{entry.conversationID}:{entry.id}";
                }
            }
            if (string.IsNullOrEmpty(key)) return 0;
            return s_instance.seenCounters.TryGetValue(key, out var value) ? value : 0;
        }

        private static DialogueEntry FindDialogueEntry(string objectIdentifier)
        {
            foreach (var conversation in DialogueManager.masterDatabase.conversations)
            {
                foreach (var entry in conversation.dialogueEntries)
                {
                    if (objectIdentifier == Field.LookupValue(entry.fields, ArticyTechnicalNameFieldTitle) ||
                        objectIdentifier == Field.LookupValue(entry.fields, ArticyIdFieldTitle))
                    {
                        return entry;
                    }
                }
            }
            return null;
        }

        public static void setSeenCounter(string objectIdentifier, double value)
        {
            if (s_instance == null) return;
        }

        public static void resetAllSeenCounters()
        {
            if (s_instance != null) s_instance.seenCounters.Clear();
        }

        // Save seenCounters.
        public override string RecordData()
        {
            if (!useSeenCounters) return string.Empty;
            var sb = new StringBuilder("##:##=##;".Length * seenCounters.Count);
            var first = true;
            foreach (var kvp in seenCounters)
            {
                if (!first) sb.Append(';');
                first = false;
                sb.Append(kvp.Key);
                sb.Append('=');
                sb.Append(kvp.Value);
            }
            return sb.ToString();
        }

        // Restore seenCounters.
        public override void ApplyData(string s)
        {
            if (!useSeenCounters) return;
            if (string.IsNullOrEmpty(s)) return;
            seenCounters.Clear();
            var list = s.Split(';');
            foreach (var element in list)
            {
                var fields = element.Split('=');
                if (fields == null || fields.Length != 2) continue;
                seenCounters[fields[0]] = SafeConvert.ToInt(fields[1]);
            }
        }
    }
}
#endif
