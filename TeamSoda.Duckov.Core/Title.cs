using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

// Token: 0x0200016B RID: 363
public class Title : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000B0D RID: 2829 RVA: 0x0002FAEC File Offset: 0x0002DCEC
	private void Start()
	{
		this.StartTask().Forget();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0002FAFC File Offset: 0x0002DCFC
	private UniTask StartTask()
	{
		Title.<StartTask>d__5 <StartTask>d__;
		<StartTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<StartTask>d__.<>4__this = this;
		<StartTask>d__.<>1__state = -1;
		<StartTask>d__.<>t__builder.Start<Title.<StartTask>d__5>(ref <StartTask>d__);
		return <StartTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x0002FB40 File Offset: 0x0002DD40
	private UniTask ContinueTask()
	{
		Title.<ContinueTask>d__6 <ContinueTask>d__;
		<ContinueTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<ContinueTask>d__.<>4__this = this;
		<ContinueTask>d__.<>1__state = -1;
		<ContinueTask>d__.<>t__builder.Start<Title.<ContinueTask>d__6>(ref <ContinueTask>d__);
		return <ContinueTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0002FB84 File Offset: 0x0002DD84
	private UniTask WaitForTimeline(PlayableDirector timeline)
	{
		Title.<WaitForTimeline>d__7 <WaitForTimeline>d__;
		<WaitForTimeline>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<WaitForTimeline>d__.timeline = timeline;
		<WaitForTimeline>d__.<>1__state = -1;
		<WaitForTimeline>d__.<>t__builder.Start<Title.<WaitForTimeline>d__7>(ref <WaitForTimeline>d__);
		return <WaitForTimeline>d__.<>t__builder.Task;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x0002FBC7 File Offset: 0x0002DDC7
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.fadeGroup.IsShown)
		{
			this.ContinueTask().Forget();
		}
	}

	// Token: 0x04000992 RID: 2450
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000993 RID: 2451
	[SerializeField]
	private PlayableDirector timelineToTitle;

	// Token: 0x04000994 RID: 2452
	[SerializeField]
	private PlayableDirector timelineToMainMenu;

	// Token: 0x04000995 RID: 2453
	private string sfx_PressStart = "UI/game_start";
}
