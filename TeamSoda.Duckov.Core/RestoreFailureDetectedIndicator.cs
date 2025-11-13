using System;
using Duckov.UI.Animations;
using Saves;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class RestoreFailureDetectedIndicator : MonoBehaviour
{
	// Token: 0x060009AE RID: 2478 RVA: 0x0002A676 File Offset: 0x00028876
	private void OnEnable()
	{
		SavesSystem.OnRestoreFailureDetected += this.Refresh;
		SavesSystem.OnSetFile += this.Refresh;
		this.Refresh();
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0002A6A0 File Offset: 0x000288A0
	private void OnDisable()
	{
		SavesSystem.OnRestoreFailureDetected -= this.Refresh;
		SavesSystem.OnSetFile -= this.Refresh;
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0002A6C4 File Offset: 0x000288C4
	private void Refresh()
	{
		if (SavesSystem.RestoreFailureMarker)
		{
			this.fadeGroup.Show();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x04000893 RID: 2195
	[SerializeField]
	private FadeGroup fadeGroup;
}
