using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ColonistBarBelowDev {
	public static class Patch_Mod_LTOColonyGroupsFinal {
		private static readonly FieldInfo fieldOriginal = AccessTools.Field("TacticalGroups.TacticalGroupsSettings:ColonistBarPositionY");
		private static readonly MethodInfo helper = AccessTools.Method(typeof(Patch_Mod_LTOColonyGroupsFinal), nameof(GetColonistBarPositionY));

		private static float GetColonistBarPositionY() {
			return Helper.AdjustY((float)fieldOriginal.GetValue(null));
		}

		public static void PatchAll(Harmony h) {
			Log.Message("[DevModeColonistBar] Colony Groups mod detected.");
			Patch_CalculateDrawLocs(h);
		}

		private static void Patch_CalculateDrawLocs(Harmony h) {
			MethodInfo method = AccessTools.Method("TacticalGroups.ColonistBarDrawLocsFinder:CalculateDrawLocs", new[] { typeof(List<Rect>), typeof(float), typeof(bool), typeof(int) });
			HarmonyMethod transpiler = new HarmonyMethod(AccessTools.Method(typeof(Patch_Mod_LTOColonyGroupsFinal), nameof(Transpiler)));
			h.Patch(method, transpiler: transpiler);
			Log.Message("[DevModeColonistBar] Patched TacticalGroups.ColonistBarDrawLocsFinder.CalculateDrawLocs");
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
			List<CodeInstruction> list = new List<CodeInstruction>(instructions);

			for (int i = 0; i < list.Count; i++) {
				if (list[i].LoadsField(fieldOriginal)) list[i] = new CodeInstruction(OpCodes.Call, helper);
			}

			return list;
		}
	}
}
