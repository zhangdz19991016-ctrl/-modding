using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001BE RID: 446
public class InputRebinderIndicator : MonoBehaviour
{
	// Token: 0x06000D5A RID: 3418 RVA: 0x00037ED0 File Offset: 0x000360D0
	private void Awake()
	{
		InputRebinder.OnRebindBegin = (Action<InputAction>)Delegate.Combine(InputRebinder.OnRebindBegin, new Action<InputAction>(this.OnRebindBegin));
		InputRebinder.OnRebindComplete = (Action<InputAction>)Delegate.Combine(InputRebinder.OnRebindComplete, new Action<InputAction>(this.OnRebindComplete));
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x00037F28 File Offset: 0x00036128
	private void OnRebindComplete(InputAction action)
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x00037F35 File Offset: 0x00036135
	private void OnRebindBegin(InputAction action)
	{
		this.fadeGroup.Show();
	}

	// Token: 0x04000B7C RID: 2940
	[SerializeField]
	private FadeGroup fadeGroup;
}
