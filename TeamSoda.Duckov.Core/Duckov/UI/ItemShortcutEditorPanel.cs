using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B3 RID: 947
	public class ItemShortcutEditorPanel : MonoBehaviour
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002228 RID: 8744 RVA: 0x000776F4 File Offset: 0x000758F4
		private PrefabPool<ItemShortcutEditorEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<ItemShortcutEditorEntry>(this.entryTemplate, this.entryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.entryTemplate.gameObject.SetActive(false);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0007774D File Offset: 0x0007594D
		private void OnEnable()
		{
			this.Setup();
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00077758 File Offset: 0x00075958
		private void Setup()
		{
			this.EntryPool.ReleaseAll();
			for (int i = 0; i <= ItemShortcut.MaxIndex; i++)
			{
				ItemShortcutEditorEntry itemShortcutEditorEntry = this.EntryPool.Get(this.entryTemplate.transform.parent);
				itemShortcutEditorEntry.Setup(i);
				itemShortcutEditorEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x0400170A RID: 5898
		[SerializeField]
		private ItemShortcutEditorEntry entryTemplate;

		// Token: 0x0400170B RID: 5899
		private PrefabPool<ItemShortcutEditorEntry> _entryPool;
	}
}
