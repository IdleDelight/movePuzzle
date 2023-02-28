using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace movePuzzle
{
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
