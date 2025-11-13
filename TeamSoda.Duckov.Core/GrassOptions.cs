using System;
using Duckov.Options;
using SodaCraft.Localizations;
using SymmetryBreakStudio.TastyGrassShader;
using UnityEngine.Rendering.Universal;

// Token: 0x020001D5 RID: 469
public class GrassOptions : OptionsProviderBase
{
	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000E00 RID: 3584 RVA: 0x000399D4 File Offset: 0x00037BD4
	public override string Key
	{
		get
		{
			return "GrassSettings";
		}
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x000399DB File Offset: 0x00037BDB
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00039A00 File Offset: 0x00037C00
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.offKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		return this.onKey.ToPlainText();
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x00039A46 File Offset: 0x00037C46
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00039A59 File Offset: 0x00037C59
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00039A6C File Offset: 0x00037C6C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00039A90 File Offset: 0x00037C90
	public override void Set(int index)
	{
		ScriptableRendererFeature scriptableRendererFeature = this.rendererData.rendererFeatures.Find((ScriptableRendererFeature e) => e is TastyGrassShaderGlobalSettings);
		if (scriptableRendererFeature != null)
		{
			TastyGrassShaderGlobalSettings tastyGrassShaderGlobalSettings = scriptableRendererFeature as TastyGrassShaderGlobalSettings;
			if (index != 0)
			{
				if (index == 1)
				{
					tastyGrassShaderGlobalSettings.SetActive(true);
					TgsManager.Enable = true;
				}
			}
			else
			{
				tastyGrassShaderGlobalSettings.SetActive(false);
				TgsManager.Enable = false;
			}
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x04000BCD RID: 3021
	[LocalizationKey("Default")]
	public string offKey = "GrassOptions_Off";

	// Token: 0x04000BCE RID: 3022
	[LocalizationKey("Default")]
	public string onKey = "GrassOptions_On";

	// Token: 0x04000BCF RID: 3023
	public UniversalRendererData rendererData;
}
