using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x02000379 RID: 889
	public class TaskList : MonoBehaviour, ITaskBehaviour
	{
		// Token: 0x06001EE6 RID: 7910 RVA: 0x0006D286 File Offset: 0x0006B486
		private void Start()
		{
			if (this.beginOnStart)
			{
				this.Begin();
			}
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x0006D298 File Offset: 0x0006B498
		private UniTask MainTask()
		{
			TaskList.<MainTask>d__10 <MainTask>d__;
			<MainTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<MainTask>d__.<>4__this = this;
			<MainTask>d__.<>1__state = -1;
			<MainTask>d__.<>t__builder.Start<TaskList.<MainTask>d__10>(ref <MainTask>d__);
			return <MainTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x0006D2DB File Offset: 0x0006B4DB
		public void Begin()
		{
			if (this.running)
			{
				return;
			}
			this.skip = false;
			this.running = true;
			this.complete = false;
			UnityEvent unityEvent = this.onBegin;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.MainTask().Forget();
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x0006D317 File Offset: 0x0006B517
		public bool IsComplete()
		{
			return this.complete;
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x0006D31F File Offset: 0x0006B51F
		public bool IsPending()
		{
			return this.running;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x0006D327 File Offset: 0x0006B527
		public void Skip()
		{
			this.skip = true;
		}

		// Token: 0x0400150F RID: 5391
		[SerializeField]
		private bool beginOnStart;

		// Token: 0x04001510 RID: 5392
		[SerializeField]
		private List<MonoBehaviour> tasks;

		// Token: 0x04001511 RID: 5393
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x04001512 RID: 5394
		[SerializeField]
		private UnityEvent onComplete;

		// Token: 0x04001513 RID: 5395
		[SerializeField]
		private bool listenToSkipSignal;

		// Token: 0x04001514 RID: 5396
		private bool running;

		// Token: 0x04001515 RID: 5397
		private bool complete;

		// Token: 0x04001516 RID: 5398
		private int currentTaskIndex;

		// Token: 0x04001517 RID: 5399
		private ITaskBehaviour currentTask;

		// Token: 0x04001518 RID: 5400
		private bool skip;
	}
}
