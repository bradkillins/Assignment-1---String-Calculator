/* 
 *      Assignment 1 - String Calculator
 *      By Bradley Killins
 *      
 *      SODV2202 - Object Oriented Programming
 *      Instructor: Sean Lynch
 *      October 13, 2020
 *      
 *      Please see attached PDF for UML Diagram (or D2L)
 *      
 *      ** Some inspiration for the design came from Ruslan's Blog "Let's build a Simple
 *      Interpreter - Part 7: Abstract Syntax Tree" - https://ruslanspivak.com/lsbasi-part7/
 *      
 */

using System;
using System.Collections.Generic;

namespace StringCalculator
{
    public class Program
    {
        public static string ProcessCommand(string input)
        {
            try
            {
                Lexer lexer = new Lexer(input);
                Parser parser = new Parser(lexer.tokens);
                return parser.Compute();
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e.Message;
            }
        }

        static void Main()
        {
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                Console.WriteLine(ProcessCommand(input));
            }
        }
    }
}
