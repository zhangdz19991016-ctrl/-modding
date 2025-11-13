using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
[Serializable]
public struct EvacuationInfo
{
	// Token: 0x06000932 RID: 2354 RVA: 0x000293A2 File Offset: 0x000275A2
	public EvacuationInfo(string subsceneID, Vector3 position)
	{
		this.subsceneID = subsceneID;
		this.position = position;
	}

	// Token: 0x04000852 RID: 2130
	public string subsceneID;

	// Token: 0x04000853 RID: 2131
	public Vector3 position;
}
