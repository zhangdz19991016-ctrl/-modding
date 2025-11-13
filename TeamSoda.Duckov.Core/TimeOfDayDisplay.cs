using System;
using Duckov.Weathers;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000D2 RID: 210
public class TimeOfDayDisplay : MonoBehaviour
{
	// Token: 0x0600067F RID: 1663 RVA: 0x0001D741 File Offset: 0x0001B941
	private void Start()
	{
		this.RefreshPhase(TimeOfDayController.Instance.CurrentPhase.timePhaseTag);
		this.RefreshWeather(TimeOfDayController.Instance.CurrentWeather);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0001D768 File Offset: 0x0001B968
	private void Update()
	{
		this.refreshTimer -= Time.unscaledDeltaTime;
		if (this.refreshTimer > 0f)
		{
			return;
		}
		this.refreshTimer = this.refreshTimeSpace;
		TimePhaseTags timePhaseTag = TimeOfDayController.Instance.CurrentPhase.timePhaseTag;
		if (this.currentPhaseTag != timePhaseTag)
		{
			this.RefreshPhase(timePhaseTag);
		}
		Weather weather = TimeOfDayController.Instance.CurrentWeather;
		if (this.currentWeather != weather)
		{
			this.RefreshWeather(weather);
		}
		this.RefreshStormText(weather);
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0001D7E4 File Offset: 0x0001B9E4
	private void RefreshStormText(Weather _weather)
	{
		TimeSpan timeSpan = default(TimeSpan);
		float fillAmount;
		if (_weather == Weather.Stormy_I)
		{
			this.stormIndicatorAnimator.SetBool("Grow", false);
			this.stormTitleText.text = this.StormPhaseIIETAKey.ToPlainText();
			timeSpan = WeatherManager.Instance.Storm.GetStormIOverETA(GameClock.Now);
			fillAmount = WeatherManager.Instance.Storm.GetStormRemainPercent(GameClock.Now);
			this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
		}
		else if (_weather == Weather.Stormy_II)
		{
			this.stormIndicatorAnimator.SetBool("Grow", false);
			this.stormTitleText.text = this.StormOverETAKey.ToPlainText();
			timeSpan = WeatherManager.Instance.Storm.GetStormIIOverETA(GameClock.Now);
			fillAmount = WeatherManager.Instance.Storm.GetStormRemainPercent(GameClock.Now);
			this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
		}
		else
		{
			this.stormIndicatorAnimator.SetBool("Grow", true);
			fillAmount = WeatherManager.Instance.Storm.GetSleepPercent(GameClock.Now);
			timeSpan = WeatherManager.Instance.Storm.GetStormETA(GameClock.Now);
			if (timeSpan.TotalHours < 24.0)
			{
				this.stormTitleText.text = this.StormComingOneDayKey.ToPlainText();
				this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
			}
			else
			{
				this.stormTitleText.text = this.StormComingETAKey.ToPlainText();
				this.stormDescObject.SetActive(false);
			}
		}
		this.stormFillImage.fillAmount = fillAmount;
		this.stormText.text = string.Format("{0:000}:{1:00}", Mathf.FloorToInt((float)timeSpan.TotalHours), timeSpan.Minutes);
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0001D9B8 File Offset: 0x0001BBB8
	private void RefreshPhase(TimePhaseTags _phase)
	{
		this.currentPhaseTag = _phase;
		this.phaseText.text = TimeOfDayController.GetTimePhaseNameByPhaseTag(_phase);
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0001D9D2 File Offset: 0x0001BBD2
	private void RefreshWeather(Weather _weather)
	{
		this.currentWeather = _weather;
		this.weatherText.text = TimeOfDayController.GetWeatherNameByWeather(_weather);
	}

	// Token: 0x04000642 RID: 1602
	private TimePhaseTags currentPhaseTag;

	// Token: 0x04000643 RID: 1603
	private Weather currentWeather;

	// Token: 0x04000644 RID: 1604
	public TextMeshProUGUI phaseText;

	// Token: 0x04000645 RID: 1605
	public TextMeshProUGUI weatherText;

	// Token: 0x04000646 RID: 1606
	public TextMeshProUGUI stormTitleText;

	// Token: 0x04000647 RID: 1607
	public TextMeshProUGUI stormText;

	// Token: 0x04000648 RID: 1608
	[LocalizationKey("Default")]
	public string StormComingETAKey = "StormETA";

	// Token: 0x04000649 RID: 1609
	[LocalizationKey("Default")]
	public string StormComingOneDayKey = "StormOneDayETA";

	// Token: 0x0400064A RID: 1610
	[LocalizationKey("Default")]
	public string StormPhaseIIETAKey = "StormPhaseIIETA";

	// Token: 0x0400064B RID: 1611
	[LocalizationKey("Default")]
	public string StormOverETAKey = "StormOverETA";

	// Token: 0x0400064C RID: 1612
	public GameObject stormDescObject;

	// Token: 0x0400064D RID: 1613
	private float refreshTimeSpace = 0.5f;

	// Token: 0x0400064E RID: 1614
	private float refreshTimer;

	// Token: 0x0400064F RID: 1615
	public Animator stormIndicatorAnimator;

	// Token: 0x04000650 RID: 1616
	public ProceduralImage stormFillImage;
}
