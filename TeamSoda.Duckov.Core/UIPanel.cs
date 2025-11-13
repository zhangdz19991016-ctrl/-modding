using System;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class UIPanel : MonoBehaviour
{
	// Token: 0x06000A91 RID: 2705 RVA: 0x0002EAB1 File Offset: 0x0002CCB1
	protected virtual void OnOpen()
	{
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0002EAB3 File Offset: 0x0002CCB3
	protected virtual void OnClose()
	{
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0002EAB5 File Offset: 0x0002CCB5
	protected virtual void OnChildOpened(UIPanel child)
	{
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0002EAB7 File Offset: 0x0002CCB7
	protected virtual void OnChildClosed(UIPanel child)
	{
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0002EAB9 File Offset: 0x0002CCB9
	internal void Open(UIPanel parent = null, bool controlFadeGroup = true)
	{
		this.parent = parent;
		this.OnOpen();
		if (controlFadeGroup)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0002EADC File Offset: 0x0002CCDC
	public void Close()
	{
		if (this.activeChild != null)
		{
			this.activeChild.Close();
		}
		this.OnClose();
		UIPanel uipanel = this.parent;
		if (uipanel != null)
		{
			uipanel.NotifyChildClosed(this);
		}
		FadeGroup fadeGroup = this.fadeGroup;
		if (fadeGroup == null)
		{
			return;
		}
		fadeGroup.Hide();
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0002EB2C File Offset: 0x0002CD2C
	public void OpenChild(UIPanel childPanel)
	{
		if (childPanel == null)
		{
			return;
		}
		if (this.activeChild != null)
		{
			this.activeChild.Close();
		}
		this.activeChild = childPanel;
		childPanel.Open(this, true);
		this.OnChildOpened(childPanel);
		if (this.hideWhenChildActive)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Hide();
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0002EB8A File Offset: 0x0002CD8A
	private void NotifyChildClosed(UIPanel child)
	{
		this.OnChildClosed(child);
		if (this.hideWhenChildActive)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}
	}

	// Token: 0x04000946 RID: 2374
	[SerializeField]
	protected FadeGroup fadeGroup;

	// Token: 0x04000947 RID: 2375
	[SerializeField]
	private bool hideWhenChildActive;

	// Token: 0x04000948 RID: 2376
	private UIPanel parent;

	// Token: 0x04000949 RID: 2377
	private UIPanel activeChild;
}
