using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000196 RID: 406
public class TimeOfDayVolumeControl : MonoBehaviour
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0003378F File Offset: 0x0003198F
	public VolumeProfile CurrentProfile
	{
		get
		{
			return this.currentProfile;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000C08 RID: 3080 RVA: 0x00033797 File Offset: 0x00031997
	public VolumeProfile BufferTargetProfile
	{
		get
		{
			return this.bufferTargetProfile;
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x000337A0 File Offset: 0x000319A0
	private void Update()
	{
		if (!this.blending && this.bufferTargetProfile != null)
		{
			this.StartBlendToBufferdTarget();
		}
		if (this.blending)
		{
			this.UpdateBlending(Time.deltaTime);
		}
		if (!this.blending && this.fromVolume.gameObject.activeSelf)
		{
			this.fromVolume.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00033808 File Offset: 0x00031A08
	private void UpdateBlending(float deltaTime)
	{
		this.blendTimer += deltaTime;
		float num = this.blendTimer / this.blendTime;
		if (num > 1f)
		{
			num = 1f;
			this.blending = false;
		}
		this.toVolume.weight = this.blendCurve.Evaluate(num);
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x0003385D File Offset: 0x00031A5D
	public void SetTargetProfile(VolumeProfile profile)
	{
		this.bufferTargetProfile = profile;
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00033868 File Offset: 0x00031A68
	private void StartBlendToBufferdTarget()
	{
		this.blending = true;
		this.blendingTargetProfile = this.bufferTargetProfile;
		this.bufferTargetProfile = null;
		this.currentProfile = this.blendingTargetProfile;
		this.fromVolume.gameObject.SetActive(true);
		this.fromVolume.profile = this.toVolume.profile;
		this.fromVolume.weight = 1f;
		this.toVolume.profile = this.blendingTargetProfile;
		this.toVolume.weight = 0f;
		this.blendTimer = 0f;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x000338FE File Offset: 0x00031AFE
	public void ForceSetProfile(VolumeProfile profile)
	{
		this.bufferTargetProfile = profile;
		this.StartBlendToBufferdTarget();
		this.UpdateBlending(999f);
	}

	// Token: 0x04000A7C RID: 2684
	private VolumeProfile currentProfile;

	// Token: 0x04000A7D RID: 2685
	private VolumeProfile blendingTargetProfile;

	// Token: 0x04000A7E RID: 2686
	private VolumeProfile bufferTargetProfile;

	// Token: 0x04000A7F RID: 2687
	public Volume fromVolume;

	// Token: 0x04000A80 RID: 2688
	public Volume toVolume;

	// Token: 0x04000A81 RID: 2689
	private bool blending;

	// Token: 0x04000A82 RID: 2690
	private float blendTimer;

	// Token: 0x04000A83 RID: 2691
	public float blendTime = 2f;

	// Token: 0x04000A84 RID: 2692
	public AnimationCurve blendCurve;
}
