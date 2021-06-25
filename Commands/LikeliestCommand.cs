using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Försommarpyssel.Crypto;
using Försommarpyssel.Crypto.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Försommarpyssel.Commands
{
	[Command("likeliest", Description = "Print all possible decrypted message in order of fitness to the language.")]
	public class LikeliestCommand : BaseCommand, ICommand
	{
		[CommandParameter(
			2,
			Description = "The crypto, without any spaces.",
			Converter = typeof(LanguageConverter)
		)]
		public Language Language { get; init; }

		public ValueTask ExecuteAsync(IConsole _)
		{
			CryptoBox box = new CryptoBox(Crypto, Width);
			var list = Utils.AllPermutation(Enumerable.Range(0, box.Width).ToList())
				.Select(co => (co, m: box.Message(co)))
				.OrderByDescending(p => Language.ScoreString(p.m));
			foreach (var p in list)
			{
				Console.WriteLine($"{String.Join(", ", p.co.Select(static i => i + 1))}\n\t{p.m}");
			}

			return default;
		}
	}
}
