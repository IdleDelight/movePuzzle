using System;
using System.Collections.Generic;

namespace movePuzzle
{
	internal class Program
	{
		static void Main( string[] args )
		{
			Level level = new Level(23, 23);

			List<Flag> flags = new List<Flag>();
			flags.Add(new Flag(5, 5, 10));
			flags.Add(new Flag(9, 9, 5));
			flags.Add(new Flag(15, 15, 2));

			Player player = new Player(level, flags);

			player.FlagPlacement();

			Console.SetCursorPosition(level.Width / 2, level.Height / 2);
			level.RenderLevel();

			player.Update();
		}
	}
}
