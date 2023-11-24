﻿
using Verse;

namespace AnimalBehaviours
{
    public class CompProperties_GraphicByStyle : CompProperties
    {


        public CompProperties_GraphicByStyle()
        {
            this.compClass = typeof(CompGraphicByStyle);
        }

        public StyleCategoryDef style;
        public string newImagePath;
        public string maskPath = null;
        public bool changeDesiccatedGraphic = false;
        public string dessicatedTxt;
        public int changeGraphicsInterval = 2000;

    }
}