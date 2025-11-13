using System;
using ECM2;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002DD RID: 733
	public class FPSMovement : Character
	{
		// Token: 0x06001736 RID: 5942 RVA: 0x0005545C File Offset: 0x0005365C
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00055464 File Offset: 0x00053664
		protected override void Start()
		{
			base.Start();
			if (this.game == null)
			{
				this.game.GetComponentInParent<MiniGame>();
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00055486 File Offset: 0x00053686
		public void SetGame(MiniGame game)
		{
			this.game = game;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0005548F File Offset: 0x0005368F
		private void Update()
		{
			this.UpdateRotation();
			this.UpdateMovement();
			if (this.game.GetButtonDown(MiniGame.Button.B))
			{
				this.Jump();
				return;
			}
			if (this.game.GetButtonUp(MiniGame.Button.B))
			{
				this.StopJumping();
			}
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000554C8 File Offset: 0x000536C8
		private void UpdateMovement()
		{
			Vector2 axis = this.game.GetAxis(0);
			Vector3 vector = Vector3.zero;
			vector += Vector3.right * axis.x;
			vector += Vector3.forward * axis.y;
			if (base.camera)
			{
				vector = vector.relativeTo(base.cameraTransform, true);
			}
			base.SetMovementDirection(vector);
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00055538 File Offset: 0x00053738
		private void UpdateRotation()
		{
			Vector2 axis = this.game.GetAxis(1);
			this.AddYawInput(axis.x * this.lookSensitivity.x);
			if (axis.y == 0f)
			{
				return;
			}
			float num = MathLib.ClampAngle(-base.cameraTransform.localRotation.eulerAngles.x + axis.y * this.lookSensitivity.y, -80f, 80f);
			base.cameraTransform.localRotation = Quaternion.Euler(-num, 0f, 0f);
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x000555D0 File Offset: 0x000537D0
		public void AddControlYawInput(float value)
		{
			this.AddYawInput(value);
		}

		// Token: 0x040010F4 RID: 4340
		[SerializeField]
		private MiniGame game;

		// Token: 0x040010F5 RID: 4341
		[SerializeField]
		private Vector2 lookSensitivity;
	}
}
