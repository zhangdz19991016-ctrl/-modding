using System;
using System.Collections.Generic;
using System.IO;
using Duckov.Utilities;
using MiniExcelLibs;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000326 RID: 806
	[CreateAssetMenu]
	public class AchievementDatabase : ScriptableObject
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x00061D42 File Offset: 0x0005FF42
		public static AchievementDatabase Instance
		{
			get
			{
				return GameplayDataSettings.AchievementDatabase;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00061D49 File Offset: 0x0005FF49
		private Dictionary<string, AchievementDatabase.Achievement> dic
		{
			get
			{
				if (this._dic == null)
				{
					this.RebuildDictionary();
				}
				return this._dic;
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00061D60 File Offset: 0x0005FF60
		private void RebuildDictionary()
		{
			if (this._dic == null)
			{
				this._dic = new Dictionary<string, AchievementDatabase.Achievement>();
			}
			this._dic.Clear();
			if (this.achievementChart == null)
			{
				Debug.LogError("Achievement Chart is not assinged", this);
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(this.achievementChart.bytes))
			{
				foreach (AchievementDatabase.Achievement achievement in memoryStream.Query(null, ExcelType.UNKNOWN, "A1", null))
				{
					this._dic[achievement.id.Trim()] = achievement;
				}
			}
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00061E24 File Offset: 0x00060024
		public static bool TryGetAchievementData(string id, out AchievementDatabase.Achievement achievement)
		{
			achievement = null;
			return !(AchievementDatabase.Instance == null) && AchievementDatabase.Instance.dic.TryGetValue(id, out achievement);
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x00061E49 File Offset: 0x00060049
		internal bool IsIDValid(string id)
		{
			return this.dic.ContainsKey(id);
		}

		// Token: 0x04001345 RID: 4933
		[SerializeField]
		private XlsxObject achievementChart;

		// Token: 0x04001346 RID: 4934
		private Dictionary<string, AchievementDatabase.Achievement> _dic;

		// Token: 0x020005CB RID: 1483
		[Serializable]
		public class Achievement
		{
			// Token: 0x1700078D RID: 1933
			// (get) Token: 0x06002964 RID: 10596 RVA: 0x000998CA File Offset: 0x00097ACA
			// (set) Token: 0x06002965 RID: 10597 RVA: 0x000998D2 File Offset: 0x00097AD2
			public string id { get; set; }

			// Token: 0x1700078E RID: 1934
			// (get) Token: 0x06002966 RID: 10598 RVA: 0x000998DB File Offset: 0x00097ADB
			// (set) Token: 0x06002967 RID: 10599 RVA: 0x000998E3 File Offset: 0x00097AE3
			public string overrideDisplayNameKey { get; set; }

			// Token: 0x1700078F RID: 1935
			// (get) Token: 0x06002968 RID: 10600 RVA: 0x000998EC File Offset: 0x00097AEC
			// (set) Token: 0x06002969 RID: 10601 RVA: 0x000998F4 File Offset: 0x00097AF4
			public string overrideDescriptionKey { get; set; }

			// Token: 0x17000790 RID: 1936
			// (get) Token: 0x0600296A RID: 10602 RVA: 0x000998FD File Offset: 0x00097AFD
			// (set) Token: 0x0600296B RID: 10603 RVA: 0x00099923 File Offset: 0x00097B23
			[LocalizationKey("Default")]
			private string DisplayNameKey
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.overrideDisplayNameKey))
					{
						return this.overrideDisplayNameKey;
					}
					return "Achievement_" + this.id;
				}
				set
				{
				}
			}

			// Token: 0x17000791 RID: 1937
			// (get) Token: 0x0600296C RID: 10604 RVA: 0x00099925 File Offset: 0x00097B25
			// (set) Token: 0x0600296D RID: 10605 RVA: 0x00099950 File Offset: 0x00097B50
			[LocalizationKey("Default")]
			public string DescriptionKey
			{
				get
				{
					if (!string.IsNullOrWhiteSpace(this.overrideDescriptionKey))
					{
						return this.overrideDescriptionKey;
					}
					return "Achievement_" + this.id + "_Desc";
				}
				set
				{
				}
			}

			// Token: 0x17000792 RID: 1938
			// (get) Token: 0x0600296E RID: 10606 RVA: 0x00099952 File Offset: 0x00097B52
			public string DisplayName
			{
				get
				{
					return this.DisplayNameKey.ToPlainText();
				}
			}

			// Token: 0x17000793 RID: 1939
			// (get) Token: 0x0600296F RID: 10607 RVA: 0x0009995F File Offset: 0x00097B5F
			public string Description
			{
				get
				{
					return this.DescriptionKey.ToPlainText();
				}
			}
		}
	}
}
