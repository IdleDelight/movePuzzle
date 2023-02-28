using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace movePuzzle
{
	public class Level
	{
		private int _width;
		private int _height;
		private char[,] _cells;

		public int Width {
			get { return _width; }
			set {
				_width = value;
				_cells = new char[_height, _width];
			}
		}

		public int Height {
			get { return _height; }
			set {
				_height = value;
				_cells = new char[_height, _width];
			}
		}

		public Level( int width = 7, int height = 7 )
		{
			_width = width;
			_height = height;
			_cells = new char[_height, _width];
			InitializeLevel();
		}

		public char this[int x, int y] {
			get { return _cells[y, x]; }
			set { _cells[y, x] = value; }
		}

		private void InitializeLevel()
		{
			for (int i = 0; i < _height; i++) {
				for (int j = 0; j < _width; j++) {
					if (i == 0 || i == _height - 1 || j == 0 || j == _width - 1) {
						_cells[i, j] = '#';
					}
					else {
						_cells[i, j] = '-';
					}
				}
			}
		}

		public void RenderLevel()
		{
			Console.Clear();

			for (int i = 0; i < _height; i++) {
				for (int j = 0; j < _width; j++) {
					if (_cells[i, j] == '-' || _cells[i, j] == '=' || _cells[i, j] == '?') {
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.Write(_cells[i, j]);
						Console.ResetColor();
					}
					else if (_cells[i, j] == '$') {
						Console.ForegroundColor = ConsoleColor.DarkGreen;
						Console.Write(_cells[i, j]);
						Console.ResetColor();
					}
					else if (_cells[i, j] == 'X') {
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.Write(_cells[i, j]);
						Console.ResetColor();
					}
					else {
						Console.Write(_cells[i, j]);
					}
				}
				Console.WriteLine();
			}

			//Console.SetCursorPosition(0, _level.Height);
			//Console.WriteLine("SCORE 00 | 00 MOVES");
			"SCORE ".WriteColored(ConsoleColor.DarkGray);
			Console.Write("00");
			" | ".WriteColored(ConsoleColor.DarkGray);
			Console.Write("00");
			" MOVES\n\n".WriteColored(ConsoleColor.DarkGray);
			Console.WriteLine("Use the arrow keys.");
			Console.WriteLine("Reach flags before");
			Console.WriteLine("they deplete.");
			Console.WriteLine();
			// Console.WriteLine("[ Press X to Exit ]");
			Console.Write("[ ");
			"Press".WriteColored(ConsoleColor.DarkGray);
			Console.Write(" X ");
			"to Exit".WriteColored(ConsoleColor.DarkGray);
			Console.Write(" ]");



		}

		public void SetLevelWidth( int width )
		{
			_width = width;
			_cells = new char[_height, _width];
			InitializeLevel();
		}

		public void SetLevelHeight( int height )
		{
			_height = height;
			_cells = new char[_height, _width];
			InitializeLevel();
		}
	}

	public static class ConsoleExtensions
	{
		public static void WriteColored( this string text, ConsoleColor color )
		{
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ResetColor();
		}
	}
}
