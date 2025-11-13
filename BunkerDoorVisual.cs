using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class BunkerDoorVisual : MonoBehaviour
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x000311FC File Offset: 0x0002F3FC
	private void Awake()
	{
		this.animator.SetBool("InRange", this.inRange);
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00031214 File Offset: 0x0002F414
	public void OnEnter()
	{
		if (this.inRange)
		{
			return;
		}
		this.inRange = true;
		this.animator.SetBool("InRange", this.inRange);
		this.PopText(this.welcomeText.ToPlainText(), 0.5f, this.inRange).Forget();
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00031268 File Offset: 0x0002F468
	public void OnExit()
	{
		if (!this.inRange)
		{
			return;
		}
		this.inRange = false;
		this.animator.SetBool("InRange", this.inRange);
		this.PopText(this.leaveText.ToPlainText(), 0f, this.inRange).Forget();
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x000312BC File Offset: 0x0002F4BC
	private UniTask PopText(string text, float delay, bool _inRange)
	{
		BunkerDoorVisual.<PopText>d__8 <PopText>d__;
		<PopText>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<PopText>d__.<>4__this = this;
		<PopText>d__.text = text;
		<PopText>d__.delay = delay;
		<PopText>d__._inRange = _inRange;
		<PopText>d__.<>1__state = -1;
		<PopText>d__.<>t__builder.Start<BunkerDoorVisual.<PopText>d__8>(ref <PopText>d__);
		return <PopText>d__.<>t__builder.Task;
	}

	// Token: 0x040009D5 RID: 2517
	[LocalizationKey("Dialogues")]
	public string welcomeText;

	// Token: 0x040009D6 RID: 2518
	[LocalizationKey("Dialogues")]
	public string leaveText;

	// Token: 0x040009D7 RID: 2519
	public Transform textBubblePoint;

	// Token: 0x040009D8 RID: 2520
	public bool inRange = true;

	// Token: 0x040009D9 RID: 2521
	public Animator animator;
}
