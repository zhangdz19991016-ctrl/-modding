using System;
using Duckov.Options;
using SodaCraft.Localizations;

// Token: 0x020001C9 RID: 457
public static class ScrollWheelBehaviour
{
	// Token: 0x06000DAB RID: 3499 RVA: 0x00038B7E File Offset: 0x00036D7E
	public static string GetDisplayName(ScrollWheelBehaviour.Behaviour behaviour)
	{
		return string.Format("ScrollWheelBehaviour_{0}", behaviour).ToPlainText();
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00038B95 File Offset: 0x00036D95
	// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00038BA2 File Offset: 0x00036DA2
	public static ScrollWheelBehaviour.Behaviour CurrentBehaviour
	{
		get
		{
			return OptionsManager.Load<ScrollWheelBehaviour.Behaviour>("ScrollWheelBehaviour", ScrollWheelBehaviour.Behaviour.AmmoAndInteract);
		}
		set
		{
			OptionsManager.Save<ScrollWheelBehaviour.Behaviour>("ScrollWheelBehaviour", value);
		}
	}

	// Token: 0x020004DF RID: 1247
	public enum Behaviour
	{
		// Token: 0x04001D41 RID: 7489
		AmmoAndInteract,
		// Token: 0x04001D42 RID: 7490
		Weapon
	}
}
