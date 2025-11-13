using System;
using Duckov.MiniGames.GoldMiner.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A5 RID: 677
	public class GoldMinerShopUIEntry : MonoBehaviour
	{
		// Token: 0x06001634 RID: 5684 RVA: 0x00052780 File Offset: 0x00050980
		private void Awake()
		{
			if (!this.navEntry)
			{
				this.navEntry = base.GetComponent<NavEntry>();
			}
			NavEntry navEntry = this.navEntry;
			navEntry.onInteract = (Action<NavEntry>)Delegate.Combine(navEntry.onInteract, new Action<NavEntry>(this.OnInteract));
			this.VCT = base.GetComponent<VirtualCursorTarget>();
			if (this.VCT)
			{
				this.VCT.onEnter.AddListener(new UnityAction(this.OnVCTEnter));
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x00052802 File Offset: 0x00050A02
		private void OnVCTEnter()
		{
			this.master.hoveringEntry = this;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x00052810 File Offset: 0x00050A10
		private void OnInteract(NavEntry entry)
		{
			this.master.target.Buy(this.target);
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0005282C File Offset: 0x00050A2C
		internal void Setup(GoldMinerShopUI master, ShopEntity target)
		{
			this.master = master;
			this.target = target;
			if (target == null || target.artifact == null)
			{
				this.SetupEmpty();
				return;
			}
			this.mainLayout.SetActive(true);
			this.nameText.text = target.artifact.DisplayName;
			this.icon.sprite = target.artifact.Icon;
			this.Refresh();
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x000528A0 File Offset: 0x00050AA0
		private void Refresh()
		{
			bool flag;
			int num = this.master.target.CalculateDealPrice(this.target, out flag);
			this.priceText.text = num.ToString(this.priceFormat);
			this.priceIndicator.SetActive(num > 0);
			this.freeIndicator.SetActive(num <= 0);
			this.soldIndicator.SetActive(this.target.sold);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x00052915 File Offset: 0x00050B15
		private void SetupEmpty()
		{
			this.mainLayout.SetActive(false);
		}

		// Token: 0x0400105F RID: 4191
		[SerializeField]
		private NavEntry navEntry;

		// Token: 0x04001060 RID: 4192
		[SerializeField]
		private VirtualCursorTarget VCT;

		// Token: 0x04001061 RID: 4193
		[SerializeField]
		private GameObject mainLayout;

		// Token: 0x04001062 RID: 4194
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001063 RID: 4195
		[SerializeField]
		private TextMeshProUGUI priceText;

		// Token: 0x04001064 RID: 4196
		[SerializeField]
		private string priceFormat = "0";

		// Token: 0x04001065 RID: 4197
		[SerializeField]
		private GameObject priceIndicator;

		// Token: 0x04001066 RID: 4198
		[SerializeField]
		private GameObject freeIndicator;

		// Token: 0x04001067 RID: 4199
		[SerializeField]
		private Image icon;

		// Token: 0x04001068 RID: 4200
		[SerializeField]
		private GameObject soldIndicator;

		// Token: 0x04001069 RID: 4201
		private GoldMinerShopUI master;

		// Token: 0x0400106A RID: 4202
		public ShopEntity target;
	}
}
