using System;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Rules
{
	// Token: 0x020003F7 RID: 1015
	public class GameRulesManager : MonoBehaviour
	{
		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060024C2 RID: 9410 RVA: 0x0008040C File Offset: 0x0007E60C
		public static GameRulesManager Instance
		{
			get
			{
				return GameManager.DifficultyManager;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x00080413 File Offset: 0x0007E613
		public static Ruleset Current
		{
			get
			{
				return GameRulesManager.Instance.mCurrent;
			}
		}

		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x060024C4 RID: 9412 RVA: 0x00080420 File Offset: 0x0007E620
		// (remove) Token: 0x060024C5 RID: 9413 RVA: 0x00080454 File Offset: 0x0007E654
		public static event Action OnRuleChanged;

		// Token: 0x060024C6 RID: 9414 RVA: 0x00080487 File Offset: 0x0007E687
		public static void NotifyRuleChanged()
		{
			Action onRuleChanged = GameRulesManager.OnRuleChanged;
			if (onRuleChanged == null)
			{
				return;
			}
			onRuleChanged();
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x00080498 File Offset: 0x0007E698
		private Ruleset mCurrent
		{
			get
			{
				if (GameRulesManager.SelectedRuleIndex == RuleIndex.Custom)
				{
					return this.CustomRuleSet;
				}
				foreach (GameRulesManager.RuleIndexFileEntry ruleIndexFileEntry in this.entries)
				{
					if (ruleIndexFileEntry.index == GameRulesManager.SelectedRuleIndex)
					{
						return ruleIndexFileEntry.file.Data;
					}
				}
				return this.entries[0].file.Data;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060024C8 RID: 9416 RVA: 0x00080500 File Offset: 0x0007E700
		// (set) Token: 0x060024C9 RID: 9417 RVA: 0x0008051A File Offset: 0x0007E71A
		public static RuleIndex SelectedRuleIndex
		{
			get
			{
				if (SavesSystem.KeyExisits("GameRulesManager_RuleIndex"))
				{
					return SavesSystem.Load<RuleIndex>("GameRulesManager_RuleIndex");
				}
				return RuleIndex.Standard;
			}
			internal set
			{
				SavesSystem.Save<RuleIndex>("GameRulesManager_RuleIndex", value);
				GameRulesManager.NotifyRuleChanged();
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0008052C File Offset: 0x0007E72C
		public static RuleIndex GetRuleIndexOfSaveSlot(int slot)
		{
			return SavesSystem.Load<RuleIndex>("GameRulesManager_RuleIndex", slot);
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x00080539 File Offset: 0x0007E739
		private Ruleset CustomRuleSet
		{
			get
			{
				if (this.customRuleSet == null)
				{
					this.ReloadCustomRuleSet();
				}
				return this.customRuleSet;
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0008054F File Offset: 0x0007E74F
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.OnCollectSaveData;
			SavesSystem.OnSetFile += this.OnSetFile;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00080573 File Offset: 0x0007E773
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.OnCollectSaveData;
			SavesSystem.OnSetFile -= this.OnSetFile;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00080597 File Offset: 0x0007E797
		private void OnSetFile()
		{
			this.ReloadCustomRuleSet();
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000805A0 File Offset: 0x0007E7A0
		private void ReloadCustomRuleSet()
		{
			if (SavesSystem.KeyExisits("Rule_Custom"))
			{
				this.customRuleSet = SavesSystem.Load<Ruleset>("Rule_Custom");
			}
			if (this.customRuleSet == null)
			{
				this.customRuleSet = new Ruleset();
				this.customRuleSet.displayNameKey = "Rule_Custom";
			}
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000805EC File Offset: 0x0007E7EC
		private void OnCollectSaveData()
		{
			if (GameRulesManager.SelectedRuleIndex == RuleIndex.Custom && this.customRuleSet != null)
			{
				SavesSystem.Save<Ruleset>("Rule_Custom", this.customRuleSet);
			}
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x00080610 File Offset: 0x0007E810
		internal static string GetRuleIndexDisplayNameOfSlot(int slotIndex)
		{
			RuleIndex ruleIndexOfSaveSlot = GameRulesManager.GetRuleIndexOfSaveSlot(slotIndex);
			return string.Format("Rule_{0}", ruleIndexOfSaveSlot).ToPlainText();
		}

		// Token: 0x040018FD RID: 6397
		private const string SelectedRuleIndexSaveKey = "GameRulesManager_RuleIndex";

		// Token: 0x040018FE RID: 6398
		private Ruleset customRuleSet;

		// Token: 0x040018FF RID: 6399
		private const string CustomRuleSetKey = "Rule_Custom";

		// Token: 0x04001900 RID: 6400
		[SerializeField]
		private GameRulesManager.RuleIndexFileEntry[] entries;

		// Token: 0x0200065C RID: 1628
		[Serializable]
		private struct RuleIndexFileEntry
		{
			// Token: 0x040022E3 RID: 8931
			public RuleIndex index;

			// Token: 0x040022E4 RID: 8932
			public RulesetFile file;
		}
	}
}
