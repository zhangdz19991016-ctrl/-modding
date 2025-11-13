using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003B2 RID: 946
	public class ItemShortcutEditorEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IItemDragSource, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x00077428 File Offset: 0x00075628
		private Item TargetItem
		{
			get
			{
				return ItemShortcut.Get(this.index);
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x00077438 File Offset: 0x00075638
		private void Awake()
		{
			this.itemDisplay.onPointerClick += this.OnItemDisplayClicked;
			this.itemDisplay.onReceiveDrop += this.OnDrop;
			ItemShortcut.OnSetItem += this.OnSetItem;
			this.hoveringIndicator.SetActive(false);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x00077491 File Offset: 0x00075691
		private void OnSetItem(int index)
		{
			if (index == this.index)
			{
				this.Refresh();
			}
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000774A2 File Offset: 0x000756A2
		private void OnItemDisplayClicked(ItemDisplay display, PointerEventData data)
		{
			this.OnPointerClick(data);
			data.Use();
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000774B1 File Offset: 0x000756B1
		public void OnPointerClick(PointerEventData eventData)
		{
			if (ItemUIUtilities.SelectedItem != null && ItemShortcut.Set(this.index, ItemUIUtilities.SelectedItem))
			{
				this.Refresh();
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000774D8 File Offset: 0x000756D8
		internal void Refresh()
		{
			this.UnregisterEvents();
			if (this.displayingItem != this.TargetItem)
			{
				this.itemDisplay.Punch();
			}
			this.displayingItem = this.TargetItem;
			this.itemDisplay.Setup(this.displayingItem);
			this.itemDisplay.ShowOperationButtons = false;
			this.RegisterEvents();
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x00077538 File Offset: 0x00075738
		private void RegisterEvents()
		{
			if (this.displayingItem != null)
			{
				this.displayingItem.onParentChanged += this.OnTargetParentChanged;
				this.displayingItem.onSetStackCount += this.OnTargetStackCountChanged;
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00077576 File Offset: 0x00075776
		private void UnregisterEvents()
		{
			if (this.displayingItem != null)
			{
				this.displayingItem.onParentChanged -= this.OnTargetParentChanged;
				this.displayingItem.onSetStackCount -= this.OnTargetStackCountChanged;
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000775B4 File Offset: 0x000757B4
		private void OnTargetStackCountChanged(Item item)
		{
			this.SetDirty();
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000775BC File Offset: 0x000757BC
		private void OnTargetParentChanged(Item item)
		{
			this.SetDirty();
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000775C4 File Offset: 0x000757C4
		private void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000775CD File Offset: 0x000757CD
		private void Update()
		{
			if (this.dirty)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000775DD File Offset: 0x000757DD
		private void OnDestroy()
		{
			this.UnregisterEvents();
			ItemShortcut.OnSetItem -= this.OnSetItem;
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000775F8 File Offset: 0x000757F8
		internal void Setup(int i)
		{
			this.index = i;
			this.Refresh();
			InputActionReference inputActionRef = InputActionReference.Create(GameplayDataSettings.InputActions[string.Format("Character/ItemShortcut{0}", i + 3)]);
			this.indicator.Setup(inputActionRef, -1);
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00077644 File Offset: 0x00075844
		public void OnDrop(PointerEventData eventData)
		{
			eventData.Use();
			IItemDragSource component = eventData.pointerDrag.gameObject.GetComponent<IItemDragSource>();
			if (component == null)
			{
				return;
			}
			if (!component.IsEditable())
			{
				return;
			}
			Item item = component.GetItem();
			if (item == null)
			{
				return;
			}
			if (!item.IsInPlayerCharacter())
			{
				ItemUtilities.SendToPlayer(item, false, false);
			}
			if (ItemShortcut.Set(this.index, item))
			{
				this.Refresh();
				AudioManager.Post("UI/click");
			}
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000776B5 File Offset: 0x000758B5
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.hoveringIndicator.SetActive(true);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000776C3 File Offset: 0x000758C3
		public void OnPointerExit(PointerEventData eventData)
		{
			this.hoveringIndicator.SetActive(false);
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000776D1 File Offset: 0x000758D1
		public bool IsEditable()
		{
			return this.TargetItem != null;
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x000776DF File Offset: 0x000758DF
		public Item GetItem()
		{
			return this.TargetItem;
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000776E7 File Offset: 0x000758E7
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x04001704 RID: 5892
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04001705 RID: 5893
		[SerializeField]
		private GameObject hoveringIndicator;

		// Token: 0x04001706 RID: 5894
		[SerializeField]
		private int index;

		// Token: 0x04001707 RID: 5895
		[SerializeField]
		private InputIndicator indicator;

		// Token: 0x04001708 RID: 5896
		private Item displayingItem;

		// Token: 0x04001709 RID: 5897
		private bool dirty;
	}
}
