using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GameAnimationBuilder
{
    /// Author: CuteTN
    /// I love Auto completion of VSCode so here's my attempt to implement one :)
    static class SmartAutoCompletionSorter
    {
        // Target: user's expected string
        
        static private Int64 inf = Int64.MaxValue;

        // penalty for every unmatched character
        private const Int64 NotMatchPenalty = (Int64)1e9;
        private const Int64 SkipCharPenalty = (Int64)1;
        private const Int64 StartPosPenalty = (Int64)10;


        static SmartAutoCompletionSorter()
        {

        }

        /// heuristic function to calculate how good the element is matched with the target
        static public bool CheckMatched(string Target, string Element, out Int64 Score)
        {
            Target = Target.ToUpper();
            Element = Element.ToUpper();
            Score = 0;

            if(Target=="" || Element=="")
            {
                Score = inf;
                return false;
            }

            // simple 2 pointer technique here ;)
            int pointT = 0;
            int pointEnew = 0;
            int pointEold = 0;

            Score = 0;
            bool flag = true;
            
            for(pointT = 0; pointT < Target.Length; pointT++)
            {
                while(pointEnew<Element.Length && Element[pointEnew] != Target[pointT])
                {
                    pointEnew++;
                    Score += SkipCharPenalty;
                }

                if( pointEnew==Element.Length )
                {
                    Score += NotMatchPenalty;
                    flag = false;
                }
                else
                    Score += (pointEnew - pointEold)*(pointEnew - pointEold);

                if(pointT == 0)
                    Score += pointEnew*StartPosPenalty; 
                
                pointEold = pointEnew;
            }
            if( pointEnew < Element.Length )
                Score += SkipCharPenalty*(Element.Length - pointEnew);

            return flag;
        }

        static public void Sort(ref List<string> SuggestionList, string Target)
        {
            Sort(ref SuggestionList, Target, 0, SuggestionList.Count-1); 
        }

        static public void Sort(ref List<string> SuggestionList, string Target, int LeftRange, int RightRange)
        {
            if(LeftRange >= RightRange)
                return;

            int i = LeftRange;
            int j = RightRange;

            string pivot = SuggestionList[ (LeftRange + RightRange)/2 ];
            
            while ( true )
            {
                while( IsMorePriority02(Target, SuggestionList[i], pivot) )
                    i++;
                while( IsMorePriority02(Target, pivot, SuggestionList[j]) )
                    j--;

                if(i <= j)
                {
                    string temp = SuggestionList[i];
                    SuggestionList[i] = SuggestionList[j];
                    SuggestionList[j] = temp;

                    i++;
                    j--;
                }
                if( i>j )
                    break;
            }

            Sort(ref SuggestionList, Target, LeftRange, j);
            Sort(ref SuggestionList, Target, i, RightRange);
        }


        // many level of comparing 2 strings... //////////////////////////////////////////////////////////////////////////////////////////
        
        /// simply use both string. 
        /// delta is custom bonus penalty
        static private bool IsMorePriority01(string Target, string str1, string str2, Int64 delta1=0, Int64 delta2=0)
        {
            Int64 Score1;
            Int64 score2;
            bool matched1;
            bool matched2;

            matched1 = CheckMatched(Target, str1, out Score1);
            matched2 = CheckMatched(Target, str2, out score2);

            if(matched1 && !matched2)
                return true;
            if(matched2 && !matched1)
                return false;
            
            // this is for further priority functions
            Score1+=delta1;
            score2+=delta2;

            return Score1 < score2;
        }

        /// swap the last 2 characters
        static private bool IsMorePriority02(string Target, string str1, string str2)
        {
            const Int64 SWAPPENALTY = 10;

            Int64 delta1 = 0;
            Int64 delta2 = 0;

            // swap character
            if( str1.Length >= 2 )
            {
                string str1_ = str1.Remove( str1.Length - 2, 2 ) + str1[ str1.Length - 1 ] + str1[ str1.Length - 2 ];
                if( IsMorePriority01(Target, str1_, str1, SWAPPENALTY, 0) )
                {
                    str1 = str1_;
                    delta1 = SWAPPENALTY;
                }
            }
            if( str2.Length >= 2 )
            {
                string str2_ = str2.Remove( str2.Length - 2, 2 ) + str2[ str2.Length - 1 ] + str2[ str2.Length - 2 ];
                if( IsMorePriority01(Target, str2_, str2, SWAPPENALTY, 0) )
                {
                    str2 = str2_;
                    delta2 = SWAPPENALTY;
                }
            }

            return IsMorePriority01(Target, str1, str2, delta1, delta2);
        }

    }
}
