using System;
using Duckov;
using Saves;
using UnityEngine;

// Token: 0x02000175 RID: 373
public static class RaidUtilities
{
	// Token: 0x14000061 RID: 97
	// (add) Token: 0x06000B6C RID: 2924 RVA: 0x00030CC8 File Offset: 0x0002EEC8
	// (remove) Token: 0x06000B6D RID: 2925 RVA: 0x00030CFC File Offset: 0x0002EEFC
	public static event Action<RaidUtilities.RaidInfo> OnNewRaid;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06000B6E RID: 2926 RVA: 0x00030D30 File Offset: 0x0002EF30
	// (remove) Token: 0x06000B6F RID: 2927 RVA: 0x00030D64 File Offset: 0x0002EF64
	public static event Action<RaidUtilities.RaidInfo> OnRaidDead;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06000B70 RID: 2928 RVA: 0x00030D98 File Offset: 0x0002EF98
	// (remove) Token: 0x06000B71 RID: 2929 RVA: 0x00030DCC File Offset: 0x0002EFCC
	public static event Action<RaidUtilities.RaidInfo> OnRaidEnd;

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00030E00 File Offset: 0x0002F000
	// (set) Token: 0x06000B73 RID: 2931 RVA: 0x00030E4C File Offset: 0x0002F04C
	public static RaidUtilities.RaidInfo CurrentRaid
	{
		get
		{
			RaidUtilities.RaidInfo raidInfo = SavesSystem.Load<RaidUtilities.RaidInfo>("RaidInfo");
			raidInfo.totalTime = Time.unscaledTime - raidInfo.raidBeginTime;
			raidInfo.expOnEnd = EXPManager.EXP;
			raidInfo.expGained = raidInfo.expOnEnd - raidInfo.expOnBegan;
			return raidInfo;
		}
		private set
		{
			SavesSystem.Save<RaidUtilities.RaidInfo>("RaidInfo", value);
		}
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x00030E5C File Offset: 0x0002F05C
	public static void NewRaid()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		RaidUtilities.RaidInfo raidInfo = new RaidUtilities.RaidInfo
		{
			valid = true,
			ID = currentRaid.ID + 1U,
			dead = false,
			ended = false,
			raidBeginTime = Time.unscaledTime,
			raidEndTime = 0f,
			expOnBegan = EXPManager.EXP
		};
		RaidUtilities.CurrentRaid = raidInfo;
		Action<RaidUtilities.RaidInfo> onNewRaid = RaidUtilities.OnNewRaid;
		if (onNewRaid == null)
		{
			return;
		}
		onNewRaid(raidInfo);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00030EDC File Offset: 0x0002F0DC
	public static void NotifyDead()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		currentRaid.dead = true;
		currentRaid.ended = true;
		currentRaid.raidEndTime = Time.unscaledTime;
		currentRaid.totalTime = currentRaid.raidEndTime - currentRaid.raidBeginTime;
		currentRaid.expOnEnd = EXPManager.EXP;
		currentRaid.expGained = currentRaid.expOnEnd - currentRaid.expOnBegan;
		RaidUtilities.CurrentRaid = currentRaid;
		Action<RaidUtilities.RaidInfo> onRaidEnd = RaidUtilities.OnRaidEnd;
		if (onRaidEnd != null)
		{
			onRaidEnd(currentRaid);
		}
		Action<RaidUtilities.RaidInfo> onRaidDead = RaidUtilities.OnRaidDead;
		if (onRaidDead == null)
		{
			return;
		}
		onRaidDead(currentRaid);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x00030F68 File Offset: 0x0002F168
	public static void NotifyEnd()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		currentRaid.ended = true;
		currentRaid.raidEndTime = Time.unscaledTime;
		currentRaid.totalTime = currentRaid.raidEndTime - currentRaid.raidBeginTime;
		currentRaid.expOnEnd = EXPManager.EXP;
		currentRaid.expGained = currentRaid.expOnEnd - currentRaid.expOnBegan;
		RaidUtilities.CurrentRaid = currentRaid;
		Action<RaidUtilities.RaidInfo> onRaidEnd = RaidUtilities.OnRaidEnd;
		if (onRaidEnd == null)
		{
			return;
		}
		onRaidEnd(currentRaid);
	}

	// Token: 0x040009CB RID: 2507
	private const string SaveID = "RaidInfo";

	// Token: 0x020004BD RID: 1213
	[Serializable]
	public struct RaidInfo
	{
		// Token: 0x04001CAE RID: 7342
		public bool valid;

		// Token: 0x04001CAF RID: 7343
		public uint ID;

		// Token: 0x04001CB0 RID: 7344
		public bool dead;

		// Token: 0x04001CB1 RID: 7345
		public bool ended;

		// Token: 0x04001CB2 RID: 7346
		public float raidBeginTime;

		// Token: 0x04001CB3 RID: 7347
		public float raidEndTime;

		// Token: 0x04001CB4 RID: 7348
		public float totalTime;

		// Token: 0x04001CB5 RID: 7349
		public long expOnBegan;

		// Token: 0x04001CB6 RID: 7350
		public long expOnEnd;

		// Token: 0x04001CB7 RID: 7351
		public long expGained;
	}
}
