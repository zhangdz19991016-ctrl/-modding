using System;
using Duckov;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class PostAudioEventOnEnter : StateMachineBehaviour
{
	// Token: 0x06000C63 RID: 3171 RVA: 0x000349F7 File Offset: 0x00032BF7
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		AudioManager.Post(this.eventName, animator.gameObject);
	}

	// Token: 0x04000AC5 RID: 2757
	[SerializeField]
	private string eventName;
}
