using System;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000249 RID: 585
	[Serializable]
	public class Precipitation
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x0004648E File Offset: 0x0004468E
		public float CloudyThreshold
		{
			get
			{
				return this.cloudyThreshold;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x00046496 File Offset: 0x00044696
		public float RainyThreshold
		{
			get
			{
				return this.rainyThreshold;
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0004649E File Offset: 0x0004469E
		public bool IsRainy(TimeSpan dayAndTime)
		{
			return this.Get(dayAndTime) > this.rainyThreshold;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000464AF File Offset: 0x000446AF
		public bool IsCloudy(TimeSpan dayAndTime)
		{
			return this.Get(dayAndTime) > this.cloudyThreshold;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000464C0 File Offset: 0x000446C0
		public float Get(TimeSpan dayAndTime)
		{
			Vector2 perlinNoiseCoord = this.GetPerlinNoiseCoord(dayAndTime);
			return Mathf.Clamp01(((Mathf.PerlinNoise(perlinNoiseCoord.x, perlinNoiseCoord.y) + Mathf.PerlinNoise(perlinNoiseCoord.x + 0.5f + 123.4f, perlinNoiseCoord.y - 567.8f)) / 2f - 0.5f) * this.contrast + 0.5f + this.offset);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00046530 File Offset: 0x00044730
		public Vector2 GetPerlinNoiseCoord(TimeSpan dayAndTime)
		{
			float num = (float)(dayAndTime.Days % 3650) * 24f + (float)dayAndTime.Hours + (float)dayAndTime.Minutes / 60f;
			int num2 = dayAndTime.Days / 3650;
			return new Vector2(num * this.frequency, (float)(this.seed + num2));
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0004658C File Offset: 0x0004478C
		internal void SetSeed(int seed)
		{
			this.seed = seed;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00046598 File Offset: 0x00044798
		public float Get()
		{
			TimeSpan now = GameClock.Now;
			return this.Get(now);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000465B4 File Offset: 0x000447B4
		public bool IsRainy()
		{
			TimeSpan now = GameClock.Now;
			return this.IsRainy(now);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000465D0 File Offset: 0x000447D0
		public bool IsCloudy()
		{
			TimeSpan now = GameClock.Now;
			return this.IsCloudy(now);
		}

		// Token: 0x04000E10 RID: 3600
		[SerializeField]
		private int seed;

		// Token: 0x04000E11 RID: 3601
		[SerializeField]
		[Range(0f, 1f)]
		private float cloudyThreshold;

		// Token: 0x04000E12 RID: 3602
		[SerializeField]
		[Range(0f, 1f)]
		private float rainyThreshold;

		// Token: 0x04000E13 RID: 3603
		[SerializeField]
		private float frequency = 1f;

		// Token: 0x04000E14 RID: 3604
		[SerializeField]
		private float offset;

		// Token: 0x04000E15 RID: 3605
		[SerializeField]
		private float contrast = 1f;
	}
}
