
using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace PauseSongInfo {
	internal class Config {
		public static Config Instance;
		public virtual float MenuScale { get; set; } = 1f; // Must be 'virtual' if you want BSIPA to detect a value change and save the config automatically.
	}
}