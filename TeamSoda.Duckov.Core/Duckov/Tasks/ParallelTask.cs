using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x0200037A RID: 890
	public class ParallelTask : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001EED RID: 7917 RVA: 0x0006D338 File Offset: 0x0006B538
		private void Start()
		{
			if (this.beginOnStart)
			{
				this.Begin();
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x0006D348 File Offset: 0x0006B548
		private UniTask MainTask()
		{
			ParallelTask.<MainTask>d__7 <MainTask>d__;
			<MainTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<MainTask>d__.<>4__this = this;
			<MainTask>d__.<>1__state = -1;
			<MainTask>d__.<>t__builder.Start<ParallelTask.<MainTask>d__7>(ref <MainTask>d__);
			return <MainTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0006D38B File Offset: 0x0006B58B
		public void Begin()
		{
			if (this.running)
			{
				return;
			}
			this.running = true;
			this.complete = false;
			UnityEvent unityEvent = this.onBegin;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.MainTask().Forget();
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0006D3C0 File Offset: 0x0006B5C0
		public bool IsComplete()
		{
			return this.complete;
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0006D3C8 File Offset: 0x0006B5C8
		public bool IsPending()
		{
			return this.running;
		}

		// Token: 0x04001519 RID: 5401
		[SerializeField]
		private bool beginOnStart;

		// Token: 0x0400151A RID: 5402
		[SerializeField]
		private List<MonoBehaviour> tasks;

		// Token: 0x0400151B RID: 5403
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x0400151C RID: 5404
		[SerializeField]
		private UnityEvent onComplete;

		// Token: 0x0400151D RID: 5405
		private bool running;

		// Token: 0x0400151E RID: 5406
		private bool complete;
	}
}
