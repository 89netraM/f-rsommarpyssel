using CliFx.Attributes;

namespace Försommarpyssel.Commands
{
	public abstract class BaseCommand
	{
		[CommandParameter(0, Description = "The crypto, without any spaces.")]
		public string Crypto { get; init; }

		[CommandParameter(1, Description = "The width of the transposition box.")]
		public int Width { get; init; }
	}
}
