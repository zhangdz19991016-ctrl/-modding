using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
[Serializable]
public struct CharacterRandomPresetInfo
{
	// Token: 0x040004AF RID: 1199
	public CharacterRandomPreset randomPreset;

	// Token: 0x040004B0 RID: 1200
	[Range(0f, 1f)]
	public float weight;
}
