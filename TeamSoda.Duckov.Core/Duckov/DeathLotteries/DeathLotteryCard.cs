using System;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.DeathLotteries
{
	// Token: 0x0200030A RID: 778
	public class DeathLotteryCard : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x0005CD74 File Offset: 0x0005AF74
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0005CD7C File Offset: 0x0005AF7C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.Target == null)
			{
				return;
			}
			DeathLottery.OptionalCosts cost = this.master.Target.GetCost();
			this.master.NotifyEntryClicked(this, cost.costA);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0005CDD0 File Offset: 0x0005AFD0
		public void Setup(DeathLotteryVIew master, int index)
		{
			if (master == null)
			{
				return;
			}
			if (master.Target == null)
			{
				return;
			}
			this.master = master;
			this.targetItem = master.Target.ItemInstances[index];
			this.index = index;
			this.itemDisplay.Setup(this.targetItem);
			this.cardDisplay.SetFacing(master.Target.CurrentStatus.selectedItems.Contains(index), true);
			this.Refresh();
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0005CE54 File Offset: 0x0005B054
		public void NotifyFacing(bool uncovered)
		{
			this.cardDisplay.SetFacing(uncovered, false);
			this.Refresh();
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0005CE6C File Offset: 0x0005B06C
		private bool Selected
		{
			get
			{
				return !(this.master == null) && !(this.master.Target == null) && this.master.Target.CurrentStatus.selectedItems != null && this.master.Target.CurrentStatus.selectedItems.Contains(this.Index);
			}
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0005CED7 File Offset: 0x0005B0D7
		private void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0005CEEA File Offset: 0x0005B0EA
		private void Awake()
		{
			this.costFade.Hide();
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0005CEF8 File Offset: 0x0005B0F8
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.master.Target.CurrentStatus.SelectedCount >= this.master.Target.MaxChances)
			{
				return;
			}
			Cost costA = this.master.Target.GetCost().costA;
			this.costDisplay.Setup(costA, 1);
			this.freeIndicator.SetActive(costA.IsFree);
			this.costFade.Show();
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0005CF70 File Offset: 0x0005B170
		public void OnPointerExit(PointerEventData eventData)
		{
			this.costFade.Hide();
		}

		// Token: 0x0400126C RID: 4716
		[SerializeField]
		private CardDisplay cardDisplay;

		// Token: 0x0400126D RID: 4717
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x0400126E RID: 4718
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x0400126F RID: 4719
		[SerializeField]
		private GameObject freeIndicator;

		// Token: 0x04001270 RID: 4720
		[SerializeField]
		private FadeGroup costFade;

		// Token: 0x04001271 RID: 4721
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001272 RID: 4722
		private DeathLotteryVIew master;

		// Token: 0x04001273 RID: 4723
		private int index;

		// Token: 0x04001274 RID: 4724
		private Item targetItem;
	}
}
