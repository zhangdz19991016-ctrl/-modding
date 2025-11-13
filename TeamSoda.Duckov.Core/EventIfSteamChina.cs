using System;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001EB RID: 491
public class EventIfSteamChina : MonoBehaviour
{
	// Token: 0x06000E98 RID: 3736 RVA: 0x0003B257 File Offset: 0x00039457
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (SteamUtils.IsSteamChinaLauncher())
		{
			this.onStart_IsSteamChina.Invoke();
			return;
		}
		this.onStart_IsNotSteamChina.Invoke();
	}

	// Token: 0x04000C1E RID: 3102
	public UnityEvent onStart_IsSteamChina;

	// Token: 0x04000C1F RID: 3103
	public UnityEvent onStart_IsNotSteamChina;
}
