using System;
using Duckov;
using Duckov.Utilities;
using FMOD.Studio;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class ItemAgent_Kazoo : DuckovItemAgent
{
	// Token: 0x060007B4 RID: 1972 RVA: 0x00022BE0 File Offset: 0x00020DE0
	private void Update()
	{
		if (!base.Holder)
		{
			return;
		}
		if (!this.camera)
		{
			if (GameCamera.Instance)
			{
				this.camera = GameCamera.Instance.renderCamera;
			}
			if (!this.camera)
			{
				return;
			}
		}
		if (!this.holderInited)
		{
			base.Holder.OnTriggerInputUpdateEvent += this.OnTriggerUpdate;
			this.uiInstance = UnityEngine.Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.KazooUi, base.Holder.transform.position, quaternion.identity);
			this.uiInstance.transform.localScale = Vector3.one * 2f * this.maxScale;
			this.SyncUi(base.Holder.transform);
			this.holderInited = true;
		}
		if (this.targetMakingSound != this.currentMakingSound)
		{
			this.makeAiSoundCoolTimer = this.makeAiSoundCoolTime;
			this.currentMakingSound = this.targetMakingSound;
			if (this.currentMakingSound)
			{
				if (this.currentEvent != null)
				{
					this.currentEvent.Value.stop(STOP_MODE.ALLOWFADEOUT);
				}
				this.currentEvent = AudioManager.Post(this.audioEvent, base.gameObject);
				if (this.particle)
				{
					this.particle.Emit(1);
				}
			}
			else if (this.currentEvent != null)
			{
				this.currentEvent.Value.stop(STOP_MODE.ALLOWFADEOUT);
			}
		}
		if (this.currentMakingSound)
		{
			Vector3 right = this.camera.transform.right;
			right.y = 0f;
			right.Normalize();
			Vector3 position = base.Holder.transform.position;
			Vector3 rhs = base.Holder.GetCurrentAimPoint() - position;
			rhs.y = 0f;
			float value = Vector3.Dot(right, rhs) * 24f / this.maxScale;
			AudioManager.SetRTPC("Kazoo/Pitch", value, base.gameObject);
			AudioManager.SetRTPC("Kazoo/Intensity", 1f, base.gameObject);
			this.makeAiSoundCoolTimer -= Time.deltaTime;
			if (this.makeAiSoundCoolTimer <= 0f)
			{
				this.makeAiSoundCoolTimer = this.makeAiSoundCoolTime;
				AIMainBrain.MakeSound(new AISound
				{
					fromCharacter = base.Holder,
					fromObject = base.gameObject,
					pos = base.transform.position,
					fromTeam = base.Holder.Team,
					soundType = SoundTypes.unknowNoise,
					radius = this.maxSoundRange
				});
			}
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00022E94 File Offset: 0x00021094
	private void LateUpdate()
	{
		if (base.Holder)
		{
			this.SyncUi(base.Holder.transform);
		}
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00022EB4 File Offset: 0x000210B4
	public void OnTriggerUpdate(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		this.targetMakingSound = trigger;
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00022EBD File Offset: 0x000210BD
	protected override void OnInitialize()
	{
		base.OnInitialize();
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00022EC8 File Offset: 0x000210C8
	private void SyncUi(Transform parent)
	{
		if (!this.uiInstance || !parent)
		{
			return;
		}
		Vector3 forward = this.camera.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		this.uiInstance.transform.position = parent.position - forward * this.zOffset;
		this.uiInstance.transform.rotation = quaternion.LookRotation(forward, Vector3.up);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00022F60 File Offset: 0x00021160
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.uiInstance)
		{
			UnityEngine.Object.Destroy(this.uiInstance.gameObject);
		}
		if (base.Holder)
		{
			base.Holder.OnTriggerInputUpdateEvent -= this.OnTriggerUpdate;
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00022FB4 File Offset: 0x000211B4
	private void OnDisable()
	{
		if (this.currentEvent != null)
		{
			this.currentEvent.Value.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x04000746 RID: 1862
	private string audioEvent = "SFX/Special/Kazoo";

	// Token: 0x04000747 RID: 1863
	private EventInstance? currentEvent;

	// Token: 0x04000748 RID: 1864
	private bool currentMakingSound;

	// Token: 0x04000749 RID: 1865
	private bool targetMakingSound;

	// Token: 0x0400074A RID: 1866
	private float makeAiSoundCoolTimer = 0.15f;

	// Token: 0x0400074B RID: 1867
	private float makeAiSoundCoolTime = 0.15f;

	// Token: 0x0400074C RID: 1868
	public float maxScale = 15f;

	// Token: 0x0400074D RID: 1869
	public float maxSoundRange = 18f;

	// Token: 0x0400074E RID: 1870
	private Camera camera;

	// Token: 0x0400074F RID: 1871
	private bool holderInited;

	// Token: 0x04000750 RID: 1872
	private GameObject uiInstance;

	// Token: 0x04000751 RID: 1873
	private float zOffset = 6f;

	// Token: 0x04000752 RID: 1874
	[SerializeField]
	private ParticleSystem particle;
}
