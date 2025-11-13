using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001D8 RID: 472
public class MoveDirectionOptions : OptionsProviderBase
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00039E1B File Offset: 0x0003801B
	public override string Key
	{
		get
		{
			return "MoveDirModeSettings";
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000E19 RID: 3609 RVA: 0x00039E22 File Offset: 0x00038022
	public static bool MoveViaCharacterDirection
	{
		get
		{
			return MoveDirectionOptions.moveViaCharacterDirection;
		}
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00039E29 File Offset: 0x00038029
	public override string[] GetOptions()
	{
		return new string[]
		{
			this.cameraModeKey.ToPlainText(),
			this.aimModeKey.ToPlainText()
		};
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x00039E50 File Offset: 0x00038050
	public override string GetCurrentOption()
	{
		int num = OptionsManager.Load<int>(this.Key, 0);
		if (num == 0)
		{
			return this.cameraModeKey.ToPlainText();
		}
		if (num != 1)
		{
			return this.cameraModeKey.ToPlainText();
		}
		return this.aimModeKey.ToPlainText();
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x00039E96 File Offset: 0x00038096
	public override void Set(int index)
	{
		if (index != 0)
		{
			if (index == 1)
			{
				MoveDirectionOptions.moveViaCharacterDirection = true;
			}
		}
		else
		{
			MoveDirectionOptions.moveViaCharacterDirection = false;
		}
		OptionsManager.Save<int>(this.Key, index);
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00039EBB File Offset: 0x000380BB
	private void Awake()
	{
		LevelManager.OnLevelInitialized += this.RefreshOnLevelInited;
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x00039ECE File Offset: 0x000380CE
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.RefreshOnLevelInited;
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x00039EE4 File Offset: 0x000380E4
	private void RefreshOnLevelInited()
	{
		int index = OptionsManager.Load<int>(this.Key, 0);
		this.Set(index);
	}

	// Token: 0x04000BD7 RID: 3031
	[LocalizationKey("Default")]
	public string cameraModeKey = "MoveDirectionMode_Camera";

	// Token: 0x04000BD8 RID: 3032
	[LocalizationKey("Default")]
	public string aimModeKey = "MoveDirectionMode_Aim";

	// Token: 0x04000BD9 RID: 3033
	private static bool moveViaCharacterDirection;
}
