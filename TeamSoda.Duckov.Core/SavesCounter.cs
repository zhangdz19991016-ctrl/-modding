using System;
using Saves;

// Token: 0x02000127 RID: 295
public class SavesCounter
{
	// Token: 0x060009B2 RID: 2482 RVA: 0x0002A6EC File Offset: 0x000288EC
	public static int AddCount(string countKey)
	{
		int num = SavesSystem.Load<int>("Count/" + countKey);
		num++;
		SavesSystem.Save<int>("Count/" + countKey, num);
		return num;
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0002A720 File Offset: 0x00028920
	public static int GetCount(string countKey)
	{
		return SavesSystem.Load<int>("Count/" + countKey);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0002A734 File Offset: 0x00028934
	public static int AddKillCount(string key)
	{
		int num = SavesCounter.AddCount("Kills/" + key);
		Action<string, int> onKillCountChanged = SavesCounter.OnKillCountChanged;
		if (onKillCountChanged != null)
		{
			onKillCountChanged(key, num);
		}
		return num;
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0002A765 File Offset: 0x00028965
	public static int GetKillCount(string key)
	{
		return SavesCounter.GetCount("Kills/" + key);
	}

	// Token: 0x04000894 RID: 2196
	public static Action<string, int> OnKillCountChanged;
}
