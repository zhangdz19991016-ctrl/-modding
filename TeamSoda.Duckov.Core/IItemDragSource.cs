using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine.EventSystems;

// Token: 0x02000161 RID: 353
public interface IItemDragSource : IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06000AD2 RID: 2770 RVA: 0x0002F398 File Offset: 0x0002D598
	// (remove) Token: 0x06000AD3 RID: 2771 RVA: 0x0002F3CC File Offset: 0x0002D5CC
	public static event Action<Item> OnStartDragItem;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06000AD4 RID: 2772 RVA: 0x0002F400 File Offset: 0x0002D600
	// (remove) Token: 0x06000AD5 RID: 2773 RVA: 0x0002F434 File Offset: 0x0002D634
	public static event Action<Item> OnEndDragItem;

	// Token: 0x06000AD6 RID: 2774
	bool IsEditable();

	// Token: 0x06000AD7 RID: 2775
	Item GetItem();

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0002F468 File Offset: 0x0002D668
	void OnBeginDrag(PointerEventData eventData)
	{
		if (!this.IsEditable())
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		Item item = this.GetItem();
		Action<Item> onStartDragItem = IItemDragSource.OnStartDragItem;
		if (onStartDragItem != null)
		{
			onStartDragItem(item);
		}
		if (item == null)
		{
			return;
		}
		ItemUIUtilities.NotifyPutItem(item, true);
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0002F4B0 File Offset: 0x0002D6B0
	void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		Item item = this.GetItem();
		Action<Item> onEndDragItem = IItemDragSource.OnEndDragItem;
		if (onEndDragItem == null)
		{
			return;
		}
		onEndDragItem(item);
	}
}
