using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class OcclusionFadeTrigger : MonoBehaviour
{
	// Token: 0x06000BE0 RID: 3040 RVA: 0x00032CF6 File Offset: 0x00030EF6
	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00032D0D File Offset: 0x00030F0D
	public void Enter()
	{
		this.parent.OnEnter();
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x00032D1A File Offset: 0x00030F1A
	public void Leave()
	{
		this.parent.OnLeave();
	}

	// Token: 0x04000A3E RID: 2622
	public OcclusionFadeObject parent;
}
