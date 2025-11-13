using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000401 RID: 1025
	[RequireComponent(typeof(Points))]
	public class LootSpawner : MonoBehaviour
	{
		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002540 RID: 9536 RVA: 0x000812EA File Offset: 0x0007F4EA
		public bool RandomFromPool
		{
			get
			{
				return this.randomGenrate && this.randomFromPool;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x000812FC File Offset: 0x0007F4FC
		public bool RandomButNotFromPool
		{
			get
			{
				return this.randomGenrate && !this.randomFromPool;
			}
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x00081311 File Offset: 0x0007F511
		public void CalculateChances()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
			this.randomPool.RefreshPercent();
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x00081334 File Offset: 0x0007F534
		private void Start()
		{
			if (this.points == null)
			{
				this.points = base.GetComponent<Points>();
			}
			bool flag = false;
			int key = this.GetKey();
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj) && obj is bool)
			{
				bool flag2 = (bool)obj;
				flag = flag2;
			}
			if (!flag)
			{
				if (UnityEngine.Random.Range(0f, 1f) <= this.spawnChance)
				{
					this.Setup().Forget();
				}
				MultiSceneCore.Instance.inLevelData.Add(key, true);
			}
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000813C8 File Offset: 0x0007F5C8
		private int GetKey()
		{
			Transform parent = base.transform.parent;
			string text = base.transform.GetSiblingIndex().ToString();
			while (parent != null)
			{
				text = string.Format("{0}/{1}", parent.GetSiblingIndex(), text);
				parent = parent.parent;
			}
			text = string.Format("{0}/{1}", base.gameObject.scene.buildIndex, text);
			return text.GetHashCode();
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x00081448 File Offset: 0x0007F648
		public UniTask Setup()
		{
			LootSpawner.<Setup>d__20 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<LootSpawner.<Setup>d__20>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x0008148B File Offset: 0x0007F68B
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x00081493 File Offset: 0x0007F693
		private void OnValidate()
		{
			if (this.points == null)
			{
				this.points = base.GetComponent<Points>();
			}
		}

		// Token: 0x04001957 RID: 6487
		[Range(0f, 1f)]
		public float spawnChance = 1f;

		// Token: 0x04001958 RID: 6488
		public bool randomGenrate = true;

		// Token: 0x04001959 RID: 6489
		public bool randomFromPool;

		// Token: 0x0400195A RID: 6490
		[SerializeField]
		private Vector2Int randomCount = new Vector2Int(1, 1);

		// Token: 0x0400195B RID: 6491
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x0400195C RID: 6492
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x0400195D RID: 6493
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x0400195E RID: 6494
		[SerializeField]
		private RandomContainer<LootSpawner.Entry> randomPool;

		// Token: 0x0400195F RID: 6495
		[ItemTypeID]
		[SerializeField]
		private List<int> fixedItems;

		// Token: 0x04001960 RID: 6496
		[SerializeField]
		private Points points;

		// Token: 0x04001961 RID: 6497
		private bool loading;

		// Token: 0x04001962 RID: 6498
		[SerializeField]
		[ItemTypeID]
		private List<int> typeIds;

		// Token: 0x0200066F RID: 1647
		[Serializable]
		private struct Entry
		{
			// Token: 0x04002352 RID: 9042
			[ItemTypeID]
			[SerializeField]
			public int itemTypeID;
		}
	}
}
