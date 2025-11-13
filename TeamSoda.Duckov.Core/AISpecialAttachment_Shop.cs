using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class AISpecialAttachment_Shop : AISpecialAttachmentBase
{
	// Token: 0x060004F1 RID: 1265 RVA: 0x0001652A File Offset: 0x0001472A
	protected override void OnInited()
	{
		base.OnInited();
		this.aiCharacterController.hideIfFoundEnemy = this.shop;
	}

	// Token: 0x04000425 RID: 1061
	public GameObject shop;
}
