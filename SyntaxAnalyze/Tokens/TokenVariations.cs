namespace SyntaxAnalyze;

public class TokenValue : Token
{
    private Value _value;

    public TokenValue() : base(TokenType.Value)
    {
    }
    
    public TokenValue(Value value) : base(TokenType.Value)
    {
        Value = value;
    }

    public Value Value
    {
        get => _value;
        set => _value = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class TokenOperation : Token
{
    private string _operation;

    public TokenOperation() : base(TokenType.Operation)
    {
    }
    public TokenOperation(string operation) : base(TokenType.Operation)
    {
        _operation = operation;
    }

    public string Operation
    {
        get => _operation;
        set => _operation = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class TokenGetLocalVariable : Token
{
    private string _name;

    public TokenGetLocalVariable(): base(TokenType.GetLocalVariable)
    {
    }

    public TokenGetLocalVariable(string name) : base(TokenType.GetLocalVariable)
    {
        this._name = name;
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }
}

public class TokenSetLocalVariable : Token
{
    private string _name;
    private int _numberOfTokensAfter;

    public TokenSetLocalVariable(): base(TokenType.SetLocalVariable)
    {
    }

    public TokenSetLocalVariable(string name) : base(TokenType.SetLocalVariable)
    {
        this._name = name;
        _numberOfTokensAfter = -1;
    }
    public TokenSetLocalVariable(string name, int numberOfTokensAfter) : base(TokenType.SetLocalVariable)
    {
        this._name = name;
        _numberOfTokensAfter = numberOfTokensAfter;
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    public int NumberOfTokensAfter
    {
        get => _numberOfTokensAfter;
        set => _numberOfTokensAfter = value;
    }
}

public class TokenBlock : Token
{
    private int _TokenToGo;
    private int _numOfTokens;

    public TokenBlock() : base(TokenType.Block)
    {
    }
    
    public TokenBlock(int tokenToGo) : base(TokenType.Block)
    {
        _TokenToGo = tokenToGo;
    }

    public TokenBlock(int tokenToGo, int numOfTokens) : base(TokenType.Block)
    {
        _TokenToGo = tokenToGo;
        this._numOfTokens = numOfTokens;
    }

    public int PositionToGo
    {
        get => _TokenToGo;
        set => _TokenToGo = value;
    }

    public int NumOfTokens
    {
        get => _numOfTokens;
        set => _numOfTokens = value;
    }
}

public class TokenFunction : Token
{
    private FuncDef _funcDef;

    public TokenFunction() : base (TokenType.FunctionDef)
    {
    }

    public TokenFunction(FuncDef funcDef) : base (TokenType.FunctionDef)
    {
        _funcDef = funcDef;
    }

    public FuncDef FuncDef
    {
        get => _funcDef;
        set => _funcDef = value ?? throw new ArgumentNullException(nameof(value));
    }
} 

public class TokenFunctionCall : Token
{
    private FuncDef _funcDef;

    public TokenFunctionCall() : base (TokenType.FunctionCall)
    {
    }

    public TokenFunctionCall(FuncDef funcDef) : base (TokenType.FunctionCall)
    {
        _funcDef = funcDef;
    }

    public FuncDef FuncDef
    {
        get => _funcDef;
        set => _funcDef = value ?? throw new ArgumentNullException(nameof(value));
    }
} 