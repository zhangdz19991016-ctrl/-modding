using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class ATMView : View
{
	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000C4F RID: 3151 RVA: 0x000346C2 File Offset: 0x000328C2
	public static ATMView Instance
	{
		get
		{
			return View.GetViewInstance<ATMView>();
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x000346C9 File Offset: 0x000328C9
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x000346D4 File Offset: 0x000328D4
	public static void Show()
	{
		ATMView instance = ATMView.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Open(null);
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x000346F8 File Offset: 0x000328F8
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.atmPanel.ShowSelectPanel(true);
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00034717 File Offset: 0x00032917
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x04000ABB RID: 2747
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000ABC RID: 2748
	[SerializeField]
	private ATMPanel atmPanel;
}
