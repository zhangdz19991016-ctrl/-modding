using System;
using Shapes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020000CB RID: 203
[ExecuteAlways]
public class ShapesSkillLine : MonoBehaviour
{
	// Token: 0x06000661 RID: 1633 RVA: 0x0001CD97 File Offset: 0x0001AF97
	private void Awake()
	{
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0001CD9C File Offset: 0x0001AF9C
	public void DrawLine()
	{
		if (!this.cam)
		{
			if (LevelManager.Instance)
			{
				this.cam = LevelManager.Instance.GameCamera.renderCamera;
			}
			if (!this.cam)
			{
				return;
			}
		}
		if (this.points.Length == 0)
		{
			return;
		}
		using (Draw.Command(this.cam, RenderPassEvent.BeforeRenderingPostProcessing))
		{
			Draw.LineGeometry = LineGeometry.Billboard;
			Draw.BlendMode = this.blendMode;
			Draw.ThicknessSpace = ThicknessSpace.Meters;
			Draw.Thickness = this.lineThickness;
			Draw.ZTest = CompareFunction.Always;
			if (!this.worldSpace)
			{
				Draw.Matrix = base.transform.localToWorldMatrix;
			}
			for (int i = 0; i < this.points.Length - 1; i++)
			{
				Draw.Sphere(this.points[i], this.dotRadius, this.colors[i]);
				Draw.Line(this.points[i], this.points[i + 1], this.colors[i]);
			}
			Draw.Sphere(this.points[this.points.Length - 1], this.dotRadius, this.colors[this.colors.Length - 1]);
			if (this.hitObsticle)
			{
				Draw.Sphere(this.hitPoint, this.dotRadius, this.colors[0]);
			}
		}
	}

	// Token: 0x04000623 RID: 1571
	public Vector3[] points;

	// Token: 0x04000624 RID: 1572
	public Color[] colors;

	// Token: 0x04000625 RID: 1573
	public Vector3 hitPoint;

	// Token: 0x04000626 RID: 1574
	public bool hitObsticle;

	// Token: 0x04000627 RID: 1575
	public ShapesBlendMode blendMode;

	// Token: 0x04000628 RID: 1576
	public bool worldSpace;

	// Token: 0x04000629 RID: 1577
	public float dotRadius = 0.02f;

	// Token: 0x0400062A RID: 1578
	public float lineThickness = 0.02f;

	// Token: 0x0400062B RID: 1579
	private Camera cam;
}
