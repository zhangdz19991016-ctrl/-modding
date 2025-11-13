using System;
using Duckov.PerkTrees;
using Duckov.UI.Animations;
using Duckov.Utilities;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C1 RID: 961
	public class PerkDetails : MonoBehaviour
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x0007AA17 File Offset: 0x00078C17
		[SerializeField]
		private string RequireLevelFormatKey
		{
			get
			{
				return "UI_Perk_RequireLevel";
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x0007AA1E File Offset: 0x00078C1E
		[SerializeField]
		private string RequireLevelFormat
		{
			get
			{
				return this.RequireLevelFormatKey.ToPlainText();
			}
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0007AA2B File Offset: 0x00078C2B
		private void Awake()
		{
			this.beginButton.onClick.AddListener(new UnityAction(this.OnBeginButtonClicked));
			this.activateButton.onClick.AddListener(new UnityAction(this.OnActivateButtonClicked));
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0007AA65 File Offset: 0x00078C65
		private void OnActivateButtonClicked()
		{
			this.showingPerk.ConfirmUnlock();
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0007AA73 File Offset: 0x00078C73
		private void OnBeginButtonClicked()
		{
			this.showingPerk.SubmitItemsAndBeginUnlocking();
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x0007AA81 File Offset: 0x00078C81
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x0007AA89 File Offset: 0x00078C89
		public void Setup(Perk perk, bool editable = false)
		{
			this.UnregisterEvents();
			this.showingPerk = perk;
			this.editable = editable;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0007AAAB File Offset: 0x00078CAB
		private void RegisterEvents()
		{
			if (this.showingPerk)
			{
				this.showingPerk.onUnlockStateChanged += this.OnTargetStateChanged;
			}
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0007AAD1 File Offset: 0x00078CD1
		private void OnTargetStateChanged(Perk perk, bool arg2)
		{
			this.Refresh();
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0007AAD9 File Offset: 0x00078CD9
		private void UnregisterEvents()
		{
			if (this.showingPerk)
			{
				this.showingPerk.onUnlockStateChanged -= this.OnTargetStateChanged;
			}
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0007AB00 File Offset: 0x00078D00
		private void Refresh()
		{
			if (this.showingPerk == null)
			{
				this.content.Hide();
				this.placeHolder.Show();
				return;
			}
			this.text_Name.text = this.showingPerk.DisplayName;
			this.text_Description.text = this.showingPerk.Description;
			this.icon.sprite = this.showingPerk.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(this.showingPerk.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			bool flag = !this.showingPerk.Unlocked && this.editable;
			bool flag2 = this.showingPerk.AreAllParentsUnlocked();
			bool flag3 = false;
			if (flag2)
			{
				flag3 = this.showingPerk.Requirement.AreSatisfied();
			}
			this.activateButton.gameObject.SetActive(false);
			this.beginButton.gameObject.SetActive(false);
			this.buttonUnavaliablePlaceHolder.SetActive(false);
			this.buttonUnsatisfiedPlaceHolder.SetActive(false);
			this.inProgressPlaceHolder.SetActive(false);
			this.unlockedIndicator.SetActive(this.showingPerk.Unlocked);
			if (!this.showingPerk.Unlocked)
			{
				if (this.showingPerk.Unlocking)
				{
					if (this.showingPerk.GetRemainingTime() <= TimeSpan.Zero)
					{
						this.activateButton.gameObject.SetActive(true);
					}
					else
					{
						this.inProgressPlaceHolder.SetActive(true);
					}
				}
				else if (flag2)
				{
					if (flag3)
					{
						this.beginButton.gameObject.SetActive(true);
					}
					else
					{
						this.buttonUnsatisfiedPlaceHolder.SetActive(true);
					}
				}
				else
				{
					this.buttonUnavaliablePlaceHolder.SetActive(true);
				}
			}
			if (flag)
			{
				this.SetupActivationInfo();
			}
			this.activationInfoParent.SetActive(flag);
			this.content.Show();
			this.placeHolder.Hide();
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0007AD10 File Offset: 0x00078F10
		private void SetupActivationInfo()
		{
			if (!this.showingPerk)
			{
				return;
			}
			int level = this.showingPerk.Requirement.level;
			if (level > 0)
			{
				bool flag = EXPManager.Level >= level;
				string text = "#" + (flag ? this.normalTextColor.ToHexString() : this.unsatisfiedTextColor.ToHexString());
				this.text_RequireLevel.gameObject.SetActive(true);
				int level2 = this.showingPerk.Requirement.level;
				string color = text;
				this.text_RequireLevel.text = this.RequireLevelFormat.Format(new
				{
					level = level2,
					color = color
				});
			}
			else
			{
				this.text_RequireLevel.gameObject.SetActive(false);
			}
			this.costDisplay.Setup(this.showingPerk.Requirement.cost, 1);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0007ADE0 File Offset: 0x00078FE0
		private void Update()
		{
			if (this.showingPerk && this.showingPerk.Unlocking && this.inProgressPlaceHolder.activeSelf)
			{
				this.UpdateCountDown();
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0007AE10 File Offset: 0x00079010
		private void UpdateCountDown()
		{
			TimeSpan remainingTime = this.showingPerk.GetRemainingTime();
			if (remainingTime <= TimeSpan.Zero)
			{
				this.Refresh();
				return;
			}
			this.progressFillImage.fillAmount = this.showingPerk.GetProgress01();
			this.countDownText.text = string.Format("{0} {1:00}:{2:00}:{3:00}.{4:000}", new object[]
			{
				remainingTime.Days,
				remainingTime.Hours,
				remainingTime.Minutes,
				remainingTime.Seconds,
				remainingTime.Milliseconds
			});
		}

		// Token: 0x040017BB RID: 6075
		[SerializeField]
		private FadeGroup content;

		// Token: 0x040017BC RID: 6076
		[SerializeField]
		private FadeGroup placeHolder;

		// Token: 0x040017BD RID: 6077
		[SerializeField]
		private TextMeshProUGUI text_Name;

		// Token: 0x040017BE RID: 6078
		[SerializeField]
		private TextMeshProUGUI text_Description;

		// Token: 0x040017BF RID: 6079
		[SerializeField]
		private Image icon;

		// Token: 0x040017C0 RID: 6080
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x040017C1 RID: 6081
		[SerializeField]
		private GameObject unlockedIndicator;

		// Token: 0x040017C2 RID: 6082
		[SerializeField]
		private GameObject activationInfoParent;

		// Token: 0x040017C3 RID: 6083
		[SerializeField]
		private TextMeshProUGUI text_RequireLevel;

		// Token: 0x040017C4 RID: 6084
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x040017C5 RID: 6085
		[SerializeField]
		private Color normalTextColor = Color.white;

		// Token: 0x040017C6 RID: 6086
		[SerializeField]
		private Color unsatisfiedTextColor = Color.red;

		// Token: 0x040017C7 RID: 6087
		[SerializeField]
		private Button activateButton;

		// Token: 0x040017C8 RID: 6088
		[SerializeField]
		private Button beginButton;

		// Token: 0x040017C9 RID: 6089
		[SerializeField]
		private GameObject buttonUnsatisfiedPlaceHolder;

		// Token: 0x040017CA RID: 6090
		[SerializeField]
		private GameObject buttonUnavaliablePlaceHolder;

		// Token: 0x040017CB RID: 6091
		[SerializeField]
		private GameObject inProgressPlaceHolder;

		// Token: 0x040017CC RID: 6092
		[SerializeField]
		private Image progressFillImage;

		// Token: 0x040017CD RID: 6093
		[SerializeField]
		private TextMeshProUGUI countDownText;

		// Token: 0x040017CE RID: 6094
		private Perk showingPerk;

		// Token: 0x040017CF RID: 6095
		private bool editable;
	}
}
