namespace Sagen.Core.Nodes
{
	internal class SyllableNode : SpeechNode
	{
		public SyllableNode(double duration) : base(duration)
		{
		}

		public override void OnEnter(Synthesizer synth)
		{
			
		}

		public override void OnExit(Synthesizer synth)
		{
			synth.State.GlottisLevel = 1.0;
		}

		public override void OnUpdate(Synthesizer synth)
		{
			if (synth.State.LastGlottisLevel < 1.0)
			{
				double localTime = GetRelativeSeconds(synth);

				synth.State.GlottisLevel = 
					localTime >= synth.Voice.GlottisOpenTime 
					? 1.0 
					: Util.CosineInterpolate(synth.State.LastGlottisLevel, 1.0, localTime / synth.Voice.GlottisOpenTime);
			}
		}
	}
}