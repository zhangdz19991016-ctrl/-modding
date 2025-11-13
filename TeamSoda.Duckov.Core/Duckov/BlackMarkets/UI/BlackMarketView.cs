using System;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x0200030D RID: 781
	public class BlackMarketView : View
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x0005DA02 File Offset: 0x0005BC02
		public static BlackMarketView Instance
		{
			get
			{
				return View.GetViewInstance<BlackMarketView>();
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0005DA09 File Offset: 0x0005BC09
		protected override bool ShowOpenCloseButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0005DA0C File Offset: 0x0005BC0C
		protected override void Awake()
		{
			base.Awake();
			this.btn_demandPanel.onClick.AddListener(delegate()
			{
				this.SetMode(BlackMarketView.Mode.Demand);
			});
			this.btn_supplyPanel.onClick.AddListener(delegate()
			{
				this.SetMode(BlackMarketView.Mode.Supply);
			});
			this.btn_refresh.onClick.AddListener(new UnityAction(this.OnRefreshBtnClicked));
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0005DA73 File Offset: 0x0005BC73
		private void OnEnable()
		{
			BlackMarket.onRefreshChanceChanged += this.OnRefreshChanceChanced;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0005DA86 File Offset: 0x0005BC86
		private void OnDisable()
		{
			BlackMarket.onRefreshChanceChanged -= this.OnRefreshChanceChanced;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0005DA99 File Offset: 0x0005BC99
		private void OnRefreshChanceChanced(BlackMarket market)
		{
			this.RefreshRefreshButton();
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0005DAA4 File Offset: 0x0005BCA4
		private void RefreshRefreshButton()
		{
			if (this.Target == null)
			{
				this.refreshChanceText.text = "ERROR";
				this.refreshInteractableIndicator.SetActive(false);
			}
			int refreshChance = this.Target.RefreshChance;
			int maxRefreshChance = this.Target.MaxRefreshChance;
			this.refreshChanceText.text = string.Format("{0}/{1}", refreshChance, maxRefreshChance);
			this.refreshInteractableIndicator.SetActive(refreshChance > 0);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0005DB23 File Offset: 0x0005BD23
		private void OnRefreshBtnClicked()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.PayAndRegenerate();
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x0005DB3F File Offset: 0x0005BD3F
		// (set) Token: 0x060019B1 RID: 6577 RVA: 0x0005DB47 File Offset: 0x0005BD47
		public BlackMarket Target { get; private set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x0005DB50 File Offset: 0x0005BD50
		private bool ShowDemand
		{
			get
			{
				return (BlackMarketView.Mode.Demand | this.mode) == this.mode;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x0005DB62 File Offset: 0x0005BD62
		private bool ShowSupply
		{
			get
			{
				return (BlackMarketView.Mode.Supply | this.mode) == this.mode;
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0005DB74 File Offset: 0x0005BD74
		public static void Show(BlackMarketView.Mode mode)
		{
			if (BlackMarketView.Instance == null)
			{
				return;
			}
			if (BlackMarket.Instance == null)
			{
				return;
			}
			BlackMarketView.Instance.Setup(BlackMarket.Instance, mode);
			BlackMarketView.Instance.Open(null);
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0005DBAD File Offset: 0x0005BDAD
		private void Setup(BlackMarket target, BlackMarketView.Mode mode)
		{
			this.Target = target;
			this.demandPanel.Setup(target);
			this.supplyPanel.Setup(target);
			this.RefreshRefreshButton();
			this.SetMode(mode);
			base.Open(null);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0005DBE2 File Offset: 0x0005BDE2
		private void SetMode(BlackMarketView.Mode mode)
		{
			this.mode = mode;
			this.demandPanel.gameObject.SetActive(this.ShowDemand);
			this.supplyPanel.gameObject.SetActive(this.ShowSupply);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0005DC17 File Offset: 0x0005BE17
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0005DC2A File Offset: 0x0005BE2A
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0005DC40 File Offset: 0x0005BE40
		private void Update()
		{
			if (this.Target == null)
			{
				return;
			}
			int refreshChance = this.Target.RefreshChance;
			int maxRefreshChance = this.Target.MaxRefreshChance;
			string text;
			if (refreshChance < maxRefreshChance)
			{
				TimeSpan remainingTimeBeforeRefresh = this.Target.RemainingTimeBeforeRefresh;
				text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)remainingTimeBeforeRefresh.TotalHours), remainingTimeBeforeRefresh.Minutes, remainingTimeBeforeRefresh.Seconds);
			}
			else
			{
				text = "--:--:--";
			}
			this.refreshETAText.text = text;
		}

		// Token: 0x04001297 RID: 4759
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001298 RID: 4760
		[SerializeField]
		private DemandPanel demandPanel;

		// Token: 0x04001299 RID: 4761
		[SerializeField]
		private SupplyPanel supplyPanel;

		// Token: 0x0400129A RID: 4762
		[SerializeField]
		private TextMeshProUGUI refreshETAText;

		// Token: 0x0400129B RID: 4763
		[SerializeField]
		private TextMeshProUGUI refreshChanceText;

		// Token: 0x0400129C RID: 4764
		[SerializeField]
		private Button btn_demandPanel;

		// Token: 0x0400129D RID: 4765
		[SerializeField]
		private Button btn_supplyPanel;

		// Token: 0x0400129E RID: 4766
		[SerializeField]
		private Button btn_refresh;

		// Token: 0x0400129F RID: 4767
		[SerializeField]
		private GameObject refreshInteractableIndicator;

		// Token: 0x040012A1 RID: 4769
		private BlackMarketView.Mode mode;

		// Token: 0x020005AA RID: 1450
		public enum Mode
		{
			// Token: 0x0400206D RID: 8301
			None,
			// Token: 0x0400206E RID: 8302
			Demand,
			// Token: 0x0400206F RID: 8303
			Supply
		}
	}
}
