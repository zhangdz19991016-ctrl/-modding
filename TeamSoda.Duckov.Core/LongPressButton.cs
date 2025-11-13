using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public class LongPressButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler
{
	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0002ED3F File Offset: 0x0002CF3F
	private float TimeSincePressStarted
	{
		get
		{
			return Time.unscaledTime - this.timeWhenPressStarted;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0002ED4D File Offset: 0x0002CF4D
	private float Progress
	{
		get
		{
			if (!this.pressed)
			{
				return 0f;
			}
			return this.TimeSincePressStarted / this.pressTime;
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0002ED6A File Offset: 0x0002CF6A
	private void Update()
	{
		this.fill.fillAmount = this.Progress;
		if (this.pressed && this.Progress >= 1f)
		{
			UnityEvent unityEvent = this.onPressFullfilled;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.pressed = false;
		}
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0002EDAA File Offset: 0x0002CFAA
	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressed = true;
		this.timeWhenPressStarted = Time.unscaledTime;
		UnityEvent unityEvent = this.onPressStarted;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0002EDCE File Offset: 0x0002CFCE
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!this.pressed)
		{
			return;
		}
		this.pressed = false;
		UnityEvent unityEvent = this.onPressCanceled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0002EDF0 File Offset: 0x0002CFF0
	public void OnPointerUp(PointerEventData eventData)
	{
		if (!this.pressed)
		{
			return;
		}
		this.pressed = false;
		UnityEvent unityEvent = this.onPressCanceled;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04000953 RID: 2387
	[SerializeField]
	private Image fill;

	// Token: 0x04000954 RID: 2388
	[SerializeField]
	private float pressTime = 1f;

	// Token: 0x04000955 RID: 2389
	public UnityEvent onPressStarted;

	// Token: 0x04000956 RID: 2390
	public UnityEvent onPressCanceled;

	// Token: 0x04000957 RID: 2391
	public UnityEvent onPressFullfilled;

	// Token: 0x04000958 RID: 2392
	private float timeWhenPressStarted;

	// Token: 0x04000959 RID: 2393
	private bool pressed;
}
