using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Duckov.Tasks
{
	// Token: 0x0200037B RID: 891
	public class PlayTimelineTask : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001EF3 RID: 7923 RVA: 0x0006D3D8 File Offset: 0x0006B5D8
		private void Awake()
		{
			this.timeline.stopped += this.OnTimelineStopped;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0006D3F1 File Offset: 0x0006B5F1
		private void OnDestroy()
		{
			if (this.timeline != null)
			{
				this.timeline.stopped -= this.OnTimelineStopped;
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0006D418 File Offset: 0x0006B618
		private void OnTimelineStopped(PlayableDirector director)
		{
			this.running = false;
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0006D421 File Offset: 0x0006B621
		public void Begin()
		{
			this.running = true;
			this.timeline.Play();
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0006D435 File Offset: 0x0006B635
		public bool IsComplete()
		{
			return this.timeline.time > this.timeline.duration - 0.009999999776482582 || this.timeline.state != PlayState.Playing;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0006D46C File Offset: 0x0006B66C
		public bool IsPending()
		{
			return this.timeline.time <= this.timeline.duration - 0.009999999776482582 && this.timeline.state == PlayState.Playing;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0006D4A0 File Offset: 0x0006B6A0
		public void Skip()
		{
			this.timeline.Stop();
		}

		// Token: 0x0400151F RID: 5407
		[SerializeField]
		private PlayableDirector timeline;

		// Token: 0x04001520 RID: 5408
		private bool running;
	}
}
