using System;
using LogicInterpreter.Core;
using LogicInterpreter.Parsing;
using LogicInterpreter.ExpressionTree;
using LogicInterpreter.DataStructures;

namespace LogicInterpreter.Commands
{
    /// <summary>
    /// Handles the DEFINE command
    /// </summary>
    public class DefineCommand
    {
        private FunctionRegistry registry;

        public DefineCommand(FunctionRegistry registry)
        {
            this.registry = registry;
        }

        public void Execute(ParsedCommand command)
        {
            try
            {
                // Tokenize the expression
                Tokenizer tokenizer = new Tokenizer(command.Expression);
                DynamicArray<Token> tokens = tokenizer.Tokenize();

                // Parse into expression tree
                ExpressionParser parser = new ExpressionParser(tokens);
                TreeNode expressionRoot = parser.Parse();

                // Define the function in registry (validation happens here)
                registry.DefineFunction(command.FunctionName, command.Parameters, expressionRoot);

                Console.WriteLine($"Function '{command.FunctionName}' defined successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error defining function: {ex.Message}");
            }
        }
    }
}
