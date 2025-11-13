using System;
using System.Collections;
using System.Collections.Generic;

namespace ItemStatsSystem.Items
{
	// Token: 0x0200002A RID: 42
	public class SlotCollection : ItemComponent, ICollection<Slot>, IEnumerable<Slot>, IEnumerable
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00008656 File Offset: 0x00006856
		private Dictionary<int, Slot> slotsDictionary
		{
			get
			{
				if (this._cachedSlotsDictionary == null)
				{
					this.BuildDictionary();
				}
				return this._cachedSlotsDictionary;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000866C File Offset: 0x0000686C
		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00008683 File Offset: 0x00006883
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008686 File Offset: 0x00006886
		public Slot GetSlotByIndex(int index)
		{
			return this.list[index];
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008694 File Offset: 0x00006894
		public Slot GetSlot(int hash)
		{
			Slot result;
			if (this.slotsDictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000086B4 File Offset: 0x000068B4
		public Slot GetSlot(string key)
		{
			int hashCode = key.GetHashCode();
			Slot slot = this.GetSlot(hashCode);
			if (slot == null)
			{
				slot = this.list.Find((Slot e) => e.Key == key);
			}
			return slot;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008700 File Offset: 0x00006900
		private void BuildDictionary()
		{
			if (this._cachedSlotsDictionary == null)
			{
				this._cachedSlotsDictionary = new Dictionary<int, Slot>();
			}
			this._cachedSlotsDictionary.Clear();
			foreach (Slot slot in this.list)
			{
				int hashCode = slot.Key.GetHashCode();
				this._cachedSlotsDictionary[hashCode] = slot;
			}
		}

		// Token: 0x1700009B RID: 155
		public Slot this[string key]
		{
			get
			{
				return this.GetSlot(key);
			}
		}

		// Token: 0x1700009C RID: 156
		public Slot this[int index]
		{
			get
			{
				return this.GetSlotByIndex(index);
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008798 File Offset: 0x00006998
		internal override void OnInitialize()
		{
			foreach (Slot slot in this.list)
			{
				slot.Initialize(this);
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000087EC File Offset: 0x000069EC
		public IEnumerator<Slot> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000087FE File Offset: 0x000069FE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008810 File Offset: 0x00006A10
		public void Add(Slot item)
		{
			this.list.Add(item);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000881E File Offset: 0x00006A1E
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000882B File Offset: 0x00006A2B
		public bool Contains(Slot item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008839 File Offset: 0x00006A39
		public void CopyTo(Slot[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00008848 File Offset: 0x00006A48
		public bool Remove(Slot item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x040000C9 RID: 201
		public Action<Slot> OnSlotContentChanged;

		// Token: 0x040000CA RID: 202
		public List<Slot> list;

		// Token: 0x040000CB RID: 203
		private Dictionary<int, Slot> _cachedSlotsDictionary;
	}
}
