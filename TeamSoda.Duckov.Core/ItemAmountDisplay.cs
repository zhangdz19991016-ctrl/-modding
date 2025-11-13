using System;
using ItemStatsSystem;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200015C RID: 348
public class ItemAmountDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IItemMetaDataProvider
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06000AB5 RID: 2741 RVA: 0x0002F060 File Offset: 0x0002D260
	// (remove) Token: 0x06000AB6 RID: 2742 RVA: 0x0002F094 File Offset: 0x0002D294
	public static event Action<ItemAmountDisplay> OnMouseEnter;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06000AB7 RID: 2743 RVA: 0x0002F0C8 File Offset: 0x0002D2C8
	// (remove) Token: 0x06000AB8 RID: 2744 RVA: 0x0002F0FC File Offset: 0x0002D2FC
	public static event Action<ItemAmountDisplay> OnMouseExit;

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002F12F File Offset: 0x0002D32F
	public int TypeID
	{
		get
		{
			return this.typeID;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0002F137 File Offset: 0x0002D337
	public ItemMetaData MetaData
	{
		get
		{
			return this.metaData;
		}
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0002F13F File Offset: 0x0002D33F
	public ItemMetaData GetMetaData()
	{
		return this.metaData;
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0002F147 File Offset: 0x0002D347
	private void Awake()
	{
		ItemUtilities.OnPlayerItemOperation += this.Refresh;
		LevelManager.OnLevelInitialized += this.Refresh;
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0002F16B File Offset: 0x0002D36B
	private void OnDestroy()
	{
		ItemUtilities.OnPlayerItemOperation -= this.Refresh;
		LevelManager.OnLevelInitialized -= this.Refresh;
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0002F18F File Offset: 0x0002D38F
	public void Setup(int itemTypeID, long amount)
	{
		this.typeID = itemTypeID;
		this.amount = amount;
		this.Refresh();
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0002F1A8 File Offset: 0x0002D3A8
	private void Refresh()
	{
		int itemCount = ItemUtilities.GetItemCount(this.typeID);
		this.metaData = ItemAssetsCollection.GetMetaData(this.typeID);
		this.icon.sprite = this.metaData.icon;
		this.amountText.text = this.amountFormat.Format(new
		{
			amount = this.amount,
			possess = itemCount
		});
		bool flag = (long)itemCount >= this.amount;
		this.background.color = (flag ? this.enoughColor : this.normalColor);
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0002F234 File Offset: 0x0002D434
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<ItemAmountDisplay> onMouseEnter = ItemAmountDisplay.OnMouseEnter;
		if (onMouseEnter == null)
		{
			return;
		}
		onMouseEnter(this);
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0002F246 File Offset: 0x0002D446
	public void OnPointerExit(PointerEventData eventData)
	{
		Action<ItemAmountDisplay> onMouseExit = ItemAmountDisplay.OnMouseExit;
		if (onMouseExit == null)
		{
			return;
		}
		onMouseExit(this);
	}

	// Token: 0x04000968 RID: 2408
	[SerializeField]
	private Image background;

	// Token: 0x04000969 RID: 2409
	[SerializeField]
	private Image icon;

	// Token: 0x0400096A RID: 2410
	[SerializeField]
	private TextMeshProUGUI amountText;

	// Token: 0x0400096B RID: 2411
	[SerializeField]
	private string amountFormat = "( {possess} / {amount} )";

	// Token: 0x0400096C RID: 2412
	[SerializeField]
	private Color normalColor;

	// Token: 0x0400096D RID: 2413
	[SerializeField]
	private Color enoughColor;

	// Token: 0x0400096E RID: 2414
	private int typeID;

	// Token: 0x0400096F RID: 2415
	private long amount;

	// Token: 0x04000970 RID: 2416
	private ItemMetaData metaData;
}
