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
		private bool displayMessage = false;

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

		public void SetDrawPlayer() 
		{
			Console.SetCursorPosition(_x, _y);
			Console.ForegroundColor = ConsoleColor.Cyan;
			//Console.BackgroundColor = ConsoleColor.Black;
			Console.Write('@');
			Console.ResetColor();
			Console.SetCursorPosition(_x, _y);
		}

		public void UpdateGame()
		{
			// Set the cursor and draw player
			SetDrawPlayer();

			while (true) {
				PlayerUpdate();
				FlagUpdate();
				LevelUpdate();

				if (EndGame()) {
					break;
				}
			}
		}

		private void PlayerUpdate()
{
    bool arrowKeyPressed = false;
    ConsoleKeyInfo key = Console.ReadKey(true);

    switch (key.Key) {
        case ConsoleKey.LeftArrow:
            if (_level[_y, _x - 1] != '║' && _level[_y, _x - 1] != '═' && _level[_y, _x - 1] != '╔' && _level[_y, _x - 1] != '╗' && _level[_y, _x - 1] != '╚' && _level[_y, _x - 1] != '╝' ) {
                _x--;
                arrowKeyPressed = true;
            }
            else {
                _x = _level.Width - 2;
                arrowKeyPressed = true;
            }
            break;
        case ConsoleKey.RightArrow:
            if (_level[_y, _x + 1] != '║' && _level[_y, _x + 1] != '═' && _level[_y, _x + 1] != '╔' && _level[_y, _x + 1] != '╗' && _level[_y, _x + 1] != '╚' && _level[_y, _x + 1] != '╝') {
					_x++;
                arrowKeyPressed = true;
            }
            else {
                _x = 1;
                arrowKeyPressed = true;
            }
            break;
        case ConsoleKey.UpArrow:
            if (_level[_y - 1, _x] != '║' && _level[_y - 1, _x] != '═' && _level[_y - 1, _x] != '╔' && _level[_y - 1, _x] != '╗' && _level[_y - 1, _x] != '╚' && _level[_y - 1, _x] != '╝') {
                _y--;
                arrowKeyPressed = true;
            }
            else {
                _y = _level.Height - 2;
                arrowKeyPressed = true;
            }
            break;
        case ConsoleKey.DownArrow:
            if (_level[_y + 1, _x] != '║' && _level[_y + 1, _x] != '═' && _level[_y + 1, _x] != '╔' && _level[_y + 1, _x] != '╗' && _level[_y + 1, _x] != '╚' && _level[_y + 1, _x] != '╝') {
                _y++;
                arrowKeyPressed = true;
            }
            else {
                _y = 1;
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
					IncreaseActiveFlagCounter();
				}

				// Update flag symbols
				if (flag.IsCollected) {
					flag.FlagSymbol = '$';
				}
				else if (flag.Counter < -3 && !flag.IsCollected) {
					flag.FlagSymbol = '×';
				}
				else if (flag.Counter < 0 && !flag.IsCollected) {
					flag.FlagSymbol = 'X';
				}
				else if (flag.Counter > 19 && !flag.IsCollected) {
					flag.FlagSymbol = '·';
				}
				else if (flag.Counter > 14 && !flag.IsCollected) {
					flag.FlagSymbol = '-';
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

		private void IncreaseActiveFlagCounter()
		{
			foreach (var aFlag in _flags) {
				if (aFlag.Counter >= 0 && !aFlag.IsScored) {
					aFlag.Counter++;
					aFlag.Counter++;
					displayMessage = true;
				}
			}
		}

		private void LevelUpdate()
		{
			if (displayMessage == true) {
				ScoringUiMessage();
				displayMessage = false;
			}
			else {
			_level.RenderLevel();
			}

			// Display the current score and moves
			Console.SetCursorPosition(0, _level.Height);
			Console.Write($"SCORE {score:00} | {moves:00} MOVES");

			//Console.WriteLine($"\n*** {_flags.Count} ***");
			//Console.WriteLine($"\n#P x:{_x}|{_y}:y");
			//int itr = 1;
			//foreach (var flag in _flags) {
			//	Console.Write($"\n#{itr} x:{flag.X}|{flag.Y}:y | {_level[flag.Y, flag.X]} | Symbol: {flag.FlagSymbol} | Counter: {flag.Counter} | C:{flag.IsCollected} | S:{flag.IsScored}");
			//	itr++;
			//}

			// Set the cursor and draw player
			SetDrawPlayer();
		}

		private void MessageUI(string theMessageIs)
		{
			Console.SetCursorPosition(0, _level.Height);
			//Console.WriteLine($"SCORE {score:00} | {moves:00} MOVES");

			"SCORE ".WriteColored(ConsoleColor.DarkGray);
			Console.Write($"{score:00}");
			" | ".WriteColored(ConsoleColor.DarkGray);
			Console.Write($"{moves:00}");
			" MOVES\n".WriteColored(ConsoleColor.DarkGray);

			Console.SetCursorPosition(0, _level.Height + 2);
			Console.WriteLine("┌─────────────────┐");
			Console.Write("│ ");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(theMessageIs);
			Console.ResetColor();
			Console.WriteLine(" │");
			Console.WriteLine("└─────────────────┘");
			Console.SetCursorPosition(0, _level.Height + 6);
			//Console.SetCursorPosition(0, _level.Height + 2);
			//Console.WriteLine("###################");
			//Console.WriteLine($"# {theMessageIs} #");
			//Console.WriteLine("###################");
			//Console.SetCursorPosition(0, _level.Height + 6);
		}

		private void ScoringUiMessage() 
		{
				_level.RenderLevel();
				MessageUI("FLAG COUNTERS++");
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
			int AmountOfFlags = _flags.Count;

			_level.RenderLevel();

			if (score > AmountOfFlags * 0.75) {
				return "R U CH3@T1NG!?!";
			}
			else if (score > AmountOfFlags * 0.5) {
				return "EXCELLENT WIN!!";
			}
			else if (score > AmountOfFlags * 0.25) {
				return "NOT TOO SHABBY!";
			}
			else if (score > 0) {
				return "GO GET EM BUDDY";
			}
			else {
				return " =[GAME OVER]= ";
			}
		}

		public bool PlayAgain()
		{
			Console.CursorVisible = false;

			MessageUI(EndGameMessage());
			
			//Console.WriteLine("[ PLAY AGAIN? Y/N ]");
			Console.Write("[ ");
			"PLAY AGAIN?".WriteColored(ConsoleColor.DarkGray);
			Console.WriteLine(" Y/N ]");

			while (true) {
				ConsoleKeyInfo key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.Y) {

					Console.SetCursorPosition(0, _level.Height + 6);

					"Restarting...     ".WriteColored(ConsoleColor.DarkGray);

					ProcessingAnim();

					Console.WriteLine();

					return true;
				}
				else if (key.Key == ConsoleKey.N) {
					ExitGame();
				}
				else {
					Console.SetCursorPosition(0, _level.Height + 6);
					//Console.WriteLine("[ PICK EITHER Y/N ]");
					Console.Write("[ ");
					"PICK EITHER".WriteColored(ConsoleColor.DarkGray);
					Console.WriteLine(" Y/N ]");
				}
			}
		}

		private void ExitGame() 
		{
			_level.RenderLevel();

			Console.CursorVisible = false;

			MessageUI("UNTIL NEXT TIME");

			Console.SetCursorPosition(0, _level.Height + 6);

			"Exiting...        ".WriteColored(ConsoleColor.DarkGray);

			ProcessingAnim();

			Console.WriteLine();

			Environment.Exit(0);
		}

		private void ProcessingAnim()
		{
			int waitTime = 2150; // 5 seconds
			int symbolIndex = 0;
			char[] symbols = { '|', '/', '─', '\\' };

			for (int i = 0; i < waitTime / 150; i++) {
				Console.Write(symbols[symbolIndex]);
				symbolIndex = (symbolIndex + 1) % symbols.Length;
				Thread.Sleep(150);
				Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
			}
		}
	}
}
