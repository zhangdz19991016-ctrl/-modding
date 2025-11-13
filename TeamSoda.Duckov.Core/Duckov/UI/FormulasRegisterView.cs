using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038D RID: 909
	public class FormulasRegisterView : View
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001F9A RID: 8090 RVA: 0x0006F003 File Offset: 0x0006D203
		public static FormulasRegisterView Instance
		{
			get
			{
				return View.GetViewInstance<FormulasRegisterView>();
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x0006F00A File Offset: 0x0006D20A
		private string FormulaUnlockedNotificationFormat
		{
			get
			{
				return this.formulaUnlockedFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001F9C RID: 8092 RVA: 0x0006F017 File Offset: 0x0006D217
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

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x0006F034 File Offset: 0x0006D234
		private Slot SubmitItemSlot
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
				return this.keySlotItem.Slots[this.slotKey];
			}
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0006F074 File Offset: 0x0006D274
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

		// Token: 0x06001F9F RID: 8095 RVA: 0x0006F100 File Offset: 0x0006D300
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
			if (!this.SubmitItemSlot.CanPlug(item))
			{
				return;
			}
			item.Detach();
			Item item2;
			this.SubmitItemSlot.Plug(item, out item2);
			if (item2 != null)
			{
				ItemUtilities.SendToPlayer(item2, false, true);
			}
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0006F15C File Offset: 0x0006D35C
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

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0006F188 File Offset: 0x0006D388
		private void OnSubmitButtonClicked()
		{
			if (this.SubmitItemSlot != null && this.SubmitItemSlot.Content != null)
			{
				Item content = this.SubmitItemSlot.Content;
				string formulaID = FormulasRegisterView.GetFormulaID(content);
				if (string.IsNullOrEmpty(formulaID))
				{
					return;
				}
				if (CraftingManager.IsFormulaUnlocked(formulaID))
				{
					return;
				}
				CraftingManager.UnlockFormula(formulaID);
				CraftingFormula formula = CraftingManager.GetFormula(formulaID);
				if (formula.IDValid)
				{
					ItemMetaData metaData = ItemAssetsCollection.GetMetaData(formula.result.id);
					string mainText = this.FormulaUnlockedNotificationFormat.Format(new
					{
						itemDisplayName = metaData.DisplayName
					});
					Sprite icon = metaData.icon;
					StrongNotification.Push(new StrongNotificationContent(mainText, "", icon));
				}
				content.Detach();
				content.DestroyTreeImmediate();
				this.IndicateSuccess();
			}
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0006F244 File Offset: 0x0006D444
		private void IndicateSuccess()
		{
			this.SuccessIndicationTask().Forget();
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0006F254 File Offset: 0x0006D454
		private UniTask SuccessIndicationTask()
		{
			FormulasRegisterView.<SuccessIndicationTask>d__28 <SuccessIndicationTask>d__;
			<SuccessIndicationTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SuccessIndicationTask>d__.<>4__this = this;
			<SuccessIndicationTask>d__.<>1__state = -1;
			<SuccessIndicationTask>d__.<>t__builder.Start<FormulasRegisterView.<SuccessIndicationTask>d__28>(ref <SuccessIndicationTask>d__);
			return <SuccessIndicationTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0006F297 File Offset: 0x0006D497
		private void HideSuccessIndication()
		{
			this.succeedIndicator.Hide();
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0006F2A4 File Offset: 0x0006D4A4
		private bool EntryFunc_ShouldHighligh(Item e)
		{
			return !(e == null) && this.SubmitItemSlot.CanPlug(e) && !CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(e));
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0006F2D1 File Offset: 0x0006D4D1
		private bool EntryFunc_CanOperate(Item e)
		{
			return e == null || this.SubmitItemSlot.CanPlug(e);
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0006F2EC File Offset: 0x0006D4EC
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
			this.inventoryDisplay.Setup(characterItem.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), true, null);
			if (PlayerStorage.Inventory != null)
			{
				this.playerStorageInventoryDisplay.ShowOperationButtons = false;
				this.playerStorageInventoryDisplay.gameObject.SetActive(true);
				this.playerStorageInventoryDisplay.Setup(PlayerStorage.Inventory, new Func<Item, bool>(this.EntryFunc_ShouldHighligh), new Func<Item, bool>(this.EntryFunc_CanOperate), true, null);
			}
			else
			{
				this.playerStorageInventoryDisplay.gameObject.SetActive(false);
			}
			this.registerSlotDisplay.Setup(this.SubmitItemSlot);
			this.RefreshRecordExistsIndicator();
			this.RegisterEvents();
			this.fadeGroup.Show();
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0006F3F8 File Offset: 0x0006D5F8
		protected override void OnClose()
		{
			this.UnregisterEvents();
			base.OnClose();
			this.fadeGroup.Hide();
			this.detailsFadeGroup.Hide();
			if (this.SubmitItemSlot != null && this.SubmitItemSlot.Content != null)
			{
				Item content = this.SubmitItemSlot.Content;
				content.Detach();
				ItemUtilities.SendToPlayerCharacterInventory(content, false);
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0006F45A File Offset: 0x0006D65A
		private void RegisterEvents()
		{
			this.SubmitItemSlot.onSlotContentChanged += this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged += this.OnItemSelectionChanged;
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0006F484 File Offset: 0x0006D684
		private void UnregisterEvents()
		{
			this.SubmitItemSlot.onSlotContentChanged -= this.OnSlotContentChanged;
			ItemUIUtilities.OnSelectionChanged -= this.OnItemSelectionChanged;
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0006F4AE File Offset: 0x0006D6AE
		private void OnSlotContentChanged(Slot slot)
		{
			this.RefreshRecordExistsIndicator();
			this.HideSuccessIndication();
			if (((slot != null) ? slot.Content : null) != null)
			{
				AudioManager.PlayPutItemSFX(slot.Content, false);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0006F4DC File Offset: 0x0006D6DC
		private void RefreshRecordExistsIndicator()
		{
			Item content = this.SubmitItemSlot.Content;
			if (content == null)
			{
				this.recordExistsIndicator.SetActive(false);
				return;
			}
			bool active = CraftingManager.IsFormulaUnlocked(FormulasRegisterView.GetFormulaID(content));
			this.recordExistsIndicator.SetActive(active);
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0006F523 File Offset: 0x0006D723
		private bool IsFormulaItem(Item item)
		{
			return !(item == null) && item.GetComponent<ItemSetting_Formula>() != null;
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0006F53C File Offset: 0x0006D73C
		public static string GetFormulaID(Item item)
		{
			if (item == null)
			{
				return null;
			}
			ItemSetting_Formula component = item.GetComponent<ItemSetting_Formula>();
			if (component == null)
			{
				return null;
			}
			return component.formulaID;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0006F56C File Offset: 0x0006D76C
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

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0006F5A2 File Offset: 0x0006D7A2
		public static void Show(ICollection<Tag> requireTags = null)
		{
			if (FormulasRegisterView.Instance == null)
			{
				return;
			}
			FormulasRegisterView.SetupTags(requireTags);
			FormulasRegisterView.Instance.Open(null);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0006F5C4 File Offset: 0x0006D7C4
		private static void SetupTags(ICollection<Tag> requireTags = null)
		{
			if (FormulasRegisterView.Instance == null)
			{
				return;
			}
			Slot submitItemSlot = FormulasRegisterView.Instance.SubmitItemSlot;
			if (submitItemSlot == null)
			{
				return;
			}
			submitItemSlot.requireTags.Clear();
			submitItemSlot.requireTags.Add(FormulasRegisterView.Instance.formulaTag);
			if (requireTags != null)
			{
				submitItemSlot.requireTags.AddRange(requireTags);
			}
		}

		// Token: 0x0400158D RID: 5517
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400158E RID: 5518
		[SerializeField]
		private InventoryDisplay inventoryDisplay;

		// Token: 0x0400158F RID: 5519
		[SerializeField]
		private InventoryDisplay playerStorageInventoryDisplay;

		// Token: 0x04001590 RID: 5520
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001591 RID: 5521
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04001592 RID: 5522
		[SerializeField]
		private Button submitButton;

		// Token: 0x04001593 RID: 5523
		[SerializeField]
		private Tag formulaTag;

		// Token: 0x04001594 RID: 5524
		[SerializeField]
		private Item keySlotItem;

		// Token: 0x04001595 RID: 5525
		[SerializeField]
		private string slotKey = "SubmitItem";

		// Token: 0x04001596 RID: 5526
		[SerializeField]
		private SlotDisplay registerSlotDisplay;

		// Token: 0x04001597 RID: 5527
		[SerializeField]
		private GameObject recordExistsIndicator;

		// Token: 0x04001598 RID: 5528
		[SerializeField]
		private FadeGroup succeedIndicator;

		// Token: 0x04001599 RID: 5529
		[SerializeField]
		private float successIndicationTime = 1.5f;

		// Token: 0x0400159A RID: 5530
		private string sfx_Register = "UI/register";

		// Token: 0x0400159B RID: 5531
		[LocalizationKey("Default")]
		[SerializeField]
		private string formulaUnlockedFormatKey = "UI_Formulas_RegisterSucceedFormat";
	}
}
