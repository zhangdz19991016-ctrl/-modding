using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BE RID: 958
	public class LootView : View
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x00079BCE File Offset: 0x00077DCE
		public static LootView Instance
		{
			get
			{
				return View.GetViewInstance<LootView>();
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x00079BD5 File Offset: 0x00077DD5
		private CharacterMainControl Character
		{
			get
			{
				return LevelManager.Instance.MainCharacter;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x00079BE1 File Offset: 0x00077DE1
		private Item CharacterItem
		{
			get
			{
				if (this.Character == null)
				{
					return null;
				}
				return this.Character.CharacterItem;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x00079BFE File Offset: 0x00077DFE
		public Inventory TargetInventory
		{
			get
			{
				if (this.targetLootBox != null)
				{
					return this.targetLootBox.Inventory;
				}
				if (this.targetInventory)
				{
					return this.targetInventory;
				}
				return null;
			}
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x00079C2F File Offset: 0x00077E2F
		public static bool HasInventoryEverBeenLooted(Inventory inventory)
		{
			return !(LootView.Instance == null) && LootView.Instance.lootedInventories != null && !(inventory == null) && LootView.Instance.lootedInventories.Contains(inventory);
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x00079C6C File Offset: 0x00077E6C
		protected override void Awake()
		{
			base.Awake();
			InteractableLootbox.OnStartLoot += this.OnStartLoot;
			this.pickAllButton.onClick.AddListener(new UnityAction(this.OnPickAllButtonClicked));
			CharacterMainControl.OnMainCharacterStartUseItem += this.OnMainCharacterStartUseItem;
			LevelManager.OnMainCharacterDead += this.OnMainCharacterDead;
			this.storeAllButton.onClick.AddListener(new UnityAction(this.OnStoreAllButtonClicked));
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00079CEC File Offset: 0x00077EEC
		private void OnStoreAllButtonClicked()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (this.TargetInventory != PlayerStorage.Inventory)
			{
				return;
			}
			if (this.CharacterItem == null)
			{
				return;
			}
			Inventory inventory = this.CharacterItem.Inventory;
			if (inventory == null)
			{
				return;
			}
			int lastItemPosition = inventory.GetLastItemPosition();
			for (int i = 0; i <= lastItemPosition; i++)
			{
				if (!inventory.lockedIndexes.Contains(i))
				{
					Item itemAt = inventory.GetItemAt(i);
					if (!(itemAt == null))
					{
						if (!this.TargetInventory.AddAndMerge(itemAt, 0))
						{
							break;
						}
						if (i == 0)
						{
							AudioManager.PlayPutItemSFX(itemAt, false);
						}
					}
				}
			}
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00079D8B File Offset: 0x00077F8B
		protected override void OnDestroy()
		{
			this.UnregisterEvents();
			InteractableLootbox.OnStartLoot -= this.OnStartLoot;
			LevelManager.OnMainCharacterDead -= this.OnMainCharacterDead;
			base.OnDestroy();
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x00079DBB File Offset: 0x00077FBB
		private void OnMainCharacterStartUseItem(Item _item)
		{
			if (base.open)
			{
				base.Close();
			}
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x00079DCB File Offset: 0x00077FCB
		private void OnMainCharacterDead(DamageInfo dmgInfo)
		{
			if (base.open)
			{
				base.Close();
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00079DDB File Offset: 0x00077FDB
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00079DE3 File Offset: 0x00077FE3
		private void OnDisable()
		{
			this.UnregisterEvents();
			InteractableLootbox interactableLootbox = this.targetLootBox;
			if (interactableLootbox != null)
			{
				interactableLootbox.StopInteract();
			}
			this.targetLootBox = null;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00079E03 File Offset: 0x00078003
		public void Show()
		{
			base.Open(null);
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x00079E0C File Offset: 0x0007800C
		private void OnStartLoot(InteractableLootbox lootbox)
		{
			this.targetLootBox = lootbox;
			if (this.targetLootBox == null || this.targetLootBox.Inventory == null)
			{
				Debug.LogError("Target loot box could not be found");
				return;
			}
			base.Open(null);
			if (this.TargetInventory != null)
			{
				this.lootedInventories.Add(this.TargetInventory);
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x00079E73 File Offset: 0x00078073
		private void OnStopLoot(InteractableLootbox lootbox)
		{
			if (lootbox == this.targetLootBox)
			{
				this.targetLootBox = null;
				base.Close();
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x00079E90 File Offset: 0x00078090
		public static void LootItem(Item item)
		{
			if (item == null)
			{
				return;
			}
			if (LootView.Instance == null)
			{
				return;
			}
			LootView.Instance.targetInventory = item.Inventory;
			LootView.Instance.Open(null);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00079EC8 File Offset: 0x000780C8
		protected override void OnOpen()
		{
			base.OnOpen();
			this.UnregisterEvents();
			base.gameObject.SetActive(true);
			this.characterSlotCollectionDisplay.Setup(this.CharacterItem, true);
			if (PetProxy.PetInventory)
			{
				this.petInventoryDisplay.gameObject.SetActive(true);
				this.petInventoryDisplay.Setup(PetProxy.PetInventory, null, null, false, null);
			}
			else
			{
				this.petInventoryDisplay.gameObject.SetActive(false);
			}
			this.characterInventoryDisplay.Setup(this.CharacterItem.Inventory, null, null, true, null);
			if (this.targetLootBox != null)
			{
				this.lootTargetInventoryDisplay.ShowSortButton = this.targetLootBox.ShowSortButton;
				this.lootTargetInventoryDisplay.Setup(this.TargetInventory, null, null, true, null);
				this.lootTargetDisplayName.text = this.TargetInventory.DisplayName;
				if (this.TargetInventory.GetComponent<InventoryFilterProvider>())
				{
					this.lootTargetFilterDisplay.gameObject.SetActive(true);
					this.lootTargetFilterDisplay.Setup(this.lootTargetInventoryDisplay);
					this.lootTargetFilterDisplay.Select(0);
				}
				else
				{
					this.lootTargetFilterDisplay.gameObject.SetActive(false);
				}
				this.lootTargetFadeGroup.Show();
			}
			else if (this.targetInventory != null)
			{
				this.lootTargetInventoryDisplay.ShowSortButton = false;
				this.lootTargetInventoryDisplay.Setup(this.TargetInventory, null, null, true, null);
				this.lootTargetFadeGroup.Show();
				this.lootTargetFilterDisplay.gameObject.SetActive(false);
			}
			else
			{
				this.lootTargetFadeGroup.SkipHide();
			}
			bool active = this.TargetInventory != null && this.TargetInventory == PlayerStorage.Inventory;
			this.storeAllButton.gameObject.SetActive(active);
			this.fadeGroup.Show();
			this.RefreshDetails();
			this.RefreshPickAllButton();
			this.RegisterEvents();
			this.RefreshCapacityText();
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0007A0C0 File Offset: 0x000782C0
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			InteractableLootbox interactableLootbox = this.targetLootBox;
			if (interactableLootbox != null)
			{
				interactableLootbox.StopInteract();
			}
			this.targetLootBox = null;
			this.targetInventory = null;
			if (SplitDialogue.Instance && SplitDialogue.Instance.isActiveAndEnabled)
			{
				SplitDialogue.Instance.Cancel();
			}
			this.UnregisterEvents();
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x0007A130 File Offset: 0x00078330
		private void OnTargetInventoryContentChanged(Inventory inventory, int arg2)
		{
			this.RefreshPickAllButton();
			this.RefreshCapacityText();
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0007A140 File Offset: 0x00078340
		private void RefreshCapacityText()
		{
			if (this.targetLootBox != null)
			{
				this.lootTargetCapacityText.text = this.lootTargetCapacityTextFormat.Format(new
				{
					itemCount = this.TargetInventory.GetItemCount(),
					capacity = this.TargetInventory.Capacity
				});
			}
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x0007A18C File Offset: 0x0007838C
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			this.lootTargetInventoryDisplay.onDisplayDoubleClicked += this.OnLootTargetItemDoubleClicked;
			this.characterInventoryDisplay.onDisplayDoubleClicked += this.OnCharacterInventoryItemDoubleClicked;
			this.petInventoryDisplay.onDisplayDoubleClicked += this.OnCharacterInventoryItemDoubleClicked;
			this.characterSlotCollectionDisplay.onElementDoubleClicked += this.OnCharacterSlotItemDoubleClicked;
			if (this.TargetInventory)
			{
				this.TargetInventory.onContentChanged += this.OnTargetInventoryContentChanged;
			}
			UIInputManager.OnNextPage += this.OnNextPage;
			UIInputManager.OnPreviousPage += this.OnPreviousPage;
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x0007A252 File Offset: 0x00078452
		private void OnPreviousPage(UIInputEventData data)
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (!this.lootTargetInventoryDisplay.UsePages)
			{
				return;
			}
			this.lootTargetInventoryDisplay.PreviousPage();
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x0007A27C File Offset: 0x0007847C
		private void OnNextPage(UIInputEventData data)
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			if (!this.lootTargetInventoryDisplay.UsePages)
			{
				return;
			}
			this.lootTargetInventoryDisplay.NextPage();
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x0007A2A8 File Offset: 0x000784A8
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			if (this.lootTargetInventoryDisplay)
			{
				this.lootTargetInventoryDisplay.onDisplayDoubleClicked -= this.OnLootTargetItemDoubleClicked;
			}
			if (this.characterInventoryDisplay)
			{
				this.characterInventoryDisplay.onDisplayDoubleClicked -= this.OnCharacterInventoryItemDoubleClicked;
			}
			if (this.petInventoryDisplay)
			{
				this.petInventoryDisplay.onDisplayDoubleClicked -= this.OnCharacterInventoryItemDoubleClicked;
			}
			if (this.characterSlotCollectionDisplay)
			{
				this.characterSlotCollectionDisplay.onElementDoubleClicked -= this.OnCharacterSlotItemDoubleClicked;
			}
			if (this.TargetInventory)
			{
				this.TargetInventory.onContentChanged -= this.OnTargetInventoryContentChanged;
			}
			UIInputManager.OnNextPage -= this.OnNextPage;
			UIInputManager.OnPreviousPage -= this.OnPreviousPage;
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0007A39C File Offset: 0x0007859C
		private void OnCharacterSlotItemDoubleClicked(ItemSlotCollectionDisplay collectionDisplay, SlotDisplay slotDisplay)
		{
			if (slotDisplay == null)
			{
				return;
			}
			Slot target = slotDisplay.Target;
			if (target == null)
			{
				return;
			}
			Item content = target.Content;
			if (content == null)
			{
				return;
			}
			if (this.TargetInventory == null)
			{
				return;
			}
			if (content.Sticky && !this.TargetInventory.AcceptSticky)
			{
				return;
			}
			AudioManager.PlayPutItemSFX(content, false);
			content.Detach();
			if (this.TargetInventory.AddAndMerge(content, 0))
			{
				this.RefreshDetails();
				return;
			}
			Item x;
			if (!target.Plug(content, out x))
			{
				Debug.LogError("Failed plugging back!");
			}
			if (x != null)
			{
				Debug.Log("Unplugged item should be null!");
			}
			this.RefreshDetails();
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x0007A448 File Offset: 0x00078648
		private void OnCharacterInventoryItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			Item content = entry.Content;
			if (content == null)
			{
				return;
			}
			Inventory inInventory = content.InInventory;
			if (this.TargetInventory == null)
			{
				return;
			}
			if (content.Sticky && !this.TargetInventory.AcceptSticky)
			{
				return;
			}
			AudioManager.PlayPutItemSFX(content, false);
			content.Detach();
			if (this.TargetInventory.AddAndMerge(content, 0))
			{
				this.RefreshDetails();
				return;
			}
			if (!inInventory.AddAndMerge(content, 0))
			{
				Debug.LogError("Failed sending back item");
			}
			this.RefreshDetails();
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x0007A4CF File Offset: 0x000786CF
		private void OnSelectionChanged()
		{
			this.RefreshDetails();
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x0007A4D7 File Offset: 0x000786D7
		private void RefreshDetails()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsFadeGroup.Show();
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				return;
			}
			this.detailsFadeGroup.Hide();
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0007A510 File Offset: 0x00078710
		private void OnLootTargetItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			Item item = entry.Item;
			if (item == null)
			{
				return;
			}
			if (!item.IsInPlayerCharacter())
			{
				if (this.targetLootBox != null && this.targetLootBox.needInspect && !item.Inspected)
				{
					data.Use();
					return;
				}
				data.Use();
				bool flag = false;
				LevelManager instance = LevelManager.Instance;
				bool? flag2;
				if (instance == null)
				{
					flag2 = null;
				}
				else
				{
					CharacterMainControl mainCharacter = instance.MainCharacter;
					if (mainCharacter == null)
					{
						flag2 = null;
					}
					else
					{
						Item characterItem = mainCharacter.CharacterItem;
						flag2 = ((characterItem != null) ? new bool?(characterItem.TryPlug(item, true, null, 0)) : null);
					}
				}
				bool? flag3 = flag2;
				flag |= flag3.Value;
				if (flag3 == null || !flag3.Value)
				{
					flag |= ItemUtilities.SendToPlayerCharacterInventory(item, false);
				}
				if (flag)
				{
					AudioManager.PlayPutItemSFX(item, false);
					this.RefreshDetails();
				}
			}
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x0007A5EC File Offset: 0x000787EC
		private void RefreshPickAllButton()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			this.pickAllButton.gameObject.SetActive(false);
			bool interactable = this.TargetInventory.GetItemCount() > 0;
			this.pickAllButton.interactable = interactable;
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0007A634 File Offset: 0x00078834
		private void OnPickAllButtonClicked()
		{
			if (this.TargetInventory == null)
			{
				return;
			}
			List<Item> list = new List<Item>();
			list.AddRange(this.TargetInventory);
			foreach (Item item in list)
			{
				if (!(item == null) && (!this.targetLootBox.needInspect || item.Inspected))
				{
					LevelManager instance = LevelManager.Instance;
					bool? flag;
					if (instance == null)
					{
						flag = null;
					}
					else
					{
						CharacterMainControl mainCharacter = instance.MainCharacter;
						if (mainCharacter == null)
						{
							flag = null;
						}
						else
						{
							Item characterItem = mainCharacter.CharacterItem;
							flag = ((characterItem != null) ? new bool?(characterItem.TryPlug(item, true, null, 0)) : null);
						}
					}
					bool? flag2 = flag;
					if (flag2 == null || !flag2.Value)
					{
						ItemUtilities.SendToPlayerCharacterInventory(item, false);
					}
				}
			}
			AudioManager.Post("UI/confirm");
		}

		// Token: 0x04001793 RID: 6035
		[SerializeField]
		private ItemSlotCollectionDisplay characterSlotCollectionDisplay;

		// Token: 0x04001794 RID: 6036
		[SerializeField]
		private InventoryDisplay characterInventoryDisplay;

		// Token: 0x04001795 RID: 6037
		[SerializeField]
		private InventoryDisplay petInventoryDisplay;

		// Token: 0x04001796 RID: 6038
		[SerializeField]
		private InventoryDisplay lootTargetInventoryDisplay;

		// Token: 0x04001797 RID: 6039
		[SerializeField]
		private InventoryFilterDisplay lootTargetFilterDisplay;

		// Token: 0x04001798 RID: 6040
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001799 RID: 6041
		[SerializeField]
		private Button pickAllButton;

		// Token: 0x0400179A RID: 6042
		[SerializeField]
		private TextMeshProUGUI lootTargetDisplayName;

		// Token: 0x0400179B RID: 6043
		[SerializeField]
		private TextMeshProUGUI lootTargetCapacityText;

		// Token: 0x0400179C RID: 6044
		[SerializeField]
		private string lootTargetCapacityTextFormat = "({itemCount}/{capacity})";

		// Token: 0x0400179D RID: 6045
		[SerializeField]
		private Button storeAllButton;

		// Token: 0x0400179E RID: 6046
		[SerializeField]
		private FadeGroup lootTargetFadeGroup;

		// Token: 0x0400179F RID: 6047
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x040017A0 RID: 6048
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x040017A1 RID: 6049
		[SerializeField]
		private InteractableLootbox targetLootBox;

		// Token: 0x040017A2 RID: 6050
		private Inventory targetInventory;

		// Token: 0x040017A3 RID: 6051
		private HashSet<Inventory> lootedInventories = new HashSet<Inventory>();
	}
}
