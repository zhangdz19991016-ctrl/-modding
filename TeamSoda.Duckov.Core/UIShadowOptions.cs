using System;
using Duckov.Options;
using LeTai.TrueShadow;
using SodaCraft.Localizations;

// Token: 0x020001DE RID: 478
public class UIShadowOptions : OptionsProviderBase
{
	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0003A527 File Offset: 0x00038727
	public override string Key
	{
		get
		{
			return "UIShadow";
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0003A52E File Offset: 0x0003872E
	// (set) Token: 0x06000E4C RID: 3660 RVA: 0x0003A53B File Offset: 0x0003873B
	public static bool Active
	{
		get
		{
			return OptionsManager.Load<bool>("UIShadow", true);
		}
		set
		{
			OptionsManager.Save<bool>("UIShadow", value);
		}
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x0003A548 File Offset: 0x00038748
	public static void Apply()
	{
		TrueShadow.ExternalActive = UIShadowOptions.Active;
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0003A554 File Offset: 0x00038754
	public string ActiveText
	{
		get
		{
			return "Options_On".ToPlainText();
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0003A560 File Offset: 0x00038760
	public string InactiveText
	{
		get
		{
			return "Options_Off".ToPlainText();
		}
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0003A56C File Offset: 0x0003876C
	public override string GetCurrentOption()
	{
		if (UIShadowOptions.Active)
		{
			return this.ActiveText;
		}
		return this.InactiveText;
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0003A582 File Offset: 0x00038782
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.InactiveText,
			this.ActiveText
		};
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0003A59C File Offset: 0x0003879C
	public override void Set(int index)
	{
		if (index <= 0)
		{
			UIShadowOptions.Active = false;
			return;
		}
		UIShadowOptions.Active = true;
	}

	// Token: 0x04000BE8 RID: 3048
	private const string key = "UIShadow";
}
