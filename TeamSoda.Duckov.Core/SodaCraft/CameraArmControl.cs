using System;
using UnityEngine.Rendering;

namespace SodaCraft
{
	// Token: 0x02000426 RID: 1062
	[VolumeComponentMenu("SodaCraft/CameraArmControl")]
	[Serializable]
	public class CameraArmControl : VolumeComponent
	{
		// Token: 0x06002647 RID: 9799 RVA: 0x000843F9 File Offset: 0x000825F9
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x00084406 File Offset: 0x00082606
		public override void Override(VolumeComponent state, float interpFactor)
		{
			CameraArmControl cameraArmControl = state as CameraArmControl;
			base.Override(state, interpFactor);
			CameraArm.globalPitch = cameraArmControl.pitch.value;
			CameraArm.globalYaw = cameraArmControl.yaw.value;
			CameraArm.globalDistance = cameraArmControl.distance.value;
		}

		// Token: 0x04001A0B RID: 6667
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A0C RID: 6668
		public MinFloatParameter pitch = new MinFloatParameter(55f, 0f, false);

		// Token: 0x04001A0D RID: 6669
		public FloatParameter yaw = new FloatParameter(-30f, false);

		// Token: 0x04001A0E RID: 6670
		public MinFloatParameter distance = new MinFloatParameter(45f, 2f, false);
	}
}
