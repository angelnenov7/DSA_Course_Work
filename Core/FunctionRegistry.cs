using System;
using LogicInterpreter.DataStructures;
using LogicInterpreter.ExpressionTree;

namespace LogicInterpreter.Core
{
    /// <summary>
    /// Manages all defined logical functions
    /// </summary>
    public class FunctionRegistry
    {
        private HashTable<string, LogicalFunction> functions;

        public FunctionRegistry()
        {
            functions = new HashTable<string, LogicalFunction>();
        }

        public void DefineFunction(string name, DynamicArray<string> parameters, TreeNode expressionRoot)
        {
            if (name == null || name.Length == 0)
                throw new ArgumentException("Function name cannot be empty");

            // Validate that all operands in the expression are either parameters or function calls
            ValidateExpression(expressionRoot, parameters);

            LogicalFunction function = new LogicalFunction(name, parameters, expressionRoot);

            if (functions.ContainsKey(name))
            {
                // Update existing function
                functions.Set(name, function);
            }
            else
            {
                functions.Add(name, function);
            }
        }

        public bool TryGetFunction(string name, out LogicalFunction function)
        {
            return functions.TryGetValue(name, out function);
        }

        public LogicalFunction GetFunction(string name)
        {
            if (!functions.TryGetValue(name, out LogicalFunction function))
                throw new Exception($"Function '{name}' is not defined");
            return function;
        }

        public bool ContainsFunction(string name)
        {
            return functions.ContainsKey(name);
        }

        public DynamicArray<string> GetAllFunctionNames()
        {
            return functions.GetKeys();
        }

        public HashTable<string, LogicalFunction> GetFunctions()
        {
            return functions;
        }

        private void ValidateExpression(TreeNode node, DynamicArray<string> validParameters)
        {
            if (node == null)
                return;

            if (node is OperandNode operand)
            {
                // Check if operand is a valid parameter (not a number constant)
                if (!IsNumericConstant(operand.Name) && !ContainsParameter(validParameters, operand.Name))
                {
                    throw new Exception($"Operand '{operand.Name}' is not defined as a parameter");
                }
            }
            else if (node is OperatorNode op)
            {
                ValidateExpression(op.Left, validParameters);
                ValidateExpression(op.Right, validParameters);
            }
            else if (node is FunctionCallNode funcCall)
            {
                // Check if function exists
                if (!functions.ContainsKey(funcCall.FunctionName))
                {
                    throw new Exception($"Function '{funcCall.FunctionName}' is not defined");
                }

                // Validate parameter count
                LogicalFunction calledFunc = functions.Get(funcCall.FunctionName);
                if (funcCall.Arguments.Count != calledFunc.Parameters.Count)
                {
                    throw new Exception($"Function '{funcCall.FunctionName}' expects {calledFunc.Parameters.Count} parameters, but {funcCall.Arguments.Count} were provided");
                }

                // Validate arguments
                for (int i = 0; i < funcCall.Arguments.Count; i++)
                {
                    ValidateExpression(funcCall.Arguments[i], validParameters);
                }
            }
        }

        private bool ContainsParameter(DynamicArray<string> parameters, string name)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (StringEquals(parameters[i], name))
                    return true;
            }
            return false;
        }

        private bool IsNumericConstant(string value)
        {
            return value == "0" || value == "1";
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

        public void Clear()
        {
            functions.Clear();
        }
    }
}
