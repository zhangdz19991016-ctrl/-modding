using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000295 RID: 661
	public class GoldMinerEntity : MiniGameBehaviour
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x00050D93 File Offset: 0x0004EF93
		// (set) Token: 0x060015BB RID: 5563 RVA: 0x00050D9B File Offset: 0x0004EF9B
		public GoldMiner master { get; private set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00050DA4 File Offset: 0x0004EFA4
		public string TypeID
		{
			get
			{
				return this.typeID;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x00050DAC File Offset: 0x0004EFAC
		public float Speed
		{
			get
			{
				return this.speed;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x00050DB4 File Offset: 0x0004EFB4
		// (set) Token: 0x060015BF RID: 5567 RVA: 0x00050DBC File Offset: 0x0004EFBC
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00050DC5 File Offset: 0x0004EFC5
		public void SetMaster(GoldMiner master)
		{
			this.master = master;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x00050DCE File Offset: 0x0004EFCE
		public void NotifyAttached(Hook hook)
		{
			Action<GoldMinerEntity, Hook> onAttached = this.OnAttached;
			if (onAttached != null)
			{
				onAttached(this, hook);
			}
			FXPool.Play(this.contactFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00050E05 File Offset: 0x0004F005
		public void NotifyBeginRetrieving()
		{
			FXPool.Play(this.beginMoveFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00050E29 File Offset: 0x0004F029
		internal void Explode(Vector3 origin)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			FXPool.Play(this.explodeFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00050E58 File Offset: 0x0004F058
		internal void NotifyResolved(GoldMiner game)
		{
			Action<GoldMinerEntity, GoldMiner> onResolved = this.OnResolved;
			if (onResolved != null)
			{
				onResolved(this, game);
			}
			FXPool.Play(this.resolveFX, base.transform.position, base.transform.rotation);
		}

		// Token: 0x04001001 RID: 4097
		[SerializeField]
		private string typeID;

		// Token: 0x04001002 RID: 4098
		[SerializeField]
		public GoldMinerEntity.Size size;

		// Token: 0x04001003 RID: 4099
		[SerializeField]
		public GoldMinerEntity.Tag[] tags;

		// Token: 0x04001004 RID: 4100
		[SerializeField]
		private int value;

		// Token: 0x04001005 RID: 4101
		[SerializeField]
		private float speed = 1f;

		// Token: 0x04001006 RID: 4102
		[SerializeField]
		private ParticleSystem contactFX;

		// Token: 0x04001007 RID: 4103
		[SerializeField]
		private ParticleSystem beginMoveFX;

		// Token: 0x04001008 RID: 4104
		[SerializeField]
		private ParticleSystem resolveFX;

		// Token: 0x04001009 RID: 4105
		[SerializeField]
		private ParticleSystem explodeFX;

		// Token: 0x0400100A RID: 4106
		public Action<GoldMinerEntity, Hook> OnAttached;

		// Token: 0x0400100B RID: 4107
		public Action<GoldMinerEntity, GoldMiner> OnResolved;

		// Token: 0x02000573 RID: 1395
		public enum Size
		{
			// Token: 0x04001F95 RID: 8085
			XS = -2,
			// Token: 0x04001F96 RID: 8086
			S,
			// Token: 0x04001F97 RID: 8087
			M,
			// Token: 0x04001F98 RID: 8088
			L,
			// Token: 0x04001F99 RID: 8089
			XL
		}

		// Token: 0x02000574 RID: 1396
		public enum Tag
		{
			// Token: 0x04001F9B RID: 8091
			None,
			// Token: 0x04001F9C RID: 8092
			Rock,
			// Token: 0x04001F9D RID: 8093
			Gold,
			// Token: 0x04001F9E RID: 8094
			Diamond,
			// Token: 0x04001F9F RID: 8095
			Mine,
			// Token: 0x04001FA0 RID: 8096
			Chest,
			// Token: 0x04001FA1 RID: 8097
			Pig,
			// Token: 0x04001FA2 RID: 8098
			Cable
		}
	}
}
