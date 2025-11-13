using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BB RID: 955
	public class ItemCustomizeView : View, ISingleSelectionMenu<SlotDisplay>
	{
		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x000787E3 File Offset: 0x000769E3
		public static ItemCustomizeView Instance
		{
			get
			{
				return View.GetViewInstance<ItemCustomizeView>();
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000787EC File Offset: 0x000769EC
		private PrefabPool<ItemDisplay> ItemDisplayPool
		{
			get
			{
				if (this._itemDisplayPool == null)
				{
					this.itemDisplayTemplate.gameObject.SetActive(false);
					this._itemDisplayPool = new PrefabPool<ItemDisplay>(this.itemDisplayTemplate, this.itemDisplayTemplate.transform.parent, null, null, null, true, 10, 10000, null);
				}
				return this._itemDisplayPool;
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x00078845 File Offset: 0x00076A45
		private void OnGetInventoryDisplay(InventoryDisplay display)
		{
			display.onDisplayDoubleClicked += this.OnInventoryDoubleClicked;
			display.ShowOperationButtons = false;
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x00078860 File Offset: 0x00076A60
		private void OnReleaseInventoryDisplay(InventoryDisplay display)
		{
			display.onDisplayDoubleClicked -= this.OnInventoryDoubleClicked;
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00078874 File Offset: 0x00076A74
		private void OnInventoryDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			if (entry.Item != null)
			{
				this.target.TryPlug(entry.Item, false, entry.Master.Target, 0);
				data.Use();
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002286 RID: 8838 RVA: 0x000788A9 File Offset: 0x00076AA9
		public Item Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000788B1 File Offset: 0x00076AB1
		public void Setup(Item target, List<Inventory> avaliableInventories)
		{
			this.target = target;
			this.customizingTargetDisplay.Setup(target);
			this.avaliableInventories.Clear();
			this.avaliableInventories.AddRange(avaliableInventories);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000788DD File Offset: 0x00076ADD
		public void DebugSetup(Item target, Inventory inventory1, Inventory inventory2)
		{
			this.Setup(target, new List<Inventory>
			{
				inventory1,
				inventory2
			});
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000788F9 File Offset: 0x00076AF9
		protected override void OnOpen()
		{
			base.OnOpen();
			ItemUIUtilities.Select(null);
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
			this.fadeGroup.Show();
			this.SetSelection(null);
			this.RefreshDetails();
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00078931 File Offset: 0x00076B31
		protected override void OnClose()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
			base.OnClose();
			this.fadeGroup.Hide();
			this.selectedItemDisplayFadeGroup.Hide();
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00078960 File Offset: 0x00076B60
		private void OnItemSelectionChanged()
		{
			this.RefreshDetails();
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00078968 File Offset: 0x00076B68
		private void RefreshDetails()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.selectedItemDisplayFadeGroup.Show();
				this.selectedItemDisplay.Setup(ItemUIUtilities.SelectedItem);
				Item y = this.selectedItemDisplay.Target;
				bool flag = this.selectedSlotDisplay.Target.Content != y;
				this.equipButton.gameObject.SetActive(flag);
				this.unequipButton.gameObject.SetActive(!flag);
				return;
			}
			this.selectedItemDisplayFadeGroup.Hide();
			this.equipButton.gameObject.SetActive(false);
			this.unequipButton.gameObject.SetActive(false);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x00078A14 File Offset: 0x00076C14
		protected override void Awake()
		{
			base.Awake();
			this.equipButton.onClick.AddListener(new UnityAction(this.OnEquipButtonClicked));
			this.unequipButton.onClick.AddListener(new UnityAction(this.OnUnequipButtonClicked));
			this.customizingTargetDisplay.SlotCollectionDisplay.onElementClicked += this.OnSlotElementClicked;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x00078A7C File Offset: 0x00076C7C
		private void OnUnequipButtonClicked()
		{
			if (this.selectedSlotDisplay == null)
			{
				return;
			}
			if (this.selectedItemDisplay == null)
			{
				return;
			}
			Slot slot = this.selectedSlotDisplay.Target;
			if (slot.Content != null)
			{
				Item item = slot.Unplug();
				this.HandleUnpluggledItem(item);
			}
			this.RefreshAvaliableItems();
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00078AD8 File Offset: 0x00076CD8
		private void OnEquipButtonClicked()
		{
			if (this.selectedSlotDisplay == null)
			{
				return;
			}
			if (this.selectedItemDisplay == null)
			{
				return;
			}
			Slot slot = this.selectedSlotDisplay.Target;
			Item item = this.selectedItemDisplay.Target;
			if (slot == null)
			{
				return;
			}
			if (item == null)
			{
				return;
			}
			if (slot.Content != null)
			{
				Item item2 = slot.Unplug();
				this.HandleUnpluggledItem(item2);
			}
			item.Detach();
			Item item3;
			if (!slot.Plug(item, out item3))
			{
				Debug.LogError("装备失败！");
				this.HandleUnpluggledItem(item);
			}
			this.RefreshAvaliableItems();
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00078B6D File Offset: 0x00076D6D
		private void HandleUnpluggledItem(Item item)
		{
			if (PlayerStorage.Inventory)
			{
				ItemUtilities.SendToPlayerStorage(item, false);
				return;
			}
			if (!ItemUtilities.SendToPlayerCharacterInventory(item, false))
			{
				ItemUtilities.SendToPlayerStorage(item, false);
			}
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x00078B93 File Offset: 0x00076D93
		private void OnSlotElementClicked(ItemSlotCollectionDisplay collection, SlotDisplay slot)
		{
			this.SetSelection(slot);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x00078B9D File Offset: 0x00076D9D
		public SlotDisplay GetSelection()
		{
			return this.selectedSlotDisplay;
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x00078BA5 File Offset: 0x00076DA5
		public bool SetSelection(SlotDisplay selection)
		{
			this.selectedSlotDisplay = selection;
			this.RefreshSelectionIndicator();
			this.OnSlotSelectionChanged();
			return true;
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x00078BBC File Offset: 0x00076DBC
		private void RefreshSelectionIndicator()
		{
			this.slotSelectionIndicator.gameObject.SetActive(this.selectedSlotDisplay);
			if (this.selectedSlotDisplay != null)
			{
				this.slotSelectionIndicator.position = this.selectedSlotDisplay.transform.position;
			}
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x00078C0D File Offset: 0x00076E0D
		private void OnSlotSelectionChanged()
		{
			ItemUIUtilities.Select(null);
			this.RefreshAvaliableItems();
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x00078C1C File Offset: 0x00076E1C
		private void RefreshAvaliableItems()
		{
			this.avaliableItems.Clear();
			if (!(this.selectedSlotDisplay == null))
			{
				Slot slot = this.selectedSlotDisplay.Target;
				if (!(this.selectedSlotDisplay == null))
				{
					foreach (Inventory inventory in this.avaliableInventories)
					{
						foreach (Item item in inventory)
						{
							if (!(item == null) && slot.CanPlug(item))
							{
								this.avaliableItems.Add(item);
							}
						}
					}
				}
			}
			this.RefreshItemListGraphics();
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x00078CF0 File Offset: 0x00076EF0
		private void RefreshItemListGraphics()
		{
			Debug.Log("Refreshing Item List Graphics");
			bool flag = this.selectedSlotDisplay != null;
			bool flag2 = this.avaliableItems.Count > 0;
			this.selectSlotPlaceHolder.SetActive(!flag);
			this.noAvaliableItemPlaceHolder.SetActive(flag && !flag2);
			this.avaliableItemsContainer.SetActive(flag2);
			this.ItemDisplayPool.ReleaseAll();
			if (flag2)
			{
				foreach (Item x in this.avaliableItems)
				{
					if (!(x == null))
					{
						ItemDisplay itemDisplay = this.ItemDisplayPool.Get(null);
						itemDisplay.ShowOperationButtons = false;
						itemDisplay.Setup(x);
					}
				}
			}
		}

		// Token: 0x0400175D RID: 5981
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400175E RID: 5982
		[SerializeField]
		private Button equipButton;

		// Token: 0x0400175F RID: 5983
		[SerializeField]
		private Button unequipButton;

		// Token: 0x04001760 RID: 5984
		[SerializeField]
		private ItemDetailsDisplay customizingTargetDisplay;

		// Token: 0x04001761 RID: 5985
		[SerializeField]
		private ItemDetailsDisplay selectedItemDisplay;

		// Token: 0x04001762 RID: 5986
		[SerializeField]
		private FadeGroup selectedItemDisplayFadeGroup;

		// Token: 0x04001763 RID: 5987
		[SerializeField]
		private RectTransform slotSelectionIndicator;

		// Token: 0x04001764 RID: 5988
		[SerializeField]
		private GameObject selectSlotPlaceHolder;

		// Token: 0x04001765 RID: 5989
		[SerializeField]
		private GameObject avaliableItemsContainer;

		// Token: 0x04001766 RID: 5990
		[SerializeField]
		private GameObject noAvaliableItemPlaceHolder;

		// Token: 0x04001767 RID: 5991
		[SerializeField]
		private ItemDisplay itemDisplayTemplate;

		// Token: 0x04001768 RID: 5992
		private PrefabPool<ItemDisplay> _itemDisplayPool;

		// Token: 0x04001769 RID: 5993
		private Item target;

		// Token: 0x0400176A RID: 5994
		private SlotDisplay selectedSlotDisplay;

		// Token: 0x0400176B RID: 5995
		private List<Inventory> avaliableInventories = new List<Inventory>();

		// Token: 0x0400176C RID: 5996
		private List<Item> avaliableItems = new List<Item>();
	}
}
