using System;
using Duckov.Options;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class vSyncSetting : OptionsProviderBase
{
	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0003A5B7 File Offset: 0x000387B7
	public override string Key
	{
		get
		{
			return "GSyncSetting";
		}
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x0003A5BE File Offset: 0x000387BE
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.onKey.ToPlainText(),
			this.offKey.ToPlainText()
		};
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0003A5E4 File Offset: 0x000387E4
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 1);
		if (num == 0)
		{
			this.SyncObjectActive(true);
			return this.onKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.offKey.ToPlainText();
		}
		this.SyncObjectActive(false);
		return this.offKey.ToPlainText();
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0003A638 File Offset: 0x00038838
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				QualitySettings.vSyncCount = 0;
				this.SyncObjectActive(false);
			}
		}
		else
		{
			QualitySettings.vSyncCount = 1;
			this.SyncObjectActive(true);
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0003A66B File Offset: 0x0003886B
	private void SyncObjectActive(bool active)
	{
		if (this.setActiveIfOn)
		{
			this.setActiveIfOn.SetActive(active);
		}
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0003A686 File Offset: 0x00038886
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x0003A699 File Offset: 0x00038899
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x0003A6AC File Offset: 0x000388AC
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 1);
		this.Set(index);
	}

	// Token: 0x04000BE9 RID: 3049
	[LocalizationKey("Default")]
	public string onKey = "gSync_On";

	// Token: 0x04000BEA RID: 3050
	[LocalizationKey("Default")]
	public string offKey = "gSync_Off";

	// Token: 0x04000BEB RID: 3051
	public GameObject setActiveIfOn;
}
