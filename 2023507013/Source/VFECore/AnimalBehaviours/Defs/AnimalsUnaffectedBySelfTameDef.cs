﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace AnimalBehaviours
{
    public class AnimalsUnaffectedBySelfTameDef : Def
    {
        //A list of pawnkind defNames
        public List<PawnKindDef> unaffectedBySelfTamePawns;
    }
}