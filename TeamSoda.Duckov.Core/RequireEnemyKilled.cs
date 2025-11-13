using System;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class RequireEnemyKilled : Condition
{
	// Token: 0x06000996 RID: 2454 RVA: 0x0002A2FF File Offset: 0x000284FF
	public override bool Evaluate()
	{
		return !(this.enemyPreset == null) && SavesCounter.GetKillCount(this.enemyPreset.nameKey) >= this.threshold;
	}

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private CharacterRandomPreset enemyPreset;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private int threshold = 1;
}
