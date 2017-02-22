namespace Sagen.Core.Nodes
{
	internal class PhonemeNode : SpeechNode
	{
		private readonly double _h;
		private readonly double _b;
		private readonly double _r;

		public PhonemeNode(double duration, double h, double b, double r) : base(duration)
		{
			_h = h;
			_b = b;
			_r = r;
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
			synth.State.Height = _h;
			synth.State.Backness = _b;
			synth.State.Roundedness = _r;
			synth.Pitch = Util.Lerp(0, -0.1, GetNormalizedRelativePos(synth));
		}
	}
}