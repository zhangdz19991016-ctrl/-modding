using System;
using UnityEngine;

namespace Duckov.Crops
{
	// Token: 0x020002EB RID: 747
	public class CropAnimator : MonoBehaviour
	{
		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x00058CA6 File Offset: 0x00056EA6
		private ParticleSystem PlantFX
		{
			get
			{
				return this.plantFX;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001825 RID: 6181 RVA: 0x00058CAE File Offset: 0x00056EAE
		private ParticleSystem StageChangeFX
		{
			get
			{
				return this.stageChangeFX;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x00058CB6 File Offset: 0x00056EB6
		private ParticleSystem RipenFX
		{
			get
			{
				return this.ripenFX;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x00058CBE File Offset: 0x00056EBE
		private ParticleSystem WaterFX
		{
			get
			{
				return this.waterFX;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x00058CC6 File Offset: 0x00056EC6
		private ParticleSystem HarvestFX
		{
			get
			{
				return this.harvestFX;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x00058CCE File Offset: 0x00056ECE
		private ParticleSystem DestroyFX
		{
			get
			{
				return this.destroyFX;
			}
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x00058CD8 File Offset: 0x00056ED8
		private void Awake()
		{
			if (this.crop == null)
			{
				this.crop = base.GetComponent<Crop>();
			}
			Crop crop = this.crop;
			crop.onPlant = (Action<Crop>)Delegate.Combine(crop.onPlant, new Action<Crop>(this.OnPlant));
			Crop crop2 = this.crop;
			crop2.onRipen = (Action<Crop>)Delegate.Combine(crop2.onRipen, new Action<Crop>(this.OnRipen));
			Crop crop3 = this.crop;
			crop3.onWater = (Action<Crop>)Delegate.Combine(crop3.onWater, new Action<Crop>(this.OnWater));
			Crop crop4 = this.crop;
			crop4.onHarvest = (Action<Crop>)Delegate.Combine(crop4.onHarvest, new Action<Crop>(this.OnHarvest));
			Crop crop5 = this.crop;
			crop5.onBeforeDestroy = (Action<Crop>)Delegate.Combine(crop5.onBeforeDestroy, new Action<Crop>(this.OnBeforeDestroy));
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00058DC2 File Offset: 0x00056FC2
		private void Update()
		{
			this.RefreshPosition(true);
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00058DCC File Offset: 0x00056FCC
		private void RefreshPosition(bool notifyStageChange = true)
		{
			float progress = this.crop.Progress;
			CropAnimator.Stage stage = default(CropAnimator.Stage);
			int? num = this.cachedStage;
			for (int i = 0; i < this.stages.Length; i++)
			{
				CropAnimator.Stage stage2 = this.stages[i];
				if (progress < this.stages[i].progress)
				{
					stage = stage2;
					this.cachedStage = new int?(i);
					break;
				}
			}
			this.displayParent.localPosition = Vector3.up * stage.position;
			if (!notifyStageChange)
			{
				return;
			}
			if (num == null)
			{
				return;
			}
			int value = num.Value;
			int? num2 = this.cachedStage;
			if (!(value == num2.GetValueOrDefault() & num2 != null))
			{
				this.OnStageChange();
			}
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x00058E8B File Offset: 0x0005708B
		private void OnStageChange()
		{
			FXPool.Play(this.StageChangeFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00058EAF File Offset: 0x000570AF
		private void OnWater(Crop crop)
		{
			FXPool.Play(this.WaterFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x00058ED3 File Offset: 0x000570D3
		private void OnRipen(Crop crop)
		{
			FXPool.Play(this.RipenFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00058EF7 File Offset: 0x000570F7
		private void OnHarvest(Crop crop)
		{
			FXPool.Play(this.HarvestFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x00058F1B File Offset: 0x0005711B
		private void OnPlant(Crop crop)
		{
			FXPool.Play(this.PlantFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x00058F3F File Offset: 0x0005713F
		private void OnBeforeDestroy(Crop crop)
		{
			FXPool.Play(this.DestroyFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x0400118E RID: 4494
		[SerializeField]
		private Crop crop;

		// Token: 0x0400118F RID: 4495
		[SerializeField]
		private Transform displayParent;

		// Token: 0x04001190 RID: 4496
		[SerializeField]
		private ParticleSystem plantFX;

		// Token: 0x04001191 RID: 4497
		[SerializeField]
		private ParticleSystem stageChangeFX;

		// Token: 0x04001192 RID: 4498
		[SerializeField]
		private ParticleSystem ripenFX;

		// Token: 0x04001193 RID: 4499
		[SerializeField]
		private ParticleSystem waterFX;

		// Token: 0x04001194 RID: 4500
		[SerializeField]
		private ParticleSystem harvestFX;

		// Token: 0x04001195 RID: 4501
		[SerializeField]
		private ParticleSystem destroyFX;

		// Token: 0x04001196 RID: 4502
		[SerializeField]
		private CropAnimator.Stage[] stages = new CropAnimator.Stage[]
		{
			new CropAnimator.Stage(0.333f, -0.4f),
			new CropAnimator.Stage(0.666f, -0.2f),
			new CropAnimator.Stage(0.999f, -0.1f)
		};

		// Token: 0x04001197 RID: 4503
		private int? cachedStage;

		// Token: 0x0200058B RID: 1419
		[Serializable]
		private struct Stage
		{
			// Token: 0x060028D3 RID: 10451 RVA: 0x00097066 File Offset: 0x00095266
			public Stage(float progress, float position)
			{
				this.progress = progress;
				this.position = position;
			}

			// Token: 0x04001FFD RID: 8189
			public float progress;

			// Token: 0x04001FFE RID: 8190
			public float position;
		}
	}
}
