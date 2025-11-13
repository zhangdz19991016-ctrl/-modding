using System;
using System.Text;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.Endowment
{
	// Token: 0x020002F8 RID: 760
	public class EndowmentEntry : MonoBehaviour
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060018CE RID: 6350 RVA: 0x0005AFDB File Offset: 0x000591DB
		public EndowmentIndex Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x0005AFE3 File Offset: 0x000591E3
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x0005AFFA File Offset: 0x000591FA
		[LocalizationKey("Default")]
		private string displayNameKey
		{
			get
			{
				return string.Format("Endowmment_{0}", this.index);
			}
			set
			{
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x0005AFFC File Offset: 0x000591FC
		// (set) Token: 0x060018D2 RID: 6354 RVA: 0x0005B013 File Offset: 0x00059213
		[LocalizationKey("Default")]
		private string descriptionKey
		{
			get
			{
				return string.Format("Endowmment_{0}_Desc", this.index);
			}
			set
			{
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060018D3 RID: 6355 RVA: 0x0005B015 File Offset: 0x00059215
		public string RequirementText
		{
			get
			{
				return this.requirementTextKey.ToPlainText();
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060018D4 RID: 6356 RVA: 0x0005B022 File Offset: 0x00059222
		public Sprite Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060018D5 RID: 6357 RVA: 0x0005B02A File Offset: 0x0005922A
		public string DisplayName
		{
			get
			{
				return this.displayNameKey.ToPlainText();
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060018D6 RID: 6358 RVA: 0x0005B037 File Offset: 0x00059237
		public string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060018D7 RID: 6359 RVA: 0x0005B044 File Offset: 0x00059244
		public string DescriptionAndEffects
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				string description = this.Description;
				stringBuilder.AppendLine(description);
				foreach (EndowmentEntry.ModifierDescription modifierDescription in this.Modifiers)
				{
					stringBuilder.AppendLine("- " + modifierDescription.DescriptionText);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x0005B0A2 File Offset: 0x000592A2
		public EndowmentEntry.ModifierDescription[] Modifiers
		{
			get
			{
				return this.modifiers;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x0005B0AA File Offset: 0x000592AA
		private Item CharacterItem
		{
			get
			{
				if (CharacterMainControl.Main == null)
				{
					return null;
				}
				return CharacterMainControl.Main.CharacterItem;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x0005B0C5 File Offset: 0x000592C5
		public bool UnlockedByDefault
		{
			get
			{
				return this.unlockedByDefault;
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0005B0CD File Offset: 0x000592CD
		public void Activate()
		{
			this.ApplyModifiers();
			UnityEvent<EndowmentEntry> unityEvent = this.onActivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0005B0E6 File Offset: 0x000592E6
		public void Deactivate()
		{
			this.DeleteModifiers();
			UnityEvent<EndowmentEntry> unityEvent = this.onDeactivate;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x0005B100 File Offset: 0x00059300
		private void ApplyModifiers()
		{
			if (this.CharacterItem == null)
			{
				return;
			}
			this.DeleteModifiers();
			foreach (EndowmentEntry.ModifierDescription modifierDescription in this.modifiers)
			{
				this.CharacterItem.AddModifier(modifierDescription.statKey, new Modifier(modifierDescription.type, modifierDescription.value, this));
			}
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0005B163 File Offset: 0x00059363
		private void DeleteModifiers()
		{
			if (this.CharacterItem == null)
			{
				return;
			}
			this.CharacterItem.RemoveAllModifiersFrom(this);
		}

		// Token: 0x0400120C RID: 4620
		[SerializeField]
		private EndowmentIndex index;

		// Token: 0x0400120D RID: 4621
		[SerializeField]
		private Sprite icon;

		// Token: 0x0400120E RID: 4622
		[SerializeField]
		[LocalizationKey("Default")]
		private string requirementTextKey;

		// Token: 0x0400120F RID: 4623
		[SerializeField]
		private bool unlockedByDefault;

		// Token: 0x04001210 RID: 4624
		[SerializeField]
		private EndowmentEntry.ModifierDescription[] modifiers;

		// Token: 0x04001211 RID: 4625
		public UnityEvent<EndowmentEntry> onActivate;

		// Token: 0x04001212 RID: 4626
		public UnityEvent<EndowmentEntry> onDeactivate;

		// Token: 0x02000592 RID: 1426
		[Serializable]
		public struct ModifierDescription
		{
			// Token: 0x17000777 RID: 1911
			// (get) Token: 0x060028DF RID: 10463 RVA: 0x00097184 File Offset: 0x00095384
			// (set) Token: 0x060028E0 RID: 10464 RVA: 0x00097196 File Offset: 0x00095396
			[LocalizationKey("Default")]
			private string DisplayNameKey
			{
				get
				{
					return "Stat_" + this.statKey;
				}
				set
				{
				}
			}

			// Token: 0x17000778 RID: 1912
			// (get) Token: 0x060028E1 RID: 10465 RVA: 0x00097198 File Offset: 0x00095398
			public string DescriptionText
			{
				get
				{
					string str = this.DisplayNameKey.ToPlainText();
					string str2 = "";
					ModifierType modifierType = this.type;
					if (modifierType != ModifierType.Add)
					{
						if (modifierType != ModifierType.PercentageAdd)
						{
							if (modifierType == ModifierType.PercentageMultiply)
							{
								str2 = string.Format("x{0:00.#}%", (1f + this.value) * 100f);
							}
						}
						else if (this.value >= 0f)
						{
							str2 = string.Format("+{0:00.#}%", this.value * 100f);
						}
						else
						{
							str2 = string.Format("-{0:00.#}%", -this.value * 100f);
						}
					}
					else if (this.value >= 0f)
					{
						str2 = string.Format("+{0}", this.value);
					}
					else
					{
						str2 = string.Format("{0}", this.value);
					}
					return str + " " + str2;
				}
			}

			// Token: 0x0400200C RID: 8204
			public string statKey;

			// Token: 0x0400200D RID: 8205
			public ModifierType type;

			// Token: 0x0400200E RID: 8206
			public float value;
		}
	}
}
