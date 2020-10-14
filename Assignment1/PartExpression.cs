using System;
using System.Collections.Generic;

namespace StringCalculator
{
    /// <summary>
    /// The left number and operator of an expression.
    /// </summary>
    class PartExpression
    {
        public double Value { get; set; }
        public char Op { get; set; }

        public PartExpression(Token num, Token op = null)
        {
            Value = num.NumValue;
            if (op == null)
                Op = 'e'; //op should only be null when reached the end of the expression
            else
                Op = op.OpValue;
        }
    }
}
