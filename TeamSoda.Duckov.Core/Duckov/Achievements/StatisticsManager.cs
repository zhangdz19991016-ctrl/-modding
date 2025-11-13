using System;
using Saves;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x02000328 RID: 808
	public class StatisticsManager : MonoBehaviour
	{
		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06001B12 RID: 6930 RVA: 0x00062568 File Offset: 0x00060768
		// (remove) Token: 0x06001B13 RID: 6931 RVA: 0x0006259C File Offset: 0x0006079C
		public static event Action<string, long, long> OnStatisticsChanged;

		// Token: 0x06001B14 RID: 6932 RVA: 0x000625CF File Offset: 0x000607CF
		private static string GetSaveKey(string statisticsKey)
		{
			return "Statistics/" + statisticsKey;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000625DC File Offset: 0x000607DC
		private static long Get(string key)
		{
			StatisticsManager.GetSaveKey(key);
			if (!SavesSystem.KeyExisits(key))
			{
				return 0L;
			}
			return SavesSystem.Load<long>(key);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000625F8 File Offset: 0x000607F8
		private static void Set(string key, long value)
		{
			long arg = StatisticsManager.Get(key);
			StatisticsManager.GetSaveKey(key);
			SavesSystem.Save<long>(key, value);
			Action<string, long, long> onStatisticsChanged = StatisticsManager.OnStatisticsChanged;
			if (onStatisticsChanged == null)
			{
				return;
			}
			onStatisticsChanged(key, arg, value);
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x0006262C File Offset: 0x0006082C
		public static void Add(string key, long value = 1L)
		{
			long num = StatisticsManager.Get(key);
			checked
			{
				try
				{
					num += value;
				}
				catch (OverflowException exception)
				{
					Debug.LogException(exception);
					Debug.Log("Failed changing statistics of " + key + ". Overflow detected.");
					return;
				}
				StatisticsManager.Set(key, num);
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0006267C File Offset: 0x0006087C
		private void Awake()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00062684 File Offset: 0x00060884
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0006268C File Offset: 0x0006088C
		private void RegisterEvents()
		{
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0006268E File Offset: 0x0006088E
		private void UnregisterEvents()
		{
		}
	}
}
