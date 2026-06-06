using System;
using LogicInterpreter.ExpressionTree;
using LogicInterpreter.DataStructures;

namespace LogicInterpreter.Core
{
    /// <summary>
    /// Evaluates expression trees recursively with caching support
    /// </summary>
    public class TreeEvaluator
    {
        private HashTable<string, LogicalFunction> functionRegistry;
        private HashTable<string, bool> functionResultCache;

        public TreeEvaluator(HashTable<string, LogicalFunction> functionRegistry)
        {
            this.functionRegistry = functionRegistry;
            this.functionResultCache = new HashTable<string, bool>();
        }

        public bool Evaluate(TreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            // Return cached value if available
            if (node.HasValue)
                return node.CachedValue;

            bool result = false;

            if (node is OperandNode operand)
            {
                if (!operand.IsSet)
                    throw new Exception($"Operand '{operand.Name}' has no value set");
                result = operand.Value;
            }
            else if (node is OperatorNode op)
            {
                if (op.Operator == OperatorType.NOT)
                {
                    result = !Evaluate(op.Left!);
                }
                else if (op.Operator == OperatorType.AND)
                {
                    result = Evaluate(op.Left!) && Evaluate(op.Right!);
                }
                else if (op.Operator == OperatorType.OR)
                {
                    result = Evaluate(op.Left!) || Evaluate(op.Right!);
                }
            }
            else if (node is FunctionCallNode funcCall)
            {
                result = EvaluateFunctionCall(funcCall);
            }
            else
            {
                throw new Exception($"Unknown node type: {node.GetType().Name}");
            }

            // Cache the result
            node.HasValue = true;
            node.CachedValue = result;

            return result;
        }

        public bool EvaluateFunction(LogicalFunction function, DynamicArray<bool> argumentValues)
        {
            if (argumentValues.Count != function.Parameters.Count)
                throw new Exception($"Function '{function.Name}' expects {function.Parameters.Count} arguments, but got {argumentValues.Count}");

            string cacheKey = BuildFunctionCacheKey(function.Name, argumentValues);
            if (functionResultCache.TryGetValue(cacheKey, out bool cachedResult))
                return cachedResult;

            TreeNode clonedTree = function.ExpressionRoot.Clone();

            for (int i = 0; i < function.Parameters.Count; i++)
            {
                SetOperandValue(clonedTree, function.Parameters[i], argumentValues[i]);
            }

            bool result = Evaluate(clonedTree);
            functionResultCache.Add(cacheKey, result);
            return result;
        }

        private bool EvaluateFunctionCall(FunctionCallNode funcCall)
        {
            // Look up function in registry
            if (!functionRegistry.TryGetValue(funcCall.FunctionName, out LogicalFunction function))
                throw new Exception($"Function '{funcCall.FunctionName}' is not defined");

            // Validate argument count
            if (funcCall.Arguments.Count != function.Parameters.Count)
                throw new Exception($"Function '{funcCall.FunctionName}' expects {function.Parameters.Count} arguments, but got {funcCall.Arguments.Count}");

            DynamicArray<bool> argumentValues = new DynamicArray<bool>();

            for (int i = 0; i < function.Parameters.Count; i++)
            {
                argumentValues.Add(Evaluate(funcCall.Arguments[i]));
            }

            return EvaluateFunction(function, argumentValues);
        }

        private string BuildFunctionCacheKey(string functionName, DynamicArray<bool> argumentValues)
        {
            string key = functionName + "(";
            for (int i = 0; i < argumentValues.Count; i++)
            {
                if (i > 0)
                    key += ",";
                key += argumentValues[i] ? "1" : "0";
            }
            return key + ")";
        }

        private void SetOperandValue(TreeNode node, string operandName, bool value)
        {
            if (node == null)
                return;

            if (node is OperandNode operand && operand.Name == operandName)
            {
                operand.SetValue(value);
            }
            else if (node is OperatorNode)
            {
                SetOperandValue(node.Left, operandName, value);
                SetOperandValue(node.Right, operandName, value);
            }
            else if (node is FunctionCallNode funcCall)
            {
                for (int i = 0; i < funcCall.Arguments.Count; i++)
                {
                    SetOperandValue(funcCall.Arguments[i], operandName, value);
                }
            }
        }
    }
}
