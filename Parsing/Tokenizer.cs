using System;
using LogicInterpreter.DataStructures;

namespace LogicInterpreter.Parsing
{
    /// <summary>
    /// Character-by-character tokenizer for logical expressions
    /// No built-in string functions allowed (Split, IndexOf, etc.)
    /// </summary>
    public class Tokenizer
    {
        private string input;
        private int position;

        public Tokenizer(string input)
        {
            this.input = input ?? "";
            this.position = 0;
        }

        public DynamicArray<Token> Tokenize()
        {
            DynamicArray<Token> tokens = new DynamicArray<Token>();

            while (position < input.Length)
            {
                char current = input[position];

                // Skip whitespace
                if (IsWhitespace(current))
                {
                    position++;
                    continue;
                }

                // Single character tokens
                if (current == '&')
                {
                    tokens.Add(new Token(TokenType.AND, "&"));
                    position++;
                }
                else if (current == '|')
                {
                    tokens.Add(new Token(TokenType.OR, "|"));
                    position++;
                }
                else if (current == '!')
                {
                    tokens.Add(new Token(TokenType.NOT, "!"));
                    position++;
                }
                else if (current == '(')
                {
                    tokens.Add(new Token(TokenType.LeftParen, "("));
                    position++;
                }
                else if (current == ')')
                {
                    tokens.Add(new Token(TokenType.RightParen, ")"));
                    position++;
                }
                else if (current == ',')
                {
                    tokens.Add(new Token(TokenType.Comma, ","));
                    position++;
                }
                else if (current == '0' || current == '1')
                {
                    tokens.Add(new Token(TokenType.Number, CharToString(current)));
                    position++;
                }
                else if (IsLetter(current))
                {
                    // Read identifier (function name or operand)
                    string identifier = ReadIdentifier();
                    
                    // Check if next non-whitespace character is '('
                    int savedPos = position;
                    SkipWhitespace();
                    
                    if (position < input.Length && input[position] == '(')
                    {
                        tokens.Add(new Token(TokenType.FunctionName, identifier));
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Operand, identifier));
                    }
                    
                    position = savedPos; // Restore position
                }
                else
                {
                    throw new Exception($"Unexpected character '{current}' at position {position}");
                }
            }

            tokens.Add(new Token(TokenType.EOF, ""));
            return tokens;
        }

        private string ReadIdentifier()
        {
            char[] chars = new char[100]; // Buffer for identifier
            int length = 0;

            while (position < input.Length && (IsLetter(input[position]) || IsDigit(input[position])))
            {
                chars[length++] = input[position++];
            }

            // Convert char array to string manually
            return CharsToString(chars, length);
        }

        private void SkipWhitespace()
        {
            while (position < input.Length && IsWhitespace(input[position]))
            {
                position++;
            }
        }

        private bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        private bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        // Manual string conversion methods (no built-in string functions)
        private string CharsToString(char[] chars, int length)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[i];
            }
            return new string(result);
        }

        private string CharToString(char c)
        {
            return new string(new char[] { c });
        }
    }
}
