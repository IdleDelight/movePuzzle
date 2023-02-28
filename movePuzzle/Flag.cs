using movePuzzle;


namespace movePuzzle 
{
	public class Flag
	{
		public int X { get; }
		public int Y { get; }
		public int Counter { get; set; }
		public char FlagSymbol { get; set; }
		public bool IsCollected { get; set; }
		public bool IsScored { get; set; }

		public Flag( int x, int y, char flagSymbol, int counter )
		{
			X = x;
			Y = y;
			Counter = counter;
			FlagSymbol = flagSymbol;
			IsCollected = false;
			IsScored = false;
		}
	}
}