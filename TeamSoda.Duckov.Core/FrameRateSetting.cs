using System;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class FrameRateSetting : OptionsProviderBase
{
	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000DEE RID: 3566 RVA: 0x000395DB File Offset: 0x000377DB
	public override string Key
	{
		get
		{
			return "FrameRateSetting";
		}
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x000395E2 File Offset: 0x000377E2
	public override string[] GetOptions()
	{
		return new string[]
		{
			"60",
			"90",
			"120",
			"144",
			"240",
			this.optionUnlimitKey.ToPlainText()
		};
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00039620 File Offset: 0x00037820
	public override string GetCurrentOption()
	{
		switch (OptionsManager.Load<int>(this.Key, 1))
		{
		case 0:
			return "60";
		case 1:
			return "90";
		case 2:
			return "120";
		case 3:
			return "144";
		case 4:
			return "240";
		case 5:
			return this.optionUnlimitKey.ToPlainText();
		default:
			return "60";
		}
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x0003968C File Offset: 0x0003788C
	public override void Set(int index)
	{
		switch (index)
		{
		case 0:
			Application.targetFrameRate = 60;
			break;
		case 1:
			Application.targetFrameRate = 90;
			break;
		case 2:
			Application.targetFrameRate = 120;
			break;
		case 3:
			Application.targetFrameRate = 144;
			break;
		case 4:
			Application.targetFrameRate = 240;
			break;
		case 5:
			Application.targetFrameRate = 500;
			break;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x00039702 File Offset: 0x00037902
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x00039715 File Offset: 0x00037915
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x00039728 File Offset: 0x00037928
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BC6 RID: 3014
	[LocalizationKey("Default")]
	public string optionUnlimitKey = "FrameRateUnlimit";
}
