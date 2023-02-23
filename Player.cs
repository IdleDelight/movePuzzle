using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movePuzzle
{
	public class Player
	{
		private int _x;
		private int _y;
		private int _initialX;
		private int _initialY;
		private List<Flag> _flags;
		private Level _level;
		private char[,] _initialLevelState;
		private int _score;

		public Player( Level level, List<Flag> flags )
		{
			_x = level.Width / 2;
			_y = level.Height / 2;
			_initialX = _x;
			_initialY = _y;
			_flags = flags;
			_level = level;
			_initialLevelState = new char[_level.Height, _level.Width];
			_score = 0;

			// Save a copy of the initial level state
			for (int i = 0; i < _level.Height; i++) {
				for (int j = 0; j < _level.Width; j++) {
					_initialLevelState[i, j] = _level[i, j];
				}
			}
		}

		public void Update()
		{
			// Set the cursor size to 100%
			Console.CursorSize = 100;
			// Set the cursor position at the center of the level
			Console.SetCursorPosition(_x, _y);

			while (true) {
				ConsoleKeyInfo key = Console.ReadKey(true);

				bool arrowKeyPressed = false;

				switch (key.Key) {
					case ConsoleKey.LeftArrow:
					if (_x > 1 && _level[_y, _x - 1] != '#') {
						_x--;
						arrowKeyPressed = true;
					}
					break;
					case ConsoleKey.RightArrow:
					if (_x < _level.Width - 2 && _level[_y, _x + 1] != '#') {
						_x++;
						arrowKeyPressed = true;
					}
					break;
					case ConsoleKey.UpArrow:
					if (_y > 1 && _level[_y - 1, _x] != '#') {
						_y--;
						arrowKeyPressed = true;
					}
					break;
					case ConsoleKey.DownArrow:
					if (_y < _level.Height - 2 && _level[_y + 1, _x] != '#') {
						_y++;
						arrowKeyPressed = true;
					}
					break;
				}

				// Move the cursor to the new position
				Console.SetCursorPosition(_x, _y);

				// Update flag counters
				foreach (var flag in _flags) {
					flag.Update(_level, _x, _y);
				}

				// Update the level with the new flag and player positions
				UpdateLevel();

				// Decrement flag counters after updating the level
				if (arrowKeyPressed) {
					foreach (var flag in _flags) {
						flag.Update(_level, _x, _y);
						flag.Counter--;
					}
				}
			}
		}

		private void UpdateLevel()
		{
			// Restore the initial level state
			for (int i = 0; i < _level.Height; i++) {
				for (int j = 0; j < _level.Width; j++) {
					_level[i, j] = _initialLevelState[i, j];
				}
			}

			foreach (var flag in _flags) {
				char symbol = '@';

				if (flag.IsCollected) {
					symbol = '$';
					_score++;
				}
				else if (flag.Counter < 0) {
					symbol = 'X';
				}
				else if (flag.Counter > 9) {
					symbol = '?';
				}
				else {
					symbol = (char)('0' + flag.Counter);
				}

				_level[flag.Y, flag.X] = symbol;
			}

			// Draw the cursor at its current position
			_level.RenderLevel();

			Console.SetCursorPosition(_x, _y);

			if (_score == _flags.Count) {
				Console.WriteLine("WINNER");
				//Console.ReadKey();
				//Environment.Exit(0);
			}

			// Check if all flags are 'X' and score is 0
			if (_flags.All(flag => flag.Counter < 0) && _score == 0) {
				Console.WriteLine("GAME OVER");
			}
		}

		public void FlagPlacement()
		{
			foreach (var flag in _flags) {
				char symbol = '@';

				_level[flag.Y, flag.X] = symbol;
			}
		}

		public int Score( int score) 
		{ 
			return score += _score;
		}
	}
}
