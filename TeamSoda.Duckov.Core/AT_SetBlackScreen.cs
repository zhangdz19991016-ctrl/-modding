using System;
using Cysharp.Threading.Tasks;
using Duckov.UI;
using NodeCanvas.Framework;

// Token: 0x020001B4 RID: 436
public class AT_SetBlackScreen : ActionTask
{
	// Token: 0x06000D06 RID: 3334 RVA: 0x00036ACE File Offset: 0x00034CCE
	protected override void OnExecute()
	{
		if (this.show)
		{
			this.task = BlackScreen.ShowAndReturnTask(null, 0f, 0.5f);
			return;
		}
		this.task = BlackScreen.HideAndReturnTask(null, 0f, 0.5f);
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x00036B05 File Offset: 0x00034D05
	protected override void OnUpdate()
	{
		if (this.task.Status != UniTaskStatus.Pending)
		{
			base.EndAction();
		}
	}

	// Token: 0x04000B44 RID: 2884
	public bool show;

	// Token: 0x04000B45 RID: 2885
	private UniTask task;
}
