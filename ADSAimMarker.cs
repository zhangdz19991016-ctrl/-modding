using System;
using System.Collections.Generic;
using Duckov.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

// Token: 0x02000075 RID: 117
public class ADSAimMarker : MonoBehaviour
{
	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x0600044D RID: 1101 RVA: 0x00013A39 File Offset: 0x00011C39
	public float CanvasAlpha
	{
		get
		{
			return this.canvasAlpha;
		}
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x00013A44 File Offset: 0x00011C44
	public void CollectCrosshairs()
	{
		this.crosshairs.Clear();
		foreach (SingleCrosshair item in base.GetComponentsInChildren<SingleCrosshair>())
		{
			this.crosshairs.Add(item);
		}
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x00013A84 File Offset: 0x00011C84
	private void Awake()
	{
		this.proceduralImageCanvasGroups = new List<CanvasGroup>();
		for (int i = 0; i < this.proceduralImages.Count; i++)
		{
			this.proceduralImageCanvasGroups.Add(this.proceduralImages[i].GetComponent<CanvasGroup>());
		}
		this.selfRect = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x00013ADC File Offset: 0x00011CDC
	private void LateUpdate()
	{
		if (this.selfRect)
		{
			this.selfRect.localScale = Vector3.one;
		}
		this.followUI.position = Vector3.Lerp(this.followUI.position, this.aimMarkerUI.position, Time.deltaTime * this.followSpeed);
		if (Vector3.Distance(this.followUI.position, this.aimMarkerUI.position) > this.followMaxDistance)
		{
			this.followUI.position = Vector3.MoveTowards(this.aimMarkerUI.position, this.followUI.position, this.followMaxDistance);
		}
		foreach (SingleCrosshair singleCrosshair in this.crosshairs)
		{
			if (singleCrosshair)
			{
				singleCrosshair.UpdateScatter(this.scatter);
			}
		}
		this.SetSniperRenderer();
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00013BE0 File Offset: 0x00011DE0
	public void SetAimMarkerPos(Vector3 pos)
	{
		this.aimMarkerUI.position = pos;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00013BF0 File Offset: 0x00011DF0
	public void OnShoot()
	{
		foreach (PunchReceiver punchReceiver in this.shootPunchReceivers)
		{
			if (punchReceiver)
			{
				punchReceiver.Punch();
			}
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00013C4C File Offset: 0x00011E4C
	public void SetScatter(float _currentScatter, float _minScatter)
	{
		this.scatter = _currentScatter;
		this.minScatter = _minScatter;
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00013C5C File Offset: 0x00011E5C
	public void SetAdsValue(float _adsValue)
	{
		this.adsValue = _adsValue;
		this.canvasAlpha = _adsValue;
		if (this.adsAlphaRemap.y > this.adsAlphaRemap.x)
		{
			this.canvasAlpha = Mathf.Clamp01((_adsValue - this.adsAlphaRemap.x) / (this.adsAlphaRemap.y - this.adsAlphaRemap.x));
		}
		this.canvasGroup.alpha = this.canvasAlpha;
		for (int i = 0; i < this.proceduralImages.Count; i++)
		{
			ProceduralImage proceduralImage = this.proceduralImages[i];
			if (proceduralImage)
			{
				float num = Mathf.Clamp(this.scatter - this.minScatter, 0f, 10f) * 2f;
				proceduralImage.FalloffDistance = Mathf.Lerp(25f, 1f, this.canvasAlpha) + num;
				CanvasGroup canvasGroup = this.proceduralImageCanvasGroups[i];
				if (canvasGroup)
				{
					canvasGroup.alpha = Mathf.Clamp(1f - (num - 2f) / 15f, 0.3f, 1f);
				}
			}
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00013D80 File Offset: 0x00011F80
	private void SetSniperRenderer()
	{
		if (this.sniperRoundRenderer)
		{
			Vector2 v = RectTransformUtility.WorldToScreenPoint(null, this.aimMarkerUI.position) / new Vector2((float)Screen.width, (float)Screen.height);
			this.sniperRoundRenderer.material.SetVector(this.sniperCenterShaderHash, v);
		}
		if (this.followSniperRoundRenderer)
		{
			Vector2 v2 = RectTransformUtility.WorldToScreenPoint(null, this.followUI.position) / new Vector2((float)Screen.width, (float)Screen.height);
			this.followSniperRoundRenderer.material.SetVector(this.sniperCenterShaderHash, v2);
		}
	}

	// Token: 0x0400039E RID: 926
	[HideInInspector]
	public ADSAimMarker selfPrefab;

	// Token: 0x0400039F RID: 927
	public bool hideNormalCrosshair = true;

	// Token: 0x040003A0 RID: 928
	public AimMarker parentAimMarker;

	// Token: 0x040003A1 RID: 929
	public RectTransform aimMarkerUI;

	// Token: 0x040003A2 RID: 930
	public RectTransform followUI;

	// Token: 0x040003A3 RID: 931
	public CanvasGroup canvasGroup;

	// Token: 0x040003A4 RID: 932
	public float followSpeed;

	// Token: 0x040003A5 RID: 933
	public float followMaxDistance = 30f;

	// Token: 0x040003A6 RID: 934
	private float adsValue = -1f;

	// Token: 0x040003A7 RID: 935
	private float canvasAlpha;

	// Token: 0x040003A8 RID: 936
	public Vector2 adsAlphaRemap = new Vector2(0f, 1f);

	// Token: 0x040003A9 RID: 937
	public List<ProceduralImage> proceduralImages;

	// Token: 0x040003AA RID: 938
	private List<CanvasGroup> proceduralImageCanvasGroups;

	// Token: 0x040003AB RID: 939
	public List<PunchReceiver> shootPunchReceivers;

	// Token: 0x040003AC RID: 940
	public List<SingleCrosshair> crosshairs;

	// Token: 0x040003AD RID: 941
	public Graphic sniperRoundRenderer;

	// Token: 0x040003AE RID: 942
	public Graphic followSniperRoundRenderer;

	// Token: 0x040003AF RID: 943
	private float scatter;

	// Token: 0x040003B0 RID: 944
	private float minScatter;

	// Token: 0x040003B1 RID: 945
	private RectTransform selfRect;

	// Token: 0x040003B2 RID: 946
	private int sniperCenterShaderHash = Shader.PropertyToID("_RoundCenter");
}
