using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class ToggleHUD : MonoBehaviour
{
	// Token: 0x06000A84 RID: 2692 RVA: 0x0002E8E8 File Offset: 0x0002CAE8
	private void Awake()
	{
		foreach (GameObject gameObject in this.toggleTargets)
		{
			if (gameObject != null && !gameObject.activeInHierarchy)
			{
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x04000942 RID: 2370
	public List<GameObject> toggleTargets;
}
