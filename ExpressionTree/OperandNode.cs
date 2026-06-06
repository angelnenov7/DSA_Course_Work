using System;

namespace LogicInterpreter.ExpressionTree
{
    /// <summary>
    /// Node representing an operand (variable like 'a', 'b', 'c')
    /// </summary>
    public class OperandNode : TreeNode
    {
        public string Name { get; set; }
        public bool Value { get; set; }
        public bool IsSet { get; set; }

        public OperandNode(string name)
        {
            Name = name;
            IsSet = false;
            Value = false;
        }

        public void SetValue(bool value)
        {
            Value = value;
            IsSet = true;
            HasValue = true;
            CachedValue = value;
        }

        public override TreeNode Clone()
        {
            OperandNode clone = new OperandNode(Name);
            if (IsSet)
            {
                clone.Value = Value;
                clone.IsSet = true;
                clone.HasValue = HasValue;
                clone.CachedValue = CachedValue;
            }
            return clone;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
