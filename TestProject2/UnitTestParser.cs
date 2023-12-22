using SyntaxAnalyze;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestParser;

public class Tests
{
    [TestCase("x=14+4;", true)]
    [TestCase(
        """
        x=14+4;
        x=x;
        """
      , true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x + 1;
        z = 0;
        z = 10;
        """
      , true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x + 1;
        z = 0;
        z = 10;
        """
        , true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x*(1+2/3) + 1;
        """
        , true)]
    [TestCase(
        """
        var x=0;
        var x, y;
        var x = 1, z;
        var x, z=2;
        """
        , true)]
    [TestCase(
        """
        var x=3;
        var x, y;

        function f(x,y)
        {
          x = y;
        }

        x = f(1,2);
        """
        , true)]
    [TestCase(
        """
        function f(x,y)
        {
          x = y;
        }
        """
        , true)]
    [TestCase(
        """
        function f(x,y)
        {
          x = y + 1;
        }

        x = 1;
        """
        , true)]
    [TestCase(
        """
        var x=3;
        var x, y;

        function f(x,y)
        {
          x = y;
        }
        
        function f2(x)
        {
          x = y+1;
        }
        
        x = f(1,2) + f2 ( 1 ) ;
        """
        , true)]
    [TestCase(
        """
        function f2(x)
        {
          return x;
          return (x);
          return(x);
          return;
          return ;
          return 1;
          return -1;
        }
        """
        , true)]
    [TestCase(
        """
          return-1;
        """
        , true)]
    [TestCase(
        """
        varx=1;
        """
        , true
        )]
    [TestCase(
        """
        if ( 1==1)
        {
        x=1;
        }
        """, true)]
    [TestCase(
        """
        if 1==1
        {
        x=1;
        }
        """, true)]
    [TestCase(
        """
        if 1==1
        {
         x=1;
        }
        else if (2==2)
        {
        x=2;
        }
        """, true)]
    [TestCase(
        """
        if 1==1
        {
          x=1;
        }
        
        else if 2==2
        {
           x=2;
        }
        
        else if 1==1
        {
         x=3; 
        }
        """, true)]
    [TestCase(
        """
        if 1==1
        {
        x=1;
        } else if 2==2{
        x=2;
        } else {
        x=3; 
        }
        """, true)]
    [TestCase(
        """
        while (1==1){
        x=2;
        }
        """, true)]
    [TestCase(
        """
        while (1==1){
        if(1==1){
        x=1;
        }
        x=2;
        }
        """, true)]
    [TestCase(
        """
        while (1==1){
         if(1==1){
        x=1;
        } else{
        x=2;
        }
        }
        """, true)]

    public void ValidatesParse(string expression, bool expected)

    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool actual = parser.Parse();

        Assert.That(actual, Is.EqualTo(expected));
    }
    
[TestCase("x=14+4;", 1,new int[]{3},true)]
    [TestCase(
        """
        x=14+4;
        x=x;
        """
      , 2,new int[]{3,1},true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x + 1;
        z = 0;
        z = 10;
        """
      , 4,new int[]{3,3,1,1},true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x*(1+2/3) + 1;
        """
        , 2,new int[]{3,11},true)]
[TestCase(
    """
    if(15 + 5){}
    """
    , 0,new int[]{},true)]
[TestCase(
    """
    while (1==1){
    if(1==2){}
    }
    """
    , 0,new int[]{},true)]
[TestCase(
    """
    while (1==1){
     if(1==1){
    x=1;
    } else{
    x=2;
    }
    }
    """
    , 2,new int[]{1,1},true)]
[TestCase(
    "x= 'abc';"
    , 1,new int[]{1},true)]
[TestCase(
    """
    x= 'abc';
    while (x==1){
     if(1==1){
    y=x;
    } else{
    x='a';
    }
    }
    """
    , 3,new int[]{1,1,1},true)]
[TestCase(
    """
    var x= 14 + 4;
    y= x*(1+2/3) + 1;
    """
    , 2,new int[]{3,11},true)]
[TestCase(
    """
    var x= 14 + 4;
    var y= x;
    var z =6;
    z = x;
    """
    , 4,new int[]{3,1,1,1},true)]

    public void ValidatesTokensSetLocalVariable(string expression, int numOfSets, int[] numInNewValue, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        List<SyntaxAnalyze.Token> setsList = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.SetLocalVariable);
        bool isValidSets = numOfSets == setsList.Count;
        bool isValidValues = true;
        TokenSetLocalVariable token;
        for (int i = 0; i < numInNewValue.Length;i++)
        {
            token = (TokenSetLocalVariable)setsList.ElementAt(i);
            isValidValues = numInNewValue[i] == token.NumberOfTokensAfter;
            if (!isValidValues)
            {
                break;
            }
        }
        Assert.That(isValidAnalyze && isValidSets && isValidValues, Is.EqualTo(expected));
    }
    
    [TestCase("""
              function f1(x,y) { 
              x = 15; 
              y = 17;
              }
              """, 1,new int[]{4},true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              } 
              """, 1,new int[]{2},true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              } 
              else if (1==1){
              y=x;
              }
              """, 2,new int[]{2,2},true)]
    [TestCase("""
              function f1(x,y) {
              x = 15;
              if (2==1) {
              x=y;
              } 
              y = 17;
              }
              """, 2,new int[]{13,2},true)]
    [TestCase("""
              function f1(x) { x = 15; } 
              function f2(x) {
              x=x+1;
              x=x;
              }
              """, 2,new int[]{2,6},true)]
    [TestCase("""
              function f1(x,y) { x = y-15; }
              function f2(x,y) {
              x=y+1;
              x=x-y;
              }
              """, 2,new int[]{4,8},true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              } 
              else if (1==1){
              y=x;
              }
              else{
              y=y;
              }
              """, 3,new int[]{2,2,2},true)]
    [TestCase("""
              var x=1;
              var y=5;
              function f1(x,y) { x = y-15; }
              function f2(x,y) {
              x=y+1;
              x=x-y;
              if (2==1) {
              x=y;
              }
              else if (1==1){
              y=x;
              }
              }
             
              """, 4,new int[]{4,26,2,2},true)]
    [TestCase("""
              var x=1;
              var y=5;
              while(1==1){
              x=y;
              }

              """, 1,new int[]{2},true)]
    [TestCase("""
              var x=1;
              var y=5;
              while(1==1){
              x=y;
              while(2==2){
              x=1;
              }
              }

              """, 2,new int[]{11,2},true)]
    public void ValidatesTokensBlock(string expression, int numOfBlocks, int[] tokensInEachBlock, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        List<SyntaxAnalyze.Token> setsList = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.Block);
        bool isValidSets = numOfBlocks == setsList.Count;
        bool isValidValues = true;
        TokenBlock token;
        for (int i = 0; i < tokensInEachBlock.Length;i++)
        {
            token = (TokenBlock)setsList.ElementAt(i);
            isValidValues = tokensInEachBlock[i] == token.NumOfTokens + 1;
            if (!isValidValues)
            {
                break;
            }
        }
        Assert.That(isValidAnalyze && isValidSets && isValidValues, Is.EqualTo(expected));
    }
    
    [TestCase("""
              function f1(x,y) { x = y-15; }
              function f2(x,y) {
              x=y+1;
              x=x-y;
              }
              """, 2,true)]
    [TestCase("""
              function f1(x,y) { x = y-15; }
              function f2(x,y) {
              x=y;
              }
              function f3(y){
              y=1;
              }
              function f4(x){
              x=1;
              }
              """, 4,true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              } 
              else if (1==1){
              y=x;
              }
              """, 0,true)]
    public void ValidatesTokensFunctionDef(string expression, int numOfFunctions, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        int count = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.FunctionDef).Count;
        Assert.That(isValidAnalyze && count == numOfFunctions, Is.EqualTo(expected));
    }
    
    [TestCase("""
              var x=1;
              var y=5;
              function f1(x,y) { x = y-15; }
              function f2(x,y) {
              x=y+1;
              x=x-y;
              }
              x = f1(x,y);
              """, 1,true)]
    [TestCase("""
              var x=1;
              var y=5;
              function f1(x,y) { x = y-15; }
              function f2(x,y) {
              x=y+1;
              x=x-y;
              }
              x = f1(x,y);
              y = f2(x,y);
              """, 2,true)]
    [TestCase("""
              var x=1;
              var y=5;
              function f1(x,y) { x = y-15; 
              return 1; }
              function f2(x,y) {
              x=y;
              return 1;
              }
              function f3(y){
              y=1;
              return 1;
              }
              function f4(x){
              x=1;
              return 1;
              }
              x = f1(x,y) + f2(x,y) + f3(y) - f4(x);
              """, 4,true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              }
              else if (1==1){
              y=x;
              }
              """, 0,true)]
    public void ValidatesTokensFunctionCall(string expression, int numOfCalls, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        int count = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.FunctionCall).Count;
        Assert.That(isValidAnalyze && count == numOfCalls, Is.EqualTo(expected));
    }
    
   
    
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              }
              else if (1==1){
              y=x;
              }
              """, 2,new int[]{5,5},true)]
    [TestCase("""
              var x=1;
              var y=5;
              
              """, 0,new int[]{},true)]
    [TestCase("""
              var x=1;
              var y=5;
              function f1(x,y) { x = y-15;
              return 1; }
              function f2(x,y) {
              x=y;
              return 1;
              }
              function f3(y){
              y=1;
              return 1;
              }
              function f4(x){
              x=1;
              return 1;
              }
              x = f1(x,y) + f2(x,y) + f3(y) - f4(x);
              """, 0,new int[]{},true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              }
              else if (1==1){
              y=x;
              }
              else {
              x=x;
              }
              """, 2,new int[]{5,5},true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              }
              """, 1,new int[]{5},true)]
    [TestCase("""
              var x=1;
              var y=5;
              if (2==1) {
              x=y;
              }
              """, 1,new int[]{5},true)]
    [TestCase("""
              var x=1;
              var y=5;
              while(1==1){
              x=y;
              }

              """, 1,new int[]{5},true)]
    [TestCase("""
              var x=1;
              var y=5;
              while(1==1){
              x=y;
              while(2==24){
              x=1;
              }
              }

              """, 2,new int[]{5,5},true)]
    public void ValidatesTokensCondition(string expression, int numOfConditions,int[] numOfOperands, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        List<SyntaxAnalyze.Token> setsList = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.Condition);
        bool isValidSets = numOfConditions == setsList.Count;
        bool isValidValues = true;
        TokenCondition token;
        for (int i = 0; i < numOfOperands.Length;i++)
        {
            token = (TokenCondition)setsList.ElementAt(i);
            isValidValues = numOfOperands[i] == token.NumOfTokens;
            if (!isValidValues)
            {
                break;
            }
        }
        Assert.That(isValidAnalyze && isValidSets && isValidValues, Is.EqualTo(expected));
    }
    
    [TestCase("x=14+4;", 3,true)]
    [TestCase(
        """
        x=14+4;
        x=x;
        """
        , 3,true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x + 1;
        z = 0;
        z = 10;
        """
        , 7,true)]
    [TestCase(
        """
        x= 14 + 4*( 19-7)+16/(9-3);
        """
        , 17,true)]
    [TestCase(
        """
        x=1;
        x=x;
        """
        , 1,true)]
    [TestCase(
        """
        if(15){}
        """
        , 3,true)]
    [TestCase(
        """
        while (1==1){
        if(1==2){}
        }
        """
        , 10,true)]
    public void ValidatesTokensExpression(string expression, int numOfOpersAndValues, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        int count = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.Operation || token._type == TokenType.Value).Count;
        Assert.That(isValidAnalyze && count == numOfOpersAndValues, Is.EqualTo(expected));
    }
    
    [TestCase("x=14+4;", 0,true)]
    [TestCase(
        """
        x=14+4;
        x=x-x;
        """
        , 2,true)]
    [TestCase(
        """
        x= 14 + 4;
        y= x + 1;
        z = y + x;
        z = z - y;
        """
        , 5,true)]
    [TestCase(
        """
        y=15;
        x= 14 + 4*(y-7)+y/(9-3);
        """
        , 2,true)]
    [TestCase(
        """
        x=1;
        x=x;
        """
        , 1,true)]
    [TestCase(
        """
        if(15){}
        """
        , 0,true)]
    [TestCase(
        """
        while (1==1){
        if(1==2){}
        }
        """
        , 0,true)]
    [TestCase(
        """
        x=1;
        while (1==1){
        if(x==2){}
        }
        """
        , 1,true)]
    public void ValidatesTokensGetLocalVariable(string expression, int numOfLocalVars, bool expected)
    {
        SyntaxAnalyze.Analyzer.ResetTokens();
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool isValidAnalyze = parser.Parse();
        int count = SyntaxAnalyze.Analyzer.GetListOfTokens().FindAll(token => token._type == TokenType.GetLocalVariable).Count;
        Assert.That(isValidAnalyze && count == numOfLocalVars, Is.EqualTo(expected));
    }

    [TestCase(
        "x= 'abc';"
        , true)]
    [TestCase(
        """
        x= 'bcd';
        x= 'abc';
        """
        , true)]
    [TestCase(
        """
        x= '';
        """
        , true)]
    [TestCase(
        """
        x= 'bcd';
        y= x + 'abc';
        """, true)]
    
    public void ValidatesParseSTR(string expression, bool expected)
    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool actual = parser.Parse();

        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("x=y4;")]
    [TestCase("x=x;")]
    [TestCase(
        """
        x= a';
        """)]
    [TestCase(
        """
        x= a'';
        """)]
    [TestCase(
        """
        var x, y;

        function f(x,y)
        {
          x = z;
        }
        """
        )]
    [TestCase(
        """
        function f(x,y)
        {
          x = y + 1;
        }

        x = f2();
        """
        )]
    [TestCase(
        """
        functionf(x,y)
        {
          x = y + 1;
        }
        """
        )]
    [TestCase(
        """
          return124;
        """
        )]
    [TestCase(
        """
          returnx,y;
        """
        )]
    [TestCase(
        """
          returnxy;
        """
        )]
    [TestCase(
        """
          return x,y;
        """
        )]
    [TestCase(
        """ 
          return *;
        """
        )]
    [TestCase(
        """
          return 124
        """
        )]
    [TestCase(
        """
        var *;
        """
        )]
    [TestCase(
        """
        var1 x;
        """
        )]
    [TestCase(
        """
        var x=1
        """
        )]
    [TestCase(
        """
        if if  (1==1){
        x=31;
        }
        """
        )]
    [TestCase(
        """
        if  (1==1){ 
        {
        x=32;
        }
        """
        )]
    [TestCase(
        """
        if  ((1==1){
        x=33;
        }
        """
        )]
    [TestCase(
        """
        if  (1==1){
        x=34;
        """
        )]
   
    [TestCase(
        """
        else{
        x=0;
        } if(x==1){
        x=25;
        }

        """
        )]
    [TestCase(
        """
        if  (1==1){
        x=26;
        } 
        if else{
        x=17;
        }
        """
        )]
    [TestCase(
        """
        if  (1==1){
        } 
        else else{
        x=28;
        }
        """
      )]
    [TestCase(
        """
        while (){
        }
        """
      )]
    [TestCase(
        """
        while ()
        }
        """
      )]
    [TestCase(
        """
        while (){
        

        """
      )]
    [TestCase(
        """
        while {
        }
        """
      )]
    [TestCase(
        """
        while (x){
        }
        """
      )]
    [TestCase(
        """
        while (*){
        }
        """
      )]
    [TestCase(
        """
        while (1*2=2){
        x=3
        }
        """
      )]



    public void ValidatesParseThrowException(string expression)
    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        var actual = parser.Parse();
        Assert.True(!actual && parser.Error != "");

        ////Assert.Throws(typeof(KeyNotFoundException), () => parser.Parse());
        //Assert.Catch(() => parser.Parse());
    }


    [TestCase("2 + 3", true)]
    [TestCase("2 +\n 3", true)]
    [TestCase("24 + 3 + 9", true)]
    [TestCase("24 + 3\n + 9", true)]
    [TestCase("24 + 3 + 9\t", true)]
    [TestCase("24 + (3 + 5)", true)]
    [TestCase("24 + (3 * 6)", true)]
    [TestCase("24 + (3\n\r * 6)", true)]
    [TestCase("24 \t + \r\n (3 + (5 * 4) * 4 - 4 * (2 + 3))", true)]
    [TestCase("24 + (3 + (5 * 4) * 4 - 4 * (2 + 3))", true)]
    [TestCase("2 > 1", true)]
    [TestCase("2 == 2", true)]
    [TestCase("(1+1) == (1+1)", true)]
    [TestCase("(1+11) >= (1+1)", true)]
    [TestCase("(1+1) <(1+0)", true)]
    [TestCase("(1+1) <(1+0) && 2==2", true)]
    [TestCase("(1==1) || (2==2)", true)]
    [TestCase("(1==1) || 2==2", true)]
    [TestCase("1==1 || 2==2", true)]
    [TestCase("!(1==1)", true)]
    [TestCase("1!=1", true)]
    [TestCase("-1", true)]
    [TestCase("-(1+1)", true)]
    [TestCase("-(-1)", true)]
    [TestCase("-(-(1+1))", true)]
    [TestCase("1-(-(1+1))", true)]
    [TestCase("1-(-(1+1))-1", true)]
    [TestCase("+1", true)]
    [TestCase("+(1+1)", true)]
    [TestCase("+(+1)", true)]
    [TestCase("+(+(1+1))", true)]
    [TestCase("1+(+(1+1))-1", true)]
    [TestCase("1-(+(1+1))-1", true)]
    public void ValidatesExpression(string expression, bool expected)

    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        bool actual = parser.IsValidExpression();

        Assert.That(actual, Is.EqualTo(expected));
    }


    [TestCase(
        """
        2 + 3 // Hello world
        // It's test
        + 4
        """)]
    [TestCase(
        """
        2 + 3 // Hello world
        + 4
        """)]
    [TestCase(
        """
        2 + 3 // Hello world 
                     // It's test
        + 4
        """)]
    public void AcceptsComments(string expression)
    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        Assert.That(parser.IsValidExpression(), Is.True);
        // Assert.That(SyntaxAnalyze.Analyzer.IsValidExpression(expression), Is.True);
    }

    [TestCase("((24 + 3) 4)")]
    [TestCase("(24 + 3")]
    [TestCase("24 + 3)")]
    [TestCase("24 + (3 & 6)")]
    [TestCase("24 + 3y,3")]
    [TestCase("24 + 1_2x + (x_*_y)")]
    [TestCase("24 + 12x + (x_*_y)")]
    [TestCase("! ")]
    [TestCase(" 1! ")]
    [TestCase(" !1! ")]
    [TestCase(" (1+1)! ")]
    [TestCase(" !(1+1)! ")]
    [TestCase(" !! ")]
    [TestCase(" !+ ")]
    [TestCase(" !-1 ")]
    [TestCase(" -! ")]
    [TestCase(" -- ")]
    [TestCase(" -1- ")]
    [TestCase(" --- ")]
    [TestCase(" --! ")]
    [TestCase(" !-- ")]
    // no static type analyzing
    //[TestCase("2 > '3' ")]
    //[TestCase("1 < '1' ")]
    //[TestCase("2 + '3' ")]
    //[TestCase("2 + (1==1) ")]
    //[TestCase("(1+1) + (1==1) ")]
    //[TestCase("1 > (1==1) ")]
    //[TestCase("!'1'")]
    public void ThrowsException(string expression)
    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        var actual = parser.IsValidExpression();
        Assert.True(!actual && parser.Error != "");

        //Assert.That(() => parser.IsValidExpression(), Throws.InvalidOperationException);
        //Assert.That(() => SyntaxAnalyze.Analyzer.IsValidExpression(expression), Throws.InvalidOperationException);
    }

    [TestCase(
        """
        x= 'abc;
        """)]
    [TestCase(
        """
        x= ';
        """)]
    [TestCase(
        """
        x= '''';
        """)]
    [TestCase(
        """
        x= ''';
        """)]
    [TestCase(
        """
        a=1;
        x= ''a;
        """)]
    [TestCase(
        """
        x= 'a;
        """)]
    // no checking dynamic types
    //[TestCase(
    //    """
    //    x= 'bcd';
    //    y= x + 1;
    //    """)]
    public void ParseThrowsException(string expression)
    {
        var parser = new SyntaxAnalyze.Analyzer(expression);
        var actual = parser.Parse();
        Assert.True(!actual && parser.Error != "");

        //Assert.That(() => parser.Parse(), Throws.InvalidOperationException);

        //Assert.That(() => SyntaxAnalyze.Analyzer.IsValidExpression(expression), Throws.InvalidOperationException);
    }
}