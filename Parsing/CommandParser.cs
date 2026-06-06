using System;
using LogicInterpreter.DataStructures;

namespace LogicInterpreter.Parsing
{
    /// <summary>
    /// Types of commands
    /// </summary>
    public enum CommandType
    {
        DEFINE,
        SOLVE,
        EXIT,
        UNKNOWN
    }

    /// <summary>
    /// Parsed command data
    /// </summary>
    public class ParsedCommand
    {
        public CommandType Type { get; set; }
        public string FunctionName { get; set; }
        public DynamicArray<string> Parameters { get; set; }
        public string Expression { get; set; }
        public DynamicArray<string> Values { get; set; }

        public ParsedCommand()
        {
            FunctionName = "";
            Parameters = new DynamicArray<string>();
            Expression = "";
            Values = new DynamicArray<string>();
        }
    }

    /// <summary>
    /// Character-by-character command parser
    /// No built-in string functions allowed
    /// </summary>
    public class CommandParser
    {
        public ParsedCommand Parse(string input)
        {
            if (input == null || input.Length == 0)
            {
                ParsedCommand empty = new ParsedCommand();
                empty.Type = CommandType.UNKNOWN;
                return empty;
            }

            ParsedCommand command = new ParsedCommand();
            int pos = 0;

            // Skip leading whitespace
            pos = SkipWhitespace(input, pos);

            // Read command type
            string commandWord = ReadWord(input, ref pos);
            command.Type = GetCommandType(commandWord);

            if (command.Type == CommandType.EXIT || command.Type == CommandType.UNKNOWN)
                return command;

            pos = SkipWhitespace(input, pos);

            if (command.Type == CommandType.DEFINE)
            {
                ParseDefineCommand(input, ref pos, command);
            }
            else if (command.Type == CommandType.SOLVE)
            {
                ParseSolveCommand(input, ref pos, command);
            }

            return command;
        }

        private void ParseDefineCommand(string input, ref int pos, ParsedCommand command)
        {
            // Format: DEFINE funcName(a, b, c): "expression"
            
            // Read function name
            command.FunctionName = ReadWord(input, ref pos);
            pos = SkipWhitespace(input, pos);

            // Expect '('
            if (pos >= input.Length || input[pos] != '(')
                throw new Exception("Expected '(' after function name");
            pos++;

            // Read parameters
            while (pos < input.Length && input[pos] != ')')
            {
                pos = SkipWhitespace(input, pos);
                if (input[pos] == ')')
                    break;

                string param = ReadWord(input, ref pos);
                command.Parameters.Add(param);

                pos = SkipWhitespace(input, pos);
                if (pos < input.Length && input[pos] == ',')
                    pos++;
            }

            if (pos >= input.Length || input[pos] != ')')
                throw new Exception("Expected ')' after parameters");
            pos++;

            pos = SkipWhitespace(input, pos);

            // Expect ':'
            if (pos >= input.Length || input[pos] != ':')
                throw new Exception("Expected ':' after function signature");
            pos++;

            pos = SkipWhitespace(input, pos);

            // Read expression (might be in quotes)
            if (pos < input.Length && input[pos] == '"')
            {
                pos++; // Skip opening quote
                command.Expression = ReadUntil(input, ref pos, '"');
                pos++; // Skip closing quote
            }
            else
            {
                command.Expression = ReadUntilEnd(input, ref pos);
            }
        }

        private void ParseSolveCommand(string input, ref int pos, ParsedCommand command)
        {
            // Format: SOLVE funcName(1, 0, 1)
            
            command.FunctionName = ReadWord(input, ref pos);
            pos = SkipWhitespace(input, pos);

            // Expect '('
            if (pos >= input.Length || input[pos] != '(')
                throw new Exception("Expected '(' after function name");
            pos++;

            // Read values
            while (pos < input.Length && input[pos] != ')')
            {
                pos = SkipWhitespace(input, pos);
                if (input[pos] == ')')
                    break;

                string value = ReadWord(input, ref pos);
                command.Values.Add(value);

                pos = SkipWhitespace(input, pos);
                if (pos < input.Length && input[pos] == ',')
                    pos++;
            }

            if (pos >= input.Length || input[pos] != ')')
                throw new Exception("Expected ')' after values");
            pos++;
        }

        private CommandType GetCommandType(string word)
        {
            if (StringEquals(word, "DEFINE"))
                return CommandType.DEFINE;
            if (StringEquals(word, "SOLVE"))
                return CommandType.SOLVE;
            if (StringEquals(word, "EXIT") || StringEquals(word, "exit"))
                return CommandType.EXIT;
            return CommandType.UNKNOWN;
        }

        private string ReadWord(string input, ref int pos)
        {
            char[] buffer = new char[100];
            int length = 0;

            while (pos < input.Length && IsWordChar(input[pos]))
            {
                buffer[length++] = input[pos++];
            }

            return new string(buffer, 0, length);
        }

        private string ReadUntil(string input, ref int pos, char delimiter)
        {
            char[] buffer = new char[1000];
            int length = 0;

            while (pos < input.Length && input[pos] != delimiter)
            {
                buffer[length++] = input[pos++];
            }

            return new string(buffer, 0, length);
        }

        private string ReadUntilEnd(string input, ref int pos)
        {
            char[] buffer = new char[1000];
            int length = 0;

            while (pos < input.Length)
            {
                buffer[length++] = input[pos++];
            }

            return new string(buffer, 0, length);
        }

        private int SkipWhitespace(string input, int pos)
        {
            while (pos < input.Length && IsWhitespace(input[pos]))
            {
                pos++;
            }
            return pos;
        }

        private bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        private bool IsWordChar(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_';
        }

        private bool StringEquals(string a, string b)
        {
            if (a.Length != b.Length)
                return false;
            
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
    }
}
