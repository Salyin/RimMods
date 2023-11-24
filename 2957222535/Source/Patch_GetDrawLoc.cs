using HarmonyLib;
using RimWorld;
using UnityEngine;

namespace ColonistBarBelowDev {
	[HarmonyPatch(typeof(ColonistBarDrawLocsFinder), "GetDrawLoc")]
	public static class Patch_GetDrawLoc {
		public static void Postfix(ref Vector2 __result) {
			__result = new Vector2(__result.x, Helper.AdjustY(__result.y));
		}
	}
}
