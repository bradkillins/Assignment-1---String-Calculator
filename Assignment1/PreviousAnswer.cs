using System;
using System.Collections.Generic;

namespace StringCalculator
{
    /// <summary>
    /// Stores the value of the previous answer, and if its been set.
    /// </summary>
    static class PreviousAnswer
    {
        static public Token value;
        static public bool hasBeenSet = false;
    }
}
