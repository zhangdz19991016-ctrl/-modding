using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D6 RID: 726
	public class FPS_Enemy_HomingFly : MiniGameBehaviour
	{
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x00054BBE File Offset: 0x00052DBE
		private bool CanSeeTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x00054BC1 File Offset: 0x00052DC1
		private bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00054BCE File Offset: 0x00052DCE
		private void Awake()
		{
			if (this.rigidbody == null)
			{
				this.rigidbody = base.GetComponent<Rigidbody>();
			}
			this.health.onDead += this.OnDead;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00054C01 File Offset: 0x00052E01
		private void OnDead(FPSHealth health)
		{
			this.rigidbody.useGravity = true;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00054C0F File Offset: 0x00052E0F
		protected override void OnUpdate(float deltaTime)
		{
			if (this.Dead)
			{
				this.UpdateDead(deltaTime);
				return;
			}
			if (this.CanSeeTarget)
			{
				this.UpdateHoming(deltaTime);
				return;
			}
			this.UpdateIdle(deltaTime);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00054C38 File Offset: 0x00052E38
		private void UpdateIdle(float deltaTime)
		{
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00054C3A File Offset: 0x00052E3A
		private void UpdateDead(float deltaTime)
		{
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00054C3C File Offset: 0x00052E3C
		private void UpdateHoming(float deltaTime)
		{
		}

		// Token: 0x040010CA RID: 4298
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x040010CB RID: 4299
		[SerializeField]
		private FPSHealth health;
	}
}
