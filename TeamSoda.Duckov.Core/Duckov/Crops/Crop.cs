using System;
using Cysharp.Threading.Tasks;
using Duckov.Economy;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002EA RID: 746
	public class Crop : MonoBehaviour
	{
		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x00058808 File Offset: 0x00056A08
		public CropData Data
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x00058810 File Offset: 0x00056A10
		public CropInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x00058818 File Offset: 0x00056A18
		public float Progress
		{
			get
			{
				return (float)this.data.growTicks / (float)this.info.totalGrowTicks;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x00058833 File Offset: 0x00056A33
		public bool Ripen
		{
			get
			{
				return this.initialized && this.data.growTicks >= this.info.totalGrowTicks;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x0005885A File Offset: 0x00056A5A
		public bool Watered
		{
			get
			{
				return this.data.watered;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x00058868 File Offset: 0x00056A68
		public string DisplayName
		{
			get
			{
				return this.Info.DisplayName;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x00058884 File Offset: 0x00056A84
		public TimeSpan RemainingTime
		{
			get
			{
				if (!this.initialized)
				{
					return TimeSpan.Zero;
				}
				long num = this.info.totalGrowTicks - this.data.growTicks;
				if (num < 0L)
				{
					return TimeSpan.Zero;
				}
				return TimeSpan.FromTicks(num);
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06001818 RID: 6168 RVA: 0x000588C8 File Offset: 0x00056AC8
		// (remove) Token: 0x06001819 RID: 6169 RVA: 0x000588FC File Offset: 0x00056AFC
		public static event Action<Crop, Crop.CropEvent> onCropStatusChange;

		// Token: 0x0600181A RID: 6170 RVA: 0x00058930 File Offset: 0x00056B30
		public bool Harvest()
		{
			if (!this.Ripen)
			{
				return false;
			}
			if (this.Watered)
			{
				this.data.score = this.data.score + 50;
			}
			int product = this.info.GetProduct(this.data.Ranking);
			if (product <= 0)
			{
				Debug.LogError("Crop product is invalid:\ncrop:" + this.info.id);
				return false;
			}
			Cost cost = new Cost(new ValueTuple<int, long>[]
			{
				new ValueTuple<int, long>(product, (long)this.info.resultAmount)
			});
			cost.Return(false, false, 1, null).Forget();
			this.DestroyCrop();
			Action<Crop> action = this.onHarvest;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 != null)
			{
				action2(this, Crop.CropEvent.Harvest);
			}
			return true;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x000589F8 File Offset: 0x00056BF8
		public void DestroyCrop()
		{
			Action<Crop> action = this.onBeforeDestroy;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 != null)
			{
				action2(this, Crop.CropEvent.BeforeDestroy);
			}
			this.garden.Release(this);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00058A2C File Offset: 0x00056C2C
		public void InitializeNew(Garden garden, string id, Vector2Int coord)
		{
			CropData cropData = new CropData
			{
				gardenID = garden.GardenID,
				cropID = id,
				coord = coord,
				LastUpdateDateTime = DateTime.Now
			};
			this.Initialize(garden, cropData);
			Action<Crop> action = this.onPlant;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Plant);
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00058A98 File Offset: 0x00056C98
		public void Initialize(Garden garden, CropData data)
		{
			this.garden = garden;
			string cropID = data.cropID;
			CropInfo? cropInfo = CropDatabase.GetCropInfo(cropID);
			if (cropInfo == null)
			{
				Debug.LogError("找不到 corpInfo id: " + cropID);
				return;
			}
			this.info = cropInfo.Value;
			this.data = data;
			this.RefreshDisplayInstance();
			this.initialized = true;
			Vector3 localPosition = garden.CoordToLocalPosition(data.coord);
			base.transform.localPosition = localPosition;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00058B10 File Offset: 0x00056D10
		private void RefreshDisplayInstance()
		{
			if (this.displayInstance != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(this.displayInstance.gameObject);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(this.displayInstance.gameObject);
				}
			}
			if (this.info.displayPrefab == null)
			{
				Debug.LogError("找不到Display Prefab: " + this.info.DisplayName);
				return;
			}
			this.displayInstance = UnityEngine.Object.Instantiate<GameObject>(this.info.displayPrefab, this.displayParent);
			this.displayInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00058BB8 File Offset: 0x00056DB8
		public void Water()
		{
			if (this.data.watered)
			{
				return;
			}
			this.data.watered = true;
			Action<Crop> action = this.onWater;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Water);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00058BF7 File Offset: 0x00056DF7
		private void FixedUpdate()
		{
			this.Tick();
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00058C00 File Offset: 0x00056E00
		private void Tick()
		{
			if (!this.initialized)
			{
				return;
			}
			TimeSpan timeSpan = DateTime.Now - this.data.LastUpdateDateTime;
			this.data.LastUpdateDateTime = DateTime.Now;
			if (!this.data.watered)
			{
				return;
			}
			if (this.Ripen)
			{
				return;
			}
			long ticks = timeSpan.Ticks;
			this.data.growTicks = this.data.growTicks + ticks;
			if (this.Ripen)
			{
				this.OnRipen();
			}
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x00058C79 File Offset: 0x00056E79
		private void OnRipen()
		{
			Action<Crop> action = this.onRipen;
			if (action != null)
			{
				action(this);
			}
			Action<Crop, Crop.CropEvent> action2 = Crop.onCropStatusChange;
			if (action2 == null)
			{
				return;
			}
			action2(this, Crop.CropEvent.Ripen);
		}

		// Token: 0x04001182 RID: 4482
		[SerializeField]
		private Transform displayParent;

		// Token: 0x04001183 RID: 4483
		private Garden garden;

		// Token: 0x04001184 RID: 4484
		private bool initialized;

		// Token: 0x04001185 RID: 4485
		private CropData data;

		// Token: 0x04001186 RID: 4486
		private CropInfo info;

		// Token: 0x04001187 RID: 4487
		private GameObject displayInstance;

		// Token: 0x04001188 RID: 4488
		public Action<Crop> onPlant;

		// Token: 0x04001189 RID: 4489
		public Action<Crop> onWater;

		// Token: 0x0400118A RID: 4490
		public Action<Crop> onRipen;

		// Token: 0x0400118B RID: 4491
		public Action<Crop> onHarvest;

		// Token: 0x0400118C RID: 4492
		public Action<Crop> onBeforeDestroy;

		// Token: 0x0200058A RID: 1418
		public enum CropEvent
		{
			// Token: 0x04001FF8 RID: 8184
			Plant,
			// Token: 0x04001FF9 RID: 8185
			Water,
			// Token: 0x04001FFA RID: 8186
			Ripen,
			// Token: 0x04001FFB RID: 8187
			Harvest,
			// Token: 0x04001FFC RID: 8188
			BeforeDestroy
		}
	}
}
