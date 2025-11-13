using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Weathers;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.Utilities
{
	// Token: 0x02000400 RID: 1024
	public class FishSpawner : MonoBehaviour
	{
		// Token: 0x06002538 RID: 9528 RVA: 0x000810DE File Offset: 0x0007F2DE
		public void CalculateChances()
		{
			this.tags.RefreshPercent();
			this.qualities.RefreshPercent();
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x000810F6 File Offset: 0x0007F2F6
		private void Awake()
		{
			this.excludeTagsReal = new List<Tag>();
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x00081103 File Offset: 0x0007F303
		private void Start()
		{
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x00081108 File Offset: 0x0007F308
		public UniTask<Item> Spawn(int baitID, float luck)
		{
			FishSpawner.<Spawn>d__14 <Spawn>d__;
			<Spawn>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Spawn>d__.<>4__this = this;
			<Spawn>d__.baitID = baitID;
			<Spawn>d__.luck = luck;
			<Spawn>d__.<>1__state = -1;
			<Spawn>d__.<>t__builder.Start<FishSpawner.<Spawn>d__14>(ref <Spawn>d__);
			return <Spawn>d__.<>t__builder.Task;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0008115B File Offset: 0x0007F35B
		public static int[] Search(ItemFilter filter)
		{
			return ItemAssetsCollection.Search(filter);
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x00081164 File Offset: 0x0007F364
		private void CalculateTags(bool atNight, Weather weather)
		{
			this.excludeTagsReal.Clear();
			this.excludeTagsReal.AddRange(this.excludeTags);
			if (atNight)
			{
				this.excludeTagsReal.Add(this.Fish_OnlyDay);
			}
			else
			{
				this.excludeTagsReal.Add(this.Fish_OnlyNight);
			}
			this.excludeTagsReal.Add(this.Fish_OnlySunDay);
			this.excludeTagsReal.Add(this.Fish_OnlyRainDay);
			this.excludeTagsReal.Add(this.Fish_OnlyStorm);
			switch (weather)
			{
			case Weather.Sunny:
				this.excludeTagsReal.Remove(this.Fish_OnlySunDay);
				return;
			case Weather.Cloudy:
				break;
			case Weather.Rainy:
				this.excludeTagsReal.Remove(this.Fish_OnlyRainDay);
				return;
			case Weather.Stormy_I:
				this.excludeTagsReal.Remove(this.Fish_OnlyStorm);
				return;
			case Weather.Stormy_II:
				this.excludeTagsReal.Remove(this.Fish_OnlyStorm);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x00081250 File Offset: 0x0007F450
		private bool CheckFishDayNightAndWeather(int fishID, bool atNight, Weather currentWeather)
		{
			ItemMetaData metaData = ItemAssetsCollection.GetMetaData(fishID);
			return (!metaData.tags.Contains(this.Fish_OnlyNight) || atNight) && (!metaData.tags.Contains(this.Fish_OnlyDay) || !atNight) && (!metaData.tags.Contains(this.Fish_OnlyRainDay) || currentWeather == Weather.Rainy) && (!metaData.tags.Contains(this.Fish_OnlySunDay) || currentWeather == Weather.Sunny) && (!metaData.tags.Contains(this.Fish_OnlyStorm) || currentWeather == Weather.Stormy_I || currentWeather == Weather.Stormy_II);
		}

		// Token: 0x0400194D RID: 6477
		[SerializeField]
		private List<FishSpawner.SpecialPair> specialPairs;

		// Token: 0x0400194E RID: 6478
		[SerializeField]
		private RandomContainer<Tag> tags;

		// Token: 0x0400194F RID: 6479
		[SerializeField]
		private List<Tag> excludeTags;

		// Token: 0x04001950 RID: 6480
		[SerializeField]
		private RandomContainer<int> qualities;

		// Token: 0x04001951 RID: 6481
		private List<Tag> excludeTagsReal;

		// Token: 0x04001952 RID: 6482
		[SerializeField]
		private Tag Fish_OnlyDay;

		// Token: 0x04001953 RID: 6483
		[SerializeField]
		private Tag Fish_OnlyNight;

		// Token: 0x04001954 RID: 6484
		[SerializeField]
		private Tag Fish_OnlySunDay;

		// Token: 0x04001955 RID: 6485
		[SerializeField]
		private Tag Fish_OnlyRainDay;

		// Token: 0x04001956 RID: 6486
		[SerializeField]
		private Tag Fish_OnlyStorm;

		// Token: 0x0200066D RID: 1645
		[Serializable]
		private struct SpecialPair
		{
			// Token: 0x04002349 RID: 9033
			[ItemTypeID]
			public int baitID;

			// Token: 0x0400234A RID: 9034
			[ItemTypeID]
			public int fishID;

			// Token: 0x0400234B RID: 9035
			[Range(0f, 1f)]
			public float chance;
		}
	}
}
