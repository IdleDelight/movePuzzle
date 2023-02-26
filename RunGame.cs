using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movePuzzle
{
	public class RunGame
	{
		private Level level = new();
		private List<Flag> flags = new();
		private Player? player;

		public void Start()
		{
			int levelWidth = 19;
			int levelHeight = 19;
			int flagAmount = 15;
			int flagCounter = 19;
			level = new Level(levelWidth, levelHeight);
			flags = new List<Flag>();
			player = new Player(level, flags);

			PlaceFlags(levelWidth, levelHeight,flagAmount, flagCounter);
			level.RenderLevel();
			player.UpdateGame();
		}

		private void PlaceFlags( int levelWidth, int levelHeight, int flagAmount, int flagCounter )
		{
			int maxW = levelWidth - 1;
			int maxH = levelHeight - 1;

			Random random = new Random();

			for (int i = 0; i < flagAmount; i++) {
				int ranW = random.Next(1, maxW);
				int ranH = random.Next(1, maxH);
				char flSym = '¤';
				int ranC = random.Next(2, flagCounter);

				flags.Add(new Flag(ranW, ranH, flSym, ranC));
			}

			foreach (var flag in flags) {
				level[flag.X, flag.Y] = flag.FlagSymbol;
			}
		}
	}
}
