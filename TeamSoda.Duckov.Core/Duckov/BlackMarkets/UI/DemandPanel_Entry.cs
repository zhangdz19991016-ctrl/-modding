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
	// Token: 0x0200030F RID: 783
	public class DemandPanel_Entry : MonoBehaviour, IPoolable
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x0005DE8A File Offset: 0x0005C08A
		// (set) Token: 0x060019CB RID: 6603 RVA: 0x0005DE92 File Offset: 0x0005C092
		public BlackMarket.DemandSupplyEntry Target { get; private set; }

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x060019CC RID: 6604 RVA: 0x0005DE9C File Offset: 0x0005C09C
		// (remove) Token: 0x060019CD RID: 6605 RVA: 0x0005DED4 File Offset: 0x0005C0D4
		public event Action<DemandPanel_Entry> onDealButtonClicked;

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x0005DF09 File Offset: 0x0005C109
		private string TitleFormatKey
		{
			get
			{
				if (this.Target == null)
				{
					return "?";
				}
				if (this.Target.priceFactor >= 1.9f)
				{
					return this.titleFormatKey_High;
				}
				return this.titleFormatKey_Normal;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0005DF38 File Offset: 0x0005C138
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

		// Token: 0x060019D0 RID: 6608 RVA: 0x0005DF68 File Offset: 0x0005C168
		private bool CanInteract()
		{
			return this.Target != null && this.Target.remaining > 0 && this.Target.SellCost.Enough;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0005DFA2 File Offset: 0x0005C1A2
		public void NotifyPooled()
		{
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0005DFA4 File Offset: 0x0005C1A4
		public void NotifyReleased()
		{
			if (this.Target != null)
			{
				this.Target.onChanged -= this.OnChanged;
			}
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0005DFC5 File Offset: 0x0005C1C5
		private void OnChanged(BlackMarket.DemandSupplyEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0005DFCD File Offset: 0x0005C1CD
		public void OnDealButtonClicked()
		{
			Action<DemandPanel_Entry> action = this.onDealButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0005DFE0 File Offset: 0x0005C1E0
		internal void Setup(BlackMarket.DemandSupplyEntry target)
		{
			if (target == null)
			{
				Debug.LogError("找不到对象", base.gameObject);
				return;
			}
			this.Target = target;
			this.costDisplay.Setup(target.SellCost, 1);
			this.moneyDisplay.text = string.Format("{0}", target.TotalPrice);
			this.titleDisplay.text = this.TitleText;
			this.Refresh();
			target.onChanged += this.OnChanged;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0005E063 File Offset: 0x0005C263
		private void OnEnable()
		{
			ItemUtilities.OnPlayerItemOperation += this.Refresh;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0005E076 File Offset: 0x0005C276
		private void OnDisable()
		{
			ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0005E089 File Offset: 0x0005C289
		private void Awake()
		{
			this.dealButton.onClick.AddListener(new UnityAction(this.OnDealButtonClicked));
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x0005E0A8 File Offset: 0x0005C2A8
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

		// Token: 0x040012A5 RID: 4773
		[SerializeField]
		private TextMeshProUGUI titleDisplay;

		// Token: 0x040012A6 RID: 4774
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x040012A7 RID: 4775
		[SerializeField]
		private TextMeshProUGUI moneyDisplay;

		// Token: 0x040012A8 RID: 4776
		[SerializeField]
		private GameObject remainingInfoContainer;

		// Token: 0x040012A9 RID: 4777
		[SerializeField]
		private TextMeshProUGUI remainingAmountDisplay;

		// Token: 0x040012AA RID: 4778
		[SerializeField]
		private GameObject canInteractIndicator;

		// Token: 0x040012AB RID: 4779
		[SerializeField]
		private GameObject outOfStockIndicator;

		// Token: 0x040012AC RID: 4780
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_Normal = "BlackMarket_Demand_Title_Normal";

		// Token: 0x040012AD RID: 4781
		[SerializeField]
		[LocalizationKey("UIText")]
		private string titleFormatKey_High = "BlackMarket_Demand_Title_High";

		// Token: 0x040012AE RID: 4782
		[SerializeField]
		private Button dealButton;
	}
}
