using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;

namespace PauseSongInfo {
	[Plugin(RuntimeOptions.DynamicInit)]
	public class Plugin {
		internal static Plugin Instance;
		internal static IPALogger Log;
		internal static Harmony harmony;

		[Init]
		public Plugin(IPALogger logger, IPA.Config.Config conf) {
			Instance = this;
			Log = logger;
			Config.Instance = conf.Generated<Config>();
			harmony = new Harmony("Kinsi55.BeatSaber.PauseSongInfo");
		}

		[OnEnable]
		public void OnEnable() {
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}

		[OnDisable]
		public void OnDisable() {
			harmony.UnpatchSelf();
		}
	}
}
