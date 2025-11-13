using System;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class HalfObsticleTrigger : MonoBehaviour
{
	// Token: 0x060003A4 RID: 932 RVA: 0x00010091 File Offset: 0x0000E291
	private void OnTriggerEnter(Collider other)
	{
		this.parent.OnTriggerEnter(other);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001009F File Offset: 0x0000E29F
	private void OnTriggerExit(Collider other)
	{
		this.parent.OnTriggerExit(other);
	}

	// Token: 0x040002C1 RID: 705
	public HalfObsticle parent;
}
