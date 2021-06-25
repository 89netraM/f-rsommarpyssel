using CliFx;
using System.Reflection;
using System.Threading.Tasks;

namespace Försommarpyssel
{
	public static class Program
	{
		public static async Task<int> Main() =>
			await new CliApplicationBuilder()
				.SetExecutableName(Assembly.GetExecutingAssembly().GetName().Name)
				.AddCommandsFromThisAssembly()
				.Build()
				.RunAsync();
	}
}
