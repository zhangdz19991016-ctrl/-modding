using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using Duckov.UI.Animations;
using Duckov.Utilities;
using Saves;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Rules.UI
{
	// Token: 0x020003FB RID: 1019
	public class DifficultySelection : MonoBehaviour
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x0008073C File Offset: 0x0007E93C
		private PrefabPool<DifficultySelection_Entry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<DifficultySelection_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x00080775 File Offset: 0x0007E975
		private void Awake()
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x00080793 File Offset: 0x0007E993
		private void OnConfirmButtonClicked()
		{
			this.confirmed = true;
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x0008079C File Offset: 0x0007E99C
		public UniTask Execute()
		{
			DifficultySelection.<Execute>d__15 <Execute>d__;
			<Execute>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Execute>d__.<>4__this = this;
			<Execute>d__.<>1__state = -1;
			<Execute>d__.<>t__builder.Start<DifficultySelection.<Execute>d__15>(ref <Execute>d__);
			return <Execute>d__.<>t__builder.Task;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000807E0 File Offset: 0x0007E9E0
		private bool CheckUnlocked(DifficultySelection.SettingEntry setting)
		{
			bool flag = !MultiSceneCore.GetVisited("Base");
			RuleIndex ruleIndex = setting.ruleIndex;
			if (ruleIndex <= RuleIndex.Custom)
			{
				if (ruleIndex != RuleIndex.Standard)
				{
					if (ruleIndex != RuleIndex.Custom)
					{
						return false;
					}
					return flag || GameRulesManager.SelectedRuleIndex == RuleIndex.Custom;
				}
			}
			else if (ruleIndex - RuleIndex.Easy > 2 && ruleIndex - RuleIndex.Hard > 1)
			{
				if (ruleIndex != RuleIndex.Rage)
				{
					return false;
				}
				return this.GetRageUnlocked(flag);
			}
			return flag || (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom && GameRulesManager.SelectedRuleIndex != RuleIndex.Rage);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x00080857 File Offset: 0x0007EA57
		public static void UnlockRage()
		{
			SavesSystem.SaveGlobal<bool>("Difficulty/RageUnlocked", true);
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x00080864 File Offset: 0x0007EA64
		public bool GetRageUnlocked(bool isFirstSelect)
		{
			return SavesSystem.LoadGlobal<bool>("Difficulty/RageUnlocked", false) && (isFirstSelect || (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom && GameRulesManager.SelectedRuleIndex == RuleIndex.Rage));
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x00080890 File Offset: 0x0007EA90
		private bool CheckShouldDisplay(DifficultySelection.SettingEntry setting)
		{
			return true;
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060024EC RID: 9452 RVA: 0x00080893 File Offset: 0x0007EA93
		// (set) Token: 0x060024ED RID: 9453 RVA: 0x0008089F File Offset: 0x0007EA9F
		public static bool CustomDifficultyMarker
		{
			get
			{
				return SavesSystem.Load<bool>("CustomDifficultyMarker");
			}
			set
			{
				SavesSystem.Save<bool>("CustomDifficultyMarker", value);
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060024EE RID: 9454 RVA: 0x000808AC File Offset: 0x0007EAAC
		public RuleIndex SelectedRuleIndex
		{
			get
			{
				if (this.SelectedEntry == null)
				{
					return RuleIndex.Standard;
				}
				return this.SelectedEntry.Setting.ruleIndex;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x000808CE File Offset: 0x0007EACE
		// (set) Token: 0x060024F0 RID: 9456 RVA: 0x000808D6 File Offset: 0x0007EAD6
		public DifficultySelection_Entry SelectedEntry { get; private set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000808DF File Offset: 0x0007EADF
		// (set) Token: 0x060024F2 RID: 9458 RVA: 0x000808E7 File Offset: 0x0007EAE7
		public DifficultySelection_Entry HoveringEntry { get; private set; }

		// Token: 0x060024F3 RID: 9459 RVA: 0x000808F0 File Offset: 0x0007EAF0
		private UniTask<RuleIndex> WaitForConfirmation()
		{
			DifficultySelection.<WaitForConfirmation>d__34 <WaitForConfirmation>d__;
			<WaitForConfirmation>d__.<>t__builder = AsyncUniTaskMethodBuilder<RuleIndex>.Create();
			<WaitForConfirmation>d__.<>4__this = this;
			<WaitForConfirmation>d__.<>1__state = -1;
			<WaitForConfirmation>d__.<>t__builder.Start<DifficultySelection.<WaitForConfirmation>d__34>(ref <WaitForConfirmation>d__);
			return <WaitForConfirmation>d__.<>t__builder.Task;
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00080934 File Offset: 0x0007EB34
		internal void NotifySelected(DifficultySelection_Entry entry)
		{
			this.SelectedEntry = entry;
			GameRulesManager.SelectedRuleIndex = this.SelectedRuleIndex;
			foreach (DifficultySelection_Entry difficultySelection_Entry in this.EntryPool.ActiveEntries)
			{
				if (!(difficultySelection_Entry == null))
				{
					difficultySelection_Entry.Refresh();
				}
			}
			this.RefreshDescription();
			if (this.SelectedRuleIndex == RuleIndex.Custom)
			{
				this.ShowCustomRuleSetupPanel();
			}
			bool flag = this.SelectedRuleIndex == RuleIndex.Custom;
			this.achievementDisabledIndicator.SetActive(flag || DifficultySelection.CustomDifficultyMarker);
			this.selectedCustomDifficultyBefore.SetActive(DifficultySelection.CustomDifficultyMarker);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000809E8 File Offset: 0x0007EBE8
		private void ShowCustomRuleSetupPanel()
		{
			FadeGroup fadeGroup = this.customPanel;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000809FA File Offset: 0x0007EBFA
		internal void NotifyEntryPointerEnter(DifficultySelection_Entry entry)
		{
			this.HoveringEntry = entry;
			this.RefreshDescription();
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00080A09 File Offset: 0x0007EC09
		internal void NotifyEntryPointerExit(DifficultySelection_Entry entry)
		{
			if (this.HoveringEntry == entry)
			{
				this.HoveringEntry = null;
				this.RefreshDescription();
			}
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00080A28 File Offset: 0x0007EC28
		private void RefreshDescription()
		{
			string text;
			if (this.SelectedEntry != null)
			{
				text = this.SelectedEntry.Setting.Description;
			}
			else
			{
				text = this.description_PlaceHolderKey.ToPlainText();
			}
			this.textDescription.text = text;
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00080A71 File Offset: 0x0007EC71
		internal void SkipHide()
		{
			if (this.fadeGroup != null)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x04001916 RID: 6422
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001917 RID: 6423
		[SerializeField]
		private TextMeshProUGUI textDescription;

		// Token: 0x04001918 RID: 6424
		[SerializeField]
		[LocalizationKey("Default")]
		private string description_PlaceHolderKey = "DifficultySelection_Desc_PlaceHolder";

		// Token: 0x04001919 RID: 6425
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400191A RID: 6426
		[SerializeField]
		private FadeGroup customPanel;

		// Token: 0x0400191B RID: 6427
		[SerializeField]
		private DifficultySelection_Entry entryTemplate;

		// Token: 0x0400191C RID: 6428
		[SerializeField]
		private GameObject achievementDisabledIndicator;

		// Token: 0x0400191D RID: 6429
		[SerializeField]
		private GameObject selectedCustomDifficultyBefore;

		// Token: 0x0400191E RID: 6430
		private PrefabPool<DifficultySelection_Entry> _entryPool;

		// Token: 0x0400191F RID: 6431
		[SerializeField]
		private DifficultySelection.SettingEntry[] displaySettings;

		// Token: 0x04001922 RID: 6434
		private bool confirmed;

		// Token: 0x0200065D RID: 1629
		[Serializable]
		public struct SettingEntry
		{
			// Token: 0x170007A0 RID: 1952
			// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000A1F26 File Offset: 0x000A0126
			// (set) Token: 0x06002AA0 RID: 10912 RVA: 0x000A1F3D File Offset: 0x000A013D
			[LocalizationKey("Default")]
			private string TitleKey
			{
				get
				{
					return string.Format("Rule_{0}", this.ruleIndex);
				}
				set
				{
				}
			}

			// Token: 0x170007A1 RID: 1953
			// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000A1F3F File Offset: 0x000A013F
			public string Title
			{
				get
				{
					return this.TitleKey.ToPlainText();
				}
			}

			// Token: 0x170007A2 RID: 1954
			// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x000A1F4C File Offset: 0x000A014C
			// (set) Token: 0x06002AA3 RID: 10915 RVA: 0x000A1F63 File Offset: 0x000A0163
			[LocalizationKey("Default")]
			private string DescriptionKey
			{
				get
				{
					return string.Format("Rule_{0}_Desc", this.ruleIndex);
				}
				set
				{
				}
			}

			// Token: 0x170007A3 RID: 1955
			// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x000A1F65 File Offset: 0x000A0165
			public string Description
			{
				get
				{
					return this.DescriptionKey.ToPlainText();
				}
			}

			// Token: 0x040022E5 RID: 8933
			public RuleIndex ruleIndex;

			// Token: 0x040022E6 RID: 8934
			public Sprite icon;

			// Token: 0x040022E7 RID: 8935
			public bool recommended;
		}
	}
}
