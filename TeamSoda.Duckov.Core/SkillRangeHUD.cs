using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class SkillRangeHUD : MonoBehaviour
{
	// Token: 0x0600066C RID: 1644 RVA: 0x0001D430 File Offset: 0x0001B630
	public void SetRange(float range)
	{
		this.rangeTarget.localScale = Vector3.one * range;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x0001D448 File Offset: 0x0001B648
	public void SetProgress(float progress)
	{
		if (this.rangeMat == null)
		{
			this.rangeMat = this.rangeRenderer.material;
		}
		if (this.rangeMat == null)
		{
			return;
		}
		this.rangeMat.SetFloat("_Progress", progress);
	}

	// Token: 0x04000637 RID: 1591
	public Transform rangeTarget;

	// Token: 0x04000638 RID: 1592
	public Renderer rangeRenderer;

	// Token: 0x04000639 RID: 1593
	private Material rangeMat;
}
