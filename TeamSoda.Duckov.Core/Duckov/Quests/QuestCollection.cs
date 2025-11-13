using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000339 RID: 825
	[CreateAssetMenu(menuName = "Quest Collection")]
	public class QuestCollection : ScriptableObject, IList<Quest>, ICollection<Quest>, IEnumerable<Quest>, IEnumerable, ISelfValidator
	{
		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x00066EDF File Offset: 0x000650DF
		public static QuestCollection Instance
		{
			get
			{
				return GameplayDataSettings.QuestCollection;
			}
		}

		// Token: 0x17000534 RID: 1332
		public Quest this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				this.list[index] = value;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x00066F03 File Offset: 0x00065103
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x00066F10 File Offset: 0x00065110
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x00066F13 File Offset: 0x00065113
		public void Add(Quest item)
		{
			this.list.Add(item);
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x00066F21 File Offset: 0x00065121
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x00066F2E File Offset: 0x0006512E
		public bool Contains(Quest item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x00066F3C File Offset: 0x0006513C
		public void CopyTo(Quest[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x00066F4B File Offset: 0x0006514B
		public IEnumerator<Quest> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00066F5D File Offset: 0x0006515D
		public int IndexOf(Quest item)
		{
			return this.list.IndexOf(item);
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00066F6B File Offset: 0x0006516B
		public void Insert(int index, Quest item)
		{
			this.list.Insert(index, item);
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00066F7A File Offset: 0x0006517A
		public bool Remove(Quest item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x00066F88 File Offset: 0x00065188
		public void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x00066F96 File Offset: 0x00065196
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x00066F9E File Offset: 0x0006519E
		public void Collect()
		{
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00066FA0 File Offset: 0x000651A0
		public void Validate(SelfValidationResult result)
		{
			this.list.GroupBy(delegate(Quest e)
			{
				if (e == null)
				{
					return -1;
				}
				return e.ID;
			});
			if (this.list.GroupBy(delegate(Quest e)
			{
				if (e == null)
				{
					return -1;
				}
				return e.ID;
			}).Any((IGrouping<int, Quest> g) => g.Count<Quest>() > 1))
			{
				result.AddError("存在冲突的QuestID。").WithFix("自动重新分配ID", new Action(this.AutoFixID), true);
			}
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x0006704C File Offset: 0x0006524C
		private void AutoFixID()
		{
			int num = this.list.Max((Quest e) => e.ID) + 1;
			foreach (IEnumerable<Quest> enumerable in from e in this.list
			group e by e.ID into g
			where g.Count<Quest>() > 1
			select g)
			{
				int num2 = 0;
				foreach (Quest quest in enumerable)
				{
					if (!(quest == null) && num2++ != 0)
					{
						quest.ID = num++;
					}
				}
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00067158 File Offset: 0x00065358
		public Quest Get(int id)
		{
			return this.list.FirstOrDefault((Quest q) => q != null && q.ID == id);
		}

		// Token: 0x040013E3 RID: 5091
		[SerializeField]
		private List<Quest> list;
	}
}
