using System;
using Duckov;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C7 RID: 199
public class ActionProgressHUD : MonoBehaviour
{
	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000651 RID: 1617 RVA: 0x0001C809 File Offset: 0x0001AA09
	public bool InProgress
	{
		get
		{
			return this.inProgress;
		}
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0001C814 File Offset: 0x0001AA14
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnActionStartEvent += this.OnActionStart;
				this.characterMainControl.OnActionProgressFinishEvent += this.OnActionFinish;
			}
		}
		this.inProgress = false;
		float num = 0f;
		if (this.currentProgressInterface as UnityEngine.Object != null)
		{
			Progress progress = this.currentProgressInterface.GetProgress();
			this.inProgress = progress.inProgress;
			num = progress.progress;
			if (!this.inProgress)
			{
				this.currentProgressInterface = null;
			}
		}
		if (this.inProgress)
		{
			this.targetAlpha = 1f;
			this.fillImage.fillAmount = num;
			if (num >= 1f)
			{
				this.targetAlpha = 0f;
			}
		}
		else
		{
			this.targetAlpha = 0f;
		}
		this.parentCanvasGroup.alpha = Mathf.MoveTowards(this.parentCanvasGroup.alpha, this.targetAlpha, 8f * Time.deltaTime);
		if (this.stopIndicator && this.characterMainControl)
		{
			bool flag = false;
			CharacterActionBase currentAction = this.characterMainControl.CurrentAction;
			if (currentAction && currentAction.Running && currentAction.IsStopable())
			{
				flag = true;
			}
			if (flag != this.stopIndicator.activeSelf && this.targetAlpha != 0f)
			{
				this.stopIndicator.SetActive(flag);
			}
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0001C99C File Offset: 0x0001AB9C
	private void OnDestroy()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.OnActionStartEvent -= this.OnActionStart;
			this.characterMainControl.OnActionProgressFinishEvent -= this.OnActionFinish;
		}
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0001C9DC File Offset: 0x0001ABDC
	private void OnActionStart(CharacterActionBase action)
	{
		this.currentProgressInterface = (action as IProgress);
		if (this.specificActionType != CharacterActionBase.ActionPriorities.Whatever && action.ActionPriority() != this.specificActionType)
		{
			this.currentProgressInterface = null;
		}
		if (action && !action.progressHUD)
		{
			this.currentProgressInterface = null;
		}
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0001CA29 File Offset: 0x0001AC29
	private void OnActionFinish(CharacterActionBase action)
	{
		UnityEvent onFinishEvent = this.OnFinishEvent;
		if (onFinishEvent != null)
		{
			onFinishEvent.Invoke();
		}
		if (this.fillImage)
		{
			this.fillImage.fillAmount = 1f;
		}
	}

	// Token: 0x04000608 RID: 1544
	public CharacterActionBase.ActionPriorities specificActionType;

	// Token: 0x04000609 RID: 1545
	public ProceduralImage fillImage;

	// Token: 0x0400060A RID: 1546
	public CanvasGroup parentCanvasGroup;

	// Token: 0x0400060B RID: 1547
	private CharacterMainControl characterMainControl;

	// Token: 0x0400060C RID: 1548
	private IProgress currentProgressInterface;

	// Token: 0x0400060D RID: 1549
	private float targetAlpha;

	// Token: 0x0400060E RID: 1550
	private bool inProgress;

	// Token: 0x0400060F RID: 1551
	public UnityEvent OnFinishEvent;

	// Token: 0x04000610 RID: 1552
	[FormerlySerializedAs("cancleIndicator")]
	public GameObject stopIndicator;
}
