using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A9 RID: 937
	public class InventoryEntryTradingPriceDisplay : MonoBehaviour
	{
		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x0007539E File Offset: 0x0007359E
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x000753A6 File Offset: 0x000735A6
		public bool Selling
		{
			get
			{
				return this.selling;
			}
			set
			{
				this.selling = value;
			}
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000753AF File Offset: 0x000735AF
		private void Awake()
		{
			this.master.onRefresh += this.OnRefresh;
			TradingUIUtilities.OnActiveMerchantChanged += this.OnActiveMerchantChanged;
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000753D9 File Offset: 0x000735D9
		private void OnActiveMerchantChanged(IMerchant merchant)
		{
			this.Refresh();
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000753E1 File Offset: 0x000735E1
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000753E9 File Offset: 0x000735E9
		private void OnDestroy()
		{
			if (this.master != null)
			{
				this.master.onRefresh -= this.OnRefresh;
			}
			TradingUIUtilities.OnActiveMerchantChanged -= this.OnActiveMerchantChanged;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x00075421 File Offset: 0x00073621
		private void OnRefresh(InventoryEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0007542C File Offset: 0x0007362C
		private void Refresh()
		{
			InventoryEntry inventoryEntry = this.master;
			Item item = (inventoryEntry != null) ? inventoryEntry.Content : null;
			if (item != null)
			{
				this.canvasGroup.alpha = 1f;
				string text = this.GetPrice(item).ToString(this.moneyFormat);
				this.priceText.text = text;
				return;
			}
			this.canvasGroup.alpha = 0f;
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00075498 File Offset: 0x00073698
		private int GetPrice(Item content)
		{
			if (content == null)
			{
				return 0;
			}
			int value = content.Value;
			if (TradingUIUtilities.ActiveMerchant == null)
			{
				return value;
			}
			return TradingUIUtilities.ActiveMerchant.ConvertPrice(content, this.selling);
		}

		// Token: 0x040016B2 RID: 5810
		[SerializeField]
		private InventoryEntry master;

		// Token: 0x040016B3 RID: 5811
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040016B4 RID: 5812
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x040016B5 RID: 5813
		[SerializeField]
		private bool selling = true;

		// Token: 0x040016B6 RID: 5814
		[SerializeField]
		private string moneyFormat = "n0";
	}
}
