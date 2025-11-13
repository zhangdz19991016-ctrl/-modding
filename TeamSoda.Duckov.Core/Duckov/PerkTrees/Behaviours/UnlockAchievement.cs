using System;
using Duckov.Achievements;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025E RID: 606
	public class UnlockAchievement : PerkBehaviour
	{
		// Token: 0x060012FF RID: 4863 RVA: 0x00047E9E File Offset: 0x0004609E
		protected override void OnUnlocked()
		{
			if (AchievementManager.Instance == null)
			{
				return;
			}
			AchievementManager.Instance.Unlock(this.achievementKey.Trim());
		}

		// Token: 0x04000E52 RID: 3666
		[SerializeField]
		private string achievementKey;
	}
}
