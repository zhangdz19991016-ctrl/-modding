using System;
using Saves;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x02000246 RID: 582
	public class WeatherManager : MonoBehaviour
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x000460F5 File Offset: 0x000442F5
		// (set) Token: 0x0600123A RID: 4666 RVA: 0x000460FC File Offset: 0x000442FC
		public static WeatherManager Instance { get; private set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x00046104 File Offset: 0x00044304
		// (set) Token: 0x0600123C RID: 4668 RVA: 0x0004610C File Offset: 0x0004430C
		public bool ForceWeather { get; set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x00046115 File Offset: 0x00044315
		// (set) Token: 0x0600123E RID: 4670 RVA: 0x0004611D File Offset: 0x0004431D
		public Weather ForceWeatherValue { get; set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00046126 File Offset: 0x00044326
		public Storm Storm
		{
			get
			{
				return this.storm;
			}
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0004612E File Offset: 0x0004432E
		private void Awake()
		{
			WeatherManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
			this.Load();
			this._weatherDirty = true;
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00046154 File Offset: 0x00044354
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00046167 File Offset: 0x00044367
		private void Save()
		{
			SavesSystem.Save<WeatherManager.SaveData>("WeatherManagerData", new WeatherManager.SaveData(this));
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0004617C File Offset: 0x0004437C
		private void Load()
		{
			WeatherManager.SaveData saveData = SavesSystem.Load<WeatherManager.SaveData>("WeatherManagerData");
			if (!saveData.valid)
			{
				this.SetRandomKey();
			}
			else
			{
				saveData.Setup(this);
			}
			this.SetupModules();
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000461B2 File Offset: 0x000443B2
		private void SetRandomKey()
		{
			this.seed = UnityEngine.Random.Range(0, 100000);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000461C5 File Offset: 0x000443C5
		private void SetupModules()
		{
			this.precipitation.SetSeed(this.seed);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000461D8 File Offset: 0x000443D8
		private Weather M_GetWeather(TimeSpan dayAndTime)
		{
			if (this.ForceWeather)
			{
				return this.ForceWeatherValue;
			}
			if (!this._weatherDirty && dayAndTime == this._cachedDayAndTime)
			{
				return this._cachedWeather;
			}
			int stormLevel = this.storm.GetStormLevel(dayAndTime);
			Weather weather;
			if (stormLevel > 0)
			{
				if (stormLevel == 1)
				{
					weather = Weather.Stormy_I;
				}
				else
				{
					weather = Weather.Stormy_II;
				}
			}
			else
			{
				float num = this.precipitation.Get(dayAndTime);
				if (num > this.precipitation.RainyThreshold)
				{
					weather = Weather.Rainy;
				}
				else if (num > this.precipitation.CloudyThreshold)
				{
					weather = Weather.Cloudy;
				}
				else
				{
					weather = Weather.Sunny;
				}
			}
			this._cachedDayAndTime = dayAndTime;
			this._cachedWeather = weather;
			this._weatherDirty = false;
			return weather;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x00046277 File Offset: 0x00044477
		private void M_SetForceWeather(bool forceWeather, Weather value = Weather.Sunny)
		{
			this.ForceWeather = forceWeather;
			this.ForceWeatherValue = value;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00046287 File Offset: 0x00044487
		public static Weather GetWeather(TimeSpan dayAndTime)
		{
			if (WeatherManager.Instance == null)
			{
				return Weather.Sunny;
			}
			return WeatherManager.Instance.M_GetWeather(dayAndTime);
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000462A4 File Offset: 0x000444A4
		public static Weather GetWeather()
		{
			TimeSpan now = GameClock.Now;
			if (WeatherManager.Instance && WeatherManager.Instance.ForceWeather)
			{
				return WeatherManager.Instance.ForceWeatherValue;
			}
			return WeatherManager.GetWeather(now);
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x000462E0 File Offset: 0x000444E0
		public static void SetForceWeather(bool forceWeather, Weather value = Weather.Sunny)
		{
			if (WeatherManager.Instance == null)
			{
				return;
			}
			WeatherManager.Instance.M_SetForceWeather(forceWeather, value);
		}

		// Token: 0x04000DFF RID: 3583
		private int seed = -1;

		// Token: 0x04000E00 RID: 3584
		[SerializeField]
		private Storm storm = new Storm();

		// Token: 0x04000E01 RID: 3585
		[SerializeField]
		private Precipitation precipitation = new Precipitation();

		// Token: 0x04000E02 RID: 3586
		private const string SaveKey = "WeatherManagerData";

		// Token: 0x04000E03 RID: 3587
		private Weather _cachedWeather;

		// Token: 0x04000E04 RID: 3588
		private TimeSpan _cachedDayAndTime;

		// Token: 0x04000E05 RID: 3589
		private bool _weatherDirty;

		// Token: 0x0200053B RID: 1339
		[Serializable]
		private struct SaveData
		{
			// Token: 0x0600282D RID: 10285 RVA: 0x000937B6 File Offset: 0x000919B6
			public SaveData(WeatherManager weatherManager)
			{
				this = default(WeatherManager.SaveData);
				this.seed = weatherManager.seed;
				this.valid = true;
			}

			// Token: 0x0600282E RID: 10286 RVA: 0x000937D2 File Offset: 0x000919D2
			internal void Setup(WeatherManager weatherManager)
			{
				weatherManager.seed = this.seed;
			}

			// Token: 0x04001EC2 RID: 7874
			public bool valid;

			// Token: 0x04001EC3 RID: 7875
			public int seed;
		}
	}
}
