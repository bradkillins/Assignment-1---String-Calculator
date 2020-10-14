using System;
using System.Collections.Generic;

namespace StringCalculator
{
    /// <summary>
    /// For converting a sequence of characters into a sequence of tokens 
    /// </summary>
    class Lexer
    {
        public List<Token> tokens;

        public Lexer(string input)
        {
            if (input.Length < 1)
                throw new Exception("Empty expression, please have at least one term.");
            tokens = new List<Token>();
            BuildTokenList(input.ToLower(), tokens);
        }

        void BuildTokenList(string input, List<Token> tokens)
        {
            input = ReplaceANS(RemoveWhiteSpace(input));
            DetectImplicitMultiplication(input);
            
            for (int i = 0; i < input.Length; i++)
            {
                //detect number
                if (Char.IsDigit(input[i]))
                {
                    string tempNum = input[i].ToString();
                    i++;
                    if (i >= input.Length) //last char in string
                    {
                        MakeNewToken(Double.Parse(tempNum), tokens);
                        break; //breaking for loop
                    }
                    while (Char.IsDigit(input[i]) || input[i] == '.')
                    {
                        tempNum += input[i].ToString();
                        if (i >= input.Length-1) break;
                        i++;
                    }
                    MakeNewToken(Double.Parse(tempNum), tokens);
                }

                //detect operator or bracket
                if ( 
                    !Char.IsDigit(input[i]) &&
                    (input[i] == '+' || input[i] == '-' || input[i] == '*'
                    || input[i] == '/' || input[i] == '(' || input[i] == ')')
                   )
                {
                    MakeNewToken(input[i], tokens);
                }
                else if (Char.IsDigit(input[i]))
                {
                    continue;
                }
                else
                    throw new Exception("Unexpected symbol - Syntax error.");
            }
        }

        string RemoveWhiteSpace(string input)
        {
            return input.Replace(" ", "");
        }

        //makes a new token based on the parameters passed, then
        //addes to a list of tokens
        void MakeNewToken(char op, List<Token> tokens)
        {
            if (op == '(' || op == ')')
            {
                Token newBracket = new Token("bracket", op);
                tokens.Add(newBracket);
            }
            else
            {
                Token newOp = new Token("op", op);
                tokens.Add(newOp);
            }
        }
        void MakeNewToken(double value, List<Token> tokens)
        {
            Token newNum = new Token("num", value);
            tokens.Add(newNum);
        }

        //find and replace each occurance of 'ans' with the previous answer
        string ReplaceANS(string input)
        {
            for (int i = 0; i < input.Length - 2; i++)
            {
                if(input[i] == 'a' && input[i+1] == 'n' && input[i+2] == 's')
                {
                    if (!PreviousAnswer.hasBeenSet)
                        throw new Exception("There is no value for ANS until at least one expression has been evaluated");
                    input = input.Remove(i, "ans".Length);
                    input = input.Insert(i, PreviousAnswer.value.NumValue.ToString());
                }
            }
            return input;
        }

        void DetectImplicitMultiplication(string input)
        {
            for (int i = 1; i < input.Length; i++)
            {
                if(input[i] == '(')
                {
                    if (Char.IsDigit(input[i - 1]) || input[i - 1] == '.')
                        throw new Exception("Cannot implicitly multiply using bracket - Please use an asterisk (*)");
                }
            }
        }
    }
}
