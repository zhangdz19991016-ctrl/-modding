using System;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class SetActiveOnAwake : MonoBehaviour
{
	// Token: 0x06000A82 RID: 2690 RVA: 0x0002E8D0 File Offset: 0x0002CAD0
	private void Awake()
	{
		this.target.SetActive(true);
	}

	// Token: 0x04000941 RID: 2369
	public GameObject target;
}
