using HarmonyLib;
using Verse;

namespace ColonistBarBelowDev {
	[StaticConstructorOnStartup]
	public static class Static {
		static Static() {
			Harmony h = new Harmony("Foxy.ColonistBarBelowDev");
			h.PatchAll();
			ModCompatibility.PatchAll(h);
		}
	}
}
