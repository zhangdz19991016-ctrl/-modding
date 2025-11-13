using System;
using Duckov.PerkTrees;
using Duckov.Utilities;
using LeTai.TrueShadow;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C2 RID: 962
	public class PerkEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPoolable
	{
		// Token: 0x0600230A RID: 8970 RVA: 0x0007AED7 File Offset: 0x000790D7
		private void SwitchToActiveLook()
		{
			this.ApplyLook(this.activeLook);
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x0007AEE5 File Offset: 0x000790E5
		private void SwitchToAvaliableLook()
		{
			this.ApplyLook(this.avaliableLook);
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0007AEF3 File Offset: 0x000790F3
		private void SwitchToUnavaliableLook()
		{
			this.ApplyLook(this.unavaliableLook);
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600230D RID: 8973 RVA: 0x0007AF01 File Offset: 0x00079101
		public RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x0007AF23 File Offset: 0x00079123
		public Perk Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x0007AF2C File Offset: 0x0007912C
		public void Setup(PerkTreeView master, Perk target)
		{
			this.UnregisterEvents();
			this.master = master;
			this.target = target;
			this.icon.sprite = target.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(target.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.displayNameText.text = target.DisplayName;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0007AFCC File Offset: 0x000791CC
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			bool unlocked = this.target.Unlocked;
			bool flag = this.target.AreAllParentsUnlocked();
			if (unlocked)
			{
				this.SwitchToActiveLook();
			}
			else if (flag)
			{
				this.SwitchToAvaliableLook();
			}
			else
			{
				this.SwitchToUnavaliableLook();
			}
			bool unlocking = this.target.Unlocking;
			bool flag2 = this.target.GetRemainingTime() <= TimeSpan.Zero;
			this.avaliableForResearchIndicator.SetActive(!unlocked && !unlocking && this.target.AreAllParentsUnlocked() && this.target.Requirement.AreSatisfied());
			this.inProgressIndicator.SetActive(!unlocked && unlocking && !flag2);
			this.timeUpIndicator.SetActive(!unlocked && unlocking && flag2);
			if (this.master == null)
			{
				return;
			}
			this.selectionIndicator.SetActive(this.master.GetSelection() == this);
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x0007B0C7 File Offset: 0x000792C7
		private void OnMasterSelectionChanged(PerkEntry entry)
		{
			this.Refresh();
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x0007B0D0 File Offset: 0x000792D0
		private void RegisterEvents()
		{
			if (this.master)
			{
				this.master.onSelectionChanged += this.OnMasterSelectionChanged;
			}
			if (this.target)
			{
				this.target.onUnlockStateChanged += this.OnTargetStateChanged;
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0007B125 File Offset: 0x00079325
		private void OnTargetStateChanged(Perk perk, bool state)
		{
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			this.Refresh();
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0007B140 File Offset: 0x00079340
		private void UnregisterEvents()
		{
			if (this.master)
			{
				this.master.onSelectionChanged -= this.OnMasterSelectionChanged;
			}
			if (this.target)
			{
				this.target.onUnlockStateChanged -= this.OnTargetStateChanged;
			}
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x0007B195 File Offset: 0x00079395
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0007B19D File Offset: 0x0007939D
		public void NotifyPooled()
		{
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0007B19F File Offset: 0x0007939F
		public void NotifyReleased()
		{
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0007B1A1 File Offset: 0x000793A1
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.master == null)
			{
				return;
			}
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			this.master.SetSelection(this);
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0007B1D0 File Offset: 0x000793D0
		internal Vector2 GetLayoutPosition()
		{
			if (this.target == null)
			{
				return Vector2.zero;
			}
			return this.target.GetLayoutPosition();
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0007B1F4 File Offset: 0x000793F4
		private void ApplyLook(PerkEntry.Look look)
		{
			this.icon.material = look.material;
			this.icon.color = look.iconColor;
			this.frame.color = look.frameColor;
			this.frameGlow.enabled = (look.frameGlowColor.a > 0f);
			this.frameGlow.Color = look.frameGlowColor;
			this.background.color = look.backgroundColor;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x0007B273 File Offset: 0x00079473
		private void FixedUpdate()
		{
			if (this.inProgressIndicator.activeSelf && this.target.GetRemainingTime() <= TimeSpan.Zero)
			{
				this.Refresh();
			}
		}

		// Token: 0x040017D0 RID: 6096
		[SerializeField]
		private Image icon;

		// Token: 0x040017D1 RID: 6097
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x040017D2 RID: 6098
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x040017D3 RID: 6099
		[SerializeField]
		private Image frame;

		// Token: 0x040017D4 RID: 6100
		[SerializeField]
		private TrueShadow frameGlow;

		// Token: 0x040017D5 RID: 6101
		[SerializeField]
		private Image background;

		// Token: 0x040017D6 RID: 6102
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x040017D7 RID: 6103
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x040017D8 RID: 6104
		[SerializeField]
		private GameObject inProgressIndicator;

		// Token: 0x040017D9 RID: 6105
		[SerializeField]
		private GameObject timeUpIndicator;

		// Token: 0x040017DA RID: 6106
		[SerializeField]
		private GameObject avaliableForResearchIndicator;

		// Token: 0x040017DB RID: 6107
		[SerializeField]
		private PerkEntry.Look activeLook;

		// Token: 0x040017DC RID: 6108
		[SerializeField]
		private PerkEntry.Look avaliableLook;

		// Token: 0x040017DD RID: 6109
		[SerializeField]
		private PerkEntry.Look unavaliableLook;

		// Token: 0x040017DE RID: 6110
		private RectTransform _rectTransform;

		// Token: 0x040017DF RID: 6111
		private PerkTreeView master;

		// Token: 0x040017E0 RID: 6112
		private Perk target;

		// Token: 0x0200063A RID: 1594
		[Serializable]
		public struct Look
		{
			// Token: 0x04002244 RID: 8772
			public Color iconColor;

			// Token: 0x04002245 RID: 8773
			public Material material;

			// Token: 0x04002246 RID: 8774
			public Color frameColor;

			// Token: 0x04002247 RID: 8775
			public Color frameGlowColor;

			// Token: 0x04002248 RID: 8776
			public Color backgroundColor;
		}
	}
}
