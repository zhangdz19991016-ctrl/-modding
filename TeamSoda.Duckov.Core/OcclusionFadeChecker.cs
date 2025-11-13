using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class OcclusionFadeChecker : MonoBehaviour
{
	// Token: 0x06000BC6 RID: 3014 RVA: 0x000323E0 File Offset: 0x000305E0
	private void OnTriggerEnter(Collider other)
	{
		OcclusionFadeTrigger component = other.GetComponent<OcclusionFadeTrigger>();
		if (!component)
		{
			return;
		}
		component.Enter();
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00032404 File Offset: 0x00030604
	private void OnTriggerExit(Collider other)
	{
		OcclusionFadeTrigger component = other.GetComponent<OcclusionFadeTrigger>();
		if (!component)
		{
			return;
		}
		component.Leave();
	}
}
