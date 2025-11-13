using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200012D RID: 301
public class SceneLoadingEventsReceiver : MonoBehaviour
{
	// Token: 0x060009EB RID: 2539 RVA: 0x0002AFE2 File Offset: 0x000291E2
	private void OnEnable()
	{
		SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
		SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0002B006 File Offset: 0x00029206
	private void OnDisable()
	{
		SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
		SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x0002B02A File Offset: 0x0002922A
	private void OnStartedLoadingScene(SceneLoadingContext context)
	{
		UnityEvent unityEvent = this.onStartLoadingScene;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0002B03C File Offset: 0x0002923C
	private void OnFinishedLoadingScene(SceneLoadingContext context)
	{
		UnityEvent unityEvent = this.onFinishedLoadingScene;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040008B4 RID: 2228
	public UnityEvent onStartLoadingScene;

	// Token: 0x040008B5 RID: 2229
	public UnityEvent onFinishedLoadingScene;
}
