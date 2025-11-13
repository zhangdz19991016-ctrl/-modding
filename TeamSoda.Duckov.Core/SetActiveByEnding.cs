using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class SetActiveByEnding : MonoBehaviour
{
	// Token: 0x06000BEB RID: 3051 RVA: 0x00032ED9 File Offset: 0x000310D9
	private void Start()
	{
		this.target.SetActive(this.endingIndexs.Contains(Ending.endingIndex));
	}

	// Token: 0x04000A46 RID: 2630
	public GameObject target;

	// Token: 0x04000A47 RID: 2631
	public List<int> endingIndexs;
}
