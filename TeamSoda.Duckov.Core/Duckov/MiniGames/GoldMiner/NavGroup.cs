using System;
using System.Collections.Generic;
using Duckov.MiniGames.GoldMiner.UI;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AA RID: 682
	public class NavGroup : MiniGameBehaviour
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x00052D9C File Offset: 0x00050F9C
		// (set) Token: 0x06001653 RID: 5715 RVA: 0x00052DA3 File Offset: 0x00050FA3
		public static NavGroup ActiveNavGroup { get; private set; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001654 RID: 5716 RVA: 0x00052DAB File Offset: 0x00050FAB
		public bool active
		{
			get
			{
				return NavGroup.ActiveNavGroup == this;
			}
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x00052DB8 File Offset: 0x00050FB8
		public void SetAsActiveNavGroup()
		{
			NavGroup activeNavGroup = NavGroup.ActiveNavGroup;
			NavGroup.ActiveNavGroup = this;
			this.RefreshAll();
			if (activeNavGroup != null)
			{
				activeNavGroup.RefreshAll();
			}
			Action onNavGroupChanged = NavGroup.OnNavGroupChanged;
			if (onNavGroupChanged == null)
			{
				return;
			}
			onNavGroupChanged();
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x00052DF5 File Offset: 0x00050FF5
		// (set) Token: 0x06001657 RID: 5719 RVA: 0x00052E00 File Offset: 0x00051000
		public int NavIndex
		{
			get
			{
				return this._navIndex;
			}
			set
			{
				int navIndex = this._navIndex;
				this._navIndex = value;
				this.CleanupIndex();
				int navIndex2 = this._navIndex;
				this.RefreshEntry(navIndex);
				this.RefreshEntry(navIndex2);
			}
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x00052E36 File Offset: 0x00051036
		protected override void OnEnable()
		{
			base.OnEnable();
			this.RefreshAll();
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00052E44 File Offset: 0x00051044
		private void CleanupIndex()
		{
			if (this._navIndex < 0)
			{
				this._navIndex = this.entries.Count - 1;
			}
			if (this._navIndex >= this.entries.Count)
			{
				this._navIndex = 0;
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00052E7C File Offset: 0x0005107C
		private void RefreshAll()
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.RefreshEntry(i);
			}
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x00052EA6 File Offset: 0x000510A6
		private void RefreshEntry(int index)
		{
			if (index < 0 || index >= this.entries.Count)
			{
				return;
			}
			this.entries[index].NotifySelectionState(this.active && this.NavIndex == index);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00052EE0 File Offset: 0x000510E0
		public NavEntry GetSelectedEntry()
		{
			if (this.NavIndex < 0 || this.NavIndex >= this.entries.Count)
			{
				return null;
			}
			return this.entries[this.NavIndex];
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00052F14 File Offset: 0x00051114
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponentInParent<GoldMiner>();
			}
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x00052F62 File Offset: 0x00051162
		private void OnLevelBegin(GoldMiner miner)
		{
			this.RefreshAll();
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x00052F6A File Offset: 0x0005116A
		internal void Remove(NavEntry navEntry)
		{
			this.entries.Remove(navEntry);
			this.CleanupIndex();
			this.RefreshAll();
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x00052F85 File Offset: 0x00051185
		internal void Add(NavEntry navEntry)
		{
			this.entries.Add(navEntry);
			this.CleanupIndex();
			this.RefreshAll();
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00052FA0 File Offset: 0x000511A0
		internal void TrySelect(NavEntry navEntry)
		{
			if (!this.entries.Contains(navEntry))
			{
				return;
			}
			int navIndex = this.entries.IndexOf(navEntry);
			this.SetAsActiveNavGroup();
			this.NavIndex = navIndex;
		}

		// Token: 0x04001088 RID: 4232
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001089 RID: 4233
		[SerializeField]
		public List<NavEntry> entries;

		// Token: 0x0400108B RID: 4235
		public static Action OnNavGroupChanged;

		// Token: 0x0400108C RID: 4236
		private int _navIndex;
	}
}
