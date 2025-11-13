using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000108 RID: 264
public class LevelConfig : MonoBehaviour
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00028562 File Offset: 0x00026762
	public static LevelConfig Instance
	{
		get
		{
			if (!LevelConfig.instance)
			{
				LevelConfig.SetInstance();
			}
			return LevelConfig.instance;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0002857A File Offset: 0x0002677A
	public float LootBoxQualityLowPercent
	{
		get
		{
			return 1f - 1f / this.lootBoxHighQualityChanceMultiplier;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0002858E File Offset: 0x0002678E
	public float LootboxItemCountMultiplier
	{
		get
		{
			return this.lootboxItemCountMultiplier;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00028596 File Offset: 0x00026796
	public static bool IsBaseLevel
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.isBaseLevel;
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000285B0 File Offset: 0x000267B0
	public static bool IsRaidMap
	{
		get
		{
			return LevelConfig.Instance && LevelConfig.Instance.isRaidMap;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060008E7 RID: 2279 RVA: 0x000285CA File Offset: 0x000267CA
	public static int MinExitCount
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return 0;
			}
			return LevelConfig.Instance.minExitCount;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x000285E4 File Offset: 0x000267E4
	public static bool SpawnTomb
	{
		get
		{
			return !LevelConfig.Instance || LevelConfig.Instance.spawnTomb;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x000285FE File Offset: 0x000267FE
	public static int MaxExitCount
	{
		get
		{
			if (!LevelConfig.Instance)
			{
				return 0;
			}
			return LevelConfig.Instance.maxExitCount;
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00028618 File Offset: 0x00026818
	private void Awake()
	{
		UnityEngine.Object.Instantiate<LevelManager>(GameplayDataSettings.Prefabs.LevelManagerPrefab).transform.SetParent(base.transform);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00028639 File Offset: 0x00026839
	private static void SetInstance()
	{
		if (LevelConfig.instance)
		{
			return;
		}
		LevelConfig.instance = UnityEngine.Object.FindFirstObjectByType<LevelConfig>();
		LevelConfig.instance;
	}

	// Token: 0x0400081B RID: 2075
	private static LevelConfig instance;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	private bool isBaseLevel;

	// Token: 0x0400081D RID: 2077
	[SerializeField]
	private bool isRaidMap = true;

	// Token: 0x0400081E RID: 2078
	[SerializeField]
	private bool spawnTomb = true;

	// Token: 0x0400081F RID: 2079
	[SerializeField]
	private int minExitCount;

	// Token: 0x04000820 RID: 2080
	[SerializeField]
	private int maxExitCount;

	// Token: 0x04000821 RID: 2081
	public TimeOfDayConfig timeOfDayConfig;

	// Token: 0x04000822 RID: 2082
	[SerializeField]
	[Min(1f)]
	private float lootBoxHighQualityChanceMultiplier = 1f;

	// Token: 0x04000823 RID: 2083
	[SerializeField]
	[Range(0.1f, 10f)]
	private float lootboxItemCountMultiplier = 1f;
}
