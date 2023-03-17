using System.Collections.Generic;

namespace Assets.Code.Logic
{
    public static class SpeechRepository
    {
        // returns a list of what to output with timings
        public static List<Speech> GetMattFollowJoJoSpeech()
        {
            return new List<Speech>()
            {
                new Speech("There you are JoJo!", 1.5f),
                new Speech("I have been looking for you, I need your help.", 2f),
                new Speech(string.Empty, 2f),//pans to paige during
                new Speech("Paige is trapped in the bedroom, we need to free her!", 2.5f)
            };
        }

        public static List<Speech> GetExitLoungeSpeechNoneCollected()
        {
            return new List<Speech>()
            {
                new Speech("Damn looks like the bedroom door is locked.", 2f),
                new Speech("We need to kill the demons to unlock it!", 2f)
            };
        }

        //public static List<Speech> GetNearDoorSpeechOneMore()
        //{
        //    return new List<Speech>()
        //    {
        //        new Speech("Damn the door is locked." + Environment.NewLine + "We need to kill more of the demons to unlock it!", 3)
        //    };
        //}

        public static List<Speech> GetEnterBedroomSpeech()
        {
            return new List<Speech>()
            {
                new Speech("Paige is in that Jar, quick try to break it JoJo!", 3)
            };
        }

        public static List<Speech> GetJoJoBreakJarFailedSpeech()
        {
            return new List<Speech>()
            {
                new Speech("Huh, it wouldn't break?", 2.5f),
                new Speech("I've got something that might do the trick!", 2.5f)
            };
        }

        public static List<Speech> GetDemonDied()
        {
            return new List<Speech>()
            {
                new Speech("Woah! Didn't think you'd actually do it. One left!", 2.5f)
            };
        }

        public static List<Speech> GetNoMoreDemons()
        {
            return new List<Speech>()
            {
                new Speech("Good girl! The door is open. Time to save Paige!", 2)
            };
        }

        public static List<Speech> SheSaidYesFuckYeah()
        {
            return new List<Speech>()
            {
                new Speech("I Love you like nothing else, I can't wait to spend my life with you!!!!!!!!", 1000)
            };
        }

        public static List<Speech> PaigeFreedom()
        {
            return new List<Speech>()
            {
                new Speech("Finaaalllllly!", 3)
            };
        }
    }

    public class Speech
    {
        public Speech(string speechText, float speechTimeSeconds)
        {
            this.SpeechText = speechText;
            this.SpeechTimeSeconds = speechTimeSeconds;
        }

        public string SpeechText { get; set; }
        public float SpeechTimeSeconds { get; set; }
    }
}