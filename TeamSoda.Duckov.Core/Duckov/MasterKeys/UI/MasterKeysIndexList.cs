using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E6 RID: 742
	public class MasterKeysIndexList : MonoBehaviour, ISingleSelectionMenu<MasterKeysIndexEntry>
	{
		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x00057EF8 File Offset: 0x000560F8
		private PrefabPool<MasterKeysIndexEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<MasterKeysIndexEntry>(this.entryPrefab, this.entryContainer, new Action<MasterKeysIndexEntry>(this.OnGetEntry), new Action<MasterKeysIndexEntry>(this.OnReleaseEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x060017DF RID: 6111 RVA: 0x00057F4C File Offset: 0x0005614C
		// (remove) Token: 0x060017E0 RID: 6112 RVA: 0x00057F84 File Offset: 0x00056184
		internal event Action<MasterKeysIndexEntry> onEntryPointerClicked;

		// Token: 0x060017E1 RID: 6113 RVA: 0x00057FB9 File Offset: 0x000561B9
		private void OnGetEntry(MasterKeysIndexEntry entry)
		{
			entry.onPointerClicked += this.OnEntryPointerClicked;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00057FCD File Offset: 0x000561CD
		private void OnReleaseEntry(MasterKeysIndexEntry entry)
		{
			entry.onPointerClicked -= this.OnEntryPointerClicked;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00057FE1 File Offset: 0x000561E1
		private void OnEntryPointerClicked(MasterKeysIndexEntry entry)
		{
			Action<MasterKeysIndexEntry> action = this.onEntryPointerClicked;
			if (action == null)
			{
				return;
			}
			action(entry);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00057FF4 File Offset: 0x000561F4
		private void Awake()
		{
			this.entryPrefab.gameObject.SetActive(false);
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00058008 File Offset: 0x00056208
		internal void Refresh()
		{
			this.Pool.ReleaseAll();
			foreach (int itemID in MasterKeysManager.AllPossibleKeys)
			{
				this.Populate(itemID);
			}
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00058068 File Offset: 0x00056268
		private void Populate(int itemID)
		{
			MasterKeysIndexEntry masterKeysIndexEntry = this.Pool.Get(this.entryContainer);
			masterKeysIndexEntry.gameObject.SetActive(true);
			masterKeysIndexEntry.Setup(itemID, this);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0005808E File Offset: 0x0005628E
		public MasterKeysIndexEntry GetSelection()
		{
			return this.selection;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00058096 File Offset: 0x00056296
		public bool SetSelection(MasterKeysIndexEntry selection)
		{
			this.selection = selection;
			return true;
		}

		// Token: 0x04001167 RID: 4455
		[SerializeField]
		private MasterKeysIndexEntry entryPrefab;

		// Token: 0x04001168 RID: 4456
		[SerializeField]
		private RectTransform entryContainer;

		// Token: 0x04001169 RID: 4457
		private PrefabPool<MasterKeysIndexEntry> _pool;

		// Token: 0x0400116B RID: 4459
		private MasterKeysIndexEntry selection;
	}
}
