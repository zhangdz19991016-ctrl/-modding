using System;
using Steamworks;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class SteamStatsAPITest : MonoBehaviour
{
	// Token: 0x06000E91 RID: 3729 RVA: 0x0003B129 File Offset: 0x00039329
	private void Awake()
	{
		this.onStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatReceived));
		this.onStatsStoredCallback = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatStored));
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0003B159 File Offset: 0x00039359
	private void OnUserStatStored(UserStatsStored_t param)
	{
		Debug.Log("Stat Stored!");
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0003B168 File Offset: 0x00039368
	private void OnUserStatReceived(UserStatsReceived_t param)
	{
		string str = "Stat Fetched:";
		CSteamID steamIDUser = param.m_steamIDUser;
		Debug.Log(str + steamIDUser.ToString() + " " + param.m_nGameID.ToString());
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0003B1A9 File Offset: 0x000393A9
	private void Start()
	{
		SteamUserStats.RequestGlobalStats(60);
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x0003B1B4 File Offset: 0x000393B4
	private void Test()
	{
		int num;
		Debug.Log(SteamUserStats.GetStat("game_finished", out num).ToString() + " " + num.ToString());
		bool flag = SteamUserStats.SetStat("game_finished", num + 1);
		Debug.Log(string.Format("Set: {0}", flag));
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x0003B214 File Offset: 0x00039414
	private void GetGlobalStat()
	{
		long num;
		if (SteamUserStats.GetGlobalStat("game_finished", out num))
		{
			Debug.Log(string.Format("game finished: {0}", num));
			return;
		}
		Debug.Log("Failed");
	}

	// Token: 0x04000C1C RID: 3100
	private Callback<UserStatsReceived_t> onStatsReceivedCallback;

	// Token: 0x04000C1D RID: 3101
	private Callback<UserStatsStored_t> onStatsStoredCallback;
}
