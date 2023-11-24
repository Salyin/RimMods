using Verse;

namespace ColonistBarBelowDev {
	public class Settings : ModSettings {
		private static Settings instance;
		public static Settings Instance {
			get {
				if (instance == null) instance = LoadedModManager.GetMod<ModMain>().Settings;
				return instance;
			}
		}

		public static float Offset => Instance.offset;

		public float offset = 36f;

		public override void ExposeData() {
			Scribe_Values.Look(ref offset, "offset");
		}
	}
}
