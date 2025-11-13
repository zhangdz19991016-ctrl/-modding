using System;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class PaperBox : MonoBehaviour
{
	// Token: 0x060005BD RID: 1469 RVA: 0x00019BF4 File Offset: 0x00017DF4
	private void Update()
	{
		if (!this.character)
		{
			return;
		}
		if (!this.setActiveWhileStandStill)
		{
			return;
		}
		bool flag = this.character.Velocity.magnitude < 0.2f;
		if (this.setActiveWhileStandStill.gameObject.activeSelf != flag)
		{
			this.setActiveWhileStandStill.gameObject.SetActive(flag);
		}
	}

	// Token: 0x0400053B RID: 1339
	[HideInInspector]
	public CharacterMainControl character;

	// Token: 0x0400053C RID: 1340
	public Transform setActiveWhileStandStill;
}
