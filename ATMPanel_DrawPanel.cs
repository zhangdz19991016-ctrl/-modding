using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019C RID: 412
public class ATMPanel_DrawPanel : MonoBehaviour
{
	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06000C31 RID: 3121 RVA: 0x000341E0 File Offset: 0x000323E0
	// (remove) Token: 0x06000C32 RID: 3122 RVA: 0x00034218 File Offset: 0x00032418
	public event Action<ATMPanel_DrawPanel> onQuit;

	// Token: 0x06000C33 RID: 3123 RVA: 0x0003424D File Offset: 0x0003244D
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		this.Refresh();
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x00034266 File Offset: 0x00032466
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x0003427C File Offset: 0x0003247C
	private void Awake()
	{
		this.inputPanel.onInputFieldValueChanged += this.OnInputValueChanged;
		this.inputPanel.maxFunction = delegate()
		{
			long num = EconomyManager.Money;
			if (num > 10000000L)
			{
				num = 10000000L;
			}
			return num;
		};
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00034302 File Offset: 0x00032502
	private void OnQuitButtonClicked()
	{
		Action<ATMPanel_DrawPanel> action = this.onQuit;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00034315 File Offset: 0x00032515
	private void OnMoneyChanged(long arg1, long arg2)
	{
		this.Refresh();
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x00034320 File Offset: 0x00032520
	private void OnConfirmButtonClicked()
	{
		if (this.inputPanel.Value <= 0L)
		{
			this.inputPanel.Clear();
			return;
		}
		long num = EconomyManager.Money;
		if (num > 10000000L)
		{
			num = 10000000L;
		}
		if (this.inputPanel.Value > num)
		{
			return;
		}
		this.DrawTask(this.inputPanel.Value).Forget();
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00034384 File Offset: 0x00032584
	private UniTask DrawTask(long value)
	{
		ATMPanel_DrawPanel.<DrawTask>d__14 <DrawTask>d__;
		<DrawTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DrawTask>d__.<>4__this = this;
		<DrawTask>d__.value = value;
		<DrawTask>d__.<>1__state = -1;
		<DrawTask>d__.<>t__builder.Start<ATMPanel_DrawPanel.<DrawTask>d__14>(ref <DrawTask>d__);
		return <DrawTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x000343CF File Offset: 0x000325CF
	private void OnInputValueChanged(string v)
	{
		this.Refresh();
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000343D8 File Offset: 0x000325D8
	private void Refresh()
	{
		bool flag = EconomyManager.Money >= this.inputPanel.Value;
		flag &= (this.inputPanel.Value <= 10000000L);
		flag &= (this.inputPanel.Value >= 0L);
		this.insufficientIndicator.SetActive(!flag);
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00034438 File Offset: 0x00032638
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00034445 File Offset: 0x00032645
	internal void Hide(bool skip)
	{
		if (skip)
		{
			this.fadeGroup.SkipHide();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x04000AAD RID: 2733
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000AAE RID: 2734
	[SerializeField]
	private DigitInputPanel inputPanel;

	// Token: 0x04000AAF RID: 2735
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000AB0 RID: 2736
	[SerializeField]
	private GameObject insufficientIndicator;

	// Token: 0x04000AB1 RID: 2737
	[SerializeField]
	private Button quitButton;
}
