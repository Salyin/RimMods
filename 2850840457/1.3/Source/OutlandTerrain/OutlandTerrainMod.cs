using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace OutlandTerrain
{
    public class OutlandTerrainMod : Mod
    {
        public static OutlandTerrainMod mod;
        public static OutlandTerrainSettings settings;

        public Vector2 scrollPosition;
        internal static string VersionDir => Path.Combine(ModLister.GetActiveModWithIdentifier("Neronix17.Outland.Terrain").RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public OutlandTerrainMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<OutlandTerrainSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            LogUtil.LogMessage($"{CurrentVersion} ::");

            File.WriteAllText(VersionDir, CurrentVersion);
        }

        public override string SettingsCategory() => "Outland - Terrain";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            float secondStageHeight;
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Note("Changing settings requires a restart, the adjustments have to be performed during startup.", GameFont.Tiny);
            listingStandard.GapLine();
            listingStandard.Gap(48);
            secondStageHeight = listingStandard.CurHeight;
            listingStandard.End();

            inRect.yMin = secondStageHeight;
            Rect outRect = inRect.ContractedBy(10f);

            float scrollRectHeight = 400f;
            Rect rect = new Rect(0f, 0f, outRect.width - (scrollRectHeight > outRect.height ? 18f : 0), scrollRectHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, rect, true);
            listingStandard = new Listing_Standard();
            listingStandard.Begin(rect);

            listingStandard.CheckboxEnhanced("Grassy Terrain", "If enabled (default) this turns mossy terrain, soil and rich soil into grassy textures, some biomes of course don't look right with grass and so you can disable it with this. Comigo's Majestic Trees already does this function and both would need to be disabled for terrain to be not grassy.", ref settings.grassyTerrain);
            listingStandard.GapLine();
            listingStandard.CheckboxEnhanced("Static Water", "If enabled (default), this will load static textures for the water terrains, if disabled this leaves water to use the vanilla shaders and textures.", ref settings.waterShaders);
            listingStandard.GapLine();
            listingStandard.CheckboxEnhanced("Stone Recolours", "If enabled (default) this simply recolours the vanilla stones, if disabled it'll leave them as they are.", ref settings.stoneRecolours);
            listingStandard.GapLine();

            listingStandard.CheckboxEnhanced("Additional Stone Floors", "If enabled (default) this'll add a bunch of new stone floors to be used.", ref settings.additionalStoneFloors);
            listingStandard.GapLine();
            listingStandard.CheckboxEnhanced("Additional Wood Floors", "If enabled (default) this'll add a bunch of new wood floors to be used.", ref settings.additionalWoodFloors);
            listingStandard.GapLine();

            listingStandard.End();
            Widgets.EndScrollView();

            base.DoSettingsWindowContents(inRect);
        }
    }
}
