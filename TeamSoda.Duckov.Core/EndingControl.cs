using System;
using Duckov.Achievements;
using Duckov.Rules.UI;
using Saves;
using UnityEngine;

// Token: 0x020000A0 RID: 160
public class EndingControl : MonoBehaviour
{
	// Token: 0x0600055C RID: 1372 RVA: 0x000180D8 File Offset: 0x000162D8
	public void SetEndingIndex()
	{
		Ending.endingIndex = this.endingIndex;
		AchievementManager instance = AchievementManager.Instance;
		bool flag = SavesSystem.Load<bool>(this.MissleLuncherClosedKey);
		DifficultySelection.UnlockRage();
		if (instance)
		{
			if (this.endingIndex == 0)
			{
				if (!flag)
				{
					instance.Unlock("Ending_0");
					return;
				}
				instance.Unlock("Ending_3");
				return;
			}
			else
			{
				if (!flag)
				{
					instance.Unlock("Ending_1");
					return;
				}
				instance.Unlock("Ending_2");
			}
		}
	}

	// Token: 0x040004CF RID: 1231
	public int endingIndex;

	// Token: 0x040004D0 RID: 1232
	public string MissleLuncherClosedKey = "MissleLuncherClosed";
}
