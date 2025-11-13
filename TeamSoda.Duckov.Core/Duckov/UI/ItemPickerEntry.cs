using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000384 RID: 900
	public class ItemPickerEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x06001F62 RID: 8034 RVA: 0x0006E881 File Offset: 0x0006CA81
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0006E89A File Offset: 0x0006CA9A
		private void OnDestroy()
		{
			this.itemDisplay.onPointerClick -= this.OnItemDisplayClicked;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0006E8B3 File Offset: 0x0006CAB3
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData eventData)
		{
			this.master.NotifyEntryClicked(this, this.target);
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0006E8C8 File Offset: 0x0006CAC8
		public void Setup(ItemPicker master, Item item)
		{
			this.master = master;
			this.target = item;
			if (this.target != null)
			{
				this.itemDisplay.Setup(this.target);
			}
			else
			{
				Debug.LogError("Item Picker不应当展示空的Item。");
			}
			this.itemDisplay.ShowOperationButtons = false;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0006E91A File Offset: 0x0006CB1A
		public void NotifyPooled()
		{
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0006E91C File Offset: 0x0006CB1C
		public void NotifyReleased()
		{
		}

		// Token: 0x04001573 RID: 5491
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001574 RID: 5492
		private ItemPicker master;

		// Token: 0x04001575 RID: 5493
		private Item target;
	}
}
