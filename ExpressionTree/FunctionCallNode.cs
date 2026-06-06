using System;
using LogicInterpreter.DataStructures;

namespace LogicInterpreter.ExpressionTree
{
    /// <summary>
    /// Node representing a function call like func1(a, b)
    /// </summary>
    public class FunctionCallNode : TreeNode
    {
        public string FunctionName { get; set; }
        public DynamicArray<TreeNode> Arguments { get; set; }

        public FunctionCallNode(string functionName)
        {
            FunctionName = functionName;
            Arguments = new DynamicArray<TreeNode>();
        }

        public override TreeNode Clone()
        {
            FunctionCallNode clone = new FunctionCallNode(FunctionName);
            for (int i = 0; i < Arguments.Count; i++)
            {
                clone.Arguments.Add(Arguments[i].Clone());
            }
            clone.HasValue = HasValue;
            clone.CachedValue = CachedValue;
            return clone;
        }

        public override string ToString()
        {
            return FunctionName + "(...)";
        }
    }
}
