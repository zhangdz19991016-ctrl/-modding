using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.UI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000110 RID: 272
public class CountDownArea : MonoBehaviour
{
	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000953 RID: 2387 RVA: 0x00029A76 File Offset: 0x00027C76
	public float RequiredExtrationTime
	{
		get
		{
			return this.requiredExtrationTime;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000954 RID: 2388 RVA: 0x00029A7E File Offset: 0x00027C7E
	private float TimeSinceCountDownBegan
	{
		get
		{
			return Time.time - this.timeWhenCountDownBegan;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000955 RID: 2389 RVA: 0x00029A8C File Offset: 0x00027C8C
	public float RemainingTime
	{
		get
		{
			return Mathf.Clamp(this.RequiredExtrationTime - this.TimeSinceCountDownBegan, 0f, this.RequiredExtrationTime);
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x00029AAB File Offset: 0x00027CAB
	public float Progress
	{
		get
		{
			if (this.requiredExtrationTime <= 0f)
			{
				return 1f;
			}
			return this.TimeSinceCountDownBegan / this.RequiredExtrationTime;
		}
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x00029AD0 File Offset: 0x00027CD0
	private void OnTriggerEnter(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (component == null)
		{
			return;
		}
		if (component.IsMainCharacter())
		{
			this.hoveringMainCharacters.Add(component);
			this.OnHoveringMainCharactersChanged();
		}
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x00029B14 File Offset: 0x00027D14
	private void OnTriggerExit(Collider other)
	{
		if (!base.enabled)
		{
			return;
		}
		CharacterMainControl component = other.GetComponent<CharacterMainControl>();
		if (component == null)
		{
			return;
		}
		if (component.IsMainCharacter())
		{
			this.hoveringMainCharacters.Remove(component);
			this.OnHoveringMainCharactersChanged();
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x00029B56 File Offset: 0x00027D56
	private void OnHoveringMainCharactersChanged()
	{
		if (!this.countingDown && this.hoveringMainCharacters.Count > 0)
		{
			this.BeginCountDown();
			return;
		}
		if (this.countingDown && this.hoveringMainCharacters.Count < 1)
		{
			this.AbortCountDown();
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00029B91 File Offset: 0x00027D91
	private void BeginCountDown()
	{
		this.countingDown = true;
		this.timeWhenCountDownBegan = Time.time;
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStarted;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(this);
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00029BB6 File Offset: 0x00027DB6
	private void AbortCountDown()
	{
		this.countingDown = false;
		this.timeWhenCountDownBegan = float.MaxValue;
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStopped;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(this);
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x00029BDC File Offset: 0x00027DDC
	private void UpdateCountDown()
	{
		if (this.hoveringMainCharacters.All((CharacterMainControl e) => e.Health.IsDead))
		{
			this.AbortCountDown();
		}
		if (this.TimeSinceCountDownBegan >= this.RequiredExtrationTime)
		{
			this.OnCountdownSucceed();
		}
		int num = (int)(this.RemainingTime + Time.deltaTime);
		if ((int)this.RemainingTime != num)
		{
			UnityEvent unityEvent = this.onTickSecond;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x00029C57 File Offset: 0x00027E57
	private void OnCountdownSucceed()
	{
		UnityEvent<CountDownArea> unityEvent = this.onCountDownStopped;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		UnityEvent unityEvent2 = this.onCountDownSucceed;
		if (unityEvent2 != null)
		{
			unityEvent2.Invoke();
		}
		this.countingDown = false;
		if (this.disableWhenSucceed)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00029C92 File Offset: 0x00027E92
	private void Update()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.countingDown && View.ActiveView == null)
		{
			this.UpdateCountDown();
		}
	}

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	private float requiredExtrationTime = 5f;

	// Token: 0x04000860 RID: 2144
	[SerializeField]
	private bool disableWhenSucceed = true;

	// Token: 0x04000861 RID: 2145
	public UnityEvent onCountDownSucceed;

	// Token: 0x04000862 RID: 2146
	public UnityEvent onTickSecond;

	// Token: 0x04000863 RID: 2147
	public UnityEvent<CountDownArea> onCountDownStarted;

	// Token: 0x04000864 RID: 2148
	public UnityEvent<CountDownArea> onCountDownStopped;

	// Token: 0x04000865 RID: 2149
	private bool countingDown;

	// Token: 0x04000866 RID: 2150
	private float timeWhenCountDownBegan = float.MaxValue;

	// Token: 0x04000867 RID: 2151
	private HashSet<CharacterMainControl> hoveringMainCharacters = new HashSet<CharacterMainControl>();
}
