using System;
using Duckov;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class FmodEventTester : MonoBehaviour
{
	// Token: 0x06000C61 RID: 3169 RVA: 0x000349DB File Offset: 0x00032BDB
	public void PlayEvent()
	{
		AudioManager.Post(this.e, base.gameObject);
	}

	// Token: 0x04000AC4 RID: 2756
	[SerializeField]
	private string e;
}
