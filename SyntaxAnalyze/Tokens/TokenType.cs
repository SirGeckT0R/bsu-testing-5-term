namespace SyntaxAnalyze;

public enum TokenType
{
    Block,
    GotoIf,
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