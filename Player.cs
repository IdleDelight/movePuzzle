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
		private int moves = 0;
		private int score = 0;

		public Player( Level level, List<Flag> flags )
		{
			_x = level.Height / 2;
			_y = level.Width / 2;
			_initialX = _x;
			_initialY = _y;
			_flags = flags;
			_level = level;
			_initialLevelState = new char[_level.Height, _level.Width];

			// Save a copy of the initial level state
			for (int i = 0; i < _level.Height; i++) {
				for (int j = 0; j < _level.Width; j++) {
					_initialLevelState[i, j] = _level[i, j];
				}
			}
			//// Define cursor
			Console.CursorVisible = false;
		}

		public void UpdateGame()
		{
			// Set the cursor and draw player
			Console.SetCursorPosition(_x, _y);
			Console.Write('@');

			while (true) {
				PlayerUpdate();
				FlagUpdate();
				LevelUpdate();

				if (EndGame()) {
					PlayAgain();
					break;
				}
			}
		}

		private void PlayerUpdate()
		{
			// Move player & count moves
			bool arrowKeyPressed = false;
			ConsoleKeyInfo key = Console.ReadKey(true);

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
				case ConsoleKey.X:
				ExitGame();
				break;
			}
			if (arrowKeyPressed) {
				moves++;
			}

			foreach (var flag in _flags) {
				// Check if the player is on a flag and collect it
				if (flag.Y == _y && flag.X == _x && flag.Counter >= 0 && !flag.IsCollected) {
					flag.IsCollected = true;
				}
			}

			foreach (var flag in _flags) {
				if (arrowKeyPressed) {
					flag.Counter--;
				}
			}
		}

		private void FlagUpdate()
		{
			foreach (var flag in _flags) {
				//// Hand out point for valid flag collected
				if (flag.IsCollected && !flag.IsScored) {
					score++;
					flag.IsScored = true;
				}

				// Update flag symbols
				if (flag.IsCollected) {
					flag.FlagSymbol = '$';
				}
				else if (flag.Counter < 0 && !flag.IsCollected) {
					flag.FlagSymbol = 'X';
				}
				else if (flag.Counter > 9 && !flag.IsCollected) {
					flag.FlagSymbol = '?';
				}
				else {
					flag.FlagSymbol = (char)('0' + flag.Counter);
				}

				// Draw updated flag symbols in level
				_level[flag.X, flag.Y] = flag.FlagSymbol;
			}
		}

		private void LevelUpdate()
		{
			_level.RenderLevel();

			// Display the current score and moves
			Console.SetCursorPosition(0, _level.Height);
			Console.Write($"SCORE {score:00} | {moves:00} MOVES");

			//Console.WriteLine($"\n#P x:{_x}|{_y}:y");
			//int itr = 1;
			//foreach (var flag in _flags) {
			//	Console.Write($"\n#{itr} x:{flag.X}|{flag.Y}:y | {_level[flag.Y, flag.X]} | Symbol: {flag.FlagSymbol} | Counter: {flag.Counter} | C:{flag.IsCollected} | S:{flag.IsScored}");
			//	itr++;
			//}

			// Set the cursor and draw player
			Console.SetCursorPosition(_x, _y);
			Console.Write('@');
			Console.SetCursorPosition(_x, _y);
		}

		private void MessageUI(string theMessageIs)
		{
			Console.SetCursorPosition(0, _level.Height);
			Console.WriteLine($"SCORE {score:00} | {moves:00} MOVES");
			Console.SetCursorPosition(0, _level.Height + 2);
			Console.WriteLine("###################");
			Console.WriteLine($"# {theMessageIs} #");
			Console.WriteLine("###################");
			Console.SetCursorPosition(0, _level.Height + 6);
		}

		private bool EndGame()
		{
			foreach (var flag in _flags) {
				if (!flag.IsCollected && flag.Counter > -1) {
					return false;
				}
			}
			return true;
		}

		private string EndGameMessage() 
		{
			_level.RenderLevel();

			if (score > 7) {
				return "R U CH3@T1NG!?!";
			}
			else if (score > 5) {
				return "EXCELLENT WIN!!";
			}
			else if (score > 2) {
				return "NOT TOO SHABBY!";
			}
			else if (score > 0) {
				return  "WELL DONE CHAMP";
			}
			else {
				return " =[GAME OVER]= ";
			}
		}

		private bool PlayAgain()
		{
			_level.RenderLevel();

			MessageUI(EndGameMessage());

			Console.WriteLine("[ PLAY AGAIN? Y/N ]");

			ConsoleKeyInfo key = Console.ReadKey(true);
			if (key.Key == ConsoleKey.Y) {
				return true;
			}
			else if (key.Key == ConsoleKey.N) {
				ExitGame();
				return false;
			}
			else {
				return PlayAgain();
			}
		}

		private void ExitGame() 
		{
			_level.RenderLevel();

			MessageUI("UNTIL NEXT TIME");

			Console.SetCursorPosition(0, _level.Height + 5);

			Environment.Exit(0);
		}
	}
}
