using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sagen.Core.Voc;

namespace Sagen.Core.Layers
{
	class VocLayer : Layer
	{
		private readonly VocSynth _voc;

		public VocLayer(Synthesizer synthesizer) : base(synthesizer)
		{
			_voc = new VocSynth(0, synthesizer.SampleRate);
			_voc.SetTongueShape(10.0, 0.6);
			_voc.Fundamental = 100;
			
		}

		public override void Update(ref double sample)
		{
			_voc.Compute(ref sample);
		}
	}
}
