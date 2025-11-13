using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D0 RID: 464
public class DisableCameraOffset : OptionsProviderBase
{
	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x00039305 File Offset: 0x00037505
	public override string Key
	{
		get
		{
			return "DisableCameraOffset";
		}
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0003930C File Offset: 0x0003750C
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.onKey.ToPlainText(),
			this.offKey.ToPlainText()
		};
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x00039330 File Offset: 0x00037530
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			return this.onKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		return this.offKey.ToPlainText();
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x00039376 File Offset: 0x00037576
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				DisableCameraOffset.disableCameraOffset = false;
			}
		}
		else
		{
			DisableCameraOffset.disableCameraOffset = true;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0003939B File Offset: 0x0003759B
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x000393AE File Offset: 0x000375AE
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x000393C4 File Offset: 0x000375C4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BBF RID: 3007
	[LocalizationKey("Default")]
	public string onKey = "Options_On";

	// Token: 0x04000BC0 RID: 3008
	[LocalizationKey("Default")]
	public string offKey = "Options_Off";

	// Token: 0x04000BC1 RID: 3009
	public static bool disableCameraOffset;
}
