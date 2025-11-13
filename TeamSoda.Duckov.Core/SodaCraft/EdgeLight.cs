using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SodaCraft
{
	// Token: 0x02000427 RID: 1063
	[VolumeComponentMenu("SodaCraft/EdgeLight")]
	[Serializable]
	public class EdgeLight : VolumeComponent, IPostProcessComponent
	{
		// Token: 0x0600264A RID: 9802 RVA: 0x000844A5 File Offset: 0x000826A5
		public bool IsActive()
		{
			return this.enable.value;
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000844B2 File Offset: 0x000826B2
		public bool IsTileCompatible()
		{
			return false;
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000844B8 File Offset: 0x000826B8
		public override void Override(VolumeComponent state, float interpFactor)
		{
			EdgeLight edgeLight = state as EdgeLight;
			base.Override(state, interpFactor);
			Shader.SetGlobalVector(this.edgeLightDirectionHash, edgeLight.direction.value);
			Shader.SetGlobalFloat(this.widthHash, edgeLight.edgeLightWidth.value);
			Shader.SetGlobalFloat(this.fixHash, edgeLight.edgeLightFix.value);
			Shader.SetGlobalFloat(this.clampDistanceHash, edgeLight.EdgeLightClampDistance.value);
			Shader.SetGlobalColor(this.colorHash, edgeLight.edgeLightColor.value);
			Shader.SetGlobalFloat(this.edgeLightBlendScreenColorHash, edgeLight.blendScreenColor.value);
		}

		// Token: 0x04001A0F RID: 6671
		public BoolParameter enable = new BoolParameter(false, false);

		// Token: 0x04001A10 RID: 6672
		public Vector2Parameter direction = new Vector2Parameter(new Vector2(-1f, 1f), false);

		// Token: 0x04001A11 RID: 6673
		public ClampedFloatParameter edgeLightWidth = new ClampedFloatParameter(0.001f, 0f, 0.05f, false);

		// Token: 0x04001A12 RID: 6674
		public ClampedFloatParameter edgeLightFix = new ClampedFloatParameter(0.001f, 0f, 0.05f, false);

		// Token: 0x04001A13 RID: 6675
		public FloatParameter EdgeLightClampDistance = new ClampedFloatParameter(0.001f, 0.001f, 1f, false);

		// Token: 0x04001A14 RID: 6676
		public ColorParameter edgeLightColor = new ColorParameter(Color.white, true, false, false, false);

		// Token: 0x04001A15 RID: 6677
		public FloatParameter blendScreenColor = new ClampedFloatParameter(1f, 0f, 1f, false);

		// Token: 0x04001A16 RID: 6678
		private int edgeLightDirectionHash = Shader.PropertyToID("_EdgeLightDirection");

		// Token: 0x04001A17 RID: 6679
		private int widthHash = Shader.PropertyToID("_EdgeLightWidth");

		// Token: 0x04001A18 RID: 6680
		private int colorHash = Shader.PropertyToID("_EdgeLightColor");

		// Token: 0x04001A19 RID: 6681
		private int fixHash = Shader.PropertyToID("_EdgeLightFix");

		// Token: 0x04001A1A RID: 6682
		private int clampDistanceHash = Shader.PropertyToID("_EdgeLightClampDistance");

		// Token: 0x04001A1B RID: 6683
		private int edgeLightBlendScreenColorHash = Shader.PropertyToID("_EdgeLightBlendScreenColor");
	}
}
