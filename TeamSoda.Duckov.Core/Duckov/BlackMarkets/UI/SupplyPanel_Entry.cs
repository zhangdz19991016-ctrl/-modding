using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x02000311 RID: 785
	public class SupplyPanel_Entry : MonoBehaviour, IPoolable
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060019E8 RID: 6632 RVA: 0x0005E30A File Offset: 0x0005C50A
		// (set) Token: 0x060019E9 RID: 6633 RVA: 0x0005E312 File Offset: 0x0005C512
		public BlackMarket.DemandSupplyEntry Target { get; private set; }

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060019EA RID: 6634 RVA: 0x0005E31C File Offset: 0x0005C51C
		// (remove) Token: 0x060019EB RID: 6635 RVA: 0x0005E354 File Offset: 0x0005C554
		public event Action<SupplyPanel_Entry> onDealButtonClicked;

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x0005E389 File Offset: 0x0005C589
		private string TitleFormatKey
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				if (this.Target.priceFactor <= 0.9f)
				{
					return this.titleFormatKey_Low;
				}
				return this.titleFormatKey_Normal;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x0005E3B8 File Offset: 0x0005C5B8
		private string TitleText
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				return this.TitleFormatKey.ToPlainText().Format(new
				{
					itemName = this.Target.ItemDisplayName
				});
			}
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0005E3E8 File Offset: 0x0005C5E8
		private bool CanInteract()
		{
			return this.Target != null && this.Target.remaining > 0 && this.Target.BuyCost.Enough;
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0005E422 File Offset: 0x0005C622
		public void NotifyPooled()
		{
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0005E424 File Offset: 0x0005C624
		public void NotifyReleased()
		{
			if (this.Target != null)
			{
				this.Target.onChanged -= this.OnChanged;
			}
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x0005E445 File Offset: 0x0005C645
		private void OnChanged(BlackMarket.DemandSupplyEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0005E450 File Offset: 0x0005C650
		internal void Setup(BlackMarket.DemandSupplyEntry target)
		{
			if (target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.Target = target;
			this.costDisplay.Setup(target.BuyCost, 1);
			this.resultDisplay.Setup(target.ItemID, (long)target.ItemMetaData.defaultStackCount);
			this.titleDisplay.text = this.TitleText;
			this.Refresh();
			target.onChanged += this.OnChanged;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0005E4D0 File Offset: 0x0005C6D0
		private void OnEnable()
		{
			ItemUtilities.OnPlayerItemOperation += this.Refresh;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x0005E4E3 File Offset: 0x0005C6E3
		private void OnDisable()
		{
			ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0005E4F6 File Offset: 0x0005C6F6
		private void Awake()
		{
			this.dealButton.onClick.AddListener(new UnityAction(this.OnDealButtonClicked));
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0005E514 File Offset: 0x0005C714
		private void OnDealButtonClicked()
		{
			Action<SupplyPanel_Entry> action = this.onDealButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0005E528 File Offset: 0x0005C728
		private void Refresh()
		{
			if (this.Target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.remainingAmountDisplay.text = string.Format("{0}", this.Target.Remaining);
			bool active = this.CanInteract();
			this.canInteractIndicator.SetActive(active);
			bool active2 = this.Target.Remaining <= 0;
			this.outOfStockIndicator.SetActive(active2);
			this.remainingInfoContainer.SetActive(this.Target.remaining > 1);
		}

		// Token: 0x040012B4 RID: 4788
		[SerializeField]
		private TextMeshProUGUI titleDisplay;

		// Token: 0x040012B5 RID: 4789
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x040012B6 RID: 4790
		[SerializeField]
		private ItemAmountDisplay resultDisplay;

		// Token: 0x040012B7 RID: 4791
		[SerializeField]
		private GameObject remainingInfoContainer;

		// Token: 0x040012B8 RID: 4792
		[SerializeField]
		private TextMeshProUGUI remainingAmountDisplay;

		// Token: 0x040012B9 RID: 4793
		[SerializeField]
		private GameObject canInteractIndicator;

		// Token: 0x040012BA RID: 4794
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x040012BB RID: 4795
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Normal = "BlackMarket_Supply_Title_Normal";

		// Token: 0x040012BC RID: 4796
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Low = "BlackMarket_Supply_Title_Low";

		// Token: 0x040012BD RID: 4797
		[SerializeField]
		private Button dealButton;
	}
}
