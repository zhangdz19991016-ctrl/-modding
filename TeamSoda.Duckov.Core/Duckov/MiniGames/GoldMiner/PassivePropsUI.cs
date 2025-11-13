using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Duckov.MiniGames.GoldMiner.UI;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002AD RID: 685
	public class PassivePropsUI : MiniGameBehaviour
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x000530E8 File Offset: 0x000512E8
		private PrefabPool<PassivePropDisplay> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<PassivePropDisplay>(this.entryTemplate, null, new Action<PassivePropDisplay>(this.OnGetEntry), new Action<PassivePropDisplay>(this.OnReleaseEntry), null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x00053137 File Offset: 0x00051337
		private void OnReleaseEntry(PassivePropDisplay display)
		{
			this.navGroup.Remove(display.NavEntry);
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0005314A File Offset: 0x0005134A
		private void OnGetEntry(PassivePropDisplay display)
		{
			this.navGroup.Add(display.NavEntry);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00053160 File Offset: 0x00051360
		private void Awake()
		{
			GoldMiner goldMiner = this.master;
			goldMiner.onLevelBegin = (Action<GoldMiner>)Delegate.Combine(goldMiner.onLevelBegin, new Action<GoldMiner>(this.OnLevelBegin));
			GoldMiner goldMiner2 = this.master;
			goldMiner2.onArtifactChange = (Action<GoldMiner>)Delegate.Combine(goldMiner2.onArtifactChange, new Action<GoldMiner>(this.OnArtifactChanged));
			GoldMiner goldMiner3 = this.master;
			goldMiner3.onEarlyLevelPlayTick = (Action<GoldMiner>)Delegate.Combine(goldMiner3.onEarlyLevelPlayTick, new Action<GoldMiner>(this.OnEarlyTick));
			NavGroup.OnNavGroupChanged = (Action)Delegate.Combine(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x00053202 File Offset: 0x00051402
		private void OnDestroy()
		{
			NavGroup.OnNavGroupChanged = (Action)Delegate.Remove(NavGroup.OnNavGroupChanged, new Action(this.OnNavGroupChanged));
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x00053224 File Offset: 0x00051424
		private void OnNavGroupChanged()
		{
			this.changeLock = true;
			if (this.navGroup.active && this.Pool.ActiveEntries.Count <= 0)
			{
				this.upNavGroup.SetAsActiveNavGroup();
			}
			this.RefreshDescription();
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00053260 File Offset: 0x00051460
		private void OnEarlyTick(GoldMiner miner)
		{
			this.RefreshDescription();
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00053274 File Offset: 0x00051474
		private void SetCoord([TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> coord)
		{
			int navIndex = this.CoordToIndex(coord);
			this.navGroup.NavIndex = navIndex;
			this.RefreshDescription();
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0005329C File Offset: 0x0005149C
		private void RefreshDescription()
		{
			if (!this.navGroup.active)
			{
				this.HideDescription();
				return;
			}
			if (this.Pool.ActiveEntries.Count <= 0)
			{
				this.HideDescription();
				return;
			}
			NavEntry selectedEntry = this.navGroup.GetSelectedEntry();
			if (selectedEntry == null)
			{
				this.HideDescription();
				return;
			}
			if (!selectedEntry.VCT.IsHovering)
			{
				this.HideDescription();
				return;
			}
			PassivePropDisplay component = selectedEntry.GetComponent<PassivePropDisplay>();
			if (component == null)
			{
				this.HideDescription();
				return;
			}
			this.SetupAndShowDescription(component);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00053325 File Offset: 0x00051525
		private void HideDescription()
		{
			this.descriptionContainer.gameObject.SetActive(false);
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00053338 File Offset: 0x00051538
		private void SetupAndShowDescription(PassivePropDisplay ppd)
		{
			this.descriptionContainer.gameObject.SetActive(true);
			string description = ppd.Target.Description;
			this.descriptionText.text = description;
			this.descriptionContainer.position = ppd.rectTransform.TransformPoint(ppd.rectTransform.rect.max);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x0005339C File Offset: 0x0005159C
		private int CoordToIndex([TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> coord)
		{
			int count = this.navGroup.entries.Count;
			if (count <= 0)
			{
				return 0;
			}
			int constraintCount = this.gridLayout.constraintCount;
			int num = count / constraintCount;
			if (coord.Item2 > num)
			{
				coord.Item2 = num;
			}
			int num2 = constraintCount;
			if (coord.Item2 == num)
			{
				num2 = count % constraintCount;
			}
			if (coord.Item1 < 0)
			{
				coord.Item1 = num2 - 1;
			}
			coord.Item1 %= num2;
			if (coord.Item2 < 0)
			{
				coord.Item2 = num;
			}
			coord.Item2 %= num + 1;
			return constraintCount * coord.Item2 + coord.Item1;
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00053440 File Offset: 0x00051640
		[return: TupleElementNames(new string[]
		{
			"x",
			"y"
		})]
		private ValueTuple<int, int> IndexToCoord(int index)
		{
			int constraintCount = this.gridLayout.constraintCount;
			int item = index / constraintCount;
			return new ValueTuple<int, int>(index % constraintCount, item);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00053466 File Offset: 0x00051666
		private void OnLevelBegin(GoldMiner miner)
		{
			this.Refresh();
			this.RefreshDescription();
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00053474 File Offset: 0x00051674
		private void OnArtifactChanged(GoldMiner miner)
		{
			this.Refresh();
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0005347C File Offset: 0x0005167C
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			if (this.master == null)
			{
				return;
			}
			GoldMinerRunData run = this.master.run;
			if (run == null)
			{
				return;
			}
			foreach (IGrouping<string, GoldMinerArtifact> source in from e in run.artifacts
			where e != null
			group e by e.ID)
			{
				GoldMinerArtifact target = source.ElementAt(0);
				this.Pool.Get(null).Setup(target, source.Count<GoldMinerArtifact>());
			}
		}

		// Token: 0x04001097 RID: 4247
		[SerializeField]
		private GoldMiner master;

		// Token: 0x04001098 RID: 4248
		[SerializeField]
		private RectTransform descriptionContainer;

		// Token: 0x04001099 RID: 4249
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x0400109A RID: 4250
		[SerializeField]
		private PassivePropDisplay entryTemplate;

		// Token: 0x0400109B RID: 4251
		[SerializeField]
		private NavGroup navGroup;

		// Token: 0x0400109C RID: 4252
		[SerializeField]
		private NavGroup upNavGroup;

		// Token: 0x0400109D RID: 4253
		[SerializeField]
		private GridLayoutGroup gridLayout;

		// Token: 0x0400109E RID: 4254
		private PrefabPool<PassivePropDisplay> _pool;

		// Token: 0x0400109F RID: 4255
		private bool changeLock;
	}
}
