using System;
using Duckov.Economy;
using Duckov.Quests;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Buildings
{
	// Token: 0x02000319 RID: 793
	[Serializable]
	public struct BuildingInfo
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x0005F72F File Offset: 0x0005D92F
		public bool Valid
		{
			get
			{
				return !string.IsNullOrEmpty(this.id);
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x0005F73F File Offset: 0x0005D93F
		public Building Prefab
		{
			get
			{
				return BuildingDataCollection.GetPrefab(this.prefabName);
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x0005F74C File Offset: 0x0005D94C
		public Vector2Int Dimensions
		{
			get
			{
				if (!this.Prefab)
				{
					return default(Vector2Int);
				}
				return this.Prefab.Dimensions;
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001A54 RID: 6740 RVA: 0x0005F77B File Offset: 0x0005D97B
		[LocalizationKey("Default")]
		public string DisplayNameKey
		{
			get
			{
				return "Building_" + this.id;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x0005F78D File Offset: 0x0005D98D
		public string DisplayName
		{
			get
			{
				return this.DisplayNameKey.ToPlainText();
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x0005F79A File Offset: 0x0005D99A
		public static string GetDisplayName(string id)
		{
			return ("Building_" + id).ToPlainText();
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x0005F7AC File Offset: 0x0005D9AC
		internal bool RequirementsSatisfied()
		{
			string[] array = this.requireBuildings;
			for (int i = 0; i < array.Length; i++)
			{
				if (!BuildingManager.Any(array[i], false))
				{
					return false;
				}
			}
			return QuestManager.AreQuestFinished(this.requireQuests);
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0005F7EB File Offset: 0x0005D9EB
		[LocalizationKey("Default")]
		public string DescriptionKey
		{
			get
			{
				return "Building_" + this.id + "_Desc";
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x0005F802 File Offset: 0x0005DA02
		public string Description
		{
			get
			{
				return this.DescriptionKey.ToPlainText();
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0005F80F File Offset: 0x0005DA0F
		public int CurrentAmount
		{
			get
			{
				if (BuildingManager.Instance == null)
				{
					return 0;
				}
				return BuildingManager.GetBuildingAmount(this.id);
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0005F82B File Offset: 0x0005DA2B
		public bool ReachedAmountLimit
		{
			get
			{
				return this.maxAmount > 0 && this.CurrentAmount >= this.maxAmount;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x0005F849 File Offset: 0x0005DA49
		public int TokenAmount
		{
			get
			{
				if (BuildingManager.Instance == null)
				{
					return 0;
				}
				return BuildingManager.Instance.GetTokenAmount(this.id);
			}
		}

		// Token: 0x040012E5 RID: 4837
		public string id;

		// Token: 0x040012E6 RID: 4838
		public string prefabName;

		// Token: 0x040012E7 RID: 4839
		public int maxAmount;

		// Token: 0x040012E8 RID: 4840
		public Cost cost;

		// Token: 0x040012E9 RID: 4841
		public string[] requireBuildings;

		// Token: 0x040012EA RID: 4842
		public string[] alternativeFor;

		// Token: 0x040012EB RID: 4843
		public int[] requireQuests;

		// Token: 0x040012EC RID: 4844
		public Sprite iconReference;
	}
}
