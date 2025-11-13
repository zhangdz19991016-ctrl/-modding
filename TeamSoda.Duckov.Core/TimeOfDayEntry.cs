using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class TimeOfDayEntry : MonoBehaviour
{
	// Token: 0x06000C04 RID: 3076 RVA: 0x000336FC File Offset: 0x000318FC
	private void Start()
	{
		if (this.phases.Count > 0)
		{
			TimeOfDayPhase value = this.phases[0];
			this.phases[0] = value;
		}
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00033734 File Offset: 0x00031934
	public TimeOfDayPhase GetPhase(TimePhaseTags timePhaseTags)
	{
		for (int i = 0; i < this.phases.Count; i++)
		{
			TimeOfDayPhase timeOfDayPhase = this.phases[i];
			if (timeOfDayPhase.timePhaseTag == timePhaseTags)
			{
				return timeOfDayPhase;
			}
		}
		if (timePhaseTags == TimePhaseTags.dawn)
		{
			return this.GetPhase(TimePhaseTags.day);
		}
		return this.phases[0];
	}

	// Token: 0x04000A75 RID: 2677
	[SerializeField]
	private List<TimeOfDayPhase> phases;
}
