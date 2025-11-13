using System;
using Duckov.Buffs;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class StormWeather : MonoBehaviour
{
	// Token: 0x06000BF3 RID: 3059 RVA: 0x00033090 File Offset: 0x00031290
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
		if (this.onlyOutDoor && subSceneInfo.IsInDoor)
		{
			return;
		}
		if (!this.target)
		{
			this.target = CharacterMainControl.Main;
			if (!this.target)
			{
				return;
			}
		}
		this.addBuffTimer -= Time.deltaTime;
		if (this.addBuffTimer <= 0f)
		{
			this.addBuffTimer = this.addBuffTimeSpace;
			if (this.target.StormProtection > this.stormProtectionThreshold)
			{
				return;
			}
			this.target.AddBuff(this.buff, null, 0);
		}
	}

	// Token: 0x04000A4B RID: 2635
	public Buff buff;

	// Token: 0x04000A4C RID: 2636
	public float addBuffTimeSpace = 1f;

	// Token: 0x04000A4D RID: 2637
	private float addBuffTimer;

	// Token: 0x04000A4E RID: 2638
	private CharacterMainControl target;

	// Token: 0x04000A4F RID: 2639
	private bool onlyOutDoor = true;

	// Token: 0x04000A50 RID: 2640
	public float stormProtectionThreshold = 0.9f;
}
