using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x02000290 RID: 656
	public class GoldMinerArtifact : MiniGameBehaviour
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0004F85D File Offset: 0x0004DA5D
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x0004F86F File Offset: 0x0004DA6F
		[LocalizationKey("Default")]
		private string displayNameKey
		{
			get
			{
				return "GoldMiner_" + this.id;
			}
			set
			{
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0004F871 File Offset: 0x0004DA71
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x0004F888 File Offset: 0x0004DA88
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return "GoldMiner_" + this.id + "_Desc";
			}
			set
			{
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0004F88A File Offset: 0x0004DA8A
		public bool AllowMultiple
		{
			get
			{
				return this.allowMultiple;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x0004F892 File Offset: 0x0004DA92
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0004F89F File Offset: 0x0004DA9F
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x0600155B RID: 5467 RVA: 0x0004F8AC File Offset: 0x0004DAAC
		public int Quality
		{
			get
			{
				return this.quality;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0004F8B4 File Offset: 0x0004DAB4
		public int BasePrice
		{
			get
			{
				return this.basePrice;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x0004F8BC File Offset: 0x0004DABC
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0004F8C4 File Offset: 0x0004DAC4
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x0004F8CC File Offset: 0x0004DACC
		public GoldMiner Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004F8D4 File Offset: 0x0004DAD4
		public void Attach(GoldMiner master)
		{
			this.master = master;
			base.transform.SetParent(master.transform);
			Action<GoldMinerArtifact> onAttached = this.OnAttached;
			if (onAttached == null)
			{
				return;
			}
			onAttached(this);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0004F8FF File Offset: 0x0004DAFF
		public void Detatch(GoldMiner master)
		{
			Action<GoldMinerArtifact> onDetached = this.OnDetached;
			if (onDetached != null)
			{
				onDetached(this);
			}
			if (master != this.master)
			{
				Debug.LogError("Artifact is being notified detach by a different GoldMiner instance.", master.gameObject);
			}
			this.master = null;
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0004F938 File Offset: 0x0004DB38
		private void OnDestroy()
		{
			this.Detatch(this.master);
		}

		// Token: 0x04000FA0 RID: 4000
		[SerializeField]
		private string id;

		// Token: 0x04000FA1 RID: 4001
		[SerializeField]
		private Sprite icon;

		// Token: 0x04000FA2 RID: 4002
		[SerializeField]
		private bool allowMultiple;

		// Token: 0x04000FA3 RID: 4003
		[SerializeField]
		private int basePrice;

		// Token: 0x04000FA4 RID: 4004
		[SerializeField]
		private int quality;

		// Token: 0x04000FA5 RID: 4005
		private GoldMiner master;

		// Token: 0x04000FA6 RID: 4006
		public Action<GoldMinerArtifact> OnAttached;

		// Token: 0x04000FA7 RID: 4007
		public Action<GoldMinerArtifact> OnDetached;
	}
}
