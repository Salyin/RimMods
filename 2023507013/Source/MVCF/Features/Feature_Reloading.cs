﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MVCF.Reloading;
using MVCF.Utilities;
using Reloading;
using RimWorld;
using Verse;
using Verse.AI;
using FloatMenuUtility = MVCF.Utilities.FloatMenuUtility;

namespace MVCF.Features;

public class Feature_Reloading : Feature_VerbComps
{
    private static readonly Type AttackStaticSubType = typeof(JobDriver_AttackStatic).GetNestedType("<>c__DisplayClass4_0", BindingFlags.NonPublic);
    private static readonly FieldInfo thisPropertyInfo = AttackStaticSubType.GetField("<>4__this", BindingFlags.Public | BindingFlags.Instance);
    public override string Name => "Reloading";

    public override IEnumerable<Patch> GetPatches()
    {
        foreach (var patch in base.GetPatches()) yield return patch;

        yield return Patch.Postfix(AccessTools.Method(typeof(FloatMenuMakerMap), "AddHumanlikeOrders"),
            AccessTools.Method(typeof(FloatMenuUtility), nameof(FloatMenuUtility.AddWeaponReloadOrders)));
        yield return Patch.Postfix(AccessTools.Method(typeof(PawnInventoryGenerator), "GenerateInventoryFor"),
            AccessTools.Method(GetType(), nameof(PostGenerate)));
        yield return Patch.Prefix(AccessTools.Method(typeof(JobGiver_AIFightEnemy), "TryGiveJob"), AccessTools.Method(GetType(), nameof(PreTryGiveJob)));
    }

    public static void PostGenerate(Pawn p, PawnGenerationRequest request)
    {
        foreach (var reloadable in p.AllReloadComps())
        {
            if (reloadable.Props.GenerateAmmo != null)
                foreach (var thingDefCountRange in reloadable.Props.GenerateAmmo)
                {
                    var ammo = ThingMaker.MakeThing(thingDefCountRange.thingDef);
                    ammo.stackCount = thingDefCountRange.countRange.RandomInRange;
                    p.inventory?.innerContainer.TryAdd(ammo);
                }

            if (reloadable.Props.GenerateBackupWeapon)
            {
                var weaponPairs = Traverse.Create(typeof(PawnWeaponGenerator)).Field("allWeaponPairs").GetValue<List<ThingStuffPair>>().Where(w =>
                    !w.thing.IsRangedWeapon || !p.WorkTagIsDisabled(WorkTags.Shooting));
                if (p.kindDef.weaponMoney.Span > 0f)
                {
                    var money = p.kindDef.weaponMoney.RandomInRange / 5f;
                    weaponPairs = weaponPairs.Where(w => w.Price <= money);
                }

                if (p.kindDef.weaponStuffOverride != null)
                    weaponPairs = weaponPairs.Where(w => w.stuff == p.kindDef.weaponStuffOverride);

                weaponPairs = weaponPairs.Where(w =>
                    w.thing.weaponClasses == null || (w.thing.weaponClasses.Contains(ReloadingDefOf.RangedLight) &&
                                                      w.thing.weaponClasses.Contains(ReloadingDefOf.ShortShots)) ||
                    w.thing.weaponTags.Contains("MedievalMeleeBasic") || w.thing.weaponTags.Contains("SimpleGun"));

                if (weaponPairs.TryRandomElementByWeight(w => w.Price * w.Commonality, out var weaponPair))
                {
                    var weapon = (ThingWithComps)ThingMaker.MakeThing(weaponPair.thing, weaponPair.stuff);
                    PawnGenerator.PostProcessGeneratedGear(weapon, p);
                    var num = request.BiocodeWeaponChance > 0f ? request.BiocodeWeaponChance : p.kindDef.biocodeWeaponChance;
                    if (Rand.Chance(num)) weapon.TryGetComp<CompBiocodable>()?.CodeFor(p);

                    if (p.kindDef.weaponStyleDef != null)
                        weapon.StyleDef = p.kindDef.weaponStyleDef;
                    else if (p.Ideo != null) weapon.StyleDef = p.Ideo.GetStyleFor(weapon.def);


                    p.inventory?.innerContainer.TryAdd(weapon, false);
                }
            }
        }
    }

    public static bool PreTryGiveJob(Pawn pawn, ref Job __result)
    {
        if (JobGiver_ReloadFromInventory.TryGiveReloadJob(pawn) is { } job)
        {
            __result = job;
            return false;
        }

        JobGiver_SwitchWeapon.TrySwitchWeapon(pawn);
        return true;
    }
}