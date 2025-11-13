using System;
using System.Collections.Generic;
using Duckov.Buffs;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200037E RID: 894
	public class BuffsDisplay : MonoBehaviour
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0006D758 File Offset: 0x0006B958
		private PrefabPool<BuffsDisplayEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<BuffsDisplayEntry>(this.prefab, base.transform, delegate(BuffsDisplayEntry e)
					{
						this.activeEntries.Add(e);
					}, delegate(BuffsDisplayEntry e)
					{
						this.activeEntries.Remove(e);
					}, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0006D7AC File Offset: 0x0006B9AC
		public void ReleaseEntry(BuffsDisplayEntry entry)
		{
			this.EntryPool.Release(entry);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0006D7BA File Offset: 0x0006B9BA
		private void Awake()
		{
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.OnLevelInitialized();
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0006D7DA File Offset: 0x0006B9DA
		private void OnDestroy()
		{
			this.UnregisterEvents();
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0006D7F4 File Offset: 0x0006B9F4
		private void OnLevelInitialized()
		{
			this.UnregisterEvents();
			this.buffManager = LevelManager.Instance.MainCharacter.GetBuffManager();
			foreach (Buff buff in this.buffManager.Buffs)
			{
				this.OnAddBuff(this.buffManager, buff);
			}
			this.RegisterEvents();
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0006D870 File Offset: 0x0006BA70
		private void RegisterEvents()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff += this.OnAddBuff;
			this.buffManager.onRemoveBuff += this.OnRemoveBuff;
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0006D8AF File Offset: 0x0006BAAF
		private void UnregisterEvents()
		{
			if (this.buffManager == null)
			{
				return;
			}
			this.buffManager.onAddBuff -= this.OnAddBuff;
			this.buffManager.onRemoveBuff -= this.OnRemoveBuff;
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0006D8EE File Offset: 0x0006BAEE
		private void OnAddBuff(CharacterBuffManager manager, Buff buff)
		{
			if (buff.Hide)
			{
				return;
			}
			this.EntryPool.Get(null).Setup(this, buff);
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0006D90C File Offset: 0x0006BB0C
		private void OnRemoveBuff(CharacterBuffManager manager, Buff buff)
		{
			BuffsDisplayEntry buffsDisplayEntry = this.activeEntries.Find((BuffsDisplayEntry e) => e.Target == buff);
			if (buffsDisplayEntry == null)
			{
				return;
			}
			buffsDisplayEntry.Release();
		}

		// Token: 0x0400152E RID: 5422
		[SerializeField]
		private BuffsDisplayEntry prefab;

		// Token: 0x0400152F RID: 5423
		private PrefabPool<BuffsDisplayEntry> _entryPool;

		// Token: 0x04001530 RID: 5424
		private List<BuffsDisplayEntry> activeEntries = new List<BuffsDisplayEntry>();

		// Token: 0x04001531 RID: 5425
		private CharacterBuffManager buffManager;
	}
}
