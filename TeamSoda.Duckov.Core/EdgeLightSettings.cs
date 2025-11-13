using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D2 RID: 466
public class EdgeLightSettings : OptionsProviderBase
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x000394DA File Offset: 0x000376DA
	public override string Key
	{
		get
		{
			return "EdgeLightSetting";
		}
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x000394E1 File Offset: 0x000376E1
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00039508 File Offset: 0x00037708
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.offKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.onKey.ToPlainText();
		}
		return this.onKey.ToPlainText();
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0003954E File Offset: 0x0003774E
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				EdgeLightEntry.SetEnabled(true);
			}
		}
		else
		{
			EdgeLightEntry.SetEnabled(false);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DEA RID: 3562 RVA: 0x00039573 File Offset: 0x00037773
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DEB RID: 3563 RVA: 0x00039586 File Offset: 0x00037786
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x0003959C File Offset: 0x0003779C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BC4 RID: 3012
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000BC5 RID: 3013
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
