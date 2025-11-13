using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class SingleCrosshair : MonoBehaviour
{
	// Token: 0x060004B2 RID: 1202 RVA: 0x00015814 File Offset: 0x00013A14
	public void UpdateScatter(float _scatter)
	{
		this.currentScatter = _scatter;
		RectTransform rectTransform = base.transform as RectTransform;
		rectTransform.localRotation = Quaternion.Euler(0f, 0f, this.rotation);
		Vector3 a = Vector3.zero;
		if (this.axis != Vector3.zero)
		{
			a = rectTransform.parent.InverseTransformDirection(rectTransform.TransformDirection(this.axis));
		}
		rectTransform.anchoredPosition = a * (this.minDistance + this.currentScatter * this.scatterMoveScale);
		if (this.controlRectWidthHeight)
		{
			float d = this.minScale + this.currentScatter * this.scatterScaleFactor;
			rectTransform.sizeDelta = Vector2.one * d;
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x000158D2 File Offset: 0x00013AD2
	private void OnValidate()
	{
		this.UpdateScatter(0f);
	}

	// Token: 0x040003F0 RID: 1008
	public float rotation;

	// Token: 0x040003F1 RID: 1009
	public Vector3 axis;

	// Token: 0x040003F2 RID: 1010
	public float minDistance;

	// Token: 0x040003F3 RID: 1011
	public float scatterMoveScale = 5f;

	// Token: 0x040003F4 RID: 1012
	private float currentScatter;

	// Token: 0x040003F5 RID: 1013
	public bool controlRectWidthHeight;

	// Token: 0x040003F6 RID: 1014
	public float minScale = 100f;

	// Token: 0x040003F7 RID: 1015
	public float scatterScaleFactor = 5f;
}
