using System;
using Duckov.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

// Token: 0x020001ED RID: 493
public class TaskSkipperUI : MonoBehaviour
{
	// Token: 0x06000E9D RID: 3741 RVA: 0x0003B2E8 File Offset: 0x000394E8
	private void Awake()
	{
		UIInputManager.OnInteractInputContext += this.OnInteractInputContext;
		this.anyButtonListener = InputSystem.onAnyButtonPress.Call(new Action<InputControl>(this.OnAnyButton));
		this.skipped = false;
		this.alpha = 0f;
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0003B334 File Offset: 0x00039534
	private void OnAnyButton(InputControl control)
	{
		this.Show();
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x0003B33C File Offset: 0x0003953C
	private void OnDestroy()
	{
		UIInputManager.OnInteractInputContext -= this.OnInteractInputContext;
		this.anyButtonListener.Dispose();
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0003B35A File Offset: 0x0003955A
	private void OnInteractInputContext(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.pressing = true;
		}
		if (context.canceled)
		{
			this.pressing = false;
		}
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0003B37C File Offset: 0x0003957C
	private void Update()
	{
		this.UpdatePressing();
		this.UpdateFill();
		this.UpdateCanvasGroup();
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0003B390 File Offset: 0x00039590
	private void Show()
	{
		this.show = true;
		this.hideTimer = this.hideAfterSeconds;
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x0003B3A8 File Offset: 0x000395A8
	private void UpdatePressing()
	{
		if (UIInputManager.Instance == null)
		{
			this.pressing = Keyboard.current.fKey.isPressed;
		}
		if (this.pressing && !this.skipped)
		{
			this.pressTime += Time.deltaTime;
			if (this.pressTime >= this.totalTime)
			{
				this.skipped = true;
				this.target.Skip();
			}
			this.Show();
			return;
		}
		if (!this.skipped)
		{
			this.pressTime = Mathf.MoveTowards(this.pressTime, 0f, Time.deltaTime);
		}
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x0003B444 File Offset: 0x00039644
	private void UpdateFill()
	{
		float fillAmount = this.pressTime / this.totalTime;
		this.fill.fillAmount = fillAmount;
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x0003B46C File Offset: 0x0003966C
	private void UpdateCanvasGroup()
	{
		if (this.show)
		{
			this.alpha = Mathf.MoveTowards(this.alpha, 1f, 10f * Time.deltaTime);
			this.hideTimer = Mathf.MoveTowards(this.hideTimer, 0f, Time.deltaTime);
			if (this.hideTimer < 0.01f)
			{
				this.show = false;
			}
		}
		else
		{
			this.alpha = Mathf.MoveTowards(this.alpha, 0f, 10f * Time.deltaTime);
		}
		this.canvasGroup.alpha = this.alpha;
	}

	// Token: 0x04000C23 RID: 3107
	[SerializeField]
	private TaskList target;

	// Token: 0x04000C24 RID: 3108
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04000C25 RID: 3109
	[SerializeField]
	private Image fill;

	// Token: 0x04000C26 RID: 3110
	[SerializeField]
	private float totalTime = 2f;

	// Token: 0x04000C27 RID: 3111
	[SerializeField]
	private float hideAfterSeconds = 2f;

	// Token: 0x04000C28 RID: 3112
	private float pressTime;

	// Token: 0x04000C29 RID: 3113
	private float alpha;

	// Token: 0x04000C2A RID: 3114
	private float hideTimer;

	// Token: 0x04000C2B RID: 3115
	private bool show;

	// Token: 0x04000C2C RID: 3116
	private IDisposable anyButtonListener;

	// Token: 0x04000C2D RID: 3117
	private bool pressing;

	// Token: 0x04000C2E RID: 3118
	private bool skipped;
}
