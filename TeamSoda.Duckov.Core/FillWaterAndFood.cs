using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class FillWaterAndFood : MonoBehaviour
{
	// Token: 0x0600055E RID: 1374 RVA: 0x00018160 File Offset: 0x00016360
	public void Fill()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (!main)
		{
			return;
		}
		main.AddWater(this.water);
		main.AddEnergy(this.food);
	}

	// Token: 0x040004D1 RID: 1233
	public float water;

	// Token: 0x040004D2 RID: 1234
	public float food;
}
