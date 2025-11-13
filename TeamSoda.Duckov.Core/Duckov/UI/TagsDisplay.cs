using System;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A7 RID: 935
	public class TagsDisplay : MonoBehaviour
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x00075234 File Offset: 0x00073434
		private PrefabPool<TagsDisplayEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<TagsDisplayEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x0007526D File Offset: 0x0007346D
		private void Awake()
		{
			this.entryTemplate.gameObject.SetActive(false);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00075280 File Offset: 0x00073480
		public void Setup(Item item)
		{
			this.EntryPool.ReleaseAll();
			if (item == null)
			{
				return;
			}
			foreach (Tag tag in item.Tags)
			{
				if (!(tag == null) && tag.Show)
				{
					this.EntryPool.Get(null).Setup(tag);
				}
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00075300 File Offset: 0x00073500
		internal void Clear()
		{
			this.EntryPool.ReleaseAll();
		}

		// Token: 0x040016AD RID: 5805
		[SerializeField]
		private TagsDisplayEntry entryTemplate;

		// Token: 0x040016AE RID: 5806
		private PrefabPool<TagsDisplayEntry> _entryPool;
	}
}
