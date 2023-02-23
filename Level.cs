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

		public char this[int x, int y] {
			get { return _cells[y, x]; }
			set { _cells[y, x] = value; }
		}

		public void RenderLevel()
		{
			Console.Clear(); // clear the console

			int score = 0;
			Console.Write($"Score: /*???*/ \n");

			for (int i = 0; i < _height; i++) {
				for (int j = 0; j < _width; j++) {
					Console.Write(_cells[i, j]);
				}
				Console.WriteLine();
			}
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
}
