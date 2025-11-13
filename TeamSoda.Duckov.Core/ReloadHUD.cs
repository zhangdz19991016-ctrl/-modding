using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000CA RID: 202
public class ReloadHUD : MonoBehaviour
{
	// Token: 0x0600065D RID: 1629 RVA: 0x0001CC9C File Offset: 0x0001AE9C
	private void Update()
	{
		if (this.characterMainControl == null)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl == null)
			{
				return;
			}
			this.button.onClick.AddListener(new UnityAction(this.Reload));
		}
		this.reloadable = this.characterMainControl.GetGunReloadable();
		if (this.reloadable != this.button.interactable)
		{
			this.button.interactable = this.reloadable;
			if (this.reloadable)
			{
				UnityEvent onShowEvent = this.OnShowEvent;
				if (onShowEvent != null)
				{
					onShowEvent.Invoke();
				}
			}
			else
			{
				UnityEvent onHideEvent = this.OnHideEvent;
				if (onHideEvent != null)
				{
					onHideEvent.Invoke();
				}
			}
		}
		this.frame++;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x0001CD61 File Offset: 0x0001AF61
	private void OnDestroy()
	{
		this.button.onClick.RemoveAllListeners();
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0001CD73 File Offset: 0x0001AF73
	private void Reload()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.TryToReload(null);
		}
	}

	// Token: 0x0400061D RID: 1565
	private CharacterMainControl characterMainControl;

	// Token: 0x0400061E RID: 1566
	public Button button;

	// Token: 0x0400061F RID: 1567
	private bool reloadable;

	// Token: 0x04000620 RID: 1568
	public UnityEvent OnShowEvent;

	// Token: 0x04000621 RID: 1569
	public UnityEvent OnHideEvent;

	// Token: 0x04000622 RID: 1570
	private int frame;
}
