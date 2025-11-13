using System;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F6 RID: 502
public class ItemMetaDisplay : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IItemMetaDataProvider
{
	// Token: 0x06000EC1 RID: 3777 RVA: 0x0003B895 File Offset: 0x00039A95
	public ItemMetaData GetMetaData()
	{
		return this.data;
	}

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06000EC2 RID: 3778 RVA: 0x0003B8A0 File Offset: 0x00039AA0
	// (remove) Token: 0x06000EC3 RID: 3779 RVA: 0x0003B8D4 File Offset: 0x00039AD4
	public static event Action<ItemMetaDisplay> OnMouseEnter;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06000EC4 RID: 3780 RVA: 0x0003B908 File Offset: 0x00039B08
	// (remove) Token: 0x06000EC5 RID: 3781 RVA: 0x0003B93C File Offset: 0x00039B3C
	public static event Action<ItemMetaDisplay> OnMouseExit;

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0003B96F File Offset: 0x00039B6F
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<ItemMetaDisplay> onMouseEnter = ItemMetaDisplay.OnMouseEnter;
		if (onMouseEnter == null)
		{
			return;
		}
		onMouseEnter(this);
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0003B981 File Offset: 0x00039B81
	public void OnPointerExit(PointerEventData eventData)
	{
		Action<ItemMetaDisplay> onMouseExit = ItemMetaDisplay.OnMouseExit;
		if (onMouseExit == null)
		{
			return;
		}
		onMouseExit(this);
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0003B994 File Offset: 0x00039B94
	public void Setup(int typeID)
	{
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(typeID);
		this.Setup(metaData);
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0003B9AF File Offset: 0x00039BAF
	public void Setup(ItemMetaData data)
	{
		this.data = data;
		this.icon.sprite = data.icon;
		GameplayDataSettings.UIStyle.ApplyDisplayQualityShadow(data.displayQuality, this.displayQualityShadow);
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0003B9DF File Offset: 0x00039BDF
	internal void Setup(object rootTypeID)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04000C42 RID: 3138
	[SerializeField]
	private Image icon;

	// Token: 0x04000C43 RID: 3139
	[SerializeField]
	private TrueShadow displayQualityShadow;

	// Token: 0x04000C44 RID: 3140
	private ItemMetaData data;
}
