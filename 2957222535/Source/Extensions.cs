using UnityEngine;
using Verse;

namespace ColonistBarBelowDev {
	public static class Extensions {
		public static bool IntField(this Listing listing, string label, ref int value, ref string buffer) {
			Rect rect = listing.GetRect(Text.LineHeight);
			float labelWidth = Text.CalcSize(label).x + 10;
			Rect labelRect = new Rect(rect.x, rect.y, labelWidth, rect.height);
			Rect valueRect = new Rect(rect.x + labelWidth, rect.y, 200f, rect.height);
			if (buffer == null) buffer = value.ToString();
			TextAnchor old = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(labelRect, label);
			int previousValue = value;
			Widgets.TextFieldNumeric(valueRect, ref value, ref buffer);
			Text.Anchor = old;
			listing.Gap(listing.verticalSpacing);
			return previousValue != value;
		}
	}
}
