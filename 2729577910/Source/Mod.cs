using HarmonyLib;
using Verse;

namespace Grow_Grass_Under_Roofs
{
    public class Mod: Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            new Harmony("Garethp.rimworld.GrowGrassUnderRoofs.main").PatchAll();
        }
    }
}