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

        public Vector2 optionsScrollPosition;
        public float optionsViewRectHeight;

        internal static string VersionDir => Path.Combine(mod.Content.ModMetaData.RootDir.FullName, "Version.txt");
        public static string CurrentVersion { get; private set; }

        public OutlandTerrainMod(ModContentPack content) : base(content)
        {
            mod = this;
            settings = GetSettings<OutlandTerrainSettings>();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            if (Prefs.DevMode)
            {
                LogUtil.LogMessage($"{CurrentVersion} ::");
            }

            File.WriteAllText(VersionDir, CurrentVersion);
        }

        public override string SettingsCategory() => "Outland - Terrain";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            bool flag = optionsViewRectHeight > inRect.height;
            Rect viewRect = new Rect(inRect.x, inRect.y, inRect.width - (flag ? 26f : 0f), optionsViewRectHeight);
            Widgets.BeginScrollView(inRect, ref optionsScrollPosition, viewRect);
            Listing_Standard listing = new Listing_Standard();
            Rect rect = new Rect(viewRect.x, viewRect.y, viewRect.width, 999999f);
            listing.Begin(rect);
            // ============================ CONTENTS ================================
            DoOptionsCategoryContents(listing);
            // ======================================================================
            optionsViewRectHeight = listing.CurHeight;
            listing.End();
            Widgets.EndScrollView();
        }

        public void DoOptionsCategoryContents(Listing_Standard listing)
        {
            listing.Note("Changing settings requires a restart, the adjustments have to be performed during startup.", GameFont.Tiny);
            listing.GapLine();
            listing.CheckboxEnhanced("Grassy Terrain", "If enabled (default) this turns mossy terrain, soil and rich soil into grassy textures, some biomes of course don't look right with grass and so you can disable it with this. Comigo's Majestic Trees already does this function and both would need to be disabled for terrain to be not grassy.", ref settings.grassyTerrain);
            listing.CheckboxEnhanced("Static Water", "If enabled (default), this will load static textures for the water terrains, if disabled this leaves water to use the vanilla shaders and textures.", ref settings.waterShaders);
            listing.CheckboxEnhanced("Stone Recolours", "If enabled (default) this simply recolours the vanilla stones, if disabled it'll leave them as they are.", ref settings.stoneRecolours);
            listing.CheckboxEnhanced("Terrain Smears", "If enabled (default) this replaces the ugly terrain smears most visible on sand with a transparent texture, essentially removing them.", ref settings.terrainSmears);
            listing.GapLine();
            listing.Note("Looking for the extra terrains? Those are now a separate mod! You can find it by checking the Outland series collection at the top of the Workshop page for this mod.", GameFont.Tiny);
        }
    }
}
