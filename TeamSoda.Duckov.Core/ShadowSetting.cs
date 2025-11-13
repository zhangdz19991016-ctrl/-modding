using System;
using Duckov.Options;
using SodaCraft.Localizations;
using Umbra;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001DA RID: 474
public class ShadowSetting : OptionsProviderBase
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0003A023 File Offset: 0x00038223
	public override string Key
	{
		get
		{
			return "ShadowSettings";
		}
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0003A02A File Offset: 0x0003822A
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.lowKey.ToPlainText(),
			this.middleKey.ToPlainText(),
			this.highKey.ToPlainText()
		};
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x0003A06C File Offset: 0x0003826C
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 2))
		{
		case 0:
			return this.offKey.ToPlainText();
		case 1:
			return this.lowKey.ToPlainText();
		case 2:
			return this.middleKey.ToPlainText();
		case 3:
			return this.highKey.ToPlainText();
		default:
			return this.highKey.ToPlainText();
		}
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0003A0DC File Offset: 0x000382DC
	private void SetShadow(bool on, int res, float shadowDistance, bool softShadow, bool softShadowDownSample, bool contactShadow, int pointLightCount)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
		if (universalRenderPipelineAsset != null)
		{
			universalRenderPipelineAsset.shadowDistance = (on ? shadowDistance : 0f);
			universalRenderPipelineAsset.mainLightShadowmapResolution = res;
			universalRenderPipelineAsset.additionalLightsShadowmapResolution = res;
			universalRenderPipelineAsset.maxAdditionalLightsCount = pointLightCount;
		}
		if (this.umbraProfile)
		{
			this.umbraProfile.shadowSource = (softShadow ? ShadowSource.UmbraShadows : ShadowSource.UnityShadows);
			this.umbraProfile.downsample = softShadowDownSample;
			this.umbraProfile.contactShadows = contactShadow;
		}
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0003A160 File Offset: 0x00038360
	public override void Set(int index)
	{
		switch (index)
		{
		case 0:
			this.SetShadow(false, 512, 0f, false, false, false, 0);
			break;
		case 1:
			this.SetShadow(true, 1024, 70f, false, false, false, 0);
			break;
		case 2:
			this.SetShadow(true, 2048, 80f, true, true, true, 5);
			break;
		case 3:
			this.SetShadow(true, 4096, 90f, true, false, true, 6);
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0003A1EB File Offset: 0x000383EB
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x0003A1FE File Offset: 0x000383FE
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0003A214 File Offset: 0x00038414
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 2);
		this.Set(index);
	}

	// Token: 0x04000BDC RID: 3036
	public UmbraProfile umbraProfile;

	// Token: 0x04000BDD RID: 3037
	public float onDistance = 100f;

	// Token: 0x04000BDE RID: 3038
	[LocalizationKey("Default")]
	public string highKey = "Options_High";

	// Token: 0x04000BDF RID: 3039
	[LocalizationKey("Default")]
	public string middleKey = "Options_Middle";

	// Token: 0x04000BE0 RID: 3040
	[LocalizationKey("Default")]
	public string lowKey = "Options_Low";

	// Token: 0x04000BE1 RID: 3041
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
