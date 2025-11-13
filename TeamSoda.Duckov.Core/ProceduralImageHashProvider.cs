using System;
using LeTai.TrueShadow;
using LeTai.TrueShadow.PluginInterfaces;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000209 RID: 521
[ExecuteInEditMode]
public class ProceduralImageHashProvider : MonoBehaviour, ITrueShadowCustomHashProvider
{
	// Token: 0x06000F50 RID: 3920 RVA: 0x0003CDC1 File Offset: 0x0003AFC1
	private void Awake()
	{
		if (this.trueShadow == null)
		{
			this.trueShadow = base.GetComponent<TrueShadow>();
		}
		if (this.proceduralImage == null)
		{
			this.proceduralImage = base.GetComponent<ProceduralImage>();
		}
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x0003CDF7 File Offset: 0x0003AFF7
	private void Refresh()
	{
		this.trueShadow.CustomHash = this.Hash();
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0003CE0A File Offset: 0x0003B00A
	private void OnValidate()
	{
		if (this.trueShadow == null)
		{
			this.trueShadow = base.GetComponent<TrueShadow>();
		}
		if (this.proceduralImage == null)
		{
			this.proceduralImage = base.GetComponent<ProceduralImage>();
		}
		this.Refresh();
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x0003CE46 File Offset: 0x0003B046
	private void OnRectTransformDimensionsChange()
	{
		this.Refresh();
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x0003CE50 File Offset: 0x0003B050
	private int Hash()
	{
		return this.proceduralImage.InfoCache.GetHashCode() + this.proceduralImage.color.GetHashCode();
	}

	// Token: 0x04000C8E RID: 3214
	[SerializeField]
	private ProceduralImage proceduralImage;

	// Token: 0x04000C8F RID: 3215
	[SerializeField]
	private TrueShadow trueShadow;
}
