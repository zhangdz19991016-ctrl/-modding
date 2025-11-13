using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002DB RID: 731
	public class FPSGunControl : MiniGameBehaviour
	{
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00055196 File Offset: 0x00053396
		public FPSGun Gun
		{
			get
			{
				return this.gun;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x0005519E File Offset: 0x0005339E
		public float ScatterAngle
		{
			get
			{
				if (this.Gun)
				{
					return this.Gun.ScatterAngle;
				}
				return 0f;
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x000551BE File Offset: 0x000533BE
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.gun != null)
			{
				this.SetGun(this.gun);
			}
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x000551E0 File Offset: 0x000533E0
		protected override void OnUpdate(float deltaTime)
		{
			bool buttonDown = base.Game.GetButtonDown(MiniGame.Button.A);
			bool buttonUp = base.Game.GetButtonUp(MiniGame.Button.A);
			if (buttonDown)
			{
				this.gun.SetTrigger(true);
			}
			if (buttonUp)
			{
				this.gun.SetTrigger(false);
			}
			this.UpdateGunPhysicsStatus(deltaTime);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x0005522A File Offset: 0x0005342A
		private void UpdateGunPhysicsStatus(float deltaTime)
		{
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0005522C File Offset: 0x0005342C
		private void SetGun(FPSGun gunInstance)
		{
			if (gunInstance != this.gun)
			{
				UnityEngine.Object.Destroy(this.gun);
			}
			this.gun = gunInstance;
			this.SetupGunData();
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00055254 File Offset: 0x00053454
		private void SetupGunData()
		{
			this.gun.Setup(this.mainCamera, this.gunParent);
		}

		// Token: 0x040010E8 RID: 4328
		[SerializeField]
		private Camera mainCamera;

		// Token: 0x040010E9 RID: 4329
		[SerializeField]
		private Transform gunParent;

		// Token: 0x040010EA RID: 4330
		[SerializeField]
		private FPSGun gun;
	}
}
