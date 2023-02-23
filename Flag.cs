using movePuzzle;


namespace movePuzzle 
{
	public class Flag
	{
		public int X { get; }
		public int Y { get; }
		public int Counter { get; set; }
		public bool IsCollected { get; set; }

		public Flag( int x, int y, int counter )
		{
			X = x;
			Y = y;
			Counter = counter;
			IsCollected = false;
		}

		public void Update( Level level, int playerX, int playerY )
		{
			if (!IsCollected) {
				if (playerX == X && playerY == Y) {
					IsCollected = true;
					level[X, Y] = '$';
				}
				else {
					if (Counter > 9) {
						level[X, Y] = '?';
					}
					else if (Counter >= 0) {
						level[X, Y] = (char)('0' + Counter);
					}
					else {
						level[X, Y] = 'X';
					}
				}
			}
		}
	}
}