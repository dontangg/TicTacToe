using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe
{
	class Program
	{
		static void Main(string[] args)
		{
			var stillPlaying = true;

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("-----------------------");
			Console.WriteLine("Welcome to Tic Tac Toe!");
			Console.WriteLine("-----------------------\n");
			Console.ResetColor();

			while (stillPlaying)
			{
				Console.WriteLine("What would you like to do:");
				Console.WriteLine("1. Start a new game");
				Console.WriteLine("2. Quit\n");

				Console.Write("Type a number and hit <enter>: ");

				var choice = GetUserInput("[12]");

				switch(choice)
				{
					case "1":
						PlayGame();
						Console.Clear();
						break;
					case "2":
						stillPlaying = false;
						break;
				}
			}
		}

		private static string GetUserInput(string validPattern = null)
		{
			var input = Console.ReadLine();
			input = input.Trim();

			if (validPattern != null && !System.Text.RegularExpressions.Regex.IsMatch(input, validPattern))
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("\"" + input + "\" is not valid.\n");
				Console.ResetColor();
				return null;
			}

			return input;
		}

		private static void PlayGame()
		{
			string numRowsChoice = null;
			while (numRowsChoice == null)
			{
				Console.Write("How many rows do you want to have? (3, 4, or 5) ");
				numRowsChoice = GetUserInput("[345]");
			}
			var boardSize = (int)Math.Pow(int.Parse(numRowsChoice), 2);
			var board = new string[boardSize];

			var turn = "X";
			while (true)
			{
				Console.Clear();

				DrawBoard(board);

				if (IsBoardFull(board))
				{
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.WriteLine("\nIt's a tie!!!");
					Console.ResetColor();

					Console.Write("\nPress any key to continue...");
					Console.CursorVisible = false;
					Console.ReadKey();
					Console.CursorVisible = true;
					break;
				}

				Console.WriteLine("\nUse the arrow keys and press enter to place your " + turn + ".");

				var xoLoc = GetXOLocation(board);
				board[xoLoc] = turn;

				turn = turn == "X" ? "O" : "X";
			}

			// TODO:
			// * create a method that will take the string array as a parameter and return a string
			//    indicating who wins (null = no win, "X" = x wins, "O" = o wins). Use IsBoardFull() as an example
			// * announce the result of the game
		}

		private static int GetXOLocation(string[] board)
		{
			int numRows = (int)Math.Sqrt(board.Length);

			int curRow = 0, curCol = 0;
			
			for (int i = 0; i < board.Length; i++)
			{
				if (board[i] == null)
				{
					curRow = i / numRows;
					curCol = i % numRows;
					break;
				}
			}

			while (true)
			{
				Console.SetCursorPosition(curCol * 4 + 1, curRow * 4 + 1);
				var keyInfo = Console.ReadKey();
				Console.SetCursorPosition(curCol * 4 + 1, curRow * 4 + 1);
				Console.Write(board[curRow * numRows + curCol] ?? " ");

				switch (keyInfo.Key)
				{
					case ConsoleKey.LeftArrow:
						if (curCol > 0)
							curCol--;
						break;
					case ConsoleKey.RightArrow:
						if (curCol + 1 < numRows)
							curCol++;
						break;
					case ConsoleKey.UpArrow:
						if (curRow > 0)
							curRow--;
						break;
					case ConsoleKey.DownArrow:
						if (curRow + 1 < numRows)
							curRow++;
						break;
					case ConsoleKey.Spacebar:
					case ConsoleKey.Enter:
						if (board[curRow * numRows + curCol] == null)
							return curRow * numRows + curCol;
						break;
				}
			}
		}

		private static void DrawBoard(string[] board)
		{
			var numRows = (int)Math.Sqrt(board.Length);

			for (int row = 0; row < numRows; row++)
			{
				if (row != 0)
					Console.WriteLine(string.Join("|", Enumerable.Repeat("---", numRows)));

				Console.WriteLine(string.Join("|", Enumerable.Repeat("   ", numRows)));

				for (int col = 0; col < numRows; col++)
				{
					if (col != 0)
						Console.Write("|");

					Console.Write(" " + (board[row * numRows + col] ?? " ") + " ");
				}

				Console.WriteLine("\n" + string.Join("|", Enumerable.Repeat("   ", numRows)));
			}
		}

		private static bool IsBoardFull(string[] board)
		{
			return board.All(space => space != null);
		}
	}
}
