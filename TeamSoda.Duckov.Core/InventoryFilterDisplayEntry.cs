using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F3 RID: 499
public class InventoryFilterDisplayEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0003B7A7 File Offset: 0x000399A7
	// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x0003B7AF File Offset: 0x000399AF
	public InventoryFilterProvider.FilterEntry Filter { get; private set; }

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0003B7B8 File Offset: 0x000399B8
	public void OnPointerClick(PointerEventData eventData)
	{
		Action<InventoryFilterDisplayEntry, PointerEventData> action = this.onPointerClick;
		if (action == null)
		{
			return;
		}
		action(this, eventData);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0003B7CC File Offset: 0x000399CC
	internal void Setup(Action<InventoryFilterDisplayEntry, PointerEventData> onPointerClick, InventoryFilterProvider.FilterEntry filter)
	{
		this.onPointerClick = onPointerClick;
		this.Filter = filter;
		if (this.icon)
		{
			this.icon.sprite = filter.icon;
		}
		if (this.nameDisplay)
		{
			this.nameDisplay.text = filter.DisplayName;
		}
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0003B824 File Offset: 0x00039A24
	internal void NotifySelectionChanged(bool isThisSelected)
	{
		this.selectedIndicator.SetActive(isThisSelected);
	}

	// Token: 0x04000C3B RID: 3131
	[SerializeField]
	private Image icon;

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private TextMeshProUGUI nameDisplay;

	// Token: 0x04000C3D RID: 3133
	[SerializeField]
	private GameObject selectedIndicator;

	// Token: 0x04000C3F RID: 3135
	private Action<InventoryFilterDisplayEntry, PointerEventData> onPointerClick;
}
