using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x02000425 RID: 1061
	[VolumeComponentMenu("SodaCraft/SunFogTD")]
	[Serializable]
	public class SunFogTD : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x06002643 RID: 9795 RVA: 0x00084189 File Offset: 0x00082389
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00084196 File Offset: 0x00082396
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x0008419C File Offset: 0x0008239C
		public override void Override(VolumeComponent state, float interpFactor)
		{
			SunFogTD sunFogTD = state as SunFogTD;
			base.Override(state, interpFactor);
			Shader.SetGlobalColor(this.fogColorHash, sunFogTD.fogColor.value);
			Shader.SetGlobalColor(this.sunColorHash, sunFogTD.sunColor.value);
			Shader.SetGlobalFloat(this.nearDistanceHash, sunFogTD.clipPlanes.value.x);
			Shader.SetGlobalFloat(this.farDistanceHash, sunFogTD.clipPlanes.value.y);
			Shader.SetGlobalFloat(this.sunSizeHash, sunFogTD.sunSize.value);
			Shader.SetGlobalFloat(this.sunPowerHash, sunFogTD.sunPower.value);
			Shader.SetGlobalVector(this.sunPointHash, sunFogTD.sunPoint.value);
			Shader.SetGlobalFloat(this.sunAlphaGainHash, sunFogTD.sunAlphaGain.value);
		}

		// Token: 0x040019FB RID: 6651
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x040019FC RID: 6652
		public ColorParameter fogColor = new ColorParameter(new Color(0.68718916f, 1.070217f, 1.3615336f, 0f), true, true, false, false);

		// Token: 0x040019FD RID: 6653
		public ColorParameter sunColor = new ColorParameter(new Color(4.061477f, 2.5092788f, 1.7816858f, 0f), true, true, false, false);

		// Token: 0x040019FE RID: 6654
		public FloatRangeParameter clipPlanes = new FloatRangeParameter(new Vector2(41f, 72f), 0.3f, 1000f, false);

		// Token: 0x040019FF RID: 6655
		public Vector2Parameter sunPoint = new Vector2Parameter(new Vector2(-2.63f, 1.23f), false);

		// Token: 0x04001A00 RID: 6656
		public FloatParameter sunSize = new ClampedFloatParameter(1.85f, 0f, 10f, false);

		// Token: 0x04001A01 RID: 6657
		public ClampedFloatParameter sunPower = new ClampedFloatParameter(1f, 0.1f, 10f, false);

		// Token: 0x04001A02 RID: 6658
		public ClampedFloatParameter sunAlphaGain = new ClampedFloatParameter(0.001f, 0f, 0.25f, false);

		// Token: 0x04001A03 RID: 6659
		private int fogColorHash = Shader.PropertyToID("SunFogColor");

		// Token: 0x04001A04 RID: 6660
		private int sunColorHash = Shader.PropertyToID("SunFogSunColor");

		// Token: 0x04001A05 RID: 6661
		private int nearDistanceHash = Shader.PropertyToID("SunFogNearDistance");

		// Token: 0x04001A06 RID: 6662
		private int farDistanceHash = Shader.PropertyToID("SunFogFarDistance");

		// Token: 0x04001A07 RID: 6663
		private int sunPointHash = Shader.PropertyToID("SunFogSunPoint");

		// Token: 0x04001A08 RID: 6664
		private int sunSizeHash = Shader.PropertyToID("SunFogSunSize");

		// Token: 0x04001A09 RID: 6665
		private int sunPowerHash = Shader.PropertyToID("SunFogSunPower");

		// Token: 0x04001A0A RID: 6666
		private int sunAlphaGainHash = Shader.PropertyToID("SunFogSunAplhaGain");
	}
}
