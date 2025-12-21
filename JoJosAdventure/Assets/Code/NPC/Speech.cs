namespace JoJosAdventure.NPC
{
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

