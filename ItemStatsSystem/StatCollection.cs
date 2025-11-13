using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000023 RID: 35
	public class StatCollection : ItemComponent, ICollection<Stat>, IEnumerable<Stat>, IEnumerable
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00007CA5 File Offset: 0x00005EA5
		private Dictionary<int, Stat> statsDictionary
		{
			get
			{
				if (this._cachedStatsDictionary == null)
				{
					this.BuildDictionary();
				}
				return this._cachedStatsDictionary;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007CBB File Offset: 0x00005EBB
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00007CCC File Offset: 0x00005ECC
		public Stat GetStat(int hash)
		{
			Stat result;
			if (this.statsDictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007CEC File Offset: 0x00005EEC
		public Stat GetStat(string key)
		{
			int hashCode = key.GetHashCode();
			Stat stat = this.GetStat(hashCode);
			if (stat == null)
			{
				stat = this.list.Find((Stat e) => e.Key == key);
			}
			return stat;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007D38 File Offset: 0x00005F38
		private void BuildDictionary()
		{
			if (this._cachedStatsDictionary == null)
			{
				this._cachedStatsDictionary = new Dictionary<int, Stat>();
			}
			this._cachedStatsDictionary.Clear();
			foreach (Stat stat in this.list)
			{
				int hashCode = stat.Key.GetHashCode();
				this._cachedStatsDictionary[hashCode] = stat;
			}
		}

		// Token: 0x17000089 RID: 137
		public Stat this[int hash]
		{
			get
			{
				return this.GetStat(hash);
			}
		}

		// Token: 0x1700008A RID: 138
		public Stat this[string key]
		{
			get
			{
				return this.GetStat(key);
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007DD0 File Offset: 0x00005FD0
		internal override void OnInitialize()
		{
			foreach (Stat stat in this.list)
			{
				stat.Initialize(this);
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007E24 File Offset: 0x00006024
		public IEnumerator<Stat> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00007E36 File Offset: 0x00006036
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00007E48 File Offset: 0x00006048
		public void Add(Stat item)
		{
			this.list.Add(item);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007E56 File Offset: 0x00006056
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007E63 File Offset: 0x00006063
		public bool Contains(Stat item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007E71 File Offset: 0x00006071
		public void CopyTo(Stat[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007E80 File Offset: 0x00006080
		public bool Remove(Stat item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private List<Stat> list;

		// Token: 0x040000AD RID: 173
		private Dictionary<int, Stat> _cachedStatsDictionary;
	}
}
