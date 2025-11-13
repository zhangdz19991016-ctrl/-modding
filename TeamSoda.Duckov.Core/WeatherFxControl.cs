using System;
using Duckov;
using Duckov.Scenes;
using Duckov.Weathers;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000198 RID: 408
public class WeatherFxControl : MonoBehaviour
{
	// Token: 0x06000C11 RID: 3089 RVA: 0x00033935 File Offset: 0x00031B35
	private void Start()
	{
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00033938 File Offset: 0x00031B38
	private void Init()
	{
		this.inited = true;
		this.rainingParticleRate = new float[this.rainyFxParticles.Length];
		for (int i = 0; i < this.rainyFxParticles.Length; i++)
		{
			ParticleSystem.EmissionModule emission = this.rainyFxParticles[i].emission;
			this.rainingParticleRate[i] = emission.rateOverTime.constant;
		}
		this.SetFxActive(false);
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x0003399E File Offset: 0x00031B9E
	private void OnSubSceneChanged()
	{
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000339A0 File Offset: 0x00031BA0
	private void Update()
	{
		if (!this.inited)
		{
			if (!LevelManager.Instance)
			{
				return;
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			this.Init();
			this.SetFxActive(false);
			return;
		}
		else
		{
			if (!TimeOfDayController.Instance)
			{
				return;
			}
			if (!MultiSceneCore.Instance)
			{
				return;
			}
			bool flag = TimeOfDayController.Instance.CurrentWeather == this.targetWeather;
			SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
			if (this.onlyOutDoor && subSceneInfo.IsInDoor)
			{
				flag = false;
				this.lerpValue = 0f;
			}
			if (flag)
			{
				this.overTimer = this.deactiveDelay;
				if (!this.fxActive)
				{
					this.SetFxActive(true);
				}
			}
			else if (this.lerpValue <= 0.01f)
			{
				this.overTimer -= Time.deltaTime;
				if (this.overTimer <= 0f)
				{
					this.SetFxActive(false);
				}
			}
			if (!this.fxActive)
			{
				return;
			}
			this.lerpValue = Mathf.MoveTowards(this.lerpValue, flag ? 1f : 0f, Time.deltaTime / this.lerpTime);
			for (int i = 0; i < this.rainyFxParticles.Length; i++)
			{
				ParticleSystem.EmissionModule emission = this.rainyFxParticles[i].emission;
				float b = this.rainingParticleRate[i];
				emission.rateOverTime = Mathf.Lerp(0f, b, this.lerpValue);
			}
			if (flag != this.audioPlaying)
			{
				this.audioPlaying = flag;
				if (flag)
				{
					this.weatherSoundInstace = AudioManager.Post(this.rainSoundKey, base.gameObject);
					return;
				}
				if (this.weatherSoundInstace != null)
				{
					this.weatherSoundInstace.Value.stop(STOP_MODE.ALLOWFADEOUT);
				}
			}
			return;
		}
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00033B4C File Offset: 0x00031D4C
	private void SetFxActive(bool active)
	{
		foreach (ParticleSystem particleSystem in this.rainyFxParticles)
		{
			if (!(particleSystem == null))
			{
				particleSystem.gameObject.SetActive(active);
			}
		}
		this.fxActive = active;
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00033B90 File Offset: 0x00031D90
	private void OnDestroy()
	{
		if (this.weatherSoundInstace != null)
		{
			this.weatherSoundInstace.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x04000A88 RID: 2696
	public ParticleSystem[] rainyFxParticles;

	// Token: 0x04000A89 RID: 2697
	[HideInInspector]
	public float[] rainingParticleRate;

	// Token: 0x04000A8A RID: 2698
	public Weather targetWeather;

	// Token: 0x04000A8B RID: 2699
	private float targetParticleRate;

	// Token: 0x04000A8C RID: 2700
	private float lerpValue;

	// Token: 0x04000A8D RID: 2701
	public float lerpTime = 5f;

	// Token: 0x04000A8E RID: 2702
	public float deactiveDelay = 10f;

	// Token: 0x04000A8F RID: 2703
	private float overTimer;

	// Token: 0x04000A90 RID: 2704
	private bool fxActive;

	// Token: 0x04000A91 RID: 2705
	private bool inited;

	// Token: 0x04000A92 RID: 2706
	private EventInstance? weatherSoundInstace;

	// Token: 0x04000A93 RID: 2707
	public string rainSoundKey = "Amb/amb_rain";

	// Token: 0x04000A94 RID: 2708
	private bool audioPlaying;

	// Token: 0x04000A95 RID: 2709
	[FormerlySerializedAs("onlyInDoor")]
	public bool onlyOutDoor = true;
}
