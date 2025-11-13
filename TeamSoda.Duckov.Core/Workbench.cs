using System;
using Duckov.UI;

// Token: 0x02000199 RID: 409
public class Workbench : InteractableBase
{
	// Token: 0x06000C18 RID: 3096 RVA: 0x00033BEF File Offset: 0x00031DEF
	protected override void OnInteractFinished()
	{
		ItemCustomizeSelectionView.Show();
	}
}
