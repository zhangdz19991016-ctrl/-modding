using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class IndicatorHUD : MonoBehaviour
{
	// Token: 0x0600062A RID: 1578 RVA: 0x0001BCEC File Offset: 0x00019EEC
	private void Start()
	{
		if ((LevelManager.Instance == null || LevelManager.Instance.IsBaseLevel) && this.mapIndicator)
		{
			this.mapIndicator.SetActive(false);
		}
		this.toggleParent.SetActive(this.startActive);
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0001BD3C File Offset: 0x00019F3C
	private void Awake()
	{
		UIInputManager.OnToggleIndicatorHUD += this.Toggle;
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0001BD4F File Offset: 0x00019F4F
	private void OnDestroy()
	{
		UIInputManager.OnToggleIndicatorHUD -= this.Toggle;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0001BD62 File Offset: 0x00019F62
	private void Toggle(UIInputEventData data)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.toggleParent.SetActive(!this.toggleParent.activeInHierarchy);
	}

	// Token: 0x040005C8 RID: 1480
	public GameObject mapIndicator;

	// Token: 0x040005C9 RID: 1481
	public GameObject toggleParent;

	// Token: 0x040005CA RID: 1482
	public bool startActive;
}
