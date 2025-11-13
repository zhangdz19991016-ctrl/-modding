using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F0 RID: 496
public class ColorPunch : MonoBehaviour
{
	// Token: 0x06000EA9 RID: 3753 RVA: 0x0003B533 File Offset: 0x00039733
	private void Awake()
	{
		if (this.graphic == null)
		{
			this.graphic = base.GetComponent<Graphic>();
		}
		this.resetColor = this.graphic.color;
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0003B560 File Offset: 0x00039760
	public void Punch()
	{
		this.DoTask().Forget();
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0003B56D File Offset: 0x0003976D
	private int NewToken()
	{
		this.activeToken = UnityEngine.Random.Range(1, int.MaxValue);
		return this.activeToken;
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0003B588 File Offset: 0x00039788
	private UniTask DoTask()
	{
		ColorPunch.<DoTask>d__9 <DoTask>d__;
		<DoTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DoTask>d__.<>4__this = this;
		<DoTask>d__.<>1__state = -1;
		<DoTask>d__.<>t__builder.Start<ColorPunch.<DoTask>d__9>(ref <DoTask>d__);
		return <DoTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000C2F RID: 3119
	[SerializeField]
	private Graphic graphic;

	// Token: 0x04000C30 RID: 3120
	[SerializeField]
	private float duration;

	// Token: 0x04000C31 RID: 3121
	[SerializeField]
	private Gradient gradient;

	// Token: 0x04000C32 RID: 3122
	[SerializeField]
	private Color tint = Color.white;

	// Token: 0x04000C33 RID: 3123
	private Color resetColor;

	// Token: 0x04000C34 RID: 3124
	private int activeToken;
}
