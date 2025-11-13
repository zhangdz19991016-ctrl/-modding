using System;
using Duckov.Economy;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015B RID: 347
public class CostDisplay : MonoBehaviour
{
	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0002EE28 File Offset: 0x0002D028
	private PrefabPool<ItemAmountDisplay> ItemPool
	{
		get
		{
			if (this._itemPool == null)
			{
				this._itemPool = new PrefabPool<ItemAmountDisplay>(this.itemAmountTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._itemPool;
		}
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0002EE61 File Offset: 0x0002D061
	private void OnEnable()
	{
		EconomyManager.OnMoneyChanged += this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation += this.OnItemOperation;
		LevelManager.OnLevelInitialized += this.OnLevelInitialized;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0002EE96 File Offset: 0x0002D096
	private void OnDisable()
	{
		EconomyManager.OnMoneyChanged -= this.OnMoneyChanged;
		ItemUtilities.OnPlayerItemOperation -= this.OnItemOperation;
		LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0002EECB File Offset: 0x0002D0CB
	private void OnLevelInitialized()
	{
		this.RefreshBackground();
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0002EED3 File Offset: 0x0002D0D3
	private void OnItemOperation()
	{
		this.RefreshBackground();
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0002EEDB File Offset: 0x0002D0DB
	private void RefreshBackground()
	{
		if (this.background == null)
		{
			return;
		}
		this.background.color = (this.cost.Enough ? this.enoughColor : this.normalColor);
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0002EF12 File Offset: 0x0002D112
	private void OnMoneyChanged(long arg1, long arg2)
	{
		this.RefreshMoneyBackground();
		this.RefreshBackground();
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0002EF20 File Offset: 0x0002D120
	public void Setup(Cost cost, int multiplier = 1)
	{
		this.cost = cost;
		this.moneyContainer.SetActive(cost.money > 0L);
		this.money.text = (cost.money * (long)multiplier).ToString("n0");
		this.itemsContainer.SetActive(cost.items != null && cost.items.Length != 0);
		this.ItemPool.ReleaseAll();
		if (cost.items != null)
		{
			foreach (Cost.ItemEntry itemEntry in cost.items)
			{
				ItemAmountDisplay itemAmountDisplay = this.ItemPool.Get(null);
				itemAmountDisplay.Setup(itemEntry.id, itemEntry.amount * (long)multiplier);
				itemAmountDisplay.transform.SetAsLastSibling();
			}
		}
		this.RefreshMoneyBackground();
		this.RefreshBackground();
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0002EFF4 File Offset: 0x0002D1F4
	private void RefreshMoneyBackground()
	{
		bool flag = this.cost.money <= EconomyManager.Money;
		this.moneyBackground.color = (flag ? this.money_enoughColor : this.money_normalColor);
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0002F033 File Offset: 0x0002D233
	internal void Clear()
	{
		this.cost = default(Cost);
		this.moneyContainer.SetActive(false);
		this.ItemPool.ReleaseAll();
	}

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private GameObject moneyContainer;

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	private GameObject itemsContainer;

	// Token: 0x0400095C RID: 2396
	[SerializeField]
	private Image background;

	// Token: 0x0400095D RID: 2397
	[SerializeField]
	private Image moneyBackground;

	// Token: 0x0400095E RID: 2398
	[SerializeField]
	private TextMeshProUGUI money;

	// Token: 0x0400095F RID: 2399
	[SerializeField]
	private ItemAmountDisplay itemAmountTemplate;

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private Color normalColor;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private Color enoughColor;

	// Token: 0x04000962 RID: 2402
	[SerializeField]
	private Color money_normalColor;

	// Token: 0x04000963 RID: 2403
	[SerializeField]
	private Color money_enoughColor;

	// Token: 0x04000964 RID: 2404
	private PrefabPool<ItemAmountDisplay> _itemPool;

	// Token: 0x04000965 RID: 2405
	private Cost cost;
}
