using System;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class Condition_TimeOfDay : Condition
{
	// Token: 0x0600098D RID: 2445 RVA: 0x0002A1B0 File Offset: 0x000283B0
	public override bool Evaluate()
	{
		float num = (float)GameClock.TimeOfDay.TotalHours % 24f;
		return (num >= this.from && num <= this.to) || (this.to < this.from && (num >= this.from || num <= this.to));
	}

	// Token: 0x0400087B RID: 2171
	[Range(0f, 24f)]
	public float from;

	// Token: 0x0400087C RID: 2172
	[Range(0f, 24f)]
	public float to;
}
