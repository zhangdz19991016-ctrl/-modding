using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A6 RID: 678
	public class GoldMinerShopUIRefreshBtn : MonoBehaviour
	{
		// Token: 0x0600163B RID: 5691 RVA: 0x00052938 File Offset: 0x00050B38
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMinerShop goldMinerShop = this.shop;
			goldMinerShop.onAfterOperation = (Action)Delegate.Combine(goldMinerShop.onAfterOperation, new Action(this.OnAfterOperation));
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000529AC File Offset: 0x00050BAC
		private void OnEnable()
		{
			this.RefreshCostText();
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000529B4 File Offset: 0x00050BB4
		private void OnAfterOperation()
		{
			this.RefreshCostText();
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000529BC File Offset: 0x00050BBC
		private void RefreshCostText()
		{
			this.costText.text = string.Format("${0}", this.shop.GetRefreshCost());
			this.refreshChanceText.text = string.Format("{0}", this.shop.refreshChance);
			this.noChanceIndicator.SetActive(this.shop.refreshChance < 1);
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x00052A2C File Offset: 0x00050C2C
		private void OnInteract(NavEntry entry)
		{
			this.shop.TryRefresh();
		}

		// Token: 0x0400106B RID: 4203
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x0400106C RID: 4204
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x0400106D RID: 4205
		[SerializeField]
		private TextMeshProUGUI costText;

		// Token: 0x0400106E RID: 4206
		[SerializeField]
		private TextMeshProUGUI refreshChanceText;

		// Token: 0x0400106F RID: 4207
		[SerializeField]
		private GameObject noChanceIndicator;
	}
}
