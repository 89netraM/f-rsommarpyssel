using System;
using System.Collections.Generic;
using System.Linq;
using CliFx.Extensibility;

namespace Försommarpyssel.Crypto.Languages
{
	public static class Languages
	{
		private static readonly IReadOnlyDictionary<string, Language> languages = new Dictionary<string, Language>
		{
			["SWE"] = new Swedish(),
		};

		public static IEnumerable<string> LanguageCodes() =>
			languages.Keys;

		public static bool CanUseLanguage(string code) =>
			languages.ContainsKey(code.ToUpper());

		public static bool TryGetLanguage(string code, out Language language) =>
			languages.TryGetValue(code.ToUpper(), out language);
	}

	public abstract class Language
	{
		protected abstract IDictionary<char, double> MonoGramStatistic { get; }
		protected abstract double MonoGramMissingStatistic { get; }
		protected abstract IDictionary<(char, char), double> BiGramStatistic { get; }
		protected abstract double BiGramMissingStatistic { get; }

		public double MonoGram(char l) =>
			MonoGramStatistic.TryGetValue(l, out double percentage) ?
				percentage :
				MonoGramMissingStatistic;
		public double BiGram((char, char) letters) =>
			BiGramStatistic.TryGetValue(letters, out double percentage) ?
				percentage :
				BiGramMissingStatistic;

		public double ScoreString(string s) =>
			s.Zip(s.Skip(1)).Sum(BiGram);
	}


	public class LanguageConverter : BindingConverter<Language>
	{
		public override Language Convert(string code) =>
			Languages.TryGetLanguage(code, out Language language) ?
				language :
				throw new Exception(
					$"Should be a three letter language code. Available options are: {String.Join(", ", Languages.LanguageCodes())}."
				);
	}
}
