using System;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000162 RID: 354
public class ItemDraggingPointerDisplay : MonoBehaviour
{
	// Token: 0x06000ADA RID: 2778 RVA: 0x0002F4E0 File Offset: 0x0002D6E0
	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
		this.parentRectTransform = (base.transform.parent as RectTransform);
		IItemDragSource.OnStartDragItem += this.OnStartDragItem;
		IItemDragSource.OnEndDragItem += this.OnEndDragItem;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0002F542 File Offset: 0x0002D742
	private void OnDestroy()
	{
		IItemDragSource.OnStartDragItem -= this.OnStartDragItem;
		IItemDragSource.OnEndDragItem -= this.OnEndDragItem;
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0002F566 File Offset: 0x0002D766
	private void Update()
	{
		this.RefreshPosition();
		if (Mouse.current.leftButton.wasReleasedThisFrame)
		{
			this.OnEndDragItem(null);
		}
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0002F588 File Offset: 0x0002D788
	private unsafe void RefreshPosition()
	{
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform.parent as RectTransform, *Pointer.current.position.value, null, out v);
		this.rectTransform.localPosition = v;
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0002F5D3 File Offset: 0x0002D7D3
	private void OnStartDragItem(Item item)
	{
		this.target = item;
		if (this.target == null)
		{
			return;
		}
		this.display.Setup(this.target);
		this.RefreshPosition();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0002F60E File Offset: 0x0002D80E
	private void OnEndDragItem(Item item)
	{
		this.target = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000978 RID: 2424
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04000979 RID: 2425
	[SerializeField]
	private RectTransform parentRectTransform;

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private ItemDisplay display;

	// Token: 0x0400097B RID: 2427
	private Item target;
}
