using System;
using System.Collections.Generic;

namespace StringCalculator
{
    /// <summary>
    /// A list of part expressions that make up a full mathematical expression.
    /// </summary>
    class Expression
    {
        List<PartExpression> expressionParts;

        public Expression(List<Token> tokens)
        {
            expressionParts = new List<PartExpression>();
            BuildExpressionParts(tokens);
        }

        void BuildExpressionParts(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                Token next;
                if (i + 1 >= tokens.Count) //last token in list
                    next = null;
                else 
                    next = tokens[i+1];

                if (next != null)
                {
                    PartExpression part = new PartExpression(tokens[i], next);
                    expressionParts.Add(part);
                    tokens.RemoveAt(i + 1); //removes operator token
                }
                else
                {
                    PartExpression part = new PartExpression(tokens[i]);
                    expressionParts.Add(part);
                }
            }
        }


        public Token EvaluateExpression()
        {
            int index = 0;
            
            while (expressionParts.Count > 1)
            {
                PartExpression leftPart = expressionParts[index];
                PartExpression rightPart = expressionParts[index + 1];

                //wrong order of operations
                if ((leftPart.Op == '+' || leftPart.Op == '-') && (rightPart.Op == '*' || rightPart.Op == '/'))
                {
                    index++;
                    continue;
                }
                //correct order
                else
                {
                    PartExpression combinedPart = CombineParts(leftPart, rightPart);
                    expressionParts.RemoveAt(index); //removes leftPart
                    expressionParts.RemoveAt(index); //removes rightPart
                    expressionParts.Insert(index, combinedPart);
                    index = 0; //reset to start of expression after each combination
                }
            }
            //after while loop there will only be one part left

            PartExpression lastPart = expressionParts[0];

            double resultNum = lastPart.Value;

            Token resultToken = new Token("num", resultNum);
            return resultToken;
        }

        //combine two part expressions by preforming the mathematical operation between them
        PartExpression CombineParts(PartExpression leftPart, PartExpression rightPart)
        {
            double combinedValue;
            switch (leftPart.Op)
            {
                case '+':
                    combinedValue = leftPart.Value + rightPart.Value;
                    break;
                case '-':
                    combinedValue = leftPart.Value - rightPart.Value;
                    break;
                case '*':
                    combinedValue = leftPart.Value * rightPart.Value;
                    break;
                case '/':
                    combinedValue = leftPart.Value / rightPart.Value;
                    break;
                default:
                    throw new Exception("Unexpected Operator - Syntax error.");
            }
            Token combinedValueToken = new Token("num", combinedValue);
            //move the operator from the right part to the newly combined part
            Token rightOp = new Token("op", rightPart.Op);  
            return new PartExpression(combinedValueToken, rightOp);
        }
    }
}
