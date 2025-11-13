using System;
using System.Collections.Generic;
using Duckov.Quests;
using NodeCanvas.DialogueTrees;
using NodeCanvas.StateMachines;
using Saves;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B2 RID: 434
public class CutScene : MonoBehaviour
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0003669E File Offset: 0x0003489E
	private string SaveKey
	{
		get
		{
			return "CutScene_" + this.id;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x000366B0 File Offset: 0x000348B0
	private bool UseTrigger
	{
		get
		{
			return this.playTiming == CutScene.PlayTiming.OnTriggerEnter;
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x000366BB File Offset: 0x000348BB
	private bool HideFSMOwnerField
	{
		get
		{
			return !this.fsmOwner && this.dialogueTreeOwner;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x000366D7 File Offset: 0x000348D7
	private bool HideDialogueTreeOwnerField
	{
		get
		{
			return this.fsmOwner && !this.dialogueTreeOwner;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000CEA RID: 3306 RVA: 0x000366F6 File Offset: 0x000348F6
	private bool Played
	{
		get
		{
			return SavesSystem.Load<bool>(this.SaveKey);
		}
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x00036703 File Offset: 0x00034903
	public void MarkPlayed()
	{
		if (string.IsNullOrWhiteSpace(this.id))
		{
			return;
		}
		SavesSystem.Save<bool>(this.SaveKey, true);
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0003671F File Offset: 0x0003491F
	private void OnEnable()
	{
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x00036721 File Offset: 0x00034921
	private void Awake()
	{
		if (this.UseTrigger)
		{
			this.InitializeTrigger();
		}
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00036734 File Offset: 0x00034934
	private void InitializeTrigger()
	{
		if (this.trigger == null)
		{
			Debug.LogError("CutScene想要使用Trigger触发，但没有配置Trigger引用。", this);
		}
		OnTriggerEnterEvent onTriggerEnterEvent = this.trigger.AddComponent<OnTriggerEnterEvent>();
		onTriggerEnterEvent.onlyMainCharacter = true;
		onTriggerEnterEvent.triggerOnce = true;
		onTriggerEnterEvent.DoOnTriggerEnter.AddListener(new UnityAction(this.PlayIfNessisary));
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00036789 File Offset: 0x00034989
	private void Start()
	{
		if (this.playTiming == CutScene.PlayTiming.Start)
		{
			this.PlayIfNessisary();
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0003679C File Offset: 0x0003499C
	private void Update()
	{
		if (this.playing)
		{
			if (this.fsmOwner)
			{
				if (!this.fsmOwner.isRunning)
				{
					this.playing = false;
					this.OnPlayFinished();
					return;
				}
			}
			else if (this.dialogueTreeOwner && !this.dialogueTreeOwner.isRunning)
			{
				this.playing = false;
				this.OnPlayFinished();
			}
		}
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00036800 File Offset: 0x00034A00
	private void OnPlayFinished()
	{
		this.MarkPlayed();
		if (this.setActiveFalseWhenFinished)
		{
			base.gameObject.SetActive(false);
		}
		if (this.playOnce && string.IsNullOrWhiteSpace(this.id))
		{
			Debug.LogError("CutScene没有填写ID，无法记录", base.gameObject);
		}
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0003684C File Offset: 0x00034A4C
	public void PlayIfNessisary()
	{
		if (this.playOnce && this.Played)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!this.prerequisites.Satisfied())
		{
			return;
		}
		this.Play();
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x00036880 File Offset: 0x00034A80
	public void Play()
	{
		if (this.fsmOwner)
		{
			this.fsmOwner.StartBehaviour();
			this.playing = true;
			return;
		}
		if (this.dialogueTreeOwner)
		{
			if (this.setupActorReferencesUsingIDs)
			{
				this.SetupActors();
			}
			this.dialogueTreeOwner.StartBehaviour();
			this.playing = true;
		}
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x000368DC File Offset: 0x00034ADC
	private void SetupActors()
	{
		if (this.dialogueTreeOwner == null)
		{
			return;
		}
		if (this.dialogueTreeOwner.behaviour == null)
		{
			Debug.LogError("Dialoguetree没有配置", this.dialogueTreeOwner);
			return;
		}
		foreach (DialogueTree.ActorParameter actorParameter in this.dialogueTreeOwner.behaviour.actorParameters)
		{
			string name = actorParameter.name;
			if (!string.IsNullOrEmpty(name))
			{
				DuckovDialogueActor duckovDialogueActor = DuckovDialogueActor.Get(name);
				if (duckovDialogueActor == null)
				{
					Debug.LogError("未找到actor ID:" + name);
				}
				else
				{
					this.dialogueTreeOwner.SetActorReference(name, duckovDialogueActor);
				}
			}
		}
	}

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	private string id;

	// Token: 0x04000B36 RID: 2870
	[SerializeField]
	private bool playOnce = true;

	// Token: 0x04000B37 RID: 2871
	[SerializeField]
	private bool setActiveFalseWhenFinished = true;

	// Token: 0x04000B38 RID: 2872
	[SerializeField]
	private bool setupActorReferencesUsingIDs;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	private Collider trigger;

	// Token: 0x04000B3A RID: 2874
	[SerializeField]
	private List<Condition> prerequisites = new List<Condition>();

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	private FSMOwner fsmOwner;

	// Token: 0x04000B3C RID: 2876
	[SerializeField]
	private DialogueTreeController dialogueTreeOwner;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private CutScene.PlayTiming playTiming;

	// Token: 0x04000B3E RID: 2878
	private bool playing;

	// Token: 0x020004D4 RID: 1236
	public enum PlayTiming
	{
		// Token: 0x04001D0F RID: 7439
		Start,
		// Token: 0x04001D10 RID: 7440
		OnTriggerEnter = 2,
		// Token: 0x04001D11 RID: 7441
		Manual
	}
}
