﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VFECore
{
    [DefOf]
    public static class VFEDefOf
    {
        public static JobDef VFEC_EquipShield;
        public static NeedDef VFE_Mechanoids_Power;
        public static JobDef VFE_Mechanoids_Recharge;
        public static StatDef VEF_VerbRangeFactor;
        public static StatCategoryDef VFE_EquippedStatFactors;
        public static StatDef VEF_VerbCooldownFactor;
        public static JobDef VFEC_LeaveMap;
        public static StatDef VEF_EnergyShieldEnergyMaxOffset;
        public static StatDef VEF_EnergyShieldEnergyMaxFactor;
        public static StatDef VEF_MeleeAttackSpeedFactor;
        public static StatDef VEF_RangeAttackSpeedFactor;
        public static StatDef VEF_MeleeAttackDamageFactor;
        public static StatDef VEF_RangeAttackDamageFactor;
        public static JoyKindDef Gaming_Cerebral;
        public static JobDef VEF_UseDoorTeleporter;
    }
}