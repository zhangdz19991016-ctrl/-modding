using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B9 RID: 953
	public class InventoryView : View
	{
		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002266 RID: 8806 RVA: 0x00078239 File Offset: 0x00076439
		private static InventoryView Instance
		{
			get
			{
				return View.GetViewInstance<InventoryView>();
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x00078240 File Offset: 0x00076440
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

		// Token: 0x06002268 RID: 8808 RVA: 0x0007825D File Offset: 0x0007645D
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00078268 File Offset: 0x00076468
		private void Update()
		{
			bool editable = true;
			this.inventoryDisplay.Editable = editable;
			this.slotDisplay.Editable = editable;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00078290 File Offset: 0x00076490
		protected override void OnOpen()
		{
			this.UnregisterEvents();
			base.OnOpen();
			Item characterItem = this.CharacterItem;
			if (characterItem == null)
			{
				Debug.LogError("物品栏开启失败，角色物体不存在");
				base.Close();
				return;
			}
			base.gameObject.SetActive(true);
			this.slotDisplay.Setup(characterItem, false);
			this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x0007830C File Offset: 0x0007650C
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.itemDetailsFadeGroup.Hide();
			if (SplitDialogue.Instance && SplitDialogue.Instance.isActiveAndEnabled)
			{
				SplitDialogue.Instance.Cancel();
			}
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x0007835D File Offset: 0x0007655D
		private void RegisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00078370 File Offset: 0x00076570
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.itemDetailsFadeGroup.Show();
				return;
			}
			this.itemDetailsFadeGroup.Hide();
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000783A6 File Offset: 0x000765A6
		private void UnregisterEvents()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000783B9 File Offset: 0x000765B9
		public static void Show()
		{
			if (!LevelManager.LevelInited)
			{
				return;
			}
			LootView instance = LootView.Instance;
			if (instance != null)
			{
				instance.Show();
			}
			if (LootView.Instance == null)
			{
				Debug.Log("LOOTVIEW INSTANCE IS NULL");
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000783EA File Offset: 0x000765EA
		public static void Hide()
		{
			LootView instance = LootView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Close();
		}

		// Token: 0x04001748 RID: 5960
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001749 RID: 5961
		[SerializeField]
		private ItemSlotCollectionDisplay slotDisplay;

		// Token: 0x0400174A RID: 5962
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400174B RID: 5963
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x0400174C RID: 5964
		[SerializeField]
		private FadeGroup itemDetailsFadeGroup;
	}
}
