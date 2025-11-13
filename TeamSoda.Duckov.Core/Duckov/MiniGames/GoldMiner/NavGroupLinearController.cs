using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AB RID: 683
	public class NavGroupLinearController : MiniGameBehaviour
	{
		// Token: 0x06001663 RID: 5731 RVA: 0x00052FE0 File Offset: 0x000511E0
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			NavGroup.OnNavGroupChanged = (Action)Delegate.Combine(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00053034 File Offset: 0x00051234
		private void OnLevelBegin(GoldMiner miner)
		{
			if (this.setActiveWhenLevelBegin)
			{
				this.navGroup.SetAsActiveNavGroup();
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00053049 File Offset: 0x00051249
		private void OnNavGroupChanged()
		{
			this.changeLock = true;
		}

		// Token: 0x0400108D RID: 4237
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400108E RID: 4238
		[SerializeField]
		private NavGroup navGroup;

		// Token: 0x0400108F RID: 4239
		[SerializeField]
		private NavGroup otherNavGroup;

		// Token: 0x04001090 RID: 4240
		[SerializeField]
		private bool setActiveWhenLevelBegin;

		// Token: 0x04001091 RID: 4241
		private bool changeLock;
	}
}
