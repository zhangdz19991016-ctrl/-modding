using System;
using Duckov.UI;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class ViewTabDisplayEntry : MonoBehaviour
{
	// Token: 0x06000F43 RID: 3907 RVA: 0x0003CC70 File Offset: 0x0003AE70
	private void Awake()
	{
		ManagedUIElement.onOpen += this.OnViewOpen;
		ManagedUIElement.onClose += this.OnViewClose;
		this.HideIndicator();
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x0003CC9A File Offset: 0x0003AE9A
	private void OnDestroy()
	{
		ManagedUIElement.onOpen -= this.OnViewOpen;
		ManagedUIElement.onClose -= this.OnViewClose;
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x0003CCBE File Offset: 0x0003AEBE
	private void Start()
	{
		if (View.ActiveView != null && View.ActiveView.GetType().Name == this.viewTypeName)
		{
			this.ShowIndicator();
		}
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0003CCEF File Offset: 0x0003AEEF
	private void OnViewClose(ManagedUIElement element)
	{
		if (element.GetType().Name == this.viewTypeName)
		{
			this.HideIndicator();
		}
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0003CD0F File Offset: 0x0003AF0F
	private void OnViewOpen(ManagedUIElement element)
	{
		if (element.GetType().Name == this.viewTypeName)
		{
			this.ShowIndicator();
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0003CD2F File Offset: 0x0003AF2F
	private void ShowIndicator()
	{
		this.indicator.SetActive(true);
		this.punch.Punch();
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0003CD48 File Offset: 0x0003AF48
	private void HideIndicator()
	{
		this.indicator.SetActive(false);
	}

	// Token: 0x04000C89 RID: 3209
	[SerializeField]
	private string viewTypeName;

	// Token: 0x04000C8A RID: 3210
	[SerializeField]
	private GameObject indicator;

	// Token: 0x04000C8B RID: 3211
	[SerializeField]
	private PunchReceiver punch;
}
