using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.Examples.HelloWorld
{
	// Token: 0x020002D4 RID: 724
	public class Move : MiniGameBehaviour
	{
		// Token: 0x06001700 RID: 5888 RVA: 0x00054A27 File Offset: 0x00052C27
		private void Awake()
		{
			if (this.rigidbody == null)
			{
				this.rigidbody = base.GetComponent<Rigidbody>();
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00054A44 File Offset: 0x00052C44
		protected override void OnUpdate(float deltaTime)
		{
			bool flag = this.CanJump();
			Vector2 vector = base.Game.GetAxis(0) * this.speed;
			float y = this.rigidbody.velocity.y;
			if (base.Game.GetButtonDown(MiniGame.Button.A) && flag)
			{
				y = this.jumpSpeed;
			}
			this.rigidbody.velocity = new Vector3(vector.x, y, vector.y);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00054AB5 File Offset: 0x00052CB5
		private bool CanJump()
		{
			return this.touchingColliders.Count > 0;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00054AC8 File Offset: 0x00052CC8
		private void OnCollisionEnter(Collision collision)
		{
			this.touchingColliders.Add(collision.collider);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00054ADB File Offset: 0x00052CDB
		private void OnCollisionExit(Collision collision)
		{
			this.touchingColliders.Remove(collision.collider);
		}

		// Token: 0x040010C3 RID: 4291
		[SerializeField]
		private Rigidbody rigidbody;

		// Token: 0x040010C4 RID: 4292
		[SerializeField]
		private float speed = 10f;

		// Token: 0x040010C5 RID: 4293
		[SerializeField]
		private float jumpSpeed = 5f;

		// Token: 0x040010C6 RID: 4294
		private List<Collider> touchingColliders = new List<Collider>();
	}
}
