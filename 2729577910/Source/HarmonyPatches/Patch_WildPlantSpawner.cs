using HarmonyLib;
using RimWorld;
using Verse;

namespace Grow_Grass_Under_Roofs.HarmonyPatches
{
    [HarmonyPatch(typeof(WildPlantSpawner), "CanRegrowAt")]
    public class Patch_WildPlantSpawner
    {
        public static void Postfix(ref bool __result, ref Map ___map, WildPlantSpawner __instance, IntVec3 c)
        {
            __result = __result || c.GetTemperature(___map) > 0.0;
        }
    }
}