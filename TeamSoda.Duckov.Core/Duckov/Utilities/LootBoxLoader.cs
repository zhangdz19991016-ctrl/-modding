using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000403 RID: 1027
	public class LootBoxLoader : MonoBehaviour
	{
		// Token: 0x0600255B RID: 9563 RVA: 0x000817D6 File Offset: 0x0007F9D6
		public void CalculateChances()
		{
			this.randomPool.RefreshPercent();
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000817E3 File Offset: 0x0007F9E3
		public List<int> FixedItems
		{
			get
			{
				return this.fixedItems;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x000817EB File Offset: 0x0007F9EB
		[SerializeField]
		private Inventory Inventory
		{
			get
			{
				if (this._lootBox == null)
				{
					this._lootBox = base.GetComponent<InteractableLootbox>();
					if (this._lootBox == null)
					{
						return null;
					}
				}
				return this._lootBox.Inventory;
			}
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x00081822 File Offset: 0x0007FA22
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x0008182A File Offset: 0x0007FA2A
		private void Awake()
		{
			if (this._lootBox == null)
			{
				this._lootBox = base.GetComponent<InteractableLootbox>();
			}
			this.RandomActive();
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x0008184C File Offset: 0x0007FA4C
		private int GetKey()
		{
			Vector3 vector = base.transform.position * 10f;
			int x = Mathf.RoundToInt(vector.x);
			int y = Mathf.RoundToInt(vector.y);
			int z = Mathf.RoundToInt(vector.z);
			Vector3Int vector3Int = new Vector3Int(x, y, z);
			return string.Format("LootBoxLoader_{0}", vector3Int).GetHashCode();
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000818B0 File Offset: 0x0007FAB0
		private void RandomActive()
		{
			bool flag = false;
			int key = this.GetKey();
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(key, out obj))
			{
				if (obj is bool)
				{
					bool flag2 = (bool)obj;
					flag = flag2;
				}
			}
			else
			{
				flag = (UnityEngine.Random.Range(0f, 1f) < this.activeChance);
				MultiSceneCore.Instance.inLevelData.Add(key, flag);
			}
			base.gameObject.SetActive(flag);
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x00081927 File Offset: 0x0007FB27
		public void StartSetup()
		{
			this.Setup().Forget();
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x00081934 File Offset: 0x0007FB34
		public UniTask Setup()
		{
			LootBoxLoader.<Setup>d__27 <Setup>d__;
			<Setup>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Setup>d__.<>4__this = this;
			<Setup>d__.<>1__state = -1;
			<Setup>d__.<>t__builder.Start<LootBoxLoader.<Setup>d__27>(ref <Setup>d__);
			return <Setup>d__.<>t__builder.Task;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x00081978 File Offset: 0x0007FB78
		private UniTask CreateCash()
		{
			LootBoxLoader.<CreateCash>d__28 <CreateCash>d__;
			<CreateCash>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<CreateCash>d__.<>4__this = this;
			<CreateCash>d__.<>1__state = -1;
			<CreateCash>d__.<>t__builder.Start<LootBoxLoader.<CreateCash>d__28>(ref <CreateCash>d__);
			return <CreateCash>d__.<>t__builder.Task;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000819BB File Offset: 0x0007FBBB
		private void OnValidate()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
		}

		// Token: 0x04001969 RID: 6505
		public bool autoSetup = true;

		// Token: 0x0400196A RID: 6506
		public bool dropOnSpawnItem;

		// Token: 0x0400196B RID: 6507
		[SerializeField]
		[Range(0f, 1f)]
		private float activeChance = 1f;

		// Token: 0x0400196C RID: 6508
		[SerializeField]
		private int inventorySize = 8;

		// Token: 0x0400196D RID: 6509
		public bool ignoreLevelConfig;

		// Token: 0x0400196E RID: 6510
		[SerializeField]
		private Vector2Int randomCount = new Vector2Int(1, 1);

		// Token: 0x0400196F RID: 6511
		public bool randomFromPool;

		// Token: 0x04001970 RID: 6512
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x04001971 RID: 6513
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x04001972 RID: 6514
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x04001973 RID: 6515
		[SerializeField]
		private RandomContainer<LootBoxLoader.Entry> randomPool;

		// Token: 0x04001974 RID: 6516
		[Range(0f, 1f)]
		public float GenrateCashChance;

		// Token: 0x04001975 RID: 6517
		public int maxRandomCash;

		// Token: 0x04001976 RID: 6518
		[ItemTypeID]
		[SerializeField]
		private List<int> fixedItems;

		// Token: 0x04001977 RID: 6519
		[Range(0f, 1f)]
		[SerializeField]
		private float fixedItemSpawnChance = 1f;

		// Token: 0x04001978 RID: 6520
		[SerializeField]
		private InteractableLootbox _lootBox;

		// Token: 0x02000672 RID: 1650
		[Serializable]
		private struct Entry
		{
			// Token: 0x0400235C RID: 9052
			[ItemTypeID]
			[SerializeField]
			public int itemTypeID;
		}
	}
}
