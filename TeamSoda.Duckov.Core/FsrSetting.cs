using System;
using Duckov.MiniGames;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001D4 RID: 468
public class FsrSetting : OptionsProviderBase
{
	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0003975C File Offset: 0x0003795C
	public override string Key
	{
		get
		{
			return "FsrSetting";
		}
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00039764 File Offset: 0x00037964
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.qualityKey.ToPlainText(),
			this.balancedKey.ToPlainText(),
			this.performanceKey.ToPlainText(),
			this.ultraPerformanceKey.ToPlainText()
		};
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x000397C0 File Offset: 0x000379C0
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 0))
		{
		case 0:
			return this.offKey.ToPlainText();
		case 1:
			return this.qualityKey.ToPlainText();
		case 2:
			return this.balancedKey.ToPlainText();
		case 3:
			return this.performanceKey.ToPlainText();
		case 4:
			return this.ultraPerformanceKey.ToPlainText();
		default:
			return this.offKey.ToPlainText();
		}
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00039840 File Offset: 0x00037A40
	public override void Set(int index)
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
		int num = index;
		if (FsrSetting.gameOn)
		{
			num = 0;
		}
		switch (num)
		{
		case 0:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 1f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.Linear;
			}
			break;
		case 1:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.67f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		case 2:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.58f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		case 3:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.5f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		case 4:
			if (universalRenderPipelineAsset != null)
			{
				universalRenderPipelineAsset.renderScale = 0.33f;
				universalRenderPipelineAsset.upscalingFilter = UpscalingFilterSelection.FSR;
			}
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x00039920 File Offset: 0x00037B20
	private void Awake()
	{
		this.RefreshOnLevelInited();
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
		GamingConsole.OnGamingConsoleInteractChanged += this.OnGamingConsoleInteractChanged;
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x0003994A File Offset: 0x00037B4A
	private void OnGamingConsoleInteractChanged(bool _gameOn)
	{
		FsrSetting.gameOn = _gameOn;
		this.SyncSetting();
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00039958 File Offset: 0x00037B58
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0003996C File Offset: 0x00037B6C
	private void SyncSetting()
	{
		int index = OptionsManager.Load<int>(this.Key, 0);
		this.Set(index);
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x0003998D File Offset: 0x00037B8D
	private void RefreshOnLevelInited()
	{
		this.SyncSetting();
	}

	// Token: 0x04000BC7 RID: 3015
	[LocalizationKey("Default")]
	public string offKey = "fsr_Off";

	// Token: 0x04000BC8 RID: 3016
	[LocalizationKey("Default")]
	public string qualityKey = "fsr_Quality";

	// Token: 0x04000BC9 RID: 3017
	[LocalizationKey("Default")]
	public string balancedKey = "fsr_Balanced";

	// Token: 0x04000BCA RID: 3018
	[LocalizationKey("Default")]
	public string performanceKey = "fsr_Performance";

	// Token: 0x04000BCB RID: 3019
	[LocalizationKey("Default")]
	public string ultraPerformanceKey = "fsr_UltraPerformance";

	// Token: 0x04000BCC RID: 3020
	private static bool gameOn;
}
