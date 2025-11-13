using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E7 RID: 743
	public class MasterKeysRegisterView : View
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x000580A8 File Offset: 0x000562A8
		public static MasterKeysRegisterView Instance
		{
			get
			{
				return View.GetViewInstance<MasterKeysRegisterView>();
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060017EB RID: 6123 RVA: 0x000580AF File Offset: 0x000562AF
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

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060017EC RID: 6124 RVA: 0x000580CC File Offset: 0x000562CC
		private Slot KeySlot
		{
			get
			{
				if (this.keySlotItem == null)
				{
					return null;
				}
				if (this.keySlotItem.Slots == null)
				{
					return null;
				}
				return this.keySlotItem.Slots[this.keySlotKey];
			}
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0005810C File Offset: 0x0005630C
		protected override void Awake()
		{
			base.Awake();
			this.submitButton.onClick.AddListener(new UnityAction(this.OnSubmitButtonClicked));
			this.succeedIndicator.SkipHide();
			this.detailsFadeGroup.SkipHide();
			this.registerSlotDisplay.onSlotDisplayDoubleClicked += this.OnSlotDoubleClicked;
			this.inventoryDisplay.onDisplayDoubleClicked += this.OnInventoryItemDoubleClicked;
			this.playerStorageInventoryDisplay.onDisplayDoubleClicked += this.OnInventoryItemDoubleClicked;
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00058198 File Offset: 0x00056398
		private void OnInventoryItemDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
		{
			if (!entry.Editable)
			{
				return;
			}
			Item item = entry.Item;
			if (item == null)
			{
				return;
			}
			if (!this.KeySlot.CanPlug(item))
			{
				return;
			}
			item.Detach();
			Item item2;
			this.KeySlot.Plug(item, out item2);
			if (item2 != null)
			{
				ItemUtilities.SendToPlayer(item2, false, true);
			}
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x000581F4 File Offset: 0x000563F4
		private void OnSlotDoubleClicked(SlotDisplay display)
		{
			Item item = display.GetItem();
			if (item == null)
			{
				return;
			}
			item.Detach();
			ItemUtilities.SendToPlayer(item, false, true);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00058220 File Offset: 0x00056420
		private void OnSubmitButtonClicked()
		{
			if (this.KeySlot != null && this.KeySlot.Content != null && MasterKeysManager.SubmitAndActivate(this.KeySlot.Content))
			{
				this.IndicateSuccess();
			}
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00058255 File Offset: 0x00056455
		private void IndicateSuccess()
		{
			this.SuccessIndicationTask().Forget();
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00058264 File Offset: 0x00056464
		private UniTask SuccessIndicationTask()
		{
			MasterKeysRegisterView.<SuccessIndicationTask>d__24 <SuccessIndicationTask>d__;
			<SuccessIndicationTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SuccessIndicationTask>d__.<>4__this = this;
			<SuccessIndicationTask>d__.<>1__state = -1;
			<SuccessIndicationTask>d__.<>t__builder.Start<MasterKeysRegisterView.<SuccessIndicationTask>d__24>(ref <SuccessIndicationTask>d__);
			return <SuccessIndicationTask>d__.<>t__builder.Task;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x000582A7 File Offset: 0x000564A7
		private void HideSuccessIndication()
		{
			this.succeedIndicator.Hide();
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000582B4 File Offset: 0x000564B4
		private bool EntryFunc_ShouldHighligh(Item e)
		{
			return !(e == null) && this.KeySlot.CanPlug(e) && !MasterKeysManager.IsActive(e.TypeID);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000582E1 File Offset: 0x000564E1
		private bool EntryFunc_CanOperate(Item e)
		{
			return e == null || this.KeySlot.CanPlug(e);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x000582FC File Offset: 0x000564FC
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
			this.inventoryDisplay.ShowOperationButtons = false;
			this.inventoryDisplay.Setup(characterItem.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), false, null);
			if (PlayerStorage.Inventory != null)
			{
				this.playerStorageInventoryDisplay.ShowOperationButtons = false;
				this.playerStorageInventoryDisplay.gameObject.SetActive(true);
				this.playerStorageInventoryDisplay.Setup(PlayerStorage.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), false, null);
			}
			else
			{
				this.playerStorageInventoryDisplay.gameObject.SetActive(false);
			}
			this.registerSlotDisplay.Setup(this.KeySlot);
			this.RefreshRecordExistsIndicator();
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00058408 File Offset: 0x00056608
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			if (this.KeySlot != null && this.KeySlot.Content != null)
			{
				Item content = this.KeySlot.Content;
				content.Detach();
				ItemUtilities.SendToPlayerCharacterInventory(content, false);
			}
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x0005846A File Offset: 0x0005666A
		private void RegisterEvents()
		{
			this.KeySlot.onSlotContentChanged += this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00058494 File Offset: 0x00056694
		private void UnregisterEvents()
		{
			this.KeySlot.onSlotContentChanged -= this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x000584BE File Offset: 0x000566BE
		private void OnSlotContentChanged(Slot slot)
		{
			this.RefreshRecordExistsIndicator();
			this.HideSuccessIndication();
			if (((slot != null) ? slot.Content : null) != null)
			{
				AudioManager.PlayPutItemSFX(slot.Content, false);
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x000584EC File Offset: 0x000566EC
		private void RefreshRecordExistsIndicator()
		{
			Item content = this.KeySlot.Content;
			if (content == null)
			{
				this.recordExistsIndicator.SetActive(false);
				return;
			}
			bool active = MasterKeysManager.IsActive(content.TypeID);
			this.recordExistsIndicator.SetActive(active);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00058533 File Offset: 0x00056733
		private void OnItemSelectionChanged()
		{
			if (ItemUIUtilities.SelectedItem != null)
			{
				this.detailsDisplay.Setup(ItemUIUtilities.SelectedItem);
				this.detailsFadeGroup.Show();
				return;
			}
			this.detailsFadeGroup.Hide();
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00058569 File Offset: 0x00056769
		public static void Show()
		{
			if (MasterKeysRegisterView.Instance == null)
			{
				return;
			}
			MasterKeysRegisterView.Instance.Open(null);
		}

		// Token: 0x0400116C RID: 4460
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400116D RID: 4461
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400116E RID: 4462
		[SerializeField]
		private InventoryDisplay playerStorageInventoryDisplay;

		// Token: 0x0400116F RID: 4463
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001170 RID: 4464
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04001171 RID: 4465
		[SerializeField]
		private Button submitButton;

		// Token: 0x04001172 RID: 4466
		[SerializeField]
		private Item keySlotItem;

		// Token: 0x04001173 RID: 4467
		[SerializeField]
		private string keySlotKey = "Key";

		// Token: 0x04001174 RID: 4468
		[SerializeField]
		private SlotDisplay registerSlotDisplay;

		// Token: 0x04001175 RID: 4469
		[SerializeField]
		private GameObject recordExistsIndicator;

		// Token: 0x04001176 RID: 4470
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04001177 RID: 4471
		[SerializeField]
		private float successIndicationTime = 1.5f;

		// Token: 0x04001178 RID: 4472
		private string sfx_Register = "UI/register";
	}
}
