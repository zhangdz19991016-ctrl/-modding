using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FB RID: 507
public class HealthBar_DamageBar : MonoBehaviour
{
	// Token: 0x06000EF2 RID: 3826 RVA: 0x0003C01B File Offset: 0x0003A21B
	private void Awake()
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = (base.transform as RectTransform);
		}
		if (this.image == null)
		{
			this.image = base.GetComponent<Image>();
		}
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0003C058 File Offset: 0x0003A258
	public UniTask Animate(float damageBarPostion, float damageBarWidth, Action onComplete)
	{
		HealthBar_DamageBar.<Animate>d__7 <Animate>d__;
		<Animate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Animate>d__.<>4__this = this;
		<Animate>d__.damageBarPostion = damageBarPostion;
		<Animate>d__.damageBarWidth = damageBarWidth;
		<Animate>d__.onComplete = onComplete;
		<Animate>d__.<>1__state = -1;
		<Animate>d__.<>t__builder.Start<HealthBar_DamageBar.<Animate>d__7>(ref <Animate>d__);
		return <Animate>d__.<>t__builder.Task;
	}

	// Token: 0x04000C60 RID: 3168
	[SerializeField]
	internal RectTransform rectTransform;

	// Token: 0x04000C61 RID: 3169
	[SerializeField]
	internal Image image;

	// Token: 0x04000C62 RID: 3170
	[SerializeField]
	private float duration;

	// Token: 0x04000C63 RID: 3171
	[SerializeField]
	private float targetSizeDelta = 4f;

	// Token: 0x04000C64 RID: 3172
	[SerializeField]
	private AnimationCurve curve;

	// Token: 0x04000C65 RID: 3173
	[SerializeField]
	private Gradient colorOverTime;
}
