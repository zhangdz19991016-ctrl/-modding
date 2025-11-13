using System;
using Duckov.Buildings;
using Duckov.Buildings.UI;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class BuilderViewInvoker : InteractableBase
{
	// Token: 0x06000C65 RID: 3173 RVA: 0x00034A1C File Offset: 0x00032C1C
	protected override void OnInteractFinished()
	{
		if (this.buildingArea == null)
		{
			return;
		}
		BuilderView.Show(this.buildingArea);
	}

	// Token: 0x04000AC6 RID: 2758
	[SerializeField]
	private BuildingArea buildingArea;
}
