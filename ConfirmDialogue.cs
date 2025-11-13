using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000170 RID: 368
public class ConfirmDialogue : MonoBehaviour
{
	// Token: 0x06000B23 RID: 2851 RVA: 0x0002FDB7 File Offset: 0x0002DFB7
	private void Awake()
	{
		this.btnConfirm.onClick.AddListener(new UnityAction(this.OnConfirmed));
		this.btnCancel.onClick.AddListener(new UnityAction(this.OnCanceled));
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0002FDF1 File Offset: 0x0002DFF1
	private void OnCanceled()
	{
		this.canceled = true;
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0002FDFA File Offset: 0x0002DFFA
	private void OnConfirmed()
	{
		this.confirmed = true;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0002FE04 File Offset: 0x0002E004
	public UniTask<bool> Execute()
	{
		ConfirmDialogue.<Execute>d__9 <Execute>d__;
		<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Execute>d__.<>4__this = this;
		<Execute>d__.<>1__state = -1;
		<Execute>d__.<>t__builder.Start<ConfirmDialogue.<Execute>d__9>(ref <Execute>d__);
		return <Execute>d__.<>t__builder.Task;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0002FE48 File Offset: 0x0002E048
	private UniTask<bool> DoExecute()
	{
		ConfirmDialogue.<DoExecute>d__10 <DoExecute>d__;
		<DoExecute>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<DoExecute>d__.<>4__this = this;
		<DoExecute>d__.<>1__state = -1;
		<DoExecute>d__.<>t__builder.Start<ConfirmDialogue.<DoExecute>d__10>(ref <DoExecute>d__);
		return <DoExecute>d__.<>t__builder.Task;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0002FE8B File Offset: 0x0002E08B
	internal void SkipHide()
	{
		this.fadeGroup.SkipHide();
	}

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	private Button btnConfirm;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private Button btnCancel;

	// Token: 0x040009A0 RID: 2464
	private bool canceled;

	// Token: 0x040009A1 RID: 2465
	private bool confirmed;

	// Token: 0x040009A2 RID: 2466
	private bool executing;
}
