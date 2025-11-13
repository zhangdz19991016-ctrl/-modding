using System;
using System.Collections.Generic;
using Duckov.Quests;
using Duckov.Weathers;

// Token: 0x02000121 RID: 289
public class RequireWeathers : Condition
{
	// Token: 0x0600099D RID: 2461 RVA: 0x0002A430 File Offset: 0x00028630
	public override bool Evaluate()
	{
		if (!LevelManager.LevelInited)
		{
			return false;
		}
		Weather currentWeather = LevelManager.Instance.TimeOfDayController.CurrentWeather;
		return this.weathers.Contains(currentWeather);
	}

	// Token: 0x0400088B RID: 2187
	public List<Weather> weathers;
}
