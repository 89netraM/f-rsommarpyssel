using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Försommarpyssel.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Försommarpyssel.Commands
{
	[Command("interactive", Description = "Play with the transposition cipher by moving the columns around.")]
	public class InteractiveCommand : BaseCommand, ICommand
	{
		public ValueTask ExecuteAsync(IConsole _)
		{
			Console.TreatControlCAsInput = true;
			Console.CursorVisible = false;

			CryptoBox box = new CryptoBox(Crypto, Width);
			List<int> columnOrder = Enumerable.Range(0, box.Width).ToList();
			int selectedColumn = 0;
			while (true)
			{
				Console.Write(box.MessageBox(columnOrder, selectedColumn));

				while (true)
				{
					ConsoleKeyInfo key = Console.ReadKey(true);
					if (key.Key == ConsoleKey.LeftArrow && selectedColumn > 0)
					{
						if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
						{
							(columnOrder[selectedColumn - 1], columnOrder[selectedColumn]) = (columnOrder[selectedColumn], columnOrder[selectedColumn - 1]);
						}
						selectedColumn--;
						break;
					}
					else if (key.Key == ConsoleKey.RightArrow && selectedColumn + 1 < box.Width)
					{
						if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
						{
							(columnOrder[selectedColumn + 1], columnOrder[selectedColumn]) = (columnOrder[selectedColumn], columnOrder[selectedColumn + 1]);
						}
						selectedColumn++;
						break;
					}
					else if (key.Key == ConsoleKey.Home && selectedColumn != 0)
					{
						if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
						{
							int item = columnOrder[selectedColumn];
							columnOrder.RemoveAt(selectedColumn);
							columnOrder.Insert(0, item);
						}
						selectedColumn = 0;
						break;
					}
					else if (key.Key == ConsoleKey.End && selectedColumn != box.Width - 1)
					{
						if (key.Modifiers.HasFlag(ConsoleModifiers.Control))
						{
							int item = columnOrder[selectedColumn];
							columnOrder.RemoveAt(selectedColumn);
							columnOrder.Add(item);
						}
						selectedColumn = box.Width - 1;
						break;
					}
					else if (key.Key == ConsoleKey.R)
					{
						columnOrder = Enumerable.Range(0, box.Width).ToList();
						selectedColumn = 0;
						break;
					}
					else if ((key.Key == ConsoleKey.C && key.Modifiers.HasFlag(ConsoleModifiers.Control)) || key.Key == ConsoleKey.Escape)
					{
						Console.CursorVisible = true;
						return default;
					}
				}

				Console.SetCursorPosition(0, Console.CursorTop - box.Height);
			}
		}
	}
}
