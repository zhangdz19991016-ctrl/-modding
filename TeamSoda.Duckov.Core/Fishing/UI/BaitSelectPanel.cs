using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Fishing.UI
{
	// Token: 0x02000219 RID: 537
	public class BaitSelectPanel : MonoBehaviour, ISingleSelectionMenu<BaitSelectPanelEntry>
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x0003F558 File Offset: 0x0003D758
		private PrefabPool<BaitSelectPanelEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<BaitSelectPanelEntry>(this.entry, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600100F RID: 4111 RVA: 0x0003F594 File Offset: 0x0003D794
		// (remove) Token: 0x06001010 RID: 4112 RVA: 0x0003F5CC File Offset: 0x0003D7CC
		internal event Action onSetSelection;

		// Token: 0x06001011 RID: 4113 RVA: 0x0003F604 File Offset: 0x0003D804
		internal UniTask DoBaitSelection(ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			BaitSelectPanel.<DoBaitSelection>d__12 <DoBaitSelection>d__;
			<DoBaitSelection>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoBaitSelection>d__.<>4__this = this;
			<DoBaitSelection>d__.availableBaits = availableBaits;
			<DoBaitSelection>d__.baitSelectionResultCallback = baitSelectionResultCallback;
			<DoBaitSelection>d__.<>1__state = -1;
			<DoBaitSelection>d__.<>t__builder.Start<BaitSelectPanel.<DoBaitSelection>d__12>(ref <DoBaitSelection>d__);
			return <DoBaitSelection>d__.<>t__builder.Task;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0003F657 File Offset: 0x0003D857
		private void Open()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0003F664 File Offset: 0x0003D864
		private void Close()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0003F671 File Offset: 0x0003D871
		private Item SelectedItem
		{
			get
			{
				BaitSelectPanelEntry baitSelectPanelEntry = this.selectedEntry;
				if (baitSelectPanelEntry == null)
				{
					return null;
				}
				return baitSelectPanelEntry.Target;
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003F684 File Offset: 0x0003D884
		private UniTask<Item> WaitForSelection()
		{
			BaitSelectPanel.<WaitForSelection>d__20 <WaitForSelection>d__;
			<WaitForSelection>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<WaitForSelection>d__.<>4__this = this;
			<WaitForSelection>d__.<>1__state = -1;
			<WaitForSelection>d__.<>t__builder.Start<BaitSelectPanel.<WaitForSelection>d__20>(ref <WaitForSelection>d__);
			return <WaitForSelection>d__.<>t__builder.Task;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0003F6C8 File Offset: 0x0003D8C8
		private void Setup(ICollection<Item> availableBaits)
		{
			this.selectedEntry = null;
			this.EntryPool.ReleaseAll();
			foreach (Item cur in availableBaits)
			{
				this.EntryPool.Get(null).Setup(this, cur);
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003F730 File Offset: 0x0003D930
		internal void NotifyStop()
		{
			this.Close();
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0003F738 File Offset: 0x0003D938
		private void Awake()
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0003F772 File Offset: 0x0003D972
		private void OnConfirmButtonClicked()
		{
			if (this.SelectedItem == null)
			{
				NotificationText.Push("Fishing_PleaseSelectBait".ToPlainText());
				return;
			}
			this.confirmed = true;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0003F799 File Offset: 0x0003D999
		private void OnCancelButtonClicked()
		{
			this.canceled = true;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0003F7A2 File Offset: 0x0003D9A2
		internal void NotifySelect(BaitSelectPanelEntry baitSelectPanelEntry)
		{
			this.SetSelection(baitSelectPanelEntry);
			if (this.SelectedItem != null)
			{
				this.details.Setup(this.SelectedItem);
				this.detailsFadeGroup.Show();
				return;
			}
			this.detailsFadeGroup.SkipHide();
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0003F7E2 File Offset: 0x0003D9E2
		public BaitSelectPanelEntry GetSelection()
		{
			return this.selectedEntry;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0003F7EA File Offset: 0x0003D9EA
		public bool SetSelection(BaitSelectPanelEntry selection)
		{
			this.selectedEntry = selection;
			Action action = this.onSetSelection;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x04000CEE RID: 3310
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CEF RID: 3311
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04000CF0 RID: 3312
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04000CF1 RID: 3313
		[SerializeField]
		private ItemDetailsDisplay details;

		// Token: 0x04000CF2 RID: 3314
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04000CF3 RID: 3315
		[SerializeField]
		private BaitSelectPanelEntry entry;

		// Token: 0x04000CF4 RID: 3316
		private PrefabPool<BaitSelectPanelEntry> _entryPool;

		// Token: 0x04000CF6 RID: 3318
		private BaitSelectPanelEntry selectedEntry;

		// Token: 0x04000CF7 RID: 3319
		private bool canceled;

		// Token: 0x04000CF8 RID: 3320
		private bool confirmed;
	}
}
