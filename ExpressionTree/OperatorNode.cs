using System;

namespace LogicInterpreter.ExpressionTree
{
    /// <summary>
    /// Types of logical operators
    /// </summary>
    public enum OperatorType
    {
        AND,  // &
        OR,   // |
        NOT   // !
    }

    /// <summary>
    /// Node representing a logical operator
    /// </summary>
    public class OperatorNode : TreeNode
    {
        public OperatorType Operator { get; set; }

        public OperatorNode(OperatorType op)
        {
            Operator = op;
        }

        public override TreeNode Clone()
        {
            OperatorNode clone = new OperatorNode(Operator);
            if (Left != null)
                clone.Left = Left.Clone();
            if (Right != null)
                clone.Right = Right.Clone();
            clone.HasValue = HasValue;
            clone.CachedValue = CachedValue;
            return clone;
        }

        public override string ToString()
        {
            return Operator switch
            {
                OperatorType.AND => "&",
                OperatorType.OR => "|",
                OperatorType.NOT => "!",
                _ => "?"
            };
        }
    }
}
