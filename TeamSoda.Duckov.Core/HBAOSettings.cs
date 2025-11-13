using System;
using Duckov.Options;
using HorizonBasedAmbientOcclusion.Universal;
using SodaCraft.Localizations;
using UnityEngine.Rendering;

// Token: 0x020001D6 RID: 470
public class HBAOSettings : OptionsProviderBase
{
	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x00039B2C File Offset: 0x00037D2C
	public override string Key
	{
		get
		{
			return "HBAOSettings";
		}
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x00039B33 File Offset: 0x00037D33
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.lowKey.ToPlainText(),
			this.normalKey.ToPlainText(),
			this.highKey.ToPlainText()
		};
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x00039B74 File Offset: 0x00037D74
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 2))
		{
		case 0:
			return this.offKey.ToPlainText();
		case 1:
			return this.lowKey.ToPlainText();
		case 2:
			return this.normalKey.ToPlainText();
		case 3:
			return this.highKey.ToPlainText();
		default:
			return this.offKey.ToPlainText();
		}
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x00039BE4 File Offset: 0x00037DE4
	public override void Set(int index)
	{
		HBAO hbao;
		if (this.GlobalVolumePorfile.TryGet<HBAO>(out hbao))
		{
			switch (index)
			{
			case 0:
				hbao.EnableHBAO(false);
				break;
			case 1:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Half, false);
				hbao.bias.value = 64f;
				break;
			case 2:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Half, false);
				hbao.bias.value = 128f;
				break;
			case 3:
				hbao.EnableHBAO(true);
				hbao.resolution = new HBAO.ResolutionParameter(HBAO.Resolution.Full, false);
				hbao.bias.value = 128f;
				break;
			}
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x00039CA0 File Offset: 0x00037EA0
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00039CB3 File Offset: 0x00037EB3
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00039CC8 File Offset: 0x00037EC8
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BD0 RID: 3024
	[LocalizationKey("Default")]
	public string offKey = "HBAOSettings_Off";

	// Token: 0x04000BD1 RID: 3025
	[LocalizationKey("Default")]
	public string lowKey = "HBAOSettings_Low";

	// Token: 0x04000BD2 RID: 3026
	[LocalizationKey("Default")]
	public string normalKey = "HBAOSettings_Normal";

	// Token: 0x04000BD3 RID: 3027
	[LocalizationKey("Default")]
	public string highKey = "HBAOSettings_High";

	// Token: 0x04000BD4 RID: 3028
	public VolumeProfile GlobalVolumePorfile;
}
