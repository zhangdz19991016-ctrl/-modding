using System;

// Token: 0x0200016D RID: 365
public class PauseMenu : UIPanel
{
	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0002FC0F File Offset: 0x0002DE0F
	public static PauseMenu Instance
	{
		get
		{
			return GameManager.PauseMenu;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000B16 RID: 2838 RVA: 0x0002FC16 File Offset: 0x0002DE16
	public bool Shown
	{
		get
		{
			return !(this.fadeGroup == null) && this.fadeGroup.IsShown;
		}
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0002FC33 File Offset: 0x0002DE33
	public static void Show()
	{
		PauseMenu.Instance.Open(null, true);
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0002FC41 File Offset: 0x0002DE41
	public static void Hide()
	{
		PauseMenu.Instance.Close();
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0002FC4D File Offset: 0x0002DE4D
	public static void Toggle()
	{
		if (PauseMenu.Instance.fadeGroup.IsShown)
		{
			PauseMenu.Hide();
			return;
		}
		PauseMenu.Show();
	}
}
