using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001DB RID: 475
public class SoftShadowOptions : OptionsProviderBase
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0003A274 File Offset: 0x00038474
	public override string Key
	{
		get
		{
			return "SoftShadowSettings";
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0003A27B File Offset: 0x0003847B
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.offKey.ToPlainText(),
			this.onKey.ToPlainText()
		};
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0003A2A0 File Offset: 0x000384A0
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

	// Token: 0x06000E35 RID: 3637 RVA: 0x0003A2E6 File Offset: 0x000384E6
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0003A2F9 File Offset: 0x000384F9
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0003A30C File Offset: 0x0003850C
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0003A32D File Offset: 0x0003852D
	public override void Set(int index)
	{
	}

	// Token: 0x04000BE2 RID: 3042
	[LocalizationKey("Default")]
	public string offKey = "SoftShadowOptions_Off";

	// Token: 0x04000BE3 RID: 3043
	[LocalizationKey("Default")]
	public string onKey = "SoftShadowOptions_On";
}
