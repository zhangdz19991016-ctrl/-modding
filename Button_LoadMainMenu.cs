using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200016E RID: 366
public class Button_LoadMainMenu : MonoBehaviour
{
	// Token: 0x06000B1B RID: 2843 RVA: 0x0002FC73 File Offset: 0x0002DE73
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.BeginQuitting));
		this.dialogue.SkipHide();
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x0002FC9C File Offset: 0x0002DE9C
	private void BeginQuitting()
	{
		if (this.task.Status == UniTaskStatus.Pending)
		{
			return;
		}
		Debug.Log("Quitting");
		this.task = this.QuitTask();
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x0002FCC4 File Offset: 0x0002DEC4
	private UniTask QuitTask()
	{
		Button_LoadMainMenu.<QuitTask>d__5 <QuitTask>d__;
		<QuitTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<QuitTask>d__.<>4__this = this;
		<QuitTask>d__.<>1__state = -1;
		<QuitTask>d__.<>t__builder.Start<Button_LoadMainMenu.<QuitTask>d__5>(ref <QuitTask>d__);
		return <QuitTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	private Button button;

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private ConfirmDialogue dialogue;

	// Token: 0x04000999 RID: 2457
	private UniTask task;
}
