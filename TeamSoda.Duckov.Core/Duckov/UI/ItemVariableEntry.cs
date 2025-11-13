using System;
using Duckov.Utilities;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039D RID: 925
	public class ItemVariableEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x060020A9 RID: 8361 RVA: 0x00072701 File Offset: 0x00070901
		public void NotifyPooled()
		{
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00072703 File Offset: 0x00070903
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00072712 File Offset: 0x00070912
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x0007271A File Offset: 0x0007091A
		internal void Setup(CustomData target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00072735 File Offset: 0x00070935
		private void Refresh()
		{
			this.displayName.text = this.target.DisplayName;
			this.value.text = this.target.GetValueDisplayString("");
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00072768 File Offset: 0x00070968
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetData += this.OnTargetSetData;
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x0007278A File Offset: 0x0007098A
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnSetData -= this.OnTargetSetData;
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000727AC File Offset: 0x000709AC
		private void OnTargetSetData(CustomData data)
		{
			this.Refresh();
		}

		// Token: 0x0400163D RID: 5693
		private CustomData target;

		// Token: 0x0400163E RID: 5694
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x0400163F RID: 5695
		[SerializeField]
		private TextMeshProUGUI value;
	}
}
