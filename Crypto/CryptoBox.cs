using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Försommarpyssel.Crypto
{
	class CryptoBox
	{
		public static (int width, int height, int blanks, int lastColumnWidth) CalculateDimensions(int length) =>
			CalculateDimensions(length, (int)Math.Ceiling(Math.Sqrt(length)));
		public static (int width, int height, int blanks, int lastColumnWidth) CalculateDimensions(int length, int width)
		{
			int height = (int)Math.Ceiling(length / (double)width);
			int blanks = width * height - length;
			return (width, height, blanks, width - blanks);
		}

		public string Crypto { get; }
		public int Width { get; }
		public int Height { get; }
		public int Blanks { get; }
		public int LastColumnWidth { get; }

		public CryptoBox(string crypto) =>
			(Crypto, (Width, Height, Blanks, LastColumnWidth)) = (crypto, CalculateDimensions(crypto.Length));
		public CryptoBox(string crypto, int width) =>
			(Crypto, (Width, Height, Blanks, LastColumnWidth)) = (crypto, CalculateDimensions(crypto.Length, width));

		public string Message() => Message(null);
		public string Message(IEnumerable<int> columnOrder)
		{
			columnOrder ??= Enumerable.Range(0, Width);
			IList<int> endColumns = columnOrder.Where((_, cNumber) => cNumber >= LastColumnWidth).ToList();
			StringBuilder sb = new StringBuilder(Crypto.Length);
			for (int r = 0; r < Height; r++)
			{
				int cNumber = 0;
				foreach (int c in columnOrder)
				{
					bool isBlank = r + 1 == Height && cNumber >= LastColumnWidth;
					int i = r + c * Height - endColumns.Count(endC => endC < c);
					if (!isBlank)
					{
						if (i < Crypto.Length)
						{
							sb.Append(Crypto[i]);
						}
					}
					cNumber++;
				}
			}
			return sb.ToString();
		}

		public string MessageBox() => MessageBox(null);
		public string MessageBox(IEnumerable<int> columnOrder) =>
			MessageBox(columnOrder, null);
		public string MessageBox(IEnumerable<int> columnOrder, int selectedColumn) =>
			MessageBox(columnOrder, (int?)selectedColumn);
		private string MessageBox(IEnumerable<int> columnOrder, int? selectedColumn)
		{
			IEnumerable<string> columnHeads = (columnOrder ?? Enumerable.Range(0, Width)).Select(static i => (i + 1).ToString());
			int gapWidth = columnHeads.Max(static n => n.Length);
			string gap = new String(' ', gapWidth);
			return String.Join(" ", columnHeads.Select((n, i) =>
					i == selectedColumn ?
						String.Format($"\x1B[93m{{0,{-gapWidth}}}\x1B[0m", n) :
						String.Format($"{{0,{-gapWidth}}}", n))
				) +
				"\n" +
				String.Join(
					"\n",
					Message(columnOrder)
						.Select(static (c, i) => (c, i))
						.GroupBy(p => p.i / Width)
						.Select(g => String.Join(gap, g.Select(static p => p.c)))
				);
		}
	}
}