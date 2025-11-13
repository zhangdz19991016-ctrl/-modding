using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003BC RID: 956
	public class ItemDecomposeView : View
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x00078DE2 File Offset: 0x00076FE2
		public static ItemDecomposeView Instance
		{
			get
			{
				return View.GetViewInstance<ItemDecomposeView>();
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x00078DE9 File Offset: 0x00076FE9
		private Item SelectedItem
		{
			get
			{
				return ItemUIUtilities.SelectedItem;
			}
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x00078DF0 File Offset: 0x00076FF0
		protected override void Awake()
		{
			base.Awake();
			this.decomposeButton.onClick.AddListener(new UnityAction(this.OnDecomposeButtonClick));
			this.countSlider.OnValueChangedEvent += this.OnSliderValueChanged;
			this.SetupEmpty();
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00078E3C File Offset: 0x0007703C
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.countSlider.OnValueChangedEvent -= this.OnSliderValueChanged;
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x00078E5C File Offset: 0x0007705C
		private void OnDecomposeButtonClick()
		{
			if (this.decomposing)
			{
				return;
			}
			if (this.SelectedItem == null)
			{
				return;
			}
			int value = this.countSlider.Value;
			this.DecomposeTask(this.SelectedItem, value).Forget();
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x00078E9F File Offset: 0x0007709F
		private void OnFastPick(UIInputEventData data)
		{
			this.OnDecomposeButtonClick();
			data.Use();
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x00078EB0 File Offset: 0x000770B0
		private UniTask DecomposeTask(Item item, int count)
		{
			ItemDecomposeView.<DecomposeTask>d__21 <DecomposeTask>d__;
			<DecomposeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DecomposeTask>d__.<>4__this = this;
			<DecomposeTask>d__.item = item;
			<DecomposeTask>d__.count = count;
			<DecomposeTask>d__.<>1__state = -1;
			<DecomposeTask>d__.<>t__builder.Start<ItemDecomposeView.<DecomposeTask>d__21>(ref <DecomposeTask>d__);
			return <DecomposeTask>d__.<>t__builder.Task;
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00078F04 File Offset: 0x00077104
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			ItemUIUtilities.Select(null);
			this.detailsFadeGroup.SkipHide();
			if (CharacterMainControl.Main != null)
			{
				this.characterInventoryDisplay.gameObject.SetActive(true);
				this.characterInventoryDisplay.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, (Item e) => e == null || DecomposeDatabase.CanDecompose(e.TypeID), false, null);
			}
			else
			{
				this.characterInventoryDisplay.gameObject.SetActive(false);
			}
			if (PlayerStorage.Inventory != null)
			{
				this.storageDisplay.gameObject.SetActive(true);
				this.storageDisplay.Setup(PlayerStorage.Inventory, null, (Item e) => e == null || DecomposeDatabase.CanDecompose(e.TypeID), false, null);
			}
			else
			{
				this.storageDisplay.gameObject.SetActive(false);
			}
			this.Refresh();
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x00079009 File Offset: 0x00077209
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x0007901C File Offset: 0x0007721C
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			UIInputManager.OnFastPick += this.OnFastPick;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00079040 File Offset: 0x00077240
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			UIInputManager.OnFastPick -= this.OnFastPick;
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00079064 File Offset: 0x00077264
		private void OnSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0007906C File Offset: 0x0007726C
		private void OnSliderValueChanged(float value)
		{
			this.RefreshResult(this.SelectedItem, Mathf.RoundToInt(value));
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x00079080 File Offset: 0x00077280
		private void Refresh()
		{
			if (this.SelectedItem == null)
			{
				this.SetupEmpty();
				return;
			}
			this.Setup(this.SelectedItem);
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000790A4 File Offset: 0x000772A4
		private void SetupEmpty()
		{
			this.detailsFadeGroup.Hide();
			this.targetNameDisplay.text = "-";
			this.resultDisplay.Clear();
			this.cannotDecomposeIndicator.SetActive(false);
			this.decomposeButton.gameObject.SetActive(false);
			this.noItemSelectedIndicator.SetActive(true);
			this.busyIndicator.SetActive(false);
			this.countSlider.SetMinMax(1, 1);
			this.countSlider.Value = 1;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x00079128 File Offset: 0x00077328
		private void Setup(Item selectedItem)
		{
			if (selectedItem == null)
			{
				return;
			}
			this.noItemSelectedIndicator.SetActive(false);
			this.detailsDisplay.Setup(selectedItem);
			this.detailsFadeGroup.Show();
			this.targetNameDisplay.text = selectedItem.DisplayName;
			bool valid = DecomposeDatabase.GetDecomposeFormula(selectedItem.TypeID).valid;
			this.decomposeButton.gameObject.SetActive(valid);
			this.cannotDecomposeIndicator.gameObject.SetActive(!valid);
			this.SetupSlider(selectedItem);
			this.RefreshResult(selectedItem, Mathf.RoundToInt((float)this.countSlider.Value));
			this.busyIndicator.SetActive(this.decomposing);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000791DC File Offset: 0x000773DC
		private void SetupSlider(Item selectedItem)
		{
			if (selectedItem.Stackable)
			{
				this.countSlider.SetMinMax(1, selectedItem.StackCount);
				this.countSlider.Value = selectedItem.StackCount;
				return;
			}
			this.countSlider.SetMinMax(1, 1);
			this.countSlider.Value = 1;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00079230 File Offset: 0x00077430
		private void RefreshResult(Item selectedItem, int count)
		{
			if (selectedItem == null)
			{
				this.countSlider.SetMinMax(1, 1);
				this.countSlider.Value = 1;
				return;
			}
			DecomposeFormula decomposeFormula = DecomposeDatabase.GetDecomposeFormula(selectedItem.TypeID);
			if (decomposeFormula.valid)
			{
				bool stackable = selectedItem.Stackable;
				this.resultDisplay.Setup(decomposeFormula.result, count);
				return;
			}
			this.resultDisplay.Clear();
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x0007929C File Offset: 0x0007749C
		internal static void Show()
		{
			ItemDecomposeView instance = ItemDecomposeView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Open(null);
		}

		// Token: 0x0400176D RID: 5997
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400176E RID: 5998
		[SerializeField]
		private InventoryDisplay characterInventoryDisplay;

		// Token: 0x0400176F RID: 5999
		[SerializeField]
		private InventoryDisplay storageDisplay;

		// Token: 0x04001770 RID: 6000
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04001771 RID: 6001
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x04001772 RID: 6002
		[SerializeField]
		private DecomposeSlider countSlider;

		// Token: 0x04001773 RID: 6003
		[SerializeField]
		private TextMeshProUGUI targetNameDisplay;

		// Token: 0x04001774 RID: 6004
		[SerializeField]
		private CostDisplay resultDisplay;

		// Token: 0x04001775 RID: 6005
		[SerializeField]
		private GameObject cannotDecomposeIndicator;

		// Token: 0x04001776 RID: 6006
		[SerializeField]
		private GameObject noItemSelectedIndicator;

		// Token: 0x04001777 RID: 6007
		[SerializeField]
		private Button decomposeButton;

		// Token: 0x04001778 RID: 6008
		[SerializeField]
		private GameObject busyIndicator;

		// Token: 0x04001779 RID: 6009
		private bool decomposing;
	}
}
