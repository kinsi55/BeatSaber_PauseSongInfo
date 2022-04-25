namespace PauseSongInfo.Util {
	static class BeatmapsUtil {
		public static string GetHashOfPreview(IPreviewBeatmapLevel preview) {
			if(preview.levelID.Length < 53)
				return null;

			if(preview.levelID[12] != '_') // custom_level_<hash, 40 chars>
				return null;

			return preview.levelID.Substring(13, 40);
		}
	}
}
