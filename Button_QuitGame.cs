using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016F RID: 367
public class Button_QuitGame : MonoBehaviour
{
	// Token: 0x06000B1F RID: 2847 RVA: 0x0002FD0F File Offset: 0x0002DF0F
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.BeginQuitting));
		if (this.dialogue)
		{
			this.dialogue.SkipHide();
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0002FD45 File Offset: 0x0002DF45
	private void BeginQuitting()
	{
		if (this.task.Status == UniTaskStatus.Pending)
		{
			return;
		}
		Debug.Log("Quitting");
		this.task = this.QuitTask();
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0002FD6C File Offset: 0x0002DF6C
	private UniTask QuitTask()
	{
		Button_QuitGame.<QuitTask>d__5 <QuitTask>d__;
		<QuitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<QuitTask>d__.<>4__this = this;
		<QuitTask>d__.<>1__state = -1;
		<QuitTask>d__.<>t__builder.Start<Button_QuitGame.<QuitTask>d__5>(ref <QuitTask>d__);
		return <QuitTask>d__.<>t__builder.Task;
	}

	// Token: 0x0400099A RID: 2458
	[SerializeField]
	private Button button;

	// Token: 0x0400099B RID: 2459
	[SerializeField]
	private ConfirmDialogue dialogue;

	// Token: 0x0400099C RID: 2460
	private UniTask task;
}
