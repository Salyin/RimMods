﻿using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LayeredDestruction
{

    public class CompProperties_LayeredDestruction : CompProperties
    {
        public ThingDef NextLayerDef;
        public ThingDef NextLayerDef_Alternative;
        public ThingDef ParentLayerDef;
        public int RestoreToParentWorkAmount = 600;
        public float InstantFullDestructionChance = 0.25f;
        public float alternativeDefChance = 0.50f;
        public float doubleDowngradeChance = 0.25f;
        public CompProperties_LayeredDestruction()
        {
            compClass = typeof(DestructibleBuilding);
        }
    }

    public class DestructibleBuilding : ThingComp
    {
        public int fleckCount = 3;

        public void ThrowFlecks(Map map, Vector3 loc, FleckDef fleckDef)
        {
            
            for (int i = 0; i < fleckCount; i++)
            {
                FleckCreationData creationData = FleckMaker.GetDataStatic(loc, map, fleckDef);
                creationData.rotation = Rand.Range(0, 360);
                creationData.rotationRate = Rand.Range(1f, 4f);
                creationData.airTimeLeft = Rand.Range(0.5f, 1f);
                creationData.velocityAngle = Rand.Range(0, 360);
                creationData.velocitySpeed = Rand.Range(1.5f, 4f);
                creationData.scale = Rand.Range(0.5f, 1f);
                creationData.spawnPosition = loc;
                map.flecks.CreateFleck(creationData);
            }
        }

        public void MakeFilth(Map map, IntVec3 position, Vector3 center)
        {//creates filth on adjacent 8 tiles based on wall material
            var stuff = parent.Stuff;
            if (stuff != null)
            {
                var stuffcategory = stuff.stuffProps.categories;

                ThingDef stuffFilthDef = null;
                FleckDef stuffFleckDef = null;
                if (stuffcategory.Contains(StuffCategoryDefOf.Woody))
                {
                    stuffFilthDef = LayeredDestructionDefOfs.Filth_LWD_Planks;
                    stuffFleckDef = LayeredDestructionDefOfs.Fleck_LWD_Planks;
                }
                else if (stuffcategory.Contains(StuffCategoryDefOf.Stony))
                {
                    stuffFilthDef = LayeredDestructionDefOfs.Filth_LWD_Bricks;
                    stuffFleckDef = LayeredDestructionDefOfs.Fleck_LWD_Brick;
                }
                else if (stuffcategory.Contains(StuffCategoryDefOf.Metallic))
                {
                    stuffFilthDef = LayeredDestructionDefOfs.Filth_LWD_Smooth;
                    stuffFleckDef = LayeredDestructionDefOfs.Fleck_LWD_Smooth;
                }

                if(stuffFleckDef != null)
                {
                    ThrowFlecks(map, center, stuffFleckDef);
                }

                List<IntVec3> list = GenAdj.AdjacentCells8WayRandomized();
                for (int i = 0; i < 8; i++)
                {
                    IntVec3 cell = position + list[i];
                    if (cell.InBounds(map) && Rand.Chance(0.40f) && stuffFilthDef != null)
                    {
                        FilthMaker.TryMakeFilth(cell, map, stuffFilthDef);
                    }
                }
            }
        }

        public override void PostDestroy(DestroyMode mode, Map map)
        {
            if (mode == DestroyMode.KillFinalize)
            {//Only apply if wall is shot/burned/meleed/exploded/self-destructs
                var props = this.parent.def.GetCompProperties<CompProperties_LayeredDestruction>();

                if (!Rand.Chance(props.InstantFullDestructionChance))
                {//random chance that this whole mechanism doesn't apply per defs
                    if (props.NextLayerDef == null && props.ParentLayerDef != null && Find.PlaySettings.autoRebuild && parent.Faction == Faction.OfPlayer && props.ParentLayerDef.blueprintDef != null && props.ParentLayerDef.IsResearchFinished && map.areaManager.Home[parent.Position] && GenConstruct.CanPlaceBlueprintAt(props.ParentLayerDef, parent.Position, parent.Rotation, map))
                    {//when rubble is destroyed, place blueprints for parent when AutoRebuild is on
                        GenConstruct.PlaceBlueprintForBuild(props.ParentLayerDef, parent.Position, map, parent.Rotation, Faction.OfPlayer, parent.Stuff, parent.StyleSourcePrecept, parent.StyleDef, true);
                    }
                    if (props.NextLayerDef != null)
                    {// if this is not the last stage of wall
                        ThingDef nextStage = props.NextLayerDef;
                        if (props.NextLayerDef_Alternative != null && Rand.Chance(props.alternativeDefChance))
                        {//chance to put alt stage
                            nextStage = props.NextLayerDef_Alternative;
                            if (Rand.Chance(props.doubleDowngradeChance) && nextStage.GetCompProperties<CompProperties_LayeredDestruction>().NextLayerDef != null)
                            {
                                nextStage = nextStage.GetCompProperties<CompProperties_LayeredDestruction>().NextLayerDef;
                            }
                        }
                        Thing WallLayer;
                        //inherit properties of parent wall
                        if (parent.Stuff != null)
                        {
                            WallLayer = ThingMaker.MakeThing(nextStage, this.parent.Stuff);
                        }
                        else { WallLayer = ThingMaker.MakeThing(nextStage); }
                        //if it's not player's wall, then neutral to not have issues with ownership
                        WallLayer.SetFaction(this.parent.Faction == Faction.OfPlayer ? Faction.OfPlayer : null);
                        //create trash according to material
                        MakeFilth(map, this.parent.Position, this.parent.TrueCenter());
                        GenPlace.TryPlaceThing(WallLayer, this.parent.Position, map, ThingPlaceMode.Direct);
                        if (WallLayer.Faction == Faction.OfPlayer)
                        {//add designation for pawns to repair it
                            map.designationManager.AddDesignation(new Designation(WallLayer, RestoreDesignationDefOf.RestoreWall, null));
                        }
                    }
                }

            }
            base.PostDestroy(mode, map);
        }
    }
}
