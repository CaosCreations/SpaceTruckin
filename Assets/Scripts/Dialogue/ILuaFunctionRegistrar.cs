/// <summary>
/// Interface used for exposing C# methods to the Lua interpreter.
/// This allows them to be used by the Dialogue System at runtime.
/// </summary>
public interface ILuaFunctionRegistrar
{
    void RegisterLuaFunctions();
    void UnregisterLuaFunctions();
}
