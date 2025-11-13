using System;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class EvacuationCountdownUIProxy : MonoBehaviour
{
	// Token: 0x06000968 RID: 2408 RVA: 0x00029F0B File Offset: 0x0002810B
	public void Request(CountDownArea target)
	{
		EvacuationCountdownUI.Request(target);
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x00029F13 File Offset: 0x00028113
	public void Release(CountDownArea target)
	{
		EvacuationCountdownUI.Release(target);
	}
}
