using System;
using System.Collections.Generic;
using Duckov.UI;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class FormulasRegisterViewInvoker : InteractableBase
{
	// Token: 0x06000D12 RID: 3346 RVA: 0x00036D2D File Offset: 0x00034F2D
	protected override void Awake()
	{
		base.Awake();
		this.finishWhenTimeOut = true;
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00036D3C File Offset: 0x00034F3C
	protected override void OnInteractFinished()
	{
		FormulasRegisterView.Show(this.additionalTags);
	}

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private List<Tag> additionalTags;
}
