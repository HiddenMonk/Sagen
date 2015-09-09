namespace HAARP
{
    public class Voice
    {
        public float SentenceBetweenTime { get; set; }
        public float EllipsisPause { get; set; }
        public float ClausePauseTime { get; set; }
        public VoiceGender Gender { get; set; }
        public bool ShouldVerbalizeNumbers { get; set; } = true;
        public float Pitch { get; set; } = 1.0f;
    }

    public enum VoiceGender
    {
        Male,
        Female,
        Unknown
    }
}
