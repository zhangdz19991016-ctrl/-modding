using System;
using Duckov;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class LoopSoundWithObject : MonoBehaviour
{
	// Token: 0x060001C0 RID: 448 RVA: 0x000089AB File Offset: 0x00006BAB
	private void Start()
	{
		this.eventInstance = AudioManager.Post(this.sfx, base.gameObject);
		this.stoped = false;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x000089CC File Offset: 0x00006BCC
	public void Stop()
	{
		if (this.stoped)
		{
			return;
		}
		this.stoped = true;
		if (this.eventInstance != null)
		{
			this.eventInstance.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00008A0B File Offset: 0x00006C0B
	private void OnDestroy()
	{
		this.Stop();
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00008A13 File Offset: 0x00006C13
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x0400016C RID: 364
	public string sfx;

	// Token: 0x0400016D RID: 365
	private EventInstance? eventInstance;

	// Token: 0x0400016E RID: 366
	private bool stoped = true;
}
