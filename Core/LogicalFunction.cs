using System;
using LogicInterpreter.DataStructures;
using LogicInterpreter.ExpressionTree;

namespace LogicInterpreter.Core
{
    /// <summary>
    /// Represents a defined logical function
    /// </summary>
    public class LogicalFunction
    {
        public string Name { get; set; }
        public DynamicArray<string> Parameters { get; set; }
        public TreeNode ExpressionRoot { get; set; }

        public LogicalFunction(string name, DynamicArray<string> parameters, TreeNode expressionRoot)
        {
            Name = name;
            Parameters = parameters;
            ExpressionRoot = expressionRoot;
        }

        public override string ToString()
        {
            // Build parameter list
            char[] paramStr = new char[1000];
            int len = 0;
            
            for (int i = 0; i < Parameters.Count; i++)
            {
                string param = Parameters[i];
                for (int j = 0; j < param.Length; j++)
                {
                    paramStr[len++] = param[j];
                }
                if (i < Parameters.Count - 1)
                {
                    paramStr[len++] = ',';
                    paramStr[len++] = ' ';
                }
            }

            return $"{Name}({new string(paramStr, 0, len)})";
        }
    }
}
