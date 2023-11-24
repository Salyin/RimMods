using Verse;

namespace ColonistBarBelowDev {
	public static class Helper {
		public static float AdjustY(float original) {
			if (!Prefs.DevMode) return original;
			return original - 21f + Settings.Offset;
		}
	}
}
