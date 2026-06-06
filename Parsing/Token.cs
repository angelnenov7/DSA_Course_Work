using System;

namespace LogicInterpreter.Parsing
{
    /// <summary>
    /// Types of tokens in expressions
    /// </summary>
    public enum TokenType
    {
        Operand,        // variable name (a, b, c)
        AND,            // &
        OR,             // |
        NOT,            // !
        LeftParen,      // (
        RightParen,     // )
        Comma,          // ,
        FunctionName,   // func1, func2, etc.
        Number,         // 0 or 1
        EOF             // End of input
    }

    /// <summary>
    /// Represents a token in the expression
    /// </summary>
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}
