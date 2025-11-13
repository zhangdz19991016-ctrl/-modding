using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019D RID: 413
public class ATMPanel_SavePanel : MonoBehaviour
{
	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00034469 File Offset: 0x00032669
	private int CashAmount
	{
		get
		{
			if (this._cachedCashAmount < 0)
			{
				this._cachedCashAmount = ItemUtilities.GetItemCount(451);
			}
			return this._cachedCashAmount;
		}
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06000C40 RID: 3136 RVA: 0x0003448C File Offset: 0x0003268C
	// (remove) Token: 0x06000C41 RID: 3137 RVA: 0x000344C4 File Offset: 0x000326C4
	public event Action<ATMPanel_SavePanel> onQuit;

	// Token: 0x06000C42 RID: 3138 RVA: 0x000344F9 File Offset: 0x000326F9
	private void OnEnable()
	{
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		this.RefreshCash();
		this.Refresh();
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00034518 File Offset: 0x00032718
	private void OnDisable()
	{
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0003452B File Offset: 0x0003272B
	private void OnPlayerItemOperation()
	{
		this.RefreshCash();
		this.Refresh();
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00034539 File Offset: 0x00032739
	private void RefreshCash()
	{
		this._cachedCashAmount = ItemUtilities.GetItemCount(451);
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x0003454C File Offset: 0x0003274C
	private void Awake()
	{
		this.inputPanel.onInputFieldValueChanged += this.OnInputValueChanged;
		this.inputPanel.maxFunction = (() => (long)this.CashAmount);
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x000345BF File Offset: 0x000327BF
	private void OnQuitButtonClicked()
	{
		Action<ATMPanel_SavePanel> action = this.onQuit;
		if (action == null)
		{
			return;
		}
		action(this);
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x000345D4 File Offset: 0x000327D4
	private void OnConfirmButtonClicked()
	{
		if (this.inputPanel.Value <= 0L)
		{
			this.inputPanel.Clear();
			return;
		}
		if (this.inputPanel.Value > (long)this.CashAmount)
		{
			return;
		}
		if (ATMPanel.Save(this.inputPanel.Value))
		{
			this.inputPanel.Clear();
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0003462E File Offset: 0x0003282E
	private void OnInputValueChanged(string v)
	{
		this.Refresh();
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x00034638 File Offset: 0x00032838
	private void Refresh()
	{
		bool flag = (long)this.CashAmount >= this.inputPanel.Value;
		flag &= (this.inputPanel.Value >= 0L);
		this.insufficientIndicator.SetActive(!flag);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x00034681 File Offset: 0x00032881
	internal void Hide(bool skip = false)
	{
		if (skip)
		{
			this.fadeGroup.SkipHide();
			return;
		}
		this.fadeGroup.Hide();
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0003469D File Offset: 0x0003289D
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x04000AB3 RID: 2739
	private const int CashItemTypeID = 451;

	// Token: 0x04000AB4 RID: 2740
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000AB5 RID: 2741
	[SerializeField]
	private DigitInputPanel inputPanel;

	// Token: 0x04000AB6 RID: 2742
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000AB7 RID: 2743
	[SerializeField]
	private GameObject insufficientIndicator;

	// Token: 0x04000AB8 RID: 2744
	[SerializeField]
	private Button quitButton;

	// Token: 0x04000AB9 RID: 2745
	private int _cachedCashAmount = -1;
}
