using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D9 RID: 473
public class RunInputOptions : OptionsProviderBase
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00039F23 File Offset: 0x00038123
	public override string Key
	{
		get
		{
			return "RunInputModeSettings";
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x00039F2A File Offset: 0x0003812A
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.holdModeKey.ToPlainText(),
			this.switchModeKey.ToPlainText()
		};
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x00039F50 File Offset: 0x00038150
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 0);
		if (num == 0)
		{
			return this.holdModeKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.holdModeKey.ToPlainText();
		}
		return this.switchModeKey.ToPlainText();
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00039F96 File Offset: 0x00038196
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				InputManager.useRunInputBuffer = true;
			}
		}
		else
		{
			InputManager.useRunInputBuffer = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x00039FBB File Offset: 0x000381BB
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x00039FCE File Offset: 0x000381CE
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x00039FE4 File Offset: 0x000381E4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BDA RID: 3034
	[LocalizationKey("Default")]
	public string holdModeKey = "RunInputMode_Hold";

	// Token: 0x04000BDB RID: 3035
	[LocalizationKey("Default")]
	public string switchModeKey = "RunInputMode_Switch";
}
