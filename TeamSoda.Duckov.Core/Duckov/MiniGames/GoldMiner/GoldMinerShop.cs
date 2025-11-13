using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000296 RID: 662
	public class GoldMinerShop : MiniGameBehaviour
	{
		// Token: 0x060015C6 RID: 5574 RVA: 0x00050EA4 File Offset: 0x0004F0A4
		private void Clear()
		{
			this.capacity = this.master.run.shopCapacity;
			for (int i = 0; i < this.stock.Count; i++)
			{
				ShopEntity shopEntity = this.stock[i];
				if (shopEntity != null && (shopEntity.sold || !shopEntity.locked))
				{
					this.stock[i] = null;
				}
			}
			for (int j = this.capacity; j < this.stock.Count; j++)
			{
				if (this.stock[j] == null)
				{
					this.stock.RemoveAt(j);
				}
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00050F40 File Offset: 0x0004F140
		private void Refill()
		{
			this.capacity = this.master.run.shopCapacity;
			for (int i = 0; i < this.capacity; i++)
			{
				if (this.stock.Count <= i)
				{
					this.stock.Add(null);
				}
				ShopEntity shopEntity = this.stock[i];
				if (shopEntity == null || shopEntity.sold)
				{
					this.stock[i] = this.GenerateNewShopItem();
				}
			}
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x00050FB8 File Offset: 0x0004F1B8
		private void RefreshStock()
		{
			this.Clear();
			this.CacheValidCandiateLists();
			this.Refill();
			Action action = this.onAfterOperation;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x00050FDC File Offset: 0x0004F1DC
		private void CacheValidCandiateLists()
		{
			for (int i = 0; i < 5; i++)
			{
				int quality = i + 1;
				List<GoldMinerArtifact> list = this.SearchValidCandidateArtifactIDs(quality).ToList<GoldMinerArtifact>();
				this.validCandidateLists[i] = list;
			}
			foreach (ShopEntity shopEntity in this.stock)
			{
				if (shopEntity != null && !(shopEntity.artifact == null) && !shopEntity.artifact.AllowMultiple)
				{
					foreach (List<GoldMinerArtifact> list2 in this.validCandidateLists)
					{
						if (list2 != null)
						{
							list2.Remove(shopEntity.artifact);
						}
					}
				}
			}
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x000510A8 File Offset: 0x0004F2A8
		private IEnumerable<GoldMinerArtifact> SearchValidCandidateArtifactIDs(int quality)
		{
			using (IEnumerator<GoldMinerArtifact> enumerator = this.master.ArtifactPrefabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GoldMinerArtifact artifact = enumerator.Current;
					if (artifact.Quality == quality && (artifact.AllowMultiple || (this.master.run.GetArtifactCount(artifact.ID) <= 0 && !this.stock.Any((ShopEntity e) => e != null && !e.sold && e.ID == artifact.ID))))
					{
						yield return artifact;
					}
				}
			}
			IEnumerator<GoldMinerArtifact> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x000510BF File Offset: 0x0004F2BF
		private List<GoldMinerArtifact> GetValidCandidateArtifactIDs(int q)
		{
			return this.validCandidateLists[q - 1];
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x000510CC File Offset: 0x0004F2CC
		private ShopEntity GenerateNewShopItem()
		{
			int num = this.qualityDistribute.GetRandom(0f);
			List<GoldMinerArtifact> list = null;
			for (int i = num; i >= 1; i--)
			{
				list = this.GetValidCandidateArtifactIDs(i);
				if (list.Count > 0)
				{
					num = i;
					break;
				}
			}
			GoldMinerArtifact random = list.GetRandom(this.master.run.shopRandom);
			if (random != null && !random.AllowMultiple)
			{
				List<GoldMinerArtifact> validCandidateArtifactIDs = this.GetValidCandidateArtifactIDs(num);
				if (validCandidateArtifactIDs != null)
				{
					validCandidateArtifactIDs.Remove(random);
				}
			}
			if (random == null)
			{
				Debug.Log(string.Format("{0} failed to generate", num));
			}
			return new ShopEntity
			{
				artifact = random
			};
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00051178 File Offset: 0x0004F378
		public bool Buy(ShopEntity entity)
		{
			if (!this.stock.Contains(entity))
			{
				Debug.LogError("Buying entity that doesn't exist in shop stock");
				return false;
			}
			if (entity.sold)
			{
				return false;
			}
			bool flag;
			int price = this.CalculateDealPrice(entity, out flag);
			if (this.master.run.shopTicket > 0)
			{
				this.master.run.shopTicket--;
			}
			else if (!this.master.PayMoney(price))
			{
				return false;
			}
			this.master.run.AttachArtifactFromPrefab(entity.artifact);
			entity.sold = true;
			Action action = this.onAfterOperation;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00051220 File Offset: 0x0004F420
		public int CalculateDealPrice(ShopEntity entity, out bool useTicket)
		{
			useTicket = false;
			if (entity == null)
			{
				return 0;
			}
			if (this.master.run.shopTicket > 0)
			{
				useTicket = true;
				return 0;
			}
			GoldMinerArtifact artifact = entity.artifact;
			if (artifact == null)
			{
				return 0;
			}
			return Mathf.CeilToInt((float)artifact.BasePrice * entity.priceFactor * this.master.GlobalPriceFactor);
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x0005127E File Offset: 0x0004F47E
		// (set) Token: 0x060015D0 RID: 5584 RVA: 0x00051286 File Offset: 0x0004F486
		public int refreshChance { get; private set; }

		// Token: 0x060015D1 RID: 5585 RVA: 0x00051290 File Offset: 0x0004F490
		public UniTask Execute()
		{
			GoldMinerShop.<Execute>d__22 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<GoldMinerShop.<Execute>d__22>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x000512D3 File Offset: 0x0004F4D3
		internal void Continue()
		{
			this.complete = true;
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x000512DC File Offset: 0x0004F4DC
		internal bool TryRefresh()
		{
			if (this.refreshChance <= 0)
			{
				return false;
			}
			int refreshCost = this.GetRefreshCost();
			if (!this.master.PayMoney(refreshCost))
			{
				return false;
			}
			int refreshChance = this.refreshChance;
			this.refreshChance = refreshChance - 1;
			this.refreshPrice += Mathf.RoundToInt(this.master.run.shopRefreshPriceIncrement.Value);
			this.RefreshStock();
			return true;
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00051349 File Offset: 0x0004F549
		internal int GetRefreshCost()
		{
			return this.refreshPrice;
		}

		// Token: 0x0400100C RID: 4108
		[SerializeField]
		private GoldMiner master;

		// Token: 0x0400100D RID: 4109
		[SerializeField]
		private GoldMinerShopUI ui;

		// Token: 0x0400100E RID: 4110
		[SerializeField]
		private RandomContainer<int> qualityDistribute;

		// Token: 0x0400100F RID: 4111
		public List<ShopEntity> stock = new List<ShopEntity>();

		// Token: 0x04001010 RID: 4112
		public Action onAfterOperation;

		// Token: 0x04001011 RID: 4113
		private int capacity;

		// Token: 0x04001012 RID: 4114
		private List<GoldMinerArtifact>[] validCandidateLists = new List<GoldMinerArtifact>[5];

		// Token: 0x04001013 RID: 4115
		private bool complete;

		// Token: 0x04001014 RID: 4116
		private int refreshPrice = 100;
	}
}
