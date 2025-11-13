using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishing.UI
{
	// Token: 0x0200021A RID: 538
	public class BaitSelectPanelEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0003F80D File Offset: 0x0003DA0D
		public Item Target
		{
			get
			{
				return this.targetItem;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0003F815 File Offset: 0x0003DA15
		private bool Selected
		{
			get
			{
				return !(this.master == null) && this.master.GetSelection() == this;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0003F838 File Offset: 0x0003DA38
		internal void Setup(BaitSelectPanel master, Item cur)
		{
			this.UnregisterEvents();
			this.master = master;
			this.targetItem = cur;
			this.itemDisplay.Setup(this.targetItem);
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003F86B File Offset: 0x0003DA6B
		private void RegisterEvents()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetSelection += this.Refresh;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003F893 File Offset: 0x0003DA93
		private void UnregisterEvents()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetSelection -= this.Refresh;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0003F8BB File Offset: 0x0003DABB
		private void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0003F8CE File Offset: 0x0003DACE
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnPointerClick;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003F8E7 File Offset: 0x0003DAE7
		public void OnPointerClick(PointerEventData eventData)
		{
			eventData.Use();
			this.master.NotifySelect(this);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0003F8FB File Offset: 0x0003DAFB
		private void OnPointerClick(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
		}

		// Token: 0x04000CF9 RID: 3321
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04000CFA RID: 3322
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04000CFB RID: 3323
		private BaitSelectPanel master;

		// Token: 0x04000CFC RID: 3324
		private Item targetItem;
	}
}
