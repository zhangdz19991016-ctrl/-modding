using System;
using Duckov.Options;

// Token: 0x020001CC RID: 460
public class FullScreenOptions : OptionsProviderBase
{
	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00038C3B File Offset: 0x00036E3B
	public override string Key
	{
		get
		{
			return ResolutionSetter.Key_ScreenMode;
		}
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x00038C42 File Offset: 0x00036E42
	public override string GetCurrentOption()
	{
		return ResolutionSetter.ScreenModeToName(OptionsManager.Load<ResolutionSetter.screenModes>(this.Key, ResolutionSetter.screenModes.Borderless));
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x00038C55 File Offset: 0x00036E55
	public override string[] GetOptions()
	{
		return ResolutionSetter.GetScreenModes();
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x00038C5C File Offset: 0x00036E5C
	public override void Set(int index)
	{
		OptionsManager.Save<ResolutionSetter.screenModes>(this.Key, (ResolutionSetter.screenModes)index);
	}
}
