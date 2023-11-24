﻿using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VanillaStorytellersExpanded
{
	public static class Patch_CompUseEffect_FinishRandomResearchProject
	{
		[HarmonyPatch(typeof(CompUseEffect_FinishRandomResearchProject), "DoEffect")]
		public static class DoEffect
		{
			public static void Postfix(CompUseEffect_FinishRandomResearchProject __instance, Pawn usedBy)
			{
				if (Find.ResearchManager.currentProj == null && CustomStorytellerUtility.AllowedResearchProjectDefs().All((ResearchProjectDef r) => r.IsFinished)
					&& CustomStorytellerUtility.TryGetRandomUnfinishedResearchProject(out ResearchProjectDef research))
				{
					NonPublicMethods.CompUseEffect_FinishRandomResearchProject_FinishInstantly(__instance, research, usedBy);
				}
			}
		}

		[HarmonyPatch(typeof(CompUseEffect_FinishRandomResearchProject), "CanBeUsedBy")]
		public static class CanBeUsedBy
		{
			public static void Postfix(CompUseEffect_FinishRandomResearchProject __instance, Pawn p, ref string failReason, ref bool __result)
			{
				if (Find.ResearchManager.currentProj == null && CustomStorytellerUtility.AllowedResearchProjectDefs().All((ResearchProjectDef r) => r.IsFinished)
					&& CustomStorytellerUtility.TryGetRandomUnfinishedResearchProject(out ResearchProjectDef _))
				{
					failReason = null;
					__result = true;
				}
			}
		}
	}
}
