using System;
using System.Collections.Generic;

namespace StringCalculator
{
    /// <summary>
    /// For converting a list of tokens to an evaluated numeric value. 
    /// </summary>
    class Parser
    {
        public List<Token> lexedTokens;

        public Parser(List<Token> tokens)
        {
            lexedTokens = tokens;
        }

        //incorperates negative numbers into their numeric value
        List<Token> ProcessNegatives(List<Token> tokens)
        {
            //leading negative
            if (tokens[0].OpValue == '-')
            {
                tokens[1].NumValue *= -1;
                tokens.RemoveAt(0);
            }
            //negative after operator or left bracket
            for (int i = 0; i < tokens.Count; i++)
            {
                if((tokens[i].Kind == "op" || tokens[i].OpValue == '(') && tokens[i+1].OpValue == '-')
                {
                    tokens[i+2].NumValue *= -1;
                    tokens.RemoveAt(i+1);
                }  
            }
            return tokens;
        }

        //Evaluates any expression inside of brackets, then replaces the 
        //bracket set with that value.
        List<Token> ProcessBrackets(List<Token> tokens)
        {
            if (HasBrackets(tokens))
            {
                int innerBracketStart = FindStartingBracket(tokens);
                int innerBracketEnd = FindEndingBracket(tokens, innerBracketStart);

                //build a list of the tokens inside ( ) 
                List<Token> insideTokens = new List<Token>();
                for (int i = innerBracketStart + 1; i < innerBracketEnd; i++)
                {
                    insideTokens.Add(tokens[i]);
                }
                //evaluate, then replace with a single num token
                Expression insideExpression = new Expression(insideTokens);
                tokens.RemoveRange(innerBracketStart, innerBracketEnd - innerBracketStart + 1);
                tokens.Insert(innerBracketStart, insideExpression.EvaluateExpression());
                ProcessBrackets(tokens); //recursive call, repeat until no more bracket sets found
            }

            return tokens;
        }

        bool HasBrackets(List<Token> tokens)
        {
            bool hasBrackets = false;

            foreach (var token in tokens)
            {
                if (token.Kind == "bracket")
                {
                    hasBrackets = true;
                    break;
                } 
            }
            return hasBrackets;
        }

        int FindStartingBracket(List<Token> tokens)
        {
            int innerBracketStart = 0;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].OpValue == '(')
                    innerBracketStart = i;
            }
            return innerBracketStart;
        }

        int FindEndingBracket(List<Token> tokens, int StartBracketPosition)
        {
            for (int i = StartBracketPosition + 1; i < tokens.Count; i++)
            {
                if (tokens[i].OpValue == ')')
                {
                    return i;
                }
            }
            throw new Exception("Syntax Error, no closing bracket.");
        }

        //setup and evaluate the lexedTokens, then set PreviousAnswer
        public string Compute()
        {
            List<Token> processedTokens = ProcessBrackets(ProcessNegatives(lexedTokens));
            Expression expression = new Expression(processedTokens);
            Token computedValue = expression.EvaluateExpression();
            PreviousAnswer.value = computedValue;
            PreviousAnswer.hasBeenSet = true;
            return computedValue.NumValue.ToString();
        }
    }
}
