using System;
using System.Collections.Generic;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200035F RID: 863
	public class SubmitItems : Task
	{
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x0006BB92 File Offset: 0x00069D92
		public int ItemTypeID
		{
			get
			{
				return this.itemTypeID;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x0006BB9C File Offset: 0x00069D9C
		private ItemMetaData CachedMeta
		{
			get
			{
				if (this._cachedMeta == null || this._cachedMeta.Value.id != this.itemTypeID)
				{
					this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.itemTypeID));
				}
				return this._cachedMeta.Value;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x0006BBEF File Offset: 0x00069DEF
		private string descriptionFormatKey
		{
			get
			{
				return "Task_SubmitItems";
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x0006BBF6 File Offset: 0x00069DF6
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x0006BC03 File Offset: 0x00069E03
		private string havingAmountFormatKey
		{
			get
			{
				return "Task_SubmitItems_HavingAmount";
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x0006BC0A File Offset: 0x00069E0A
		private string HavingAmountFormat
		{
			get
			{
				return this.havingAmountFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x0006BC18 File Offset: 0x00069E18
		public override string Description
		{
			get
			{
				string text = this.DescriptionFormat.Format(new
				{
					ItemDisplayName = this.CachedMeta.DisplayName,
					submittedAmount = this.submittedAmount,
					requiredAmount = this.requiredAmount
				});
				if (!base.IsFinished())
				{
					text = text + " " + this.HavingAmountFormat.Format(new
					{
						amount = ItemUtilities.GetItemCount(this.itemTypeID)
					});
				}
				return text;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x0006BC80 File Offset: 0x00069E80
		public override Sprite Icon
		{
			get
			{
				return this.CachedMeta.icon;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x0006BC8D File Offset: 0x00069E8D
		public override bool Interactable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x0006BC90 File Offset: 0x00069E90
		public override bool PossibleValidInteraction
		{
			get
			{
				return this.CheckItemEnough();
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0006BC98 File Offset: 0x00069E98
		public override string InteractText
		{
			get
			{
				return "Task_SubmitItems_Interact".ToPlainText();
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x0006BCA4 File Offset: 0x00069EA4
		public override bool NeedInspection
		{
			get
			{
				return !base.IsFinished() && this.CheckItemEnough();
			}
		}

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06001E53 RID: 7763 RVA: 0x0006BCB8 File Offset: 0x00069EB8
		// (remove) Token: 0x06001E54 RID: 7764 RVA: 0x0006BCEC File Offset: 0x00069EEC
		public static event Action<SubmitItems> onItemEnough;

		// Token: 0x06001E55 RID: 7765 RVA: 0x0006BD1F File Offset: 0x00069F1F
		protected override void OnInit()
		{
			base.OnInit();
			PlayerStorage.OnPlayerStorageChange += this.OnPlayerStorageChanged;
			CharacterMainControl.OnMainCharacterInventoryChangedEvent = (Action<CharacterMainControl, Inventory, int>)Delegate.Combine(CharacterMainControl.OnMainCharacterInventoryChangedEvent, new Action<CharacterMainControl, Inventory, int>(this.OnMainCharacterInventoryChanged));
			this.CheckItemEnough();
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0006BD5F File Offset: 0x00069F5F
		private void OnDestroy()
		{
			PlayerStorage.OnPlayerStorageChange -= this.OnPlayerStorageChanged;
			CharacterMainControl.OnMainCharacterInventoryChangedEvent = (Action<CharacterMainControl, Inventory, int>)Delegate.Remove(CharacterMainControl.OnMainCharacterInventoryChangedEvent, new Action<CharacterMainControl, Inventory, int>(this.OnMainCharacterInventoryChanged));
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x0006BD94 File Offset: 0x00069F94
		private void OnPlayerStorageChanged(PlayerStorage storage, Inventory inventory, int index)
		{
			if (base.Master.Complete)
			{
				return;
			}
			Item itemAt = inventory.GetItemAt(index);
			if (itemAt == null)
			{
				return;
			}
			if (itemAt.TypeID == this.itemTypeID)
			{
				this.CheckItemEnough();
			}
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x0006BDD8 File Offset: 0x00069FD8
		private void OnMainCharacterInventoryChanged(CharacterMainControl control, Inventory inventory, int index)
		{
			if (base.Master.Complete)
			{
				return;
			}
			Item itemAt = inventory.GetItemAt(index);
			if (itemAt == null)
			{
				return;
			}
			if (itemAt.TypeID == this.itemTypeID)
			{
				this.CheckItemEnough();
			}
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0006BE1A File Offset: 0x0006A01A
		private bool CheckItemEnough()
		{
			if (ItemUtilities.GetItemCount(this.itemTypeID) >= this.requiredAmount)
			{
				Action<SubmitItems> action = SubmitItems.onItemEnough;
				if (action != null)
				{
					action(this);
				}
				this.SetMapElementVisable(false);
				return true;
			}
			this.SetMapElementVisable(true);
			return false;
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0006BE51 File Offset: 0x0006A051
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0006BE88 File Offset: 0x0006A088
		public void Submit(Item item)
		{
			if (item.TypeID != this.itemTypeID)
			{
				Debug.LogError("提交的物品类型与需求不一致。");
				return;
			}
			int num = this.requiredAmount - this.submittedAmount;
			if (num <= 0)
			{
				Debug.LogError("目标已达成，不需要继续提交物品");
				return;
			}
			int num2 = this.submittedAmount;
			if (num < item.StackCount)
			{
				item.StackCount -= num;
				this.submittedAmount += num;
			}
			else
			{
				foreach (Item item2 in item.GetAllChildren(false, true))
				{
					item2.Detach();
					if (!ItemUtilities.SendToPlayerCharacter(item2, false))
					{
						item2.Drop(CharacterMainControl.Main, true);
					}
				}
				item.Detach();
				item.DestroyTree();
				this.submittedAmount += item.StackCount;
			}
			Debug.Log("submission done");
			if (num2 != this.submittedAmount)
			{
				base.Master.NotifyTaskFinished(this);
			}
			base.ReportStatusChanged();
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0006BF9C File Offset: 0x0006A19C
		protected override bool CheckFinished()
		{
			return this.submittedAmount >= this.requiredAmount;
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0006BFAF File Offset: 0x0006A1AF
		public override object GenerateSaveData()
		{
			return this.submittedAmount;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0006BFBC File Offset: 0x0006A1BC
		public override void SetupSaveData(object data)
		{
			this.submittedAmount = (int)data;
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0006BFCC File Offset: 0x0006A1CC
		public override void Interact()
		{
			if (base.Master == null)
			{
				return;
			}
			List<Item> list = ItemUtilities.FindAllBelongsToPlayer((Item e) => e != null && e.TypeID == this.itemTypeID);
			for (int i = 0; i < list.Count; i++)
			{
				Item item = list[i];
				this.Submit(item);
				if (base.IsFinished())
				{
					break;
				}
			}
		}

		// Token: 0x040014CA RID: 5322
		[ItemTypeID]
		[SerializeField]
		private int itemTypeID;

		// Token: 0x040014CB RID: 5323
		[Range(1f, 100f)]
		[SerializeField]
		private int requiredAmount = 1;

		// Token: 0x040014CC RID: 5324
		[SerializeField]
		private int submittedAmount;

		// Token: 0x040014CD RID: 5325
		private ItemMetaData? _cachedMeta;

		// Token: 0x040014CE RID: 5326
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
