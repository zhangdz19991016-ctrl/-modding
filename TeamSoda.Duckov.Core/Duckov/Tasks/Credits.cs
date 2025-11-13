using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.Tasks
{
	// Token: 0x02000377 RID: 887
	public class Credits : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001EDB RID: 7899 RVA: 0x0006D0EE File Offset: 0x0006B2EE
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0006D101 File Offset: 0x0006B301
		public void Begin()
		{
			if (this.task.Status == UniTaskStatus.Pending)
			{
				return;
			}
			this.skip = false;
			this.fadeGroup.SkipHide();
			this.fadeGroup.gameObject.SetActive(true);
			this.task = this.Task();
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x0006D140 File Offset: 0x0006B340
		public bool IsPending()
		{
			return this.task.Status == UniTaskStatus.Pending;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0006D150 File Offset: 0x0006B350
		public bool IsComplete()
		{
			return !this.IsPending();
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0006D15C File Offset: 0x0006B35C
		private UniTask Task()
		{
			Credits.<Task>d__13 <Task>d__;
			<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Task>d__.<>4__this = this;
			<Task>d__.<>1__state = -1;
			<Task>d__.<>t__builder.Start<Credits.<Task>d__13>(ref <Task>d__);
			return <Task>d__.<>t__builder.Task;
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0006D19F File Offset: 0x0006B39F
		public void Skip()
		{
			this.skip = true;
			if (this.fadeOut && this.fadeGroup.IsFading)
			{
				this.fadeGroup.SkipHide();
			}
			if (!this.mute)
			{
				AudioManager.StopBGM();
			}
		}

		// Token: 0x04001503 RID: 5379
		private RectTransform rectTransform;

		// Token: 0x04001504 RID: 5380
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001505 RID: 5381
		[SerializeField]
		private RectTransform content;

		// Token: 0x04001506 RID: 5382
		[SerializeField]
		private float scrollSpeed;

		// Token: 0x04001507 RID: 5383
		[SerializeField]
		private float holdForSeconds;

		// Token: 0x04001508 RID: 5384
		[SerializeField]
		private bool fadeOut;

		// Token: 0x04001509 RID: 5385
		[SerializeField]
		private bool mute;

		// Token: 0x0400150A RID: 5386
		private UniTask task;

		// Token: 0x0400150B RID: 5387
		private bool skip;
	}
}
