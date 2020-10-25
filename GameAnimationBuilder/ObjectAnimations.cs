using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAnimationBuilder
{
    public class ObjectAnimations : AnimatingObject, IScriptable
    {
        /*
            -GetContext
            -GetHint
            -GetSnippet
            -ParseData
            -AddToLib
            GetPreviewBitmap
        */
        public List<StateAnimation> States = new List<StateAnimation>();
        static private readonly int parametersOnLine = 5;
        public int TotalDuration = 0;

        public ContextType GetContext(int order)
        {
            if (order == 0)
                return ContextType.Tag;

            if (order == 1)
                return ContextType.Id;

            switch(order % parametersOnLine)
            {
                case 2:
                    return ContextType.NewStateId;
                case 3:
                    return ContextType.Animation;
                case 4:
                    return ContextType.Bool;
                case 0:
                    return ContextType.Bool;
                case 1:
                    return ContextType.Int;
                default:
                    throw new Exception("Get Context for ObjectAnimations failed.");
            }
        }

        public string GetHint(int order)
        {
            if (order == 0)
                return $"OBJECT_ANIMATIONS: Include all animations of an object";

            if (order == 1)
                return $"ObjectAnimationID: Should be <OBJECT_KIND>_ANIMATIONS";

            int stateIndex =  (order + parametersOnLine - 2) / parametersOnLine;

            switch (order % parametersOnLine)
            {
                case 2:
                    return $"NewStateId{stateIndex}: Should be <OBJECT_KIND>_STATE_<ACTION>";
                case 3:
                    return $"AnimationId{stateIndex}: The Animation of {stateIndex}-th state";
                case 4:
                    return $"FlipX{stateIndex}: Flip the {stateIndex}-th state animation horizonal";
                case 0:
                    return $"FlipX{stateIndex}: Flip the {stateIndex}-th state animation vertical";
                case 1:
                    return $"TimesRotate90_{stateIndex}: How many times to rotate {stateIndex} 90 degrees Clockwise (after flipping)";
                default:
                    throw new Exception("Get Hint for ObjectAnimations failed.");
            }
        }

        public string GetSnippet()
        {
            string result = "";

            result += "OBJECT_ANIMATIONS\r\n"
                    + "\tObjectAnimationId\r\n"
                    + "\tNewStateId1\tAnimationId1\tFlipX1\tFlipY1\tTimesRotate90_1\r\n"
                    + "\tNewStateId2\tAnimationId2\tFlipX1\tFlipY2\tTimesRotate90_2\r\n" 
                    + "\t...";

            return result;
        }

        public string ParseData(List<string> codeWords)
        {
            if(codeWords.Count % parametersOnLine != 2)
                return $"Please follow this snippet: \n{GetSnippet()}";

            StringId = codeWords[1];

            for(int i=2; i<codeWords.Count; i+=parametersOnLine)
            {
                string stateId = codeWords[i];
                bool flipX, flipY;
                int timesRotate90;

                string animId = codeWords[i+1];
                Animation anim = AnimatingObjectsLib.Instance.Get(animId) as Animation;
                if(anim == null)
                {
                    return $"Animation Id {animId} is not found";
                }

                try
                { 
                    flipX = bool.Parse(codeWords[i+2]);
                    flipY = bool.Parse(codeWords[i+3]);
                }
                catch
                {
                    return $"FlipX and FlipY must be either {Boolean.TrueString} or {Boolean.FalseString}";
                }

                try
                {
                    timesRotate90 = int.Parse(codeWords[i+4]);
                }
                catch(Exception e)
                {
                    if (e is FormatException || e is OverflowException)
                        return $"Times_Rotate_90 must be a positive integer and not exceed 2^32";

                    return $"{Utils.UnknownErrorMsg}: {e.Message}";
                }

                StateAnimation state = new StateAnimation(stateId, anim, flipX, flipY, timesRotate90);
                States.Add(state);
                TotalDuration += state.TotalDuration;
            }

            return "";
        }

        public override void AddToLib()
        {
            base.AddToLib();

            foreach(var state in States)
                state.AddToLib();
        }

        public override Bitmap GetPreviewBitmap(int time = 0)
        {
            if (TotalDuration == 0)
                return base.GetPreviewBitmap(time);

            int modTime = time % TotalDuration;

            int sum = 0;
            for (int i = 0; i < States.Count; i++)
            {
                sum += States[i].TotalDuration;

                if (sum >= modTime)
                {
                    return States[i].GetPreviewBitmap(modTime);
                }
            }

            // It can be proven the code can not reach here
            return null;
        }
    }
}
