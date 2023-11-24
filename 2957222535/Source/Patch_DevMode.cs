using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ColonistBarBelowDev {
	[HarmonyPatch(typeof(Prefs), nameof(Prefs.DevMode), MethodType.Setter)]
	internal class Patch_DevMode {
		private static readonly FieldInfo fieldData = AccessTools.Field(typeof(Prefs), "data");
		private static PrefsData Data => (PrefsData)fieldData.GetValue(null);

		public static void Prefix(out bool __state) {
			__state = Data.devMode;
		}
		public static void Postfix(bool __state) {
			if (__state != Data.devMode && Find.UIRoot is UIRoot_Play root) {
				root.mapUI.colonistBar.MarkColonistsDirty();
			}
		}
	}
}
