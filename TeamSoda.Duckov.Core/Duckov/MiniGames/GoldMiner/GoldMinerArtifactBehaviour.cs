using System;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000291 RID: 657
	public abstract class GoldMinerArtifactBehaviour : MiniGameBehaviour
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001564 RID: 5476 RVA: 0x0004F94E File Offset: 0x0004DB4E
		protected GoldMinerRunData Run
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				if (this.master.Master == null)
				{
					return null;
				}
				return this.master.Master.run;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x0004F985 File Offset: 0x0004DB85
		protected GoldMiner GoldMiner
		{
			get
			{
				if (this.master == null)
				{
					return null;
				}
				return this.master.Master;
			}
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0004F9A4 File Offset: 0x0004DBA4
		private void Awake()
		{
			if (!this.master)
			{
				this.master = base.GetComponent<GoldMinerArtifact>();
			}
			GoldMinerArtifact goldMinerArtifact = this.master;
			goldMinerArtifact.OnAttached = (Action<GoldMinerArtifact>)Delegate.Combine(goldMinerArtifact.OnAttached, new Action<GoldMinerArtifact>(this.OnAttached));
			GoldMinerArtifact goldMinerArtifact2 = this.master;
			goldMinerArtifact2.OnDetached = (Action<GoldMinerArtifact>)Delegate.Combine(goldMinerArtifact2.OnDetached, new Action<GoldMinerArtifact>(this.OnDetached));
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0004FA1A File Offset: 0x0004DC1A
		protected virtual void OnAttached(GoldMinerArtifact artifact)
		{
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0004FA1C File Offset: 0x0004DC1C
		protected virtual void OnDetached(GoldMinerArtifact artifact)
		{
		}

		// Token: 0x04000FA8 RID: 4008
		[SerializeField]
		protected GoldMinerArtifact master;
	}
}
