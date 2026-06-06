using System;
using LogicInterpreter.DataStructures;
using LogicInterpreter.ExpressionTree;

namespace LogicInterpreter.Parsing
{
    /// <summary>
    /// Parses logical expressions into expression trees
    /// Uses recursive descent with explicit operator precedence.
    /// </summary>
    public class ExpressionParser
    {
        private DynamicArray<Token> tokens;
        private int position;

        public ExpressionParser(DynamicArray<Token> tokens)
        {
            this.tokens = tokens;
            this.position = 0;
        }

        public TreeNode Parse()
        {
            TreeNode result = ParseOr();
            if (Current.Type != TokenType.EOF && Current.Type != TokenType.Comma && Current.Type != TokenType.RightParen)
                throw new Exception($"Unexpected token: {Current}");
            return result;
        }

        private Token Current => tokens[position];

        private TreeNode ParseOr()
        {
            TreeNode left = ParseAnd();
            while (Current.Type == TokenType.OR)
            {
                position++;
                OperatorNode node = new OperatorNode(OperatorType.OR);
                node.Left = left;
                node.Right = ParseAnd();
                left = node;
            }
            return left;
        }

        private TreeNode ParseAnd()
        {
            TreeNode left = ParseNot();
            while (Current.Type == TokenType.AND)
            {
                position++;
                OperatorNode node = new OperatorNode(OperatorType.AND);
                node.Left = left;
                node.Right = ParseNot();
                left = node;
            }
            return left;
        }

        private TreeNode ParseNot()
        {
            if (Current.Type == TokenType.NOT)
            {
                position++;
                OperatorNode notNode = new OperatorNode(OperatorType.NOT);
                notNode.Left = ParseNot();
                return notNode;
            }

            return ParsePrimary();
        }

        private TreeNode ParsePrimary()
        {
            Token token = Current;

            if (token.Type == TokenType.Operand)
            {
                position++;
                return new OperandNode(token.Value);
            }

            if (token.Type == TokenType.Number)
            {
                position++;
                OperandNode numberNode = new OperandNode(token.Value);
                numberNode.SetValue(token.Value == "1");
                return numberNode;
            }

            if (token.Type == TokenType.FunctionName)
                return ParseFunctionCall();

            if (token.Type == TokenType.LeftParen)
            {
                position++;
                TreeNode expression = ParseOr();
                if (Current.Type != TokenType.RightParen)
                    throw new Exception("Expected ')' after expression");
                position++;
                return expression;
            }

            throw new Exception($"Unexpected token: {token}");
        }

        private FunctionCallNode ParseFunctionCall()
        {
            string functionName = Current.Value;
            position++;

            if (Current.Type != TokenType.LeftParen)
                throw new Exception($"Expected '(' after function name '{functionName}'");

            position++;
            FunctionCallNode funcNode = new FunctionCallNode(functionName);

            if (Current.Type == TokenType.RightParen)
            {
                position++;
                return funcNode;
            }

            while (true)
            {
                funcNode.Arguments.Add(ParseOr());

                if (Current.Type == TokenType.Comma)
                {
                    position++;
                    continue;
                }

                if (Current.Type == TokenType.RightParen)
                {
                    position++;
                    break;
                }

                throw new Exception($"Expected ',' or ')' in function call '{functionName}'");
            }

            return funcNode;
        }
    }
}
