using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A0 RID: 672
	public class BombDisplay : MonoBehaviour
	{
		// Token: 0x0600161D RID: 5661 RVA: 0x000523B0 File Offset: 0x000505B0
		private void Awake()
		{
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			GoldMiner goldMiner = this.master;
			goldMiner.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyLevelPlayTick));
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0005240C File Offset: 0x0005060C
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
			this.amountText.text = string.Format("{0}", this.master.run.bomb);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x00052460 File Offset: 0x00050660
		private void OnInteract(NavEntry entry)
		{
			this.master.UseBomb();
		}

		// Token: 0x0400104D RID: 4173
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400104E RID: 4174
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x0400104F RID: 4175
		[SerializeField]
		private NavEntry navEntry;
	}
}
