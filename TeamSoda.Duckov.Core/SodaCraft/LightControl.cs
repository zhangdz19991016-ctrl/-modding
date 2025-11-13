using System;
using Umbra;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x02000428 RID: 1064
	[VolumeComponentMenu("SodaCraft/LightControl")]
	[Serializable]
	public class LightControl : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600264E RID: 9806 RVA: 0x0008467B File Offset: 0x0008287B
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x00084688 File Offset: 0x00082888
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x0008468C File Offset: 0x0008288C
		public override void Override(VolumeComponent state, float interpFactor)
		{
			LightControl lightControl = state as LightControl;
			base.Override(state, interpFactor);
			RenderSettings.ambientSkyColor = lightControl.skyColor.value;
			RenderSettings.ambientEquatorColor = lightControl.equatorColor.value;
			RenderSettings.ambientGroundColor = lightControl.groundColor.value;
			Shader.SetGlobalColor(this.fowColorID, lightControl.fowColor.value);
			Shader.SetGlobalColor(this.SodaPointLight_EnviromentTintID, lightControl.SodaLightTint.value);
			if (!LightControl.light)
			{
				LightControl.light = RenderSettings.sun;
			}
			if (LightControl.light)
			{
				LightControl.light.color = lightControl.sunColor.value;
				LightControl.light.intensity = lightControl.sunIntensity.value;
				LightControl.light.transform.rotation = Quaternion.Euler(lightControl.sunRotation.value);
				if (!LightControl.lightShadows)
				{
					LightControl.lightShadows = LightControl.light.GetComponent<UmbraSoftShadows>();
				}
				if (LightControl.lightShadows)
				{
					float value = lightControl.sunShadowHardness.value;
					LightControl.lightShadows.profile.contactStrength = value;
				}
			}
		}

		// Token: 0x04001A1C RID: 6684
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A1D RID: 6685
		public ColorParameter skyColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x04001A1E RID: 6686
		public ColorParameter equatorColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x04001A1F RID: 6687
		public ColorParameter groundColor = new ColorParameter(Color.black, true, true, false, false);

		// Token: 0x04001A20 RID: 6688
		public ColorParameter sunColor = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x04001A21 RID: 6689
		public ColorParameter fowColor = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x04001A22 RID: 6690
		public MinFloatParameter sunIntensity = new MinFloatParameter(1f, 0f, false);

		// Token: 0x04001A23 RID: 6691
		public ClampedFloatParameter sunShadowHardness = new ClampedFloatParameter(0.96f, 0f, 1f, false);

		// Token: 0x04001A24 RID: 6692
		public Vector3Parameter sunRotation = new Vector3Parameter(new Vector3(59f, 168f, 0f), false);

		// Token: 0x04001A25 RID: 6693
		public ColorParameter SodaLightTint = new ColorParameter(Color.white, true, true, false, false);

		// Token: 0x04001A26 RID: 6694
		private int SodaPointLight_EnviromentTintID = Shader.PropertyToID("SodaPointLight_EnviromentTint");

		// Token: 0x04001A27 RID: 6695
		private int fowColorID = Shader.PropertyToID("_SodaUnknowColor");

		// Token: 0x04001A28 RID: 6696
		private static Light light;

		// Token: 0x04001A29 RID: 6697
		private static UmbraSoftShadows lightShadows;
	}
}
