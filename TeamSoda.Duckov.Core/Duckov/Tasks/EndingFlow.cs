using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Tasks
{
	// Token: 0x02000378 RID: 888
	[Obsolete]
	public class EndingFlow : MonoBehaviour
	{
		// Token: 0x06001EE2 RID: 7906 RVA: 0x0006D1DD File Offset: 0x0006B3DD
		private void Start()
		{
			this.Task().Forget();
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0006D1EC File Offset: 0x0006B3EC
		private UniTask Task()
		{
			EndingFlow.<Task>d__4 <Task>d__;
			<Task>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Task>d__.<>4__this = this;
			<Task>d__.<>1__state = -1;
			<Task>d__.<>t__builder.Start<EndingFlow.<Task>d__4>(ref <Task>d__);
			return <Task>d__.<>t__builder.Task;
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0006D230 File Offset: 0x0006B430
		private UniTask WaitForTaskBehaviour(MonoBehaviour mono)
		{
			EndingFlow.<WaitForTaskBehaviour>d__5 <WaitForTaskBehaviour>d__;
			<WaitForTaskBehaviour>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForTaskBehaviour>d__.mono = mono;
			<WaitForTaskBehaviour>d__.<>1__state = -1;
			<WaitForTaskBehaviour>d__.<>t__builder.Start<EndingFlow.<WaitForTaskBehaviour>d__5>(ref <WaitForTaskBehaviour>d__);
			return <WaitForTaskBehaviour>d__.<>t__builder.Task;
		}

		// Token: 0x0400150C RID: 5388
		[SerializeField]
		private List<MonoBehaviour> taskBehaviours = new List<MonoBehaviour>();

		// Token: 0x0400150D RID: 5389
		[SerializeField]
		private UnityEvent onBegin;

		// Token: 0x0400150E RID: 5390
		[SerializeField]
		private UnityEvent onEnd;
	}
}
