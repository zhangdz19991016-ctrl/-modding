using System;
using Saves;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class GameClock : MonoBehaviour
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00037697 File Offset: 0x00035897
	// (set) Token: 0x06000D2D RID: 3373 RVA: 0x0003769E File Offset: 0x0003589E
	public static GameClock Instance { get; private set; }

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06000D2E RID: 3374 RVA: 0x000376A8 File Offset: 0x000358A8
	// (remove) Token: 0x06000D2F RID: 3375 RVA: 0x000376DC File Offset: 0x000358DC
	public static event Action OnGameClockStep;

	// Token: 0x06000D30 RID: 3376 RVA: 0x0003770F File Offset: 0x0003590F
	private void Awake()
	{
		if (GameClock.Instance != null)
		{
			Debug.LogError("检测到多个Game Clock");
			return;
		}
		GameClock.Instance = this;
		SavesSystem.OnCollectSaveData += this.Save;
		this.Load();
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x00037746 File Offset: 0x00035946
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000D32 RID: 3378 RVA: 0x00037759 File Offset: 0x00035959
	private static string SaveKey
	{
		get
		{
			return "GameClock";
		}
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x00037760 File Offset: 0x00035960
	private void Save()
	{
		SavesSystem.Save<GameClock.SaveData>(GameClock.SaveKey, new GameClock.SaveData
		{
			days = this.days,
			secondsOfDay = this.secondsOfDay,
			realTimePlayedTicks = this.RealTimePlayed.Ticks
		});
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x000377B0 File Offset: 0x000359B0
	private void Load()
	{
		GameClock.SaveData saveData = SavesSystem.Load<GameClock.SaveData>(GameClock.SaveKey);
		this.days = saveData.days;
		this.secondsOfDay = saveData.secondsOfDay;
		this.realTimePlayed = saveData.RealTimePlayed;
		Action onGameClockStep = GameClock.OnGameClockStep;
		if (onGameClockStep == null)
		{
			return;
		}
		onGameClockStep();
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x000377FC File Offset: 0x000359FC
	public static TimeSpan GetRealTimePlayedOfSaveSlot(int saveSlot)
	{
		return SavesSystem.Load<GameClock.SaveData>(GameClock.SaveKey, saveSlot).RealTimePlayed;
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000D36 RID: 3382 RVA: 0x0003781C File Offset: 0x00035A1C
	private TimeSpan RealTimePlayed
	{
		get
		{
			return this.realTimePlayed;
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00037824 File Offset: 0x00035A24
	private static double SecondsOfDay
	{
		get
		{
			if (GameClock.Instance == null)
			{
				return 0.0;
			}
			return GameClock.Instance.secondsOfDay;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00037848 File Offset: 0x00035A48
	[TimeSpan]
	private long _TimeOfDayTicks
	{
		get
		{
			return GameClock.TimeOfDay.Ticks;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00037862 File Offset: 0x00035A62
	public static TimeSpan TimeOfDay
	{
		get
		{
			return TimeSpan.FromSeconds(GameClock.SecondsOfDay);
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0003786E File Offset: 0x00035A6E
	public static long Day
	{
		get
		{
			if (GameClock.Instance == null)
			{
				return 0L;
			}
			return GameClock.Instance.days;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0003788A File Offset: 0x00035A8A
	public static TimeSpan Now
	{
		get
		{
			return GameClock.TimeOfDay + TimeSpan.FromDays((double)GameClock.Day);
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000D3C RID: 3388 RVA: 0x000378A4 File Offset: 0x00035AA4
	public static int Hour
	{
		get
		{
			return GameClock.TimeOfDay.Hours;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000D3D RID: 3389 RVA: 0x000378C0 File Offset: 0x00035AC0
	public static int Minut
	{
		get
		{
			return GameClock.TimeOfDay.Minutes;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x000378DC File Offset: 0x00035ADC
	public static int Seconds
	{
		get
		{
			return GameClock.TimeOfDay.Seconds;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000D3F RID: 3391 RVA: 0x000378F8 File Offset: 0x00035AF8
	public static int Milliseconds
	{
		get
		{
			return GameClock.TimeOfDay.Milliseconds;
		}
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00037912 File Offset: 0x00035B12
	private void Update()
	{
		this.StepTime(Time.deltaTime * this.clockTimeScale);
		this.realTimePlayed += TimeSpan.FromSeconds((double)Time.unscaledDeltaTime);
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00037944 File Offset: 0x00035B44
	private void StepTime(float deltaTime)
	{
		this.secondsOfDay += (double)deltaTime;
		while (this.secondsOfDay > 86300.0)
		{
			this.days += 1L;
			this.secondsOfDay -= 86300.0;
		}
		Action onGameClockStep = GameClock.OnGameClockStep;
		if (onGameClockStep == null)
		{
			return;
		}
		onGameClockStep();
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x000379A8 File Offset: 0x00035BA8
	public void StepTimeTil(TimeSpan time)
	{
		if (time.Days > 0)
		{
			time = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
		}
		TimeSpan timeSpan;
		if (time > GameClock.TimeOfDay)
		{
			timeSpan = time - GameClock.TimeOfDay;
		}
		else
		{
			timeSpan = time + TimeSpan.FromDays(1.0) - GameClock.TimeOfDay;
		}
		this.StepTime((float)timeSpan.TotalSeconds);
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00037A23 File Offset: 0x00035C23
	internal static void Step(float seconds)
	{
		if (GameClock.Instance == null)
		{
			return;
		}
		GameClock.Instance.StepTime(seconds);
	}

	// Token: 0x04000B6E RID: 2926
	public float clockTimeScale = 60f;

	// Token: 0x04000B6F RID: 2927
	private long days;

	// Token: 0x04000B70 RID: 2928
	private double secondsOfDay;

	// Token: 0x04000B71 RID: 2929
	private TimeSpan realTimePlayed;

	// Token: 0x04000B72 RID: 2930
	private const double SecondsPerDay = 86300.0;

	// Token: 0x020004D7 RID: 1239
	[Serializable]
	private struct SaveData
	{
		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x0008F6AC File Offset: 0x0008D8AC
		public TimeSpan RealTimePlayed
		{
			get
			{
				return TimeSpan.FromTicks(this.realTimePlayedTicks);
			}
		}

		// Token: 0x04001D1B RID: 7451
		public long days;

		// Token: 0x04001D1C RID: 7452
		public double secondsOfDay;

		// Token: 0x04001D1D RID: 7453
		public long realTimePlayedTicks;
	}
}
