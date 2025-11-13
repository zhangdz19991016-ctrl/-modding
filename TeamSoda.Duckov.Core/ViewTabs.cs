using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class ViewTabs : MonoBehaviour
{
	// Token: 0x06000F3F RID: 3903 RVA: 0x0003CC2C File Offset: 0x0003AE2C
	public void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0003CC39 File Offset: 0x0003AE39
	public void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0003CC46 File Offset: 0x0003AE46
	private void Update()
	{
		if (this.fadeGroup.IsShown && View.ActiveView == null)
		{
			this.Hide();
		}
	}

	// Token: 0x04000C88 RID: 3208
	[SerializeField]
	private FadeGroup fadeGroup;
}
