using System;
using LogicInterpreter.Core;
using LogicInterpreter.Parsing;
using LogicInterpreter.ExpressionTree;
using LogicInterpreter.DataStructures;

namespace LogicInterpreter.Commands
{
    /// <summary>
    /// Handles the SOLVE command
    /// </summary>
    public class SolveCommand
    {
        private FunctionRegistry registry;
        private TreeEvaluator evaluator;

        public SolveCommand(FunctionRegistry registry, TreeEvaluator evaluator)
        {
            this.registry = registry;
            this.evaluator = evaluator;
        }

        public void Execute(ParsedCommand command)
        {
            try
            {
                // Get the function
                LogicalFunction function = registry.GetFunction(command.FunctionName);

                // Validate parameter count
                if (command.Values.Count != function.Parameters.Count)
                {
                    throw new Exception($"Function '{command.FunctionName}' expects {function.Parameters.Count} parameters, but {command.Values.Count} were provided");
                }

                DynamicArray<bool> argumentValues = new DynamicArray<bool>();
                for (int i = 0; i < function.Parameters.Count; i++)
                {
                    string valueStr = command.Values[i];
                    
                    if (valueStr != "0" && valueStr != "1")
                        throw new Exception($"Invalid value '{valueStr}'. Expected 0 or 1.");
                    
                    argumentValues.Add(valueStr == "1");
                }

                bool result = evaluator.EvaluateFunction(function, argumentValues);

                // Display result
                Console.WriteLine($"Result: {(result ? "1" : "0")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error solving function: {ex.Message}");
            }
        }
    }
}
