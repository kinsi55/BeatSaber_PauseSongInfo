using HarmonyLib;

namespace PauseSongInfo.HarmonyPatches {
	[HarmonyPatch]
	static class HookLeveldata {
		public static IDifficultyBeatmap difficultyBeatmap;

		[HarmonyPriority(int.MinValue)]
		[HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Init))]
		[HarmonyPatch(typeof(MissionLevelScenesTransitionSetupDataSO), nameof(MissionLevelScenesTransitionSetupDataSO.Init))]
		[HarmonyPatch(typeof(MultiplayerLevelScenesTransitionSetupDataSO), nameof(MultiplayerLevelScenesTransitionSetupDataSO.Init))]
		static void Postfix(IDifficultyBeatmap difficultyBeatmap) {
			HookLeveldata.difficultyBeatmap = difficultyBeatmap;
		}
	}
}
