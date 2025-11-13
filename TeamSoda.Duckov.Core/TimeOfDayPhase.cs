using System;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x02000195 RID: 405
[Serializable]
public struct TimeOfDayPhase
{
	// Token: 0x04000A7A RID: 2682
	[FormerlySerializedAs("phaseTag")]
	public TimePhaseTags timePhaseTag;

	// Token: 0x04000A7B RID: 2683
	public VolumeProfile volumeProfile;
}
