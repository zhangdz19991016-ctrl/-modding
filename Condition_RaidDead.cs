using System;
using Duckov.Quests;

// Token: 0x02000119 RID: 281
public class Condition_RaidDead : Condition
{
	// Token: 0x0600098B RID: 2443 RVA: 0x0002A19A File Offset: 0x0002839A
	public override bool Evaluate()
	{
		return RaidUtilities.CurrentRaid.dead;
	}
}
