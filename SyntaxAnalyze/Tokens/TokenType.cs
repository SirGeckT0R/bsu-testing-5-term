namespace SyntaxAnalyze;

public enum TokenType
{
    Block,
    Condition,
    Value,
    Operation,
    FunctionDef,
    FunctionCall,
    LocalVariable,
    GlobalVariable,
    GetLocalVariable,
    GetGlobalVariable,
    SetLocalVariable,
    SetGlobalVariable,
}