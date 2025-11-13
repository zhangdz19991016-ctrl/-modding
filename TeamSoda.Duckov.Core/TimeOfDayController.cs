using System;
using Duckov;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.Weathers;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000192 RID: 402
public class TimeOfDayController : MonoBehaviour
{
	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x000332B8 File Offset: 0x000314B8
	public static TimeOfDayController Instance
	{
		get
		{
			if (!LevelManager.Instance)
			{
				return null;
			}
			return LevelManager.Instance.TimeOfDayController;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x000332D2 File Offset: 0x000314D2
	public bool AtNight
	{
		get
		{
			return this.atNight;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000BFA RID: 3066 RVA: 0x000332DA File Offset: 0x000314DA
	public TimeOfDayPhase CurrentPhase
	{
		get
		{
			return this.currentPhase;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000BFB RID: 3067 RVA: 0x000332E2 File Offset: 0x000314E2
	public Weather CurrentWeather
	{
		get
		{
			return this.currentWeather;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x000332EA File Offset: 0x000314EA
	public float Time
	{
		get
		{
			return this.time;
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x000332F4 File Offset: 0x000314F4
	private void Start()
	{
		this.config = LevelConfig.Instance.timeOfDayConfig;
		if (this.config.forceSetTime)
		{
			TimeSpan timeSpan = new TimeSpan(0, this.config.forceSetTimeTo, 0, 0);
			GameClock.Instance.StepTimeTil(timeSpan);
		}
		if (this.config.forceSetWeather)
		{
			WeatherManager.SetForceWeather(true, this.config.forceSetWeatherTo);
		}
		this.time = (float)GameClock.TimeOfDay.TotalHours % 24f;
		TimePhaseTags timePhaseTagByTime = this.GetTimePhaseTagByTime(this.time);
		this.atNight = (timePhaseTagByTime == TimePhaseTags.night);
		this.currentWeather = WeatherManager.GetWeather();
		this.OnWeatherChanged(this.currentWeather);
		this.currentPhase = this.config.GetCurrentEntry(this.CurrentWeather).GetPhase(timePhaseTagByTime);
		this.weatherVolumeControl.ForceSetProfile(this.currentPhase.volumeProfile);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x000333D8 File Offset: 0x000315D8
	private void Update()
	{
		this.time = (float)GameClock.TimeOfDay.TotalHours % 24f;
		TimePhaseTags timePhaseTagByTime = this.GetTimePhaseTagByTime(this.time);
		this.atNight = (timePhaseTagByTime == TimePhaseTags.night);
		Weather weather = WeatherManager.GetWeather();
		if (weather != this.currentWeather)
		{
			this.currentWeather = weather;
			this.OnWeatherChanged(this.currentWeather);
		}
		this.currentPhase = this.config.GetCurrentEntry(this.CurrentWeather).GetPhase(timePhaseTagByTime);
		if (this.weatherVolumeControl.CurrentProfile != this.currentPhase.volumeProfile && this.weatherVolumeControl.BufferTargetProfile != this.currentPhase.volumeProfile)
		{
			this.weatherVolumeControl.SetTargetProfile(this.currentPhase.volumeProfile);
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x000334A8 File Offset: 0x000316A8
	private void OnWeatherChanged(Weather newWeather)
	{
		bool flag = false;
		if (MultiSceneCore.Instance)
		{
			SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
			if (subSceneInfo != null)
			{
				flag = subSceneInfo.IsInDoor;
			}
		}
		if (newWeather == Weather.Stormy_I)
		{
			this.stormIObject.SetActive(true);
			this.stormIIObject.SetActive(false);
			NotificationText.Push("Weather_Storm_I".ToPlainText());
			if (!flag && LevelManager.AfterInit)
			{
				AudioManager.Post(this.stormPhaseISoundKey, base.gameObject);
				return;
			}
		}
		else if (newWeather == Weather.Stormy_II)
		{
			this.stormIObject.SetActive(false);
			this.stormIIObject.SetActive(true);
			NotificationText.Push("Weather_Storm_II".ToPlainText());
			if (!flag && LevelManager.AfterInit)
			{
				AudioManager.Post(this.stormPhaseIISoundKey, base.gameObject);
				return;
			}
		}
		else
		{
			this.stormIObject.SetActive(false);
			this.stormIIObject.SetActive(false);
		}
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00033580 File Offset: 0x00031780
	private TimePhaseTags GetTimePhaseTagByTime(float hourTime)
	{
		hourTime %= 24f;
		if (hourTime < this.morningStart || hourTime >= this.nightStart)
		{
			return TimePhaseTags.night;
		}
		if (hourTime >= this.morningStart && hourTime < this.dawnStart)
		{
			return TimePhaseTags.day;
		}
		if (hourTime >= this.dawnStart && hourTime < this.nightStart)
		{
			return TimePhaseTags.dawn;
		}
		return TimePhaseTags.day;
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x000335D4 File Offset: 0x000317D4
	public static string GetTimePhaseNameByPhaseTag(TimePhaseTags phaseTag)
	{
		TimeOfDayController instance = TimeOfDayController.Instance;
		if (!instance)
		{
			return string.Empty;
		}
		switch (phaseTag)
		{
		case TimePhaseTags.day:
			return instance.timePhaseKey_Day.ToPlainText();
		case TimePhaseTags.dawn:
			return instance.timePhaseKey_Dawn.ToPlainText();
		case TimePhaseTags.night:
			return instance.timePhaseKey_Night.ToPlainText();
		default:
			return instance.timePhaseKey_Day.ToPlainText();
		}
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x00033638 File Offset: 0x00031838
	public static string GetWeatherNameByWeather(Weather weather)
	{
		TimeOfDayController instance = TimeOfDayController.Instance;
		if (!instance)
		{
			return string.Empty;
		}
		switch (weather)
		{
		case Weather.Sunny:
			return instance.WeatherKey_Sunny.ToPlainText();
		case Weather.Cloudy:
			return instance.WeatherKey_Cloudy.ToPlainText();
		case Weather.Rainy:
			return instance.WeatherKey_Rainy.ToPlainText();
		case Weather.Stormy_I:
			return instance.WeatherKey_Storm_I.ToPlainText();
		case Weather.Stormy_II:
			return instance.WeatherKey_Storm_II.ToPlainText();
		default:
			return instance.WeatherKey_Sunny.ToPlainText();
		}
	}

	// Token: 0x04000A5D RID: 2653
	private TimeOfDayConfig config;

	// Token: 0x04000A5E RID: 2654
	private bool atNight;

	// Token: 0x04000A5F RID: 2655
	[FormerlySerializedAs("volumeControl")]
	[SerializeField]
	private TimeOfDayVolumeControl weatherVolumeControl;

	// Token: 0x04000A60 RID: 2656
	private TimeOfDayPhase currentPhase;

	// Token: 0x04000A61 RID: 2657
	private Weather currentWeather;

	// Token: 0x04000A62 RID: 2658
	public float morningStart = 5f;

	// Token: 0x04000A63 RID: 2659
	public float dawnStart = 16f;

	// Token: 0x04000A64 RID: 2660
	public float nightStart = 19f;

	// Token: 0x04000A65 RID: 2661
	public static float NightViewAngleFactor;

	// Token: 0x04000A66 RID: 2662
	public static float NightViewDistanceFactor;

	// Token: 0x04000A67 RID: 2663
	public static float NightSenseRangeFactor;

	// Token: 0x04000A68 RID: 2664
	[LocalizationKey("Default")]
	public string timePhaseKey_Day;

	// Token: 0x04000A69 RID: 2665
	[LocalizationKey("Default")]
	public string timePhaseKey_Dawn;

	// Token: 0x04000A6A RID: 2666
	[LocalizationKey("Default")]
	public string timePhaseKey_Night;

	// Token: 0x04000A6B RID: 2667
	[LocalizationKey("Default")]
	public string WeatherKey_Sunny;

	// Token: 0x04000A6C RID: 2668
	[LocalizationKey("Default")]
	public string WeatherKey_Cloudy;

	// Token: 0x04000A6D RID: 2669
	[LocalizationKey("Default")]
	public string WeatherKey_Rainy;

	// Token: 0x04000A6E RID: 2670
	[LocalizationKey("Default")]
	public string WeatherKey_Storm_I;

	// Token: 0x04000A6F RID: 2671
	[LocalizationKey("Default")]
	public string WeatherKey_Storm_II;

	// Token: 0x04000A70 RID: 2672
	private string stormPhaseISoundKey = "Music/Stinger/stg_storm_1";

	// Token: 0x04000A71 RID: 2673
	private string stormPhaseIISoundKey = "Music/Stinger/stg_storm_2";

	// Token: 0x04000A72 RID: 2674
	public GameObject stormIObject;

	// Token: 0x04000A73 RID: 2675
	public GameObject stormIIObject;

	// Token: 0x04000A74 RID: 2676
	private float time;
}
