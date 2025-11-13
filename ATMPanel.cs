using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019B RID: 411
public class ATMPanel : MonoBehaviour
{
	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00033F73 File Offset: 0x00032173
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

	// Token: 0x06000C21 RID: 3105 RVA: 0x00033F94 File Offset: 0x00032194
	private void Awake()
	{
		this.btnSelectSave.onClick.AddListener(new UnityAction(this.ShowSavePanel));
		this.btnSelectDraw.onClick.AddListener(new UnityAction(this.ShowDrawPanel));
		this.savePanel.onQuit += this.SavePanel_onQuit;
		this.drawPanel.onQuit += this.DrawPanel_onQuit;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00034007 File Offset: 0x00032207
	private void DrawPanel_onQuit(ATMPanel_DrawPanel panel)
	{
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00034010 File Offset: 0x00032210
	private void SavePanel_onQuit(ATMPanel_SavePanel obj)
	{
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00034019 File Offset: 0x00032219
	private void HideAllPanels(bool skip = false)
	{
		if (skip)
		{
			this.selectPanel.SkipHide();
		}
		else
		{
			this.selectPanel.Hide();
		}
		this.savePanel.Hide(skip);
		this.drawPanel.Hide(skip);
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x0003404E File Offset: 0x0003224E
	public void ShowSelectPanel(bool skipHideOthers = false)
	{
		this.HideAllPanels(skipHideOthers);
		this.selectPanel.Show();
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00034062 File Offset: 0x00032262
	public void ShowDrawPanel()
	{
		this.HideAllPanels(false);
		this.drawPanel.Show();
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00034076 File Offset: 0x00032276
	public void ShowSavePanel()
	{
		this.HideAllPanels(false);
		this.savePanel.Show();
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0003408A File Offset: 0x0003228A
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation += this.OnPlayerItemOperation;
		this.RefreshCash();
		this.RefreshBalance();
		this.ShowSelectPanel(false);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x000340C1 File Offset: 0x000322C1
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation -= this.OnPlayerItemOperation;
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x000340E5 File Offset: 0x000322E5
	private void OnPlayerItemOperation()
	{
		this.RefreshCash();
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x000340ED File Offset: 0x000322ED
	private void OnMoneyChanged(long oldMoney, long changedMoney)
	{
		this.RefreshBalance();
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x000340F5 File Offset: 0x000322F5
	private void RefreshCash()
	{
		this._cachedCashAmount = ItemUtilities.GetItemCount(451);
		this.cashAmountText.text = string.Format("{0:n0}", this.CashAmount);
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00034127 File Offset: 0x00032327
	private void RefreshBalance()
	{
		this.balanceAmountText.text = string.Format("{0:n0}", EconomyManager.Money);
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00034148 File Offset: 0x00032348
	public static UniTask<bool> Draw(long amount)
	{
		ATMPanel.<Draw>d__26 <Draw>d__;
		<Draw>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Draw>d__.amount = amount;
		<Draw>d__.<>1__state = -1;
		<Draw>d__.<>t__builder.Start<ATMPanel.<Draw>d__26>(ref <Draw>d__);
		return <Draw>d__.<>t__builder.Task;
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0003418C File Offset: 0x0003238C
	public static bool Save(long amount)
	{
		Cost cost = new Cost(0L, new ValueTuple<int, long>[]
		{
			new ValueTuple<int, long>(451, amount)
		});
		if (!cost.Pay(false, true))
		{
			return false;
		}
		EconomyManager.Add(amount);
		return true;
	}

	// Token: 0x04000AA2 RID: 2722
	private const int CashItemTypeID = 451;

	// Token: 0x04000AA3 RID: 2723
	[SerializeField]
	private TextMeshProUGUI balanceAmountText;

	// Token: 0x04000AA4 RID: 2724
	[SerializeField]
	private TextMeshProUGUI cashAmountText;

	// Token: 0x04000AA5 RID: 2725
	[SerializeField]
	private Button btnSelectSave;

	// Token: 0x04000AA6 RID: 2726
	[SerializeField]
	private Button btnSelectDraw;

	// Token: 0x04000AA7 RID: 2727
	[SerializeField]
	private FadeGroup selectPanel;

	// Token: 0x04000AA8 RID: 2728
	[SerializeField]
	private ATMPanel_SavePanel savePanel;

	// Token: 0x04000AA9 RID: 2729
	[SerializeField]
	private ATMPanel_DrawPanel drawPanel;

	// Token: 0x04000AAA RID: 2730
	private int _cachedCashAmount = -1;

	// Token: 0x04000AAB RID: 2731
	private static bool drawingMoney;

	// Token: 0x04000AAC RID: 2732
	public const long MaxDrawAmount = 10000000L;
}
