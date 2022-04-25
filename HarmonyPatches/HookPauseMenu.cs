using HarmonyLib;
using PauseSongInfo.UI;

namespace PauseSongInfo.HarmonyPatches {
	//[HarmonyPatch]
	//static class HookPauseMenu {
	//	static Type funny = AccessTools.FirstInner(typeof(LevelBar), t => t.Name.StartsWith("<Setup"));

	//	static bool Prepare() => funny != null;

	//	static MethodBase TargetMethod() => funny.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance);

	//	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
	//		var match = new CodeMatcher(instructions);

	//		match.End().MatchBack(true,
	//			new CodeMatch(name: "levelbar"),
	//			new CodeMatch(OpCodes.Ldfld, name: "difflabel"),
	//			new CodeMatch(OpCodes.Ldarg_0),
	//			new CodeMatch(OpCodes.Ldfld, name: "diff"),
	//			new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(BeatmapDifficultyMethods), nameof(BeatmapDifficultyMethods.Name))),
	//			new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(TMP_Text), nameof(TMP_Text.text)))
	//		).End().Advance(-1).Insert(
	//			new CodeInstruction(match.NamedMatch("levelbar")),

	//			new CodeInstruction(match.NamedMatch("levelbar")),
	//			new CodeInstruction(match.NamedMatch("difflabel")),

	//			new CodeInstruction(OpCodes.Ldarg_0),
	//			new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(funny, "previewBeatmapLevel")),

	//			new CodeInstruction(OpCodes.Ldarg_0),
	//			new CodeInstruction(match.NamedMatch("diff")),

	//			new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(InfoUI), nameof(InfoUI.Prepare)))
	//		);

	//		return match.InstructionEnumeration();
	//	}
	//}

	[HarmonyPatch(typeof(PauseMenuManager), nameof(PauseMenuManager.Start))]
	static class HookPauseMenu {
		static void Postfix(PauseMenuManager __instance) {
			InfoUI.Create(__instance.transform.Find("Wrapper/MenuWrapper/Canvas").gameObject);
		}
	}

}
