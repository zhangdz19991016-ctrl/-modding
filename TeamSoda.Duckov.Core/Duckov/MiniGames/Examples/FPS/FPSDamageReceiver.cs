using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D8 RID: 728
	public class FPSDamageReceiver : MonoBehaviour
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00054C4E File Offset: 0x00052E4E
		public ParticleSystem DamageFX
		{
			get
			{
				if (GameManager.BloodFxOn)
				{
					return this.damageEffectPrefab;
				}
				return this.damageEffectPrefab_Censored;
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06001715 RID: 5909 RVA: 0x00054C64 File Offset: 0x00052E64
		// (remove) Token: 0x06001716 RID: 5910 RVA: 0x00054C9C File Offset: 0x00052E9C
		public event Action<FPSDamageReceiver, FPSDamageInfo> onReceiveDamage;

		// Token: 0x06001717 RID: 5911 RVA: 0x00054CD4 File Offset: 0x00052ED4
		internal void CastDamage(FPSDamageInfo damage)
		{
			if (this.DamageFX == null)
			{
				return;
			}
			FXPool.Play(this.DamageFX, damage.point, Quaternion.FromToRotation(Vector3.forward, damage.normal));
			Action<FPSDamageReceiver, FPSDamageInfo> action = this.onReceiveDamage;
			if (action == null)
			{
				return;
			}
			action(this, damage);
		}

		// Token: 0x040010CE RID: 4302
		[SerializeField]
		private ParticleSystem damageEffectPrefab;

		// Token: 0x040010CF RID: 4303
		[SerializeField]
		private ParticleSystem damageEffectPrefab_Censored;
	}
}
