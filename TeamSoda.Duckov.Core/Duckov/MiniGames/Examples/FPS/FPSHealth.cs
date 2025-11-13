using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002DC RID: 732
	public class FPSHealth : MiniGameBehaviour
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x00055275 File Offset: 0x00053475
		public int HP
		{
			get
			{
				return this.hp;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x0005527D File Offset: 0x0005347D
		public bool Dead
		{
			get
			{
				return this.dead;
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x0600172E RID: 5934 RVA: 0x00055288 File Offset: 0x00053488
		// (remove) Token: 0x0600172F RID: 5935 RVA: 0x000552C0 File Offset: 0x000534C0
		public event Action<FPSHealth> onDead;

		// Token: 0x06001730 RID: 5936 RVA: 0x000552F8 File Offset: 0x000534F8
		protected override void Start()
		{
			base.Start();
			this.hp = this.maxHp;
			this.materialPropertyBlock = new MaterialPropertyBlock();
			foreach (FPSDamageReceiver fpsdamageReceiver in this.damageReceivers)
			{
				fpsdamageReceiver.onReceiveDamage += this.OnReceiverReceiveDamage;
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x00055374 File Offset: 0x00053574
		protected override void OnUpdate(float deltaTime)
		{
			if (this.hurtValue > 0f)
			{
				this.hurtValue = Mathf.MoveTowards(this.hurtValue, 0f, deltaTime * this.hurtValueDropRate);
			}
			this.materialPropertyBlock.SetFloat("_HurtValue", this.hurtValue);
			this.meshRenderer.SetPropertyBlock(this.materialPropertyBlock, 0);
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000553D4 File Offset: 0x000535D4
		private void OnReceiverReceiveDamage(FPSDamageReceiver receiver, FPSDamageInfo info)
		{
			this.ReceiveDamage(info);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000553E0 File Offset: 0x000535E0
		private void ReceiveDamage(FPSDamageInfo info)
		{
			if (this.dead)
			{
				return;
			}
			this.hurtValue = 1f;
			this.hp -= Mathf.FloorToInt(info.amount);
			if (this.hp <= 0)
			{
				this.hp = 0;
				this.Die();
			}
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x0005542F File Offset: 0x0005362F
		private void Die()
		{
			this.dead = true;
			Action<FPSHealth> action = this.onDead;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040010EB RID: 4331
		[SerializeField]
		private int maxHp;

		// Token: 0x040010EC RID: 4332
		[SerializeField]
		private List<FPSDamageReceiver> damageReceivers;

		// Token: 0x040010ED RID: 4333
		[SerializeField]
		private MeshRenderer meshRenderer;

		// Token: 0x040010EE RID: 4334
		[SerializeField]
		private float hurtValueDropRate = 1f;

		// Token: 0x040010EF RID: 4335
		private int hp;

		// Token: 0x040010F0 RID: 4336
		private bool dead;

		// Token: 0x040010F1 RID: 4337
		private float hurtValue;

		// Token: 0x040010F3 RID: 4339
		private MaterialPropertyBlock materialPropertyBlock;
	}
}
