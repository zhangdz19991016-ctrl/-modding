using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000354 RID: 852
	public class RewardEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x0006A819 File Offset: 0x00068A19
		// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x0006A821 File Offset: 0x00068A21
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0006A82A File Offset: 0x00068A2A
		private void Awake()
		{
			this.claimButton.onClick.AddListener(new UnityAction(this.OnClaimButtonClicked));
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0006A848 File Offset: 0x00068A48
		private void OnClaimButtonClicked()
		{
			Reward reward = this.target;
			if (reward == null)
			{
				return;
			}
			reward.Claim();
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0006A85A File Offset: 0x00068A5A
		public void NotifyPooled()
		{
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0006A85C File Offset: 0x00068A5C
		public void NotifyReleased()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0006A864 File Offset: 0x00068A64
		internal void Setup(Reward target)
		{
			this.UnregisterEvents();
			this.target = target;
			if (target == null)
			{
				return;
			}
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0006A889 File Offset: 0x00068A89
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0006A8B1 File Offset: 0x00068AB1
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0006A8D9 File Offset: 0x00068AD9
		private void OnTargetStatusChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0006A8E4 File Offset: 0x00068AE4
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.rewardText.text = this.target.Description;
			Sprite icon = this.target.Icon;
			this.rewardIcon.gameObject.SetActive(icon);
			this.rewardIcon.sprite = icon;
			bool claimed = this.target.Claimed;
			bool claimable = this.target.Claimable;
			bool flag = this.Interactable && claimable;
			bool active = !this.Interactable && claimable && !claimed;
			this.claimButton.gameObject.SetActive(flag);
			if (this.claimableIndicator != null)
			{
				this.claimableIndicator.SetActive(active);
			}
			if (flag)
			{
				if (this.buttonText)
				{
					this.buttonText.text = (claimed ? this.claimedTextKey.ToPlainText() : this.claimTextKey.ToPlainText());
				}
				this.statusIcon.sprite = (claimed ? this.claimedIcon : this.claimIcon);
				this.claimButton.interactable = !claimed;
				this.statusIcon.gameObject.SetActive(!this.target.Claiming);
				this.claimingIcon.gameObject.SetActive(this.target.Claiming);
			}
		}

		// Token: 0x0400148B RID: 5259
		[SerializeField]
		private Image rewardIcon;

		// Token: 0x0400148C RID: 5260
		[SerializeField]
		private TextMeshProUGUI rewardText;

		// Token: 0x0400148D RID: 5261
		[SerializeField]
		private Button claimButton;

		// Token: 0x0400148E RID: 5262
		[SerializeField]
		private GameObject claimableIndicator;

		// Token: 0x0400148F RID: 5263
		[SerializeField]
		private Image statusIcon;

		// Token: 0x04001490 RID: 5264
		[SerializeField]
		private TextMeshProUGUI buttonText;

		// Token: 0x04001491 RID: 5265
		[SerializeField]
		private GameObject claimingIcon;

		// Token: 0x04001492 RID: 5266
		[SerializeField]
		private Sprite claimIcon;

		// Token: 0x04001493 RID: 5267
		[LocalizationKey("Default")]
		[SerializeField]
		private string claimTextKey = "UI_Quest_RewardClaim";

		// Token: 0x04001494 RID: 5268
		[SerializeField]
		private Sprite claimedIcon;

		// Token: 0x04001495 RID: 5269
		[LocalizationKey("Default")]
		[SerializeField]
		private string claimedTextKey = "UI_Quest_RewardClaimed";

		// Token: 0x04001496 RID: 5270
		[SerializeField]
		private bool interactable;

		// Token: 0x04001497 RID: 5271
		private Reward target;
	}
}
