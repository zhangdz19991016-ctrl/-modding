using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class CameraShaker : MonoBehaviour
{
	// Token: 0x06000B86 RID: 2950 RVA: 0x00031326 File Offset: 0x0002F526
	private void Awake()
	{
		CameraShaker._instance = this;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00031330 File Offset: 0x0002F530
	public static void Shake(Vector3 velocity, CameraShaker.CameraShakeTypes shakeType)
	{
		if (CameraShaker._instance == null)
		{
			return;
		}
		switch (shakeType)
		{
		case CameraShaker.CameraShakeTypes.recoil:
			CameraShaker._instance.recoilSource.GenerateImpulseWithVelocity(velocity);
			return;
		case CameraShaker.CameraShakeTypes.explosion:
			CameraShaker._instance.explosionSource.GenerateImpulseWithVelocity(velocity);
			return;
		case CameraShaker.CameraShakeTypes.meleeAttackHit:
			CameraShaker._instance.meleeAttackSource.GenerateImpulseWithVelocity(velocity);
			return;
		default:
			return;
		}
	}

	// Token: 0x040009DA RID: 2522
	private static CameraShaker _instance;

	// Token: 0x040009DB RID: 2523
	public CinemachineImpulseSource recoilSource;

	// Token: 0x040009DC RID: 2524
	public CinemachineImpulseSource meleeAttackSource;

	// Token: 0x040009DD RID: 2525
	public CinemachineImpulseSource explosionSource;

	// Token: 0x020004BF RID: 1215
	public enum CameraShakeTypes
	{
		// Token: 0x04001CC0 RID: 7360
		recoil,
		// Token: 0x04001CC1 RID: 7361
		explosion,
		// Token: 0x04001CC2 RID: 7362
		meleeAttackHit
	}
}
