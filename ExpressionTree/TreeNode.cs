using System;

namespace LogicInterpreter.ExpressionTree
{
    /// <summary>
    /// Base class for all tree nodes
    /// </summary>
    public abstract class TreeNode
    {
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }
        
        // For caching evaluation results
        public bool HasValue { get; set; }
        public bool CachedValue { get; set; }

        protected TreeNode()
        {
            HasValue = false;
            CachedValue = false;
        }

        public abstract TreeNode Clone();
        
        public void ClearCache()
        {
            HasValue = false;
            CachedValue = false;
            if (Left != null)
                Left.ClearCache();
            if (Right != null)
                Right.ClearCache();
        }
    }
}
