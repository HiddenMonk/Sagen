namespace Sagen.Internals.Nodes
{
	internal class SyllableNode : SpeechNode
	{


		public SyllableNode(SpeechNode prev, double duration) : base(prev, duration)
		{
		}

		public override double OnEnter(Synthesizer synth)
		{
			throw new System.NotImplementedException();
		}

		public override double OnExit(Synthesizer synth)
		{
			throw new System.NotImplementedException();
		}

		public override double OnUpdate(Synthesizer synth)
		{
			throw new System.NotImplementedException();
		}
	}
}