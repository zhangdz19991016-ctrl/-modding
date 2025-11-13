using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class LogOnEnableAndDisable : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x0002BD6C File Offset: 0x00029F6C
	private void OnEnable()
	{
		Debug.Log("OnEnable");
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002BD78 File Offset: 0x00029F78
	private void OnDisable()
	{
		Debug.Log("OnDisable");
	}
}
