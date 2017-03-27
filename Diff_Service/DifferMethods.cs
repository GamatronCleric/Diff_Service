using System;
using System.Collections.Generic;
using System.Linq;

namespace Diff_Service
{
    public static class DifferMethods         
    {
        /// <summary>
        /// This method will check if the input is equal and returns a specific enum value
        /// depending on the equality.
        /// </summary>
        /// <param name="leftInput"></param>
        /// <param name="rightInput"></param>
        /// <returns></returns>
        public static DiffResultType AreInputsEqual(string leftInput, string rightInput)
        {
            byte[] leftDecoded = Convert.FromBase64String(leftInput);
            byte[] rightDecoded = Convert.FromBase64String(rightInput);

            if(leftDecoded.SequenceEqual(rightDecoded))
            {
                return DiffResultType.Equals;
            }
            else if (leftDecoded.Length != rightDecoded.Length)
            {
                return DiffResultType.SizeDoesNotMatch;
            }
            else
            {
                return DiffResultType.ContentDoesNotMatch;
            }
        }


        /// <summary>
        /// This method returns a list of Diffs (offset and length of the difference).
        /// This method is only called when the content does not match and the content lengths are equal.
        /// </summary>
        /// <param name="leftInput"></param>
        /// <param name="rightInput"></param>
        /// <returns></returns>
        public static List<Diff> ReportDiffs(string leftInput, string rightInput)
        {
            List<Diff> diffs = new List<Diff>();
            byte[] leftDecoded = Convert.FromBase64String(leftInput);
            byte[] rightDecoded = Convert.FromBase64String(rightInput);

            for(int index=0; index<leftDecoded.Length; index++)
            {
                if (leftDecoded[index] != rightDecoded[index])
                {
                    //Diff found
                    Diff diff;
                    diff.offset = index;
                    
                    //Loop until equal
                    while(index < leftDecoded.Length && leftDecoded[index] != rightDecoded[index])
                    {
                        index++;
                    }
                    diff.length = index - diff.offset;

                    diffs.Add(diff);
                }
            }
            return diffs;
        }
    }
}