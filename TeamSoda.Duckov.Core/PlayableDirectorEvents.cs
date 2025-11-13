using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// Token: 0x0200013C RID: 316
public class PlayableDirectorEvents : MonoBehaviour
{
	// Token: 0x06000A2D RID: 2605 RVA: 0x0002BF34 File Offset: 0x0002A134
	private void OnEnable()
	{
		this.playableDirector.played += this.OnPlayed;
		this.playableDirector.paused += this.OnPaused;
		this.playableDirector.stopped += this.OnStopped;
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0002BF88 File Offset: 0x0002A188
	private void OnDisable()
	{
		this.playableDirector.played -= this.OnPlayed;
		this.playableDirector.paused -= this.OnPaused;
		this.playableDirector.stopped -= this.OnStopped;
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0002BFDA File Offset: 0x0002A1DA
	private void OnStopped(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onStopped;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0002BFEC File Offset: 0x0002A1EC
	private void OnPaused(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onPaused;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0002BFFE File Offset: 0x0002A1FE
	private void OnPlayed(PlayableDirector director)
	{
		UnityEvent unityEvent = this.onPlayed;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040008F3 RID: 2291
	[SerializeField]
	private PlayableDirector playableDirector;

	// Token: 0x040008F4 RID: 2292
	[SerializeField]
	private UnityEvent onPlayed;

	// Token: 0x040008F5 RID: 2293
	[SerializeField]
	private UnityEvent onPaused;

	// Token: 0x040008F6 RID: 2294
	[SerializeField]
	private UnityEvent onStopped;
}
