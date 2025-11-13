using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002B0 RID: 688
	public class StrengthPotionDisplay : MonoBehaviour
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x000537F0 File Offset: 0x000519F0
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0005384C File Offset: 0x00051A4C
		private void OnEarlyLevelPlayTick(GoldMiner miner)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.run == null)
			{
				return;
			}
			this.amountText.text = string.Format("{0}", this.master.run.strengthPotion);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000538A0 File Offset: 0x00051AA0
		private void OnInteract(NavEntry entry)
		{
			this.master.UseStrengthPotion();
		}

		// Token: 0x040010AA RID: 4266
		[SerializeField]
		private GoldMiner master;

		// Token: 0x040010AB RID: 4267
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x040010AC RID: 4268
		[SerializeField]
		private NavEntry navEntry;
	}
}
