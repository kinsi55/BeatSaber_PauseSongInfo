using System.Threading.Tasks;
using SongDetailsCache;

namespace PauseSongInfo.Util {
	static class SongDetailsUtil {
		public class AntiBox {
			public readonly SongDetails instance;

			public AntiBox(SongDetails instance) {
				this.instance = instance;
			}
		}

		public static bool finishedInitAttempt { get; private set; } = false;
		public static bool attemptedToInit { get; private set; } = false;

		static bool CheckAvailable() {
			return IPA.Loader.PluginManager.GetPluginFromId("SongDetailsCache") != null;
		}
		public static bool isAvailable => CheckAvailable();
		public static AntiBox songDetails = null;

		public static async Task<AntiBox> TryGet() {
			if(!finishedInitAttempt) {
				attemptedToInit = true;
				try {
					if(isAvailable)
						return songDetails = new AntiBox(await SongDetails.Init());
				} catch { } finally {
					finishedInitAttempt = true;
				}
			}
			return songDetails;
		}
	}
}
