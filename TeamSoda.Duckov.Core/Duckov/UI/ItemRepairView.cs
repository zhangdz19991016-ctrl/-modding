using System;
using System.Collections.Generic;
using Duckov.Economy;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BD RID: 957
	public class ItemRepairView : View
	{
		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x000792C8 File Offset: 0x000774C8
		public static ItemRepairView Instance
		{
			get
			{
				return View.GetViewInstance<ItemRepairView>();
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x000792CF File Offset: 0x000774CF
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

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x000792EC File Offset: 0x000774EC
		private bool CanRepair
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				if (selectedItem == null)
				{
					return false;
				}
				if (!selectedItem.UseDurability)
				{
					return false;
				}
				if (selectedItem.MaxDurabilityWithLoss < 1f)
				{
					return false;
				}
				if (!selectedItem.Tags.Contains("Repairable"))
				{
					Debug.Log(selectedItem.DisplayName + " 不包含tag Repairable");
					return false;
				}
				return selectedItem.Durability < selectedItem.MaxDurabilityWithLoss;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x0007935C File Offset: 0x0007755C
		private bool NoNeedToRepair
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && selectedItem.UseDurability && selectedItem.Durability >= selectedItem.MaxDurabilityWithLoss;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x00079398 File Offset: 0x00077598
		private bool Broken
		{
			get
			{
				Item selectedItem = ItemUIUtilities.SelectedItem;
				return !(selectedItem == null) && selectedItem.UseDurability && selectedItem.MaxDurabilityWithLoss < 1f;
			}
		}

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x060022B2 RID: 8882 RVA: 0x000793D0 File Offset: 0x000775D0
		// (remove) Token: 0x060022B3 RID: 8883 RVA: 0x00079404 File Offset: 0x00077604
		public static event Action OnRepaireOptionDone;

		// Token: 0x060022B4 RID: 8884 RVA: 0x00079437 File Offset: 0x00077637
		protected override void Awake()
		{
			base.Awake();
			this.repairButton.onClick.AddListener(new UnityAction(this.OnRepairButtonClicked));
			this.itemDetailsFadeGroup.SkipHide();
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x00079468 File Offset: 0x00077668
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

		// Token: 0x060022B6 RID: 8886 RVA: 0x000794E0 File Offset: 0x000776E0
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
			this.repairButtonFadeGroup.SkipHide();
			this.placeHolderFadeGroup.SkipHide();
			ItemUIUtilities.Select(null);
			this.RefreshSelectedItemInfo();
			this.repairAllPanel.Setup(this);
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x00079581 File Offset: 0x00077781
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000795A5 File Offset: 0x000777A5
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000795B8 File Offset: 0x000777B8
		private void OnItemSelectionChanged()
		{
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000795C0 File Offset: 0x000777C0
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000795D3 File Offset: 0x000777D3
		public static void Show()
		{
			if (ItemRepairView.Instance == null)
			{
				return;
			}
			ItemRepairView.Instance.Open(null);
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000795EE File Offset: 0x000777EE
		public static void Hide()
		{
			if (ItemRepairView.Instance == null)
			{
				return;
			}
			ItemRepairView.Instance.Close();
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00079608 File Offset: 0x00077808
		private void RefreshSelectedItemInfo()
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
			if (this.CanRepair)
			{
				this.placeHolderFadeGroup.Hide();
				this.repairButtonFadeGroup.Show();
			}
			else
			{
				this.repairButtonFadeGroup.Hide();
				this.placeHolderFadeGroup.Show();
			}
			Item selectedItem = ItemUIUtilities.SelectedItem;
			this.willLoseDurabilityText.text = "";
			if (selectedItem == null)
			{
				this.selectedItemName.text = this.noItemSelectedNameText;
				this.selectedItemIcon.sprite = this.noItemSelectedIconSprite;
				this.selectedItemShadow.enabled = false;
				this.noNeedToRepairIndicator.SetActive(false);
				this.brokenIndicator.SetActive(false);
				this.cannotRepairIndicator.SetActive(false);
				this.selectedItemIcon.color = Color.clear;
				this.barFill.fillAmount = 0f;
				this.lossBarFill.fillAmount = 0f;
				this.durabilityText.text = "-";
				return;
			}
			this.selectedItemShadow.enabled = true;
			this.selectedItemIcon.color = Color.white;
			this.selectedItemName.text = selectedItem.DisplayName;
			this.selectedItemIcon.sprite = selectedItem.Icon;
			GameplayDataSettings.UIStyle.GetDisplayQualityLook(selectedItem.DisplayQuality).Apply(this.selectedItemShadow);
			this.noNeedToRepairIndicator.SetActive(!this.Broken && this.NoNeedToRepair && selectedItem.Repairable);
			this.cannotRepairIndicator.SetActive(selectedItem.UseDurability && !selectedItem.Repairable && !this.Broken);
			this.brokenIndicator.SetActive(this.Broken);
			if (this.CanRepair)
			{
				float num2;
				float num3;
				float num4;
				int num = this.CalculateRepairPrice(selectedItem, out num2, out num3, out num4);
				this.repairPriceText.text = num.ToString();
				this.willLoseDurabilityText.text = "UI_MaxDurability".ToPlainText() + " -" + num3.ToString("0.#");
				this.repairButton.interactable = (EconomyManager.Money >= (long)num);
			}
			if (selectedItem.UseDurability)
			{
				float durability = selectedItem.Durability;
				float maxDurability = selectedItem.MaxDurability;
				float maxDurabilityWithLoss = selectedItem.MaxDurabilityWithLoss;
				float num5 = durability / maxDurability;
				this.barFill.fillAmount = num5;
				this.lossBarFill.fillAmount = selectedItem.DurabilityLoss;
				this.durabilityText.text = string.Format("{0:0.#} / {1} ", durability, maxDurabilityWithLoss.ToString("0.#"));
				this.barFill.color = this.barFillColorOverT.Evaluate(num5);
				return;
			}
			this.barFill.fillAmount = 0f;
			this.lossBarFill.fillAmount = 0f;
			this.durabilityText.text = "-";
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x0007990C File Offset: 0x00077B0C
		private void OnRepairButtonClicked()
		{
			Item selectedItem = ItemUIUtilities.SelectedItem;
			if (selectedItem == null)
			{
				return;
			}
			if (!selectedItem.UseDurability)
			{
				return;
			}
			this.Repair(selectedItem, false);
			this.RefreshSelectedItemInfo();
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00079940 File Offset: 0x00077B40
		private void Repair(Item item, bool prepaied = false)
		{
			float num2;
			float num3;
			float num4;
			int num = this.CalculateRepairPrice(item, out num2, out num3, out num4);
			if (!prepaied && !EconomyManager.Pay(new Cost((long)num), true, true))
			{
				return;
			}
			item.DurabilityLoss += num4;
			item.Durability = item.MaxDurability * (1f - item.DurabilityLoss);
			Action onRepaireOptionDone = ItemRepairView.OnRepaireOptionDone;
			if (onRepaireOptionDone == null)
			{
				return;
			}
			onRepaireOptionDone();
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000799A8 File Offset: 0x00077BA8
		private int CalculateRepairPrice(Item item, out float repairAmount, out float lostAmount, out float lostPercentage)
		{
			repairAmount = 0f;
			lostAmount = 0f;
			lostPercentage = 0f;
			if (item == null)
			{
				return 0;
			}
			if (!item.UseDurability)
			{
				return 0;
			}
			float maxDurability = item.MaxDurability;
			float durabilityLoss = item.DurabilityLoss;
			float num = maxDurability * (1f - durabilityLoss);
			float durability = item.Durability;
			repairAmount = num - durability;
			float repairLossRatio = item.GetRepairLossRatio();
			lostAmount = repairAmount * repairLossRatio;
			repairAmount -= lostAmount;
			if (repairAmount <= 0f)
			{
				return 0;
			}
			lostPercentage = lostAmount / maxDurability;
			float num2 = repairAmount / maxDurability;
			return Mathf.CeilToInt((float)item.Value * num2 * 0.5f);
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x00079A48 File Offset: 0x00077C48
		public List<Item> GetAllEquippedItems()
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return null;
			}
			Item characterItem = main.CharacterItem;
			if (characterItem == null)
			{
				return null;
			}
			SlotCollection slots = characterItem.Slots;
			if (slots == null)
			{
				return null;
			}
			List<Item> list = new List<Item>();
			foreach (Slot slot in slots)
			{
				if (slot != null)
				{
					Item content = slot.Content;
					if (!(content == null))
					{
						list.Add(content);
					}
				}
			}
			return list;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x00079AEC File Offset: 0x00077CEC
		public int CalculateRepairPrice(List<Item> itemsToRepair)
		{
			int num = 0;
			foreach (Item item in itemsToRepair)
			{
				float num2;
				float num3;
				float num4;
				num += this.CalculateRepairPrice(item, out num2, out num3, out num4);
			}
			return num;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00079B48 File Offset: 0x00077D48
		public void RepairItems(List<Item> itemsToRepair)
		{
			if (!EconomyManager.Pay(new Cost((long)this.CalculateRepairPrice(itemsToRepair)), true, true))
			{
				return;
			}
			foreach (Item item in itemsToRepair)
			{
				this.Repair(item, true);
			}
		}

		// Token: 0x0400177A RID: 6010
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400177B RID: 6011
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x0400177C RID: 6012
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400177D RID: 6013
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400177E RID: 6014
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;

		// Token: 0x0400177F RID: 6015
		[SerializeField]
		private ItemRepair_RepairAllPanel repairAllPanel;

		// Token: 0x04001780 RID: 6016
		[SerializeField]
		private FadeGroup repairButtonFadeGroup;

		// Token: 0x04001781 RID: 6017
		[SerializeField]
		private FadeGroup placeHolderFadeGroup;

		// Token: 0x04001782 RID: 6018
		[SerializeField]
		private Button repairButton;

		// Token: 0x04001783 RID: 6019
		[SerializeField]
		private TextMeshProUGUI repairPriceText;

		// Token: 0x04001784 RID: 6020
		[SerializeField]
		private TextMeshProUGUI selectedItemName;

		// Token: 0x04001785 RID: 6021
		[SerializeField]
		private Image selectedItemIcon;

		// Token: 0x04001786 RID: 6022
		[SerializeField]
		private TrueShadow selectedItemShadow;

		// Token: 0x04001787 RID: 6023
		[SerializeField]
		private string noItemSelectedNameText = "-";

		// Token: 0x04001788 RID: 6024
		[SerializeField]
		private Sprite noItemSelectedIconSprite;

		// Token: 0x04001789 RID: 6025
		[SerializeField]
		private GameObject noNeedToRepairIndicator;

		// Token: 0x0400178A RID: 6026
		[SerializeField]
		private GameObject brokenIndicator;

		// Token: 0x0400178B RID: 6027
		[SerializeField]
		private GameObject cannotRepairIndicator;

		// Token: 0x0400178C RID: 6028
		[SerializeField]
		private TextMeshProUGUI durabilityText;

		// Token: 0x0400178D RID: 6029
		[SerializeField]
		private TextMeshProUGUI willLoseDurabilityText;

		// Token: 0x0400178E RID: 6030
		[SerializeField]
		private Image barFill;

		// Token: 0x0400178F RID: 6031
		[SerializeField]
		private Image lossBarFill;

		// Token: 0x04001790 RID: 6032
		[SerializeField]
		private Gradient barFillColorOverT;

		// Token: 0x04001791 RID: 6033
		private List<Inventory> avaliableInventories = new List<Inventory>();
	}
}
