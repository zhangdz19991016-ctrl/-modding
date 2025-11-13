using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001DD RID: 477
public class SunFogSettings : OptionsProviderBase
{
	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000E42 RID: 3650 RVA: 0x0003A426 File Offset: 0x00038626
	public override string Key
	{
		get
		{
			return "SunFogSetting";
		}
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x0003A42D File Offset: 0x0003862D
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x0003A454 File Offset: 0x00038654
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

	// Token: 0x06000E45 RID: 3653 RVA: 0x0003A49A File Offset: 0x0003869A
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				SunFogEntry.SetEnabled(true);
			}
		}
		else
		{
			SunFogEntry.SetEnabled(false);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x0003A4BF File Offset: 0x000386BF
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x0003A4D2 File Offset: 0x000386D2
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E48 RID: 3656 RVA: 0x0003A4E8 File Offset: 0x000386E8
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BE6 RID: 3046
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000BE7 RID: 3047
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
