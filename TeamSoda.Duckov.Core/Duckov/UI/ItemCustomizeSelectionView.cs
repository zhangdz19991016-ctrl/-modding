using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BA RID: 954
	public class ItemCustomizeSelectionView : View
	{
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x00078403 File Offset: 0x00076603
		public static ItemCustomizeSelectionView Instance
		{
			get
			{
				return View.GetViewInstance<ItemCustomizeSelectionView>();
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x0007840A File Offset: 0x0007660A
		private Item CharacterItem
		{
			get
			{
				LevelManager instance = LevelManager.Instance;
				if (instance == null)
				{
					return null;
				}
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					return null;
				}
				return mainCharacter.CharacterItem;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x00078428 File Offset: 0x00076628
		private bool CanCustomize
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && !(selectedItem.Slots == null) && selectedItem.Slots.Count >= 1;
			}
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00078467 File Offset: 0x00076667
		protected override void Awake()
		{
			base.Awake();
			this.beginCustomizeButton.onClick.AddListener(new UnityAction(this.OnBeginCustomizeButtonClicked));
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0007848C File Offset: 0x0007668C
		private void OnBeginCustomizeButtonClicked()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			ItemCustomizeView instance = ItemCustomizeView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Setup(ItemUIUtilities.SelectedItem, this.GetAvaliableInventories());
			instance.Open(null);
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000784C8 File Offset: 0x000766C8
		private List<Inventory> GetAvaliableInventories()
		{
			this.avaliableInventories.Clear();
			LevelManager instance = LevelManager.Instance;
			Inventory inventory;
			if (instance == null)
			{
				inventory = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				if (mainCharacter == null)
				{
					inventory = null;
				}
				else
				{
					Item characterItem = mainCharacter.CharacterItem;
					inventory = ((characterItem != null) ? characterItem.Inventory : null);
				}
			}
			Inventory inventory2 = inventory;
			if (inventory2 != null)
			{
				this.avaliableInventories.Add(inventory2);
			}
			Inventory inventory3 = PlayerStorage.Inventory;
			if (inventory3 != null)
			{
				this.avaliableInventories.Add(inventory3);
			}
			return this.avaliableInventories;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00078540 File Offset: 0x00076740
		protected override void OnOpen()
		{
			this.UnregisterEvents();
			base.OnOpen();
			Item characterItem = this.CharacterItem;
			if (characterItem == null)
			{
				Debug.LogError("物品栏开启失败，角色物体不存在");
				return;
			}
			base.gameObject.SetActive(true);
			this.slotDisplay.Setup(characterItem, false);
			this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
			this.RegisterEvents();
			this.fadeGroup.Show();
			this.customizeButtonFadeGroup.SkipHide();
			this.placeHolderFadeGroup.SkipHide();
			ItemUIUtilities.Select(null);
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000785D5 File Offset: 0x000767D5
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000785F9 File Offset: 0x000767F9
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x0007860C File Offset: 0x0007680C
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.itemDetailsFadeGroup.Show();
			}
			else
			{
				this.itemDetailsFadeGroup.Hide();
			}
			if (this.CanCustomize)
			{
				this.placeHolderFadeGroup.Hide();
				this.customizeButtonFadeGroup.Show();
			}
			else
			{
				this.customizeButtonFadeGroup.Hide();
				this.placeHolderFadeGroup.Show();
			}
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x0007868A File Offset: 0x0007688A
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0007869D File Offset: 0x0007689D
		public static void Show()
		{
			if (ItemCustomizeSelectionView.Instance == null)
			{
				return;
			}
			ItemCustomizeSelectionView.Instance.Open(null);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000786B8 File Offset: 0x000768B8
		public static void Hide()
		{
			if (ItemCustomizeSelectionView.Instance == null)
			{
				return;
			}
			ItemCustomizeSelectionView.Instance.Close();
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000786D4 File Offset: 0x000768D4
		private void RefreshSelectedItemInfo()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem == null)
			{
				this.selectedItemName.text = this.noItemSelectedNameText;
				this.selectedItemIcon.sprite = this.noItemSelectedIconSprite;
				this.selectedItemShadow.enabled = false;
				this.customizableIndicator.SetActive(false);
				this.uncustomizableIndicator.SetActive(false);
				this.selectedItemIcon.color = Color.clear;
				return;
			}
			this.selectedItemShadow.enabled = true;
			this.selectedItemIcon.color = Color.white;
			this.selectedItemName.text = selectedItem.DisplayName;
			this.selectedItemIcon.sprite = selectedItem.Icon;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(selectedItem.DisplayQuality).Apply(this.selectedItemShadow);
			this.customizableIndicator.SetActive(this.CanCustomize);
			this.uncustomizableIndicator.SetActive(!this.CanCustomize);
		}

		// Token: 0x0400174D RID: 5965
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400174E RID: 5966
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x0400174F RID: 5967
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x04001750 RID: 5968
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001751 RID: 5969
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;

		// Token: 0x04001752 RID: 5970
		[SerializeField]
		private FadeGroup customizeButtonFadeGroup;

		// Token: 0x04001753 RID: 5971
		[SerializeField]
		private FadeGroup placeHolderFadeGroup;

		// Token: 0x04001754 RID: 5972
		[SerializeField]
		private Button beginCustomizeButton;

		// Token: 0x04001755 RID: 5973
		[SerializeField]
		private TextMeshProUGUI selectedItemName;

		// Token: 0x04001756 RID: 5974
		[SerializeField]
		private Image selectedItemIcon;

		// Token: 0x04001757 RID: 5975
		[SerializeField]
		private TrueShadow selectedItemShadow;

		// Token: 0x04001758 RID: 5976
		[SerializeField]
		private GameObject customizableIndicator;

		// Token: 0x04001759 RID: 5977
		[SerializeField]
		private GameObject uncustomizableIndicator;

		// Token: 0x0400175A RID: 5978
		[SerializeField]
		private string noItemSelectedNameText = "-";

		// Token: 0x0400175B RID: 5979
		[SerializeField]
		private Sprite noItemSelectedIconSprite;

		// Token: 0x0400175C RID: 5980
		private List<Inventory> avaliableInventories = new List<Inventory>();
	}
}
