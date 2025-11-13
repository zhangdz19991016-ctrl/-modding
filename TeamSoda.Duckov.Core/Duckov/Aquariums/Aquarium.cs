using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Duckov.Aquariums
{
	// Token: 0x02000324 RID: 804
	public class Aquarium : MonoBehaviour
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x00061A49 File Offset: 0x0005FC49
		private string ItemSaveKey
		{
			get
			{
				return "Aquarium/Item/" + this.id;
			}
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00061A5B File Offset: 0x0005FC5B
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x00061A6E File Offset: 0x0005FC6E
		private void Start()
		{
			this.Load().Forget();
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00061A7B File Offset: 0x0005FC7B
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00061A90 File Offset: 0x0005FC90
		private UniTask Load()
		{
			Aquarium.<Load>d__14 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<Aquarium.<Load>d__14>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00061AD3 File Offset: 0x0005FCD3
		private void OnChildChanged(Item item)
		{
			this.dirty = true;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00061ADC File Offset: 0x0005FCDC
		private void FixedUpdate()
		{
			if (this.loading)
			{
				return;
			}
			if (this.dirty)
			{
				this.Refresh();
				this.dirty = false;
			}
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00061AFC File Offset: 0x0005FCFC
		private void Refresh()
		{
			if (this.aquariumItem == null)
			{
				return;
			}
			foreach (Item item in this.aquariumItem.GetAllChildren(false, true))
			{
				if (!(item == null) && item.Tags.Contains("Fish"))
				{
					this.GetOrCreateGraphic(item) == null;
				}
			}
			this.graphicRecords.RemoveAll((Aquarium.ItemGraphicPair e) => e == null || e.graphic == null);
			for (int i = 0; i < this.graphicRecords.Count; i++)
			{
				Aquarium.ItemGraphicPair itemGraphicPair = this.graphicRecords[i];
				if (itemGraphicPair.item == null || itemGraphicPair.item.ParentItem != this.aquariumItem)
				{
					if (itemGraphicPair.graphic != null)
					{
						UnityEngine.Object.Destroy(itemGraphicPair.graphic);
					}
					this.graphicRecords.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x00061C24 File Offset: 0x0005FE24
		private ItemGraphicInfo GetOrCreateGraphic(Item item)
		{
			if (item == null)
			{
				return null;
			}
			Aquarium.ItemGraphicPair itemGraphicPair = this.graphicRecords.Find((Aquarium.ItemGraphicPair e) => e != null && e.item == item);
			if (itemGraphicPair != null && itemGraphicPair.graphic != null)
			{
				return itemGraphicPair.graphic;
			}
			ItemGraphicInfo itemGraphicInfo = ItemGraphicInfo.CreateAGraphic(item, this.graphicsParent);
			if (itemGraphicPair != null)
			{
				this.graphicRecords.Remove(itemGraphicPair);
			}
			if (itemGraphicInfo == null)
			{
				return null;
			}
			IAquariumContent component = itemGraphicInfo.GetComponent<IAquariumContent>();
			if (component != null)
			{
				component.Setup(this);
			}
			this.graphicRecords.Add(new Aquarium.ItemGraphicPair
			{
				item = item,
				graphic = itemGraphicInfo
			});
			return itemGraphicInfo;
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00061CE0 File Offset: 0x0005FEE0
		public void Loot()
		{
			LootView.LootItem(this.aquariumItem);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00061CED File Offset: 0x0005FEED
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			if (!this.loaded)
			{
				return;
			}
			this.aquariumItem.Save(this.ItemSaveKey);
		}

		// Token: 0x0400133C RID: 4924
		[SerializeField]
		private string id = "Default";

		// Token: 0x0400133D RID: 4925
		[SerializeField]
		private Transform graphicsParent;

		// Token: 0x0400133E RID: 4926
		[ItemTypeID]
		private int aquariumItemTypeID = 1158;

		// Token: 0x0400133F RID: 4927
		private Item aquariumItem;

		// Token: 0x04001340 RID: 4928
		private List<Aquarium.ItemGraphicPair> graphicRecords = new List<Aquarium.ItemGraphicPair>();

		// Token: 0x04001341 RID: 4929
		private bool loading;

		// Token: 0x04001342 RID: 4930
		private bool loaded;

		// Token: 0x04001343 RID: 4931
		private int loadToken;

		// Token: 0x04001344 RID: 4932
		private bool dirty = true;

		// Token: 0x020005C7 RID: 1479
		private class ItemGraphicPair
		{
			// Token: 0x040020BB RID: 8379
			public Item item;

			// Token: 0x040020BC RID: 8380
			public ItemGraphicInfo graphic;
		}
	}
}
