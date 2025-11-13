using System;
using Duckov;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200007D RID: 125
public class SetEndingMissleParameter : MonoBehaviour
{
	// Token: 0x060004BC RID: 1212 RVA: 0x00015BF8 File Offset: 0x00013DF8
	private void Start()
	{
		bool flag = this.launcherClosedCondition.Evaluate();
		AudioManager.SetRTPC("Ending_Missile", (float)(flag ? 1 : 0), null);
	}

	// Token: 0x04000402 RID: 1026
	[SerializeField]
	private Condition launcherClosedCondition;
}
