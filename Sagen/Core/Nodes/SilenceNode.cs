namespace Sagen.Core.Nodes
{
	internal class SilenceNode : SpeechNode
	{
		public SilenceNode(double duration) : base(duration)
		{
		}

		public override void OnEnter(Synthesizer synth)
		{
			
		}

		public override void OnExit(Synthesizer synth)
		{
			synth.State.GlottisLevel = 0.0;
		}

		public override void OnUpdate(Synthesizer synth)
		{
			double localTime = GetRelativeSeconds(synth);
			if (localTime >= synth.Voice.GlottisCloseTime)
			{
				synth.State.GlottisLevel = 0.0;
				return;
			}

			synth.State.GlottisLevel = (1.0 - localTime / synth.Voice.GlottisCloseTime) * synth.State.LastGlottisLevel;
		}
	}
}