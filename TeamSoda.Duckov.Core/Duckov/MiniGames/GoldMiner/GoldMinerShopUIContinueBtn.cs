using System;
using Duckov.MiniGames.GoldMiner.UI;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A4 RID: 676
	public class GoldMinerShopUIContinueBtn : MonoBehaviour
	{
		// Token: 0x06001631 RID: 5681 RVA: 0x0005271C File Offset: 0x0005091C
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x00052769 File Offset: 0x00050969
		private void OnInteract(NavEntry entry)
		{
			this.shop.Continue();
		}

		// Token: 0x0400105D RID: 4189
		[SerializeField]
		private GoldMinerShop shop;

		// Token: 0x0400105E RID: 4190
		[SerializeField]
		private NavEntry navEntry;
	}
}
