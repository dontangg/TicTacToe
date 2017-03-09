using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
	class Program
	{
		static void Main(string[] args)
		{
            // Create main game menu and get option.
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

        /// <summary>
        /// Reads the user input validating against a regex pattern.
        /// </summary>
        /// <param name="validPattern">Regex string for input validation.</param>
        /// <returns>Returns the valid input or null if not valid.</returns>
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

        /// <summary>
        /// Starts the game session.
        /// </summary>
		private static void PlayGame()
		{
            // Setting up the board and game before main loop.
			string numRowsChoice = null;
			while (numRowsChoice == null)
			{
				Console.Write("How many rows do you want to have? (3, 4, or 5) ");
				numRowsChoice = GetUserInput("[345]");
			}
			var boardSize = (int)Math.Pow(int.Parse(numRowsChoice), 2);
			var board = new string[boardSize];

			var turn = "X";

            // Main game loop.
			while (true)
			{
                // Refresh and clear everything.
				Console.Clear();

                // Determine if game is over.
				var winner = WhoWins(board);
				if (winner != null)
				{
					AnnounceResult(winner[0] + " WINS!!!", board);
					break;
				}
				if (IsBoardFull(board))
				{
					AnnounceResult("It's a tie!!!", board);
					break;
				}

                // User turn.
				Console.WriteLine("Place your " + turn + ":");

				DrawBoard(board);

				Console.WriteLine("\nUse the arrow keys and press <enter>");

				var xoLoc = GetXOLocation(board);
				board[xoLoc] = turn;

                // Flip the turn to the next player.
				turn = turn == "X" ? "O" : "X";
			}
		}

        /// <summary>
        /// Displays the result to the console.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="board">The board to draw.</param>
		private static void AnnounceResult(string message, string[] board)
		{
			Console.WriteLine();
			DrawBoard(board);

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine(message);
			Console.ResetColor();

			Console.Write("\nPress any key to continue...");
			Console.CursorVisible = false;
			Console.ReadKey();
			Console.CursorVisible = true;
		}

        /// <summary>
        /// Get the location where the user wants to place an 'X' or 'O'.
        /// The input is read from the user and the location on board is determined.
        /// </summary>
        /// <param name="board">Array of current board.</param>
        /// <returns>The index to place the next element.</returns>
		private static int GetXOLocation(string[] board)
		{
            // Calculate where the end of the rows and columns are.
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

            // Reading in navigation input and element placement.
			while (true)
			{
				Console.SetCursorPosition(curCol * 4 + 2, curRow * 4 + 3);
				var keyInfo = Console.ReadKey();
				Console.SetCursorPosition(curCol * 4 + 2, curRow * 4 + 3);
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
                        // The element has been placed.
						if (board[curRow * numRows + curCol] == null)
							return curRow * numRows + curCol;
						break;
				}
			}
		}

        /// <summary>
        /// Renders the board.
        /// </summary>
        /// <param name="board">the board data to render.</param>
		private static void DrawBoard(string[] board)
		{
			var numRows = (int)Math.Sqrt(board.Length);

			Console.WriteLine();

            // Build up the board by row.
			for (int row = 0; row < numRows; row++)
			{
				if (row != 0)
					Console.WriteLine(" " + string.Join("|", Enumerable.Repeat("---", numRows)));

				Console.Write(" " + string.Join("|", Enumerable.Repeat("   ", numRows)) + "\n ");

				for (int col = 0; col < numRows; col++)
				{
					if (col != 0)
						Console.Write("|");
					var space = board[row * numRows + col] ?? " ";
					if (space.Length > 1)
						Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write(" " + space[0] + " ");
					Console.ResetColor();
				}

				Console.WriteLine("\n " + string.Join("|", Enumerable.Repeat("   ", numRows)));
			}

			Console.WriteLine();
		}

        /// <summary>
        /// Determines if the board is full.
        /// </summary>
        /// <param name="board">The board data.</param>
        /// <returns>True if board is full, false otherwise.</returns>
		private static bool IsBoardFull(IEnumerable<string> board)
		{
			return board.All(space => space != null);
		}

        /// <summary>
        /// Calculates the winner of the game for a given board.
        /// </summary>
        /// <param name="board">The board data to check.</param>
        /// <returns>The element type of the winner.</returns>
		private static string WhoWins(string[] board)
		{
			var numRows = (int)Math.Sqrt(board.Length);
			
			// Check rows
			for (int row = 0; row < numRows; row++)
			{
				if (board[row * numRows] != null)
				{
					bool hasTicTacToe = true;
					for (int col = 1; col < numRows && hasTicTacToe; col++)
					{
						if (board[row * numRows + col] != board[row * numRows])
							hasTicTacToe = false;
					}
					if (hasTicTacToe)
					{
						// Put an indicator in the board to know which ones are part of the tic tac toe
						for (int col = 0; col < numRows; col++)
							board[row * numRows + col] += "!";
						return board[row * numRows];
					}
				}
			}

			// Check columns
			for (int col = 0; col < numRows; col++)
			{
				if (board[col] != null)
				{
					bool hasTicTacToe = true;
					for (int row = 1; row < numRows && hasTicTacToe; row++)
					{
						if (board[row * numRows + col] != board[col])
							hasTicTacToe = false;
					}
					if (hasTicTacToe)
					{
						// Put an indicator in the board to know which ones are part of the tic tac toe
						for (int row = 0; row < numRows; row++)
							board[row * numRows + col] += "!";
						return board[col];
					}
				}
			}

			// Check top left -> bottom right diagonal
			if (board[0] != null)
			{
				bool hasTicTacToe = true;
				for (int row = 1; row < numRows && hasTicTacToe; row++)
				{
					if (board[row * numRows + row] != board[0])
						hasTicTacToe = false;
				}
				if (hasTicTacToe)
				{
					// Put an indicator in the board to know which ones are part of the tic tac toe
					for (int row = 0; row < numRows; row++)
						board[row * numRows + row] += "!";
					return board[0];
				}
			}

			// Check top right -> bottom left diagonal
			if (board[numRows - 1] != null)
			{
				bool hasTicTacToe = true;
				for (int row = 1; row < numRows && hasTicTacToe; row++)
				{
					if (board[row * numRows + (numRows - 1 - row)] != board[numRows - 1])
						hasTicTacToe = false;
				}
				if (hasTicTacToe)
				{
					// Put an indicator in the board to know which ones are part of the tic tac toe
					for (int row = 0; row < numRows; row++)
						board[row * numRows + (numRows - 1 - row)] += "!";
					return board[numRows - 1];
				}
			}

			return null;
		}
	}
}
