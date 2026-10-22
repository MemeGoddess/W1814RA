using UnityEngine;
using Verse;

namespace W1814_Remedial_Action
{
    public class W1814RA_Gizmo : Gizmo
    {
        private readonly CompShotCounter comp;
        public W1814RA_Gizmo(CompShotCounter comp)
        {
            this.comp = comp;
            Order = -10f;
        }

        public override float GetWidth(float maxWidth) => 120f;
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            W1814RA_ModExtension Props = comp.parent.def.GetModExtension<W1814RA_ModExtension>();
            if (Props == null) return new GizmoResult(GizmoState.Clear);

            string wearText = "W1814_RA_GizmoWear".Translate();
            string grimeText = "W1814_RA_GizmoGrime".Translate();

            Rect rect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Widgets.DrawWindowBackground(rect);
            Rect inner = rect.ContractedBy(6f);

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(new Rect(inner.x, inner.y + 00f, inner.width, 20f), wearText + $"{comp.weaponWear}/{Props.maxServiceLife}");
            Widgets.Label(new Rect(inner.x, inner.y + 20f, inner.width, 20f), grimeText + $"{comp.weaponGrime}/{Props.maxGrime}");
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            return new GizmoResult(GizmoState.Clear);
        }
    }
}