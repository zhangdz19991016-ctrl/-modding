using System;
using Saves;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class SetTimeOnEnterBaseFirstTime : MonoBehaviour
{
	// Token: 0x060005D5 RID: 1493 RVA: 0x0001A354 File Offset: 0x00018554
	private void Start()
	{
		if (SavesSystem.Load<bool>("FirstTimeToBaseTimeSetted"))
		{
			return;
		}
		SavesSystem.Save<bool>("FirstTimeToBaseTimeSetted", true);
		TimeSpan time = new TimeSpan(this.setTimeTo, 0, 0);
		GameClock.Instance.StepTimeTil(time);
	}

	// Token: 0x04000558 RID: 1368
	public int setTimeTo;
}
