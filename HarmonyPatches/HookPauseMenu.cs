using HarmonyLib;
using PauseSongInfo.UI;

namespace PauseSongInfo.HarmonyPatches {
	[HarmonyPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.Start))]
	static class HookPauseMenu {
		static void Postfix(PauseMenuManager __instance) {
			InfoUI.Create(__instance.transform.Find("Wrapper/MenuWrapper/Canvas").gameObject);
		}
	}

}
