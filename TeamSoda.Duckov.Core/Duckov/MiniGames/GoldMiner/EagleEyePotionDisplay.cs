using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A1 RID: 673
	public class EagleEyePotionDisplay : MonoBehaviour
	{
		// Token: 0x06001621 RID: 5665 RVA: 0x00052478 File Offset: 0x00050678
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x000524D4 File Offset: 0x000506D4
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
			this.amountText.text = string.Format("{0}", this.master.run.eagleEyePotion);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00052528 File Offset: 0x00050728
		private void OnInteract(NavEntry entry)
		{
			this.master.UseEagleEyePotion();
		}

		// Token: 0x04001050 RID: 4176
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001051 RID: 4177
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x04001052 RID: 4178
		[SerializeField]
		private NavEntry navEntry;
	}
}
