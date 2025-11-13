using System;
using System.Collections.Generic;
using Duckov.Utilities;
using UnityEngine;

namespace ItemStatsSystem.Items
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	public class Slot
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00008284 File Offset: 0x00006484
		public Item Master
		{
			get
			{
				SlotCollection slotCollection = this.collection;
				if (slotCollection == null)
				{
					return null;
				}
				return slotCollection.Master;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00008297 File Offset: 0x00006497
		public Item Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000829F File Offset: 0x0000649F
		private StringList referenceKeys
		{
			get
			{
				return StringLists.SlotNames;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000082A6 File Offset: 0x000064A6
		// (set) Token: 0x06000214 RID: 532 RVA: 0x000082AE File Offset: 0x000064AE
		public Sprite SlotIcon
		{
			get
			{
				return this.slotIcon;
			}
			set
			{
				this.slotIcon = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000082B7 File Offset: 0x000064B7
		public bool ForbidItemsWithSameID
		{
			get
			{
				return this.forbidItemsWithSameID;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000216 RID: 534 RVA: 0x000082BF File Offset: 0x000064BF
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000082C8 File Offset: 0x000064C8
		public string DisplayName
		{
			get
			{
				if (this.requireTags == null || this.requireTags.Count < 1)
				{
					return "?";
				}
				Tag tag = this.requireTags[0];
				if (tag == null)
				{
					return "?";
				}
				return tag.DisplayName;
			}
		}

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000218 RID: 536 RVA: 0x00008314 File Offset: 0x00006514
		// (remove) Token: 0x06000219 RID: 537 RVA: 0x0000834C File Offset: 0x0000654C
		public event Action<Slot> onSlotContentChanged;

		// Token: 0x0600021A RID: 538 RVA: 0x00008381 File Offset: 0x00006581
		public void Initialize(SlotCollection collection)
		{
			this.collection = collection;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000838A File Offset: 0x0000658A
		public void ForceInvokeSlotContentChangedEvent()
		{
			Action<Slot> action = this.onSlotContentChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000083A0 File Offset: 0x000065A0
		public bool Plug(Item otherItem, out Item unpluggedItem)
		{
			unpluggedItem = null;
			if (!this.CheckAbleToPlug(otherItem))
			{
				Debug.Log("Unable to Plug");
				return false;
			}
			if (this.content != null)
			{
				if (this.content.Stackable && this.content.TypeID == otherItem.TypeID)
				{
					this.content.Combine(otherItem);
					this.Master.NotifySlotPlugged(this);
					Action<Slot> action = this.onSlotContentChanged;
					if (action != null)
					{
						action(this);
					}
					this.content.InitiateNotifyItemTreeChanged();
					return otherItem.StackCount <= 0;
				}
				unpluggedItem = this.Unplug();
			}
			if (otherItem.PluggedIntoSlot != null)
			{
				otherItem.Detach();
			}
			if (otherItem.InInventory != null)
			{
				otherItem.Detach();
			}
			this.content = otherItem;
			otherItem.transform.SetParent(this.collection.transform);
			otherItem.NotifyPluggedTo(this);
			this.Master.NotifySlotPlugged(this);
			otherItem.InitiateNotifyItemTreeChanged();
			Action<Slot> action2 = this.onSlotContentChanged;
			if (action2 != null)
			{
				action2(this);
			}
			return true;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000084A8 File Offset: 0x000066A8
		private bool CheckAbleToPlug(Item otherItem)
		{
			if (otherItem == null)
			{
				return false;
			}
			if (otherItem == this.content)
			{
				return false;
			}
			if (this.forbidItemsWithSameID && this.collection != null)
			{
				foreach (Slot slot in this.collection)
				{
					if (slot != null && slot != this && slot.ForbidItemsWithSameID)
					{
						Item item = slot.Content;
						if (!(item == null) && !(item == otherItem) && item.TypeID == otherItem.TypeID)
						{
							return false;
						}
					}
				}
			}
			return !this.Master.GetAllParents(false).Contains(otherItem) && otherItem.Tags.Check(this.requireTags, this.excludeTags);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008590 File Offset: 0x00006790
		public Item Unplug()
		{
			Item item = this.content;
			this.content = null;
			if (item != null)
			{
				if (!item.IsBeingDestroyed)
				{
					item.transform.SetParent(null);
				}
				item.NotifyUnpluggedFrom(this);
				this.Master.NotifySlotUnplugged(this);
				item.InitiateNotifyItemTreeChanged();
				this.Master.InitiateNotifyItemTreeChanged();
				Action<Slot> action = this.onSlotContentChanged;
				if (action != null)
				{
					action(this);
				}
			}
			return item;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000085FF File Offset: 0x000067FF
		public bool CanPlug(Item item)
		{
			return !(item == null) && this.CheckAbleToPlug(item);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008613 File Offset: 0x00006813
		public Slot()
		{
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00008631 File Offset: 0x00006831
		public Slot(string key)
		{
			this.key = key;
		}

		// Token: 0x040000C1 RID: 193
		[NonSerialized]
		private SlotCollection collection;

		// Token: 0x040000C2 RID: 194
		[SerializeField]
		private string key;

		// Token: 0x040000C3 RID: 195
		[SerializeField]
		private Sprite slotIcon;

		// Token: 0x040000C4 RID: 196
		private Item content;

		// Token: 0x040000C5 RID: 197
		public List<Tag> requireTags = new List<Tag>();

		// Token: 0x040000C6 RID: 198
		public List<Tag> excludeTags = new List<Tag>();

		// Token: 0x040000C7 RID: 199
		[SerializeField]
		private bool forbidItemsWithSameID;
	}
}
