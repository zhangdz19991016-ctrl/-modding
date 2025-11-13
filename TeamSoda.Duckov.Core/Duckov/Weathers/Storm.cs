using System;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000248 RID: 584
	[Serializable]
	public class Storm
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x00046321 File Offset: 0x00044521
		[TimeSpan]
		private long Period
		{
			get
			{
				return this.sleepTime + this.stage1Time + this.stage2Time;
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00046338 File Offset: 0x00044538
		public int GetStormLevel(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			if (num < this.sleepTime)
			{
				return 0;
			}
			if (num < this.sleepTime + this.stage1Time)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0004637C File Offset: 0x0004457C
		public TimeSpan GetStormETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			if (num < this.sleepTime)
			{
				return TimeSpan.FromTicks(this.sleepTime - num);
			}
			return TimeSpan.Zero;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x000463BC File Offset: 0x000445BC
		public TimeSpan GetStormIOverETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			return TimeSpan.FromTicks(this.sleepTime + this.stage1Time - num);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000463F4 File Offset: 0x000445F4
		public TimeSpan GetStormIIOverETA(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period;
			return TimeSpan.FromTicks(this.Period - num);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00046424 File Offset: 0x00044624
		public float GetSleepPercent(TimeSpan dayAndTime)
		{
			return (float)((dayAndTime.Ticks + this.offset) % this.Period) / (float)this.sleepTime;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00046444 File Offset: 0x00044644
		public float GetStormRemainPercent(TimeSpan dayAndTime)
		{
			long num = (dayAndTime.Ticks + this.offset) % this.Period - this.sleepTime;
			return 1f - (float)num / ((float)this.stage1Time + (float)this.stage2Time);
		}

		// Token: 0x04000E0C RID: 3596
		[SerializeField]
		[TimeSpan]
		private long offset;

		// Token: 0x04000E0D RID: 3597
		[SerializeField]
		[TimeSpan]
		private long sleepTime;

		// Token: 0x04000E0E RID: 3598
		[SerializeField]
		[TimeSpan]
		private long stage1Time;

		// Token: 0x04000E0F RID: 3599
		[SerializeField]
		[TimeSpan]
		private long stage2Time;
	}
}
