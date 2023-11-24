using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace OutlandTerrain
{
    public class OutlandTerrainSettings : ModSettings
    {
        public bool grassyTerrain = true;
        public bool waterShaders = true;
        public bool stoneRecolours = true;
        public bool terrainSmears = true;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref grassyTerrain, "grassySoil", true);
            Scribe_Values.Look(ref waterShaders, "waterShaders", true);
            Scribe_Values.Look(ref stoneRecolours, "stoneRecolours", true);
            Scribe_Values.Look(ref terrainSmears, "terrainSmears", true);
        }

        public bool IsValidSetting(string input)
        {
            if (GetType().GetFields().Where(p => p.FieldType == typeof(bool)).Any(i => i.Name == input))
            {
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetEnabledSettings
        {
            get
            {
                return GetType().GetFields().Where(p => p.FieldType == typeof(bool) && (bool)p.GetValue(this)).Select(p => p.Name);
            }
        }
    }
}
