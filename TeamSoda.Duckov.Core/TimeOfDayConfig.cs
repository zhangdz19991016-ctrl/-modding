using System;
using Duckov.Weathers;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000191 RID: 401
public class TimeOfDayConfig : MonoBehaviour
{
	// Token: 0x06000BF5 RID: 3061 RVA: 0x00033160 File Offset: 0x00031360
	public TimeOfDayEntry GetCurrentEntry(Weather weather)
	{
		switch (weather)
		{
		case Weather.Sunny:
			return this.defaultEntry;
		case Weather.Cloudy:
			return this.cloudyEntry;
		case Weather.Rainy:
			return this.rainyEntry;
		case Weather.Stormy_I:
			return this.stormIEntry;
		case Weather.Stormy_II:
			return this.stormIIEntry;
		default:
			return this.defaultEntry;
		}
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x000331B4 File Offset: 0x000313B4
	public void InvokeDebug()
	{
		TimeOfDayEntry currentEntry = this.GetCurrentEntry(this.debugWeather);
		if (!currentEntry)
		{
			Debug.Log("No entry found");
			return;
		}
		TimeOfDayPhase phase = currentEntry.GetPhase(this.debugPhase);
		if (!Application.isPlaying)
		{
			if (this.lookDevVolume && this.lookDevVolume.profile != phase.volumeProfile)
			{
				this.lookDevVolume.profile = phase.volumeProfile;
				return;
			}
		}
		else
		{
			int num;
			switch (this.debugPhase)
			{
			case TimePhaseTags.day:
				num = 9;
				break;
			case TimePhaseTags.dawn:
				num = 17;
				break;
			case TimePhaseTags.night:
				num = 22;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			WeatherManager.SetForceWeather(true, this.debugWeather);
			TimeSpan time = new TimeSpan(num, 10, 0);
			GameClock.Instance.StepTimeTil(time);
			Debug.Log(string.Format("Set Weather to {0},and time to {1}", this.debugWeather, num));
		}
	}

	// Token: 0x04000A51 RID: 2641
	[SerializeField]
	private TimeOfDayEntry defaultEntry;

	// Token: 0x04000A52 RID: 2642
	[SerializeField]
	private TimeOfDayEntry cloudyEntry;

	// Token: 0x04000A53 RID: 2643
	[SerializeField]
	private TimeOfDayEntry rainyEntry;

	// Token: 0x04000A54 RID: 2644
	[SerializeField]
	private TimeOfDayEntry stormIEntry;

	// Token: 0x04000A55 RID: 2645
	[SerializeField]
	private TimeOfDayEntry stormIIEntry;

	// Token: 0x04000A56 RID: 2646
	public bool forceSetTime;

	// Token: 0x04000A57 RID: 2647
	[Range(0f, 24f)]
	public int forceSetTimeTo = 8;

	// Token: 0x04000A58 RID: 2648
	public bool forceSetWeather;

	// Token: 0x04000A59 RID: 2649
	public Weather forceSetWeatherTo;

	// Token: 0x04000A5A RID: 2650
	[SerializeField]
	private Volume lookDevVolume;

	// Token: 0x04000A5B RID: 2651
	[SerializeField]
	private TimePhaseTags debugPhase;

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	private Weather debugWeather;
}
