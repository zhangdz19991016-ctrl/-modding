using System;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class HUDManager : MonoBehaviour
{
	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06000F0A RID: 3850 RVA: 0x0003C3C4 File Offset: 0x0003A5C4
	// (remove) Token: 0x06000F0B RID: 3851 RVA: 0x0003C3F8 File Offset: 0x0003A5F8
	private static event Action onHideTokensChanged;

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0003C42C File Offset: 0x0003A62C
	private bool ShouldDisplay
	{
		get
		{
			bool flag = HUDManager.hideTokens.Any((UnityEngine.Object e) => e != null);
			bool flag2 = View.ActiveView != null;
			bool active = DialogueUI.Active;
			bool flag3 = CustomFaceUI.ActiveView != null;
			bool active2 = CameraMode.Active;
			return !flag && !flag2 && !active && !flag3 && !active2;
		}
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0003C498 File Offset: 0x0003A698
	private void Awake()
	{
		View.OnActiveViewChanged += this.OnActiveViewChanged;
		DialogueUI.OnDialogueStatusChanged += this.OnDialogueStatusChanged;
		CustomFaceUI.OnCustomUIViewChanged += this.OnCustomFaceViewChange;
		CameraMode.OnCameraModeChanged = (Action<bool>)Delegate.Combine(CameraMode.OnCameraModeChanged, new Action<bool>(this.OnCameraModeChanged));
		HUDManager.onHideTokensChanged += this.OnHideTokensChanged;
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0003C50C File Offset: 0x0003A70C
	private void OnDestroy()
	{
		View.OnActiveViewChanged -= this.OnActiveViewChanged;
		DialogueUI.OnDialogueStatusChanged -= this.OnDialogueStatusChanged;
		CustomFaceUI.OnCustomUIViewChanged -= this.OnCustomFaceViewChange;
		CameraMode.OnCameraModeChanged = (Action<bool>)Delegate.Remove(CameraMode.OnCameraModeChanged, new Action<bool>(this.OnCameraModeChanged));
		HUDManager.onHideTokensChanged -= this.OnHideTokensChanged;
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0003C57D File Offset: 0x0003A77D
	private void OnHideTokensChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0003C585 File Offset: 0x0003A785
	private void OnCameraModeChanged(bool value)
	{
		this.Refresh();
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0003C58D File Offset: 0x0003A78D
	private void OnDialogueStatusChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x0003C595 File Offset: 0x0003A795
	private void OnActiveViewChanged()
	{
		this.Refresh();
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x0003C59D File Offset: 0x0003A79D
	private void OnCustomFaceViewChange()
	{
		this.Refresh();
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0003C5A8 File Offset: 0x0003A7A8
	private void Refresh()
	{
		if (this.ShouldDisplay)
		{
			this.canvasGroup.blocksRaycasts = true;
			if (this.fadeGroup.IsShown)
			{
				return;
			}
			this.fadeGroup.Show();
			return;
		}
		else
		{
			this.canvasGroup.blocksRaycasts = false;
			if (this.fadeGroup.IsHidden)
			{
				return;
			}
			this.fadeGroup.Hide();
			return;
		}
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0003C608 File Offset: 0x0003A808
	public static void RegisterHideToken(UnityEngine.Object obj)
	{
		HUDManager.hideTokens.Add(obj);
		Action action = HUDManager.onHideTokensChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0003C624 File Offset: 0x0003A824
	public static void UnregisterHideToken(UnityEngine.Object obj)
	{
		HUDManager.hideTokens.Remove(obj);
		Action action = HUDManager.onHideTokensChanged;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x04000C71 RID: 3185
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C72 RID: 3186
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000C73 RID: 3187
	private static List<UnityEngine.Object> hideTokens = new List<UnityEngine.Object>();
}
