using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x02000429 RID: 1065
	[VolumeComponentMenu("SodaCraft/TimeOfDayPost")]
	[Serializable]
	public class TimeOfDayPost : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06002652 RID: 9810 RVA: 0x000848C1 File Offset: 0x00082AC1
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000848CE File Offset: 0x00082ACE
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000848D4 File Offset: 0x00082AD4
		public override void Override(VolumeComponent state, float interpFactor)
		{
			TimeOfDayPost timeOfDayPost = state as TimeOfDayPost;
			base.Override(state, interpFactor);
			if (timeOfDayPost == null)
			{
				return;
			}
			TimeOfDayController.NightViewAngleFactor = timeOfDayPost.nightViewAngleFactor.value;
			TimeOfDayController.NightViewDistanceFactor = timeOfDayPost.nightViewDistanceFactor.value;
			TimeOfDayController.NightSenseRangeFactor = timeOfDayPost.nightSenseRangeFactor.value;
		}

		// Token: 0x04001A2A RID: 6698
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A2B RID: 6699
		public ClampedFloatParameter nightViewAngleFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x04001A2C RID: 6700
		public ClampedFloatParameter nightViewDistanceFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);

		// Token: 0x04001A2D RID: 6701
		public ClampedFloatParameter nightSenseRangeFactor = new ClampedFloatParameter(0.2f, 0f, 1f, false);
	}
}
