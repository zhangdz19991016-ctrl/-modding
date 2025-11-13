using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D7 RID: 471
public class HurtVisualSettings : OptionsProviderBase
{
	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00039D1D File Offset: 0x00037F1D
	public override string Key
	{
		get
		{
			return "HurtVisualSettings";
		}
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x00039D24 File Offset: 0x00037F24
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x00039D48 File Offset: 0x00037F48
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

	// Token: 0x06000E13 RID: 3603 RVA: 0x00039D8E File Offset: 0x00037F8E
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				PlayerHurtVisual.hurtVisualOn = true;
			}
		}
		else
		{
			PlayerHurtVisual.hurtVisualOn = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x00039DB3 File Offset: 0x00037FB3
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x00039DC6 File Offset: 0x00037FC6
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x00039DDC File Offset: 0x00037FDC
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BD5 RID: 3029
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000BD6 RID: 3030
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";
}
