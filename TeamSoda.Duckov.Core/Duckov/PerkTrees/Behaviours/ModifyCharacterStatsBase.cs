using System;
using System.Collections.Generic;
using System.Text;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees.Behaviours
{
	// Token: 0x0200025D RID: 605
	public class ModifyCharacterStatsBase : PerkBehaviour
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x00047BF1 File Offset: 0x00045DF1
		private string DescriptionFormat
		{
			get
			{
				return "PerkBehaviour_ModifyCharacterStatsBase".ToPlainText();
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x00047C00 File Offset: 0x00045E00
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ModifyCharacterStatsBase.Entry entry in this.entries)
				{
					if (entry != null && !string.IsNullOrEmpty(entry.key))
					{
						string statDisplayName = ("Stat_" + entry.key.Trim()).ToPlainText();
						bool flag = entry.value > 0f;
						float value = entry.value;
						string str = entry.percentage ? string.Format("{0}%", value * 100f) : value.ToString();
						string value2 = (flag ? "+" : "") + str;
						string value3 = this.DescriptionFormat.Format(new
						{
							statDisplayName = statDisplayName,
							value = value2
						});
						stringBuilder.AppendLine(value3);
					}
				}
				return stringBuilder.ToString().Trim();
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00047D08 File Offset: 0x00045F08
		protected override void OnUnlocked()
		{
			LevelManager instance = LevelManager.Instance;
			Item item;
			if (instance == null)
			{
				item = null;
			}
			else
			{
				CharacterMainControl mainCharacter = instance.MainCharacter;
				item = ((mainCharacter != null) ? mainCharacter.CharacterItem : null);
			}
			this.targetItem = item;
			if (this.targetItem == null)
			{
				return;
			}
			StatCollection stats = this.targetItem.Stats;
			if (stats == null)
			{
				return;
			}
			foreach (ModifyCharacterStatsBase.Entry entry in this.entries)
			{
				Stat stat = stats.GetStat(entry.key);
				if (stat == null)
				{
					break;
				}
				stat.BaseValue += entry.value;
				this.records.Add(new ModifyCharacterStatsBase.Record
				{
					stat = stat,
					value = entry.value
				});
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00047DEC File Offset: 0x00045FEC
		protected override void OnLocked()
		{
			if (this.targetItem == null)
			{
				return;
			}
			if (this.targetItem.Stats == null)
			{
				return;
			}
			foreach (ModifyCharacterStatsBase.Record record in this.records)
			{
				if (record.stat == null)
				{
					break;
				}
				record.stat.BaseValue -= record.value;
			}
		}

		// Token: 0x04000E4F RID: 3663
		[SerializeField]
		private List<ModifyCharacterStatsBase.Entry> entries = new List<ModifyCharacterStatsBase.Entry>();

		// Token: 0x04000E50 RID: 3664
		private Item targetItem;

		// Token: 0x04000E51 RID: 3665
		private List<ModifyCharacterStatsBase.Record> records = new List<ModifyCharacterStatsBase.Record>();

		// Token: 0x02000541 RID: 1345
		[Serializable]
		public class Entry
		{
			// Token: 0x17000766 RID: 1894
			// (get) Token: 0x06002837 RID: 10295 RVA: 0x000938CB File Offset: 0x00091ACB
			private StringList AvaliableKeys
			{
				get
				{
					return StringLists.StatKeys;
				}
			}

			// Token: 0x04001ECA RID: 7882
			public string key;

			// Token: 0x04001ECB RID: 7883
			public float value;

			// Token: 0x04001ECC RID: 7884
			public bool percentage;
		}

		// Token: 0x02000542 RID: 1346
		private struct Record
		{
			// Token: 0x04001ECD RID: 7885
			public Stat stat;

			// Token: 0x04001ECE RID: 7886
			public float value;
		}
	}
}
