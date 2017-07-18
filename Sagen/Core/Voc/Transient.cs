namespace Sagen.Core.Voc
{
	internal sealed class Transient
	{
		public int Position;
		public double TimeAlive;
		public double Lifetime;
		public double Strength;
		public double Exponent;
		public bool IsFree;
		public uint ID;
		public Transient Next;

		public Transient(uint id)
		{
			IsFree = true;
			ID = id;
			Position = 0;
			TimeAlive = 0;
			Strength = 0;
			Exponent = 0;
		}
	}
}
