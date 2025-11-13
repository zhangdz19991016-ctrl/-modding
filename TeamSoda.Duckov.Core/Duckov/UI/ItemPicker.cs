using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000383 RID: 899
	public class ItemPicker : MonoBehaviour
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0006E63C File Offset: 0x0006C83C
		// (set) Token: 0x06001F52 RID: 8018 RVA: 0x0006E643 File Offset: 0x0006C843
		public static ItemPicker Instance { get; private set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x0006E64C File Offset: 0x0006C84C
		private PrefabPool<ItemPickerEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<ItemPickerEntry>(this.entryPrefab, this.contentParent ? this.contentParent : base.transform, new Action<ItemPickerEntry>(this.OnGetEntry), null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0006E6AA File Offset: 0x0006C8AA
		private void OnGetEntry(ItemPickerEntry entry)
		{
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x0006E6AC File Offset: 0x0006C8AC
		public bool Picking
		{
			get
			{
				return this.picking;
			}
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0006E6B4 File Offset: 0x0006C8B4
		private UniTask<Item> WaitForUserPick(ICollection<Item> candidates)
		{
			ItemPicker.<WaitForUserPick>d__19 <WaitForUserPick>d__;
			<WaitForUserPick>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<WaitForUserPick>d__.<>4__this = this;
			<WaitForUserPick>d__.candidates = candidates;
			<WaitForUserPick>d__.<>1__state = -1;
			<WaitForUserPick>d__.<>t__builder.Start<ItemPicker.<WaitForUserPick>d__19>(ref <WaitForUserPick>d__);
			return <WaitForUserPick>d__.<>t__builder.Task;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0006E700 File Offset: 0x0006C900
		private void Awake()
		{
			if (ItemPicker.Instance == null)
			{
				ItemPicker.Instance = this;
			}
			else
			{
				Debug.LogError("场景中存在两个ItemPicker，请检查。");
			}
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0006E764 File Offset: 0x0006C964
		private void OnCancelButtonClicked()
		{
			this.Cancel();
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0006E76C File Offset: 0x0006C96C
		private void OnConfirmButtonClicked()
		{
			this.ConfirmPick(this.pickedItem);
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0006E77A File Offset: 0x0006C97A
		private void OnDestroy()
		{
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0006E77C File Offset: 0x0006C97C
		private void Update()
		{
			if (!this.picking && this.fadeGroup.IsShown)
			{
				this.fadeGroup.Hide();
			}
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0006E7A0 File Offset: 0x0006C9A0
		public static UniTask<Item> Pick(ICollection<Item> candidates)
		{
			ItemPicker.<Pick>d__25 <Pick>d__;
			<Pick>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Pick>d__.candidates = candidates;
			<Pick>d__.<>1__state = -1;
			<Pick>d__.<>t__builder.Start<ItemPicker.<Pick>d__25>(ref <Pick>d__);
			return <Pick>d__.<>t__builder.Task;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0006E7E3 File Offset: 0x0006C9E3
		public void ConfirmPick(Item item)
		{
			this.confirmed = true;
			this.pickedItem = item;
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0006E7F3 File Offset: 0x0006C9F3
		public void Cancel()
		{
			this.canceled = true;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0006E7FC File Offset: 0x0006C9FC
		private void SetupUI(ICollection<Item> candidates)
		{
			this.EntryPool.ReleaseAll();
			foreach (Item item in candidates)
			{
				if (!(item == null))
				{
					ItemPickerEntry itemPickerEntry = this.EntryPool.Get(null);
					itemPickerEntry.Setup(this, item);
					itemPickerEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0006E870 File Offset: 0x0006CA70
		internal void NotifyEntryClicked(ItemPickerEntry itemPickerEntry, Item target)
		{
			this.pickedItem = target;
		}

		// Token: 0x04001569 RID: 5481
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400156A RID: 5482
		[SerializeField]
		private ItemPickerEntry entryPrefab;

		// Token: 0x0400156B RID: 5483
		[SerializeField]
		private Transform contentParent;

		// Token: 0x0400156C RID: 5484
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400156D RID: 5485
		[SerializeField]
		private Button cancelButton;

		// Token: 0x0400156E RID: 5486
		private PrefabPool<ItemPickerEntry> _entryPool;

		// Token: 0x0400156F RID: 5487
		private bool picking;

		// Token: 0x04001570 RID: 5488
		private bool canceled;

		// Token: 0x04001571 RID: 5489
		private bool confirmed;

		// Token: 0x04001572 RID: 5490
		private Item pickedItem;
	}
}
