using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200039E RID: 926
	[CreateAssetMenu(menuName = "Duckov/Stat Info Database")]
	public class StatInfoDatabase : ScriptableObject
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060020B2 RID: 8370 RVA: 0x000727BC File Offset: 0x000709BC
		public static StatInfoDatabase Instance
		{
			get
			{
				return GameplayDataSettings.StatInfo;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x000727C3 File Offset: 0x000709C3
		private static Dictionary<string, StatInfoDatabase.Entry> Dic
		{
			get
			{
				return StatInfoDatabase.Instance._dic;
			}
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000727D0 File Offset: 0x000709D0
		public static StatInfoDatabase.Entry Get(string statName)
		{
			if (!(StatInfoDatabase.Instance == null))
			{
				if (StatInfoDatabase.Dic == null)
				{
					StatInfoDatabase.RebuildDic();
				}
				StatInfoDatabase.Entry result;
				if (StatInfoDatabase.Dic.TryGetValue(statName, out result))
				{
					return result;
				}
			}
			return new StatInfoDatabase.Entry
			{
				statName = statName,
				polarity = Polarity.Neutral,
				displayFormat = "0.##"
			};
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x0007282C File Offset: 0x00070A2C
		public static Polarity GetPolarity(string statName)
		{
			return StatInfoDatabase.Get(statName).polarity;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x0007283C File Offset: 0x00070A3C
		[ContextMenu("Rebuild Dic")]
		private static void RebuildDic()
		{
			if (StatInfoDatabase.Instance == null)
			{
				return;
			}
			StatInfoDatabase.Instance._dic = new Dictionary<string, StatInfoDatabase.Entry>();
			foreach (StatInfoDatabase.Entry entry in StatInfoDatabase.Instance.entries)
			{
				if (StatInfoDatabase.Instance._dic.ContainsKey(entry.statName))
				{
					Debug.LogError("Stat Info 中有重复的 key: " + entry.statName);
				}
				else
				{
					StatInfoDatabase.Instance._dic[entry.statName] = entry;
				}
			}
		}

		// Token: 0x04001640 RID: 5696
		[SerializeField]
		private StatInfoDatabase.Entry[] entries = new StatInfoDatabase.Entry[0];

		// Token: 0x04001641 RID: 5697
		private Dictionary<string, StatInfoDatabase.Entry> _dic;

		// Token: 0x02000627 RID: 1575
		[Serializable]
		public struct Entry
		{
			// Token: 0x1700079F RID: 1951
			// (get) Token: 0x06002A3D RID: 10813 RVA: 0x0009E102 File Offset: 0x0009C302
			public string DisplayFormat
			{
				get
				{
					if (string.IsNullOrEmpty(this.displayFormat))
					{
						return "0.##";
					}
					return this.displayFormat;
				}
			}

			// Token: 0x040021F8 RID: 8696
			public string statName;

			// Token: 0x040021F9 RID: 8697
			public Polarity polarity;

			// Token: 0x040021FA RID: 8698
			public string displayFormat;
		}
	}
}
