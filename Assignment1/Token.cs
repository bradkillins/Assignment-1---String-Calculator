using System;
using System.Collections.Generic;

namespace StringCalculator
{
    /// <summary>
    /// A symbol that is either a number or mathematical operator.
    /// </summary>
    class Token
    {
        public string Kind { get; set; } //num, op, bracket
        public double NumValue { get; set; } = 1;
        public char OpValue { get; set; } = '#';

        public Token(string kind, double value)
        {
            Kind = kind;
            NumValue = value;
        }
        public Token(string kind, char value)
        {
            Kind = kind;
            OpValue = value;
        }
    }
}
