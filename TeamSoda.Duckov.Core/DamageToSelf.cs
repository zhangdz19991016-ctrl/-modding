using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class DamageToSelf : MonoBehaviour
{
	// Token: 0x06000A17 RID: 2583 RVA: 0x0002BA1E File Offset: 0x00029C1E
	private void Start()
	{
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0002BA20 File Offset: 0x00029C20
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			this.dmg.fromCharacter = CharacterMainControl.Main;
			CharacterMainControl.Main.Health.Hurt(this.dmg);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			float value = CharacterMainControl.Main.CharacterItem.GetStat("InventoryCapacity").Value;
			Debug.Log(string.Format("InventorySize:{0}", value));
		}
	}

	// Token: 0x040008E6 RID: 2278
	public DamageInfo dmg;
}
