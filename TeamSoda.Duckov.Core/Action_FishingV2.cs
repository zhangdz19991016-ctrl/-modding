using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Quests.Conditions;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A3 RID: 163
public class Action_FishingV2 : CharacterActionBase
{
	// Token: 0x06000585 RID: 1413 RVA: 0x0001880E File Offset: 0x00016A0E
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Fishing;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00018811 File Offset: 0x00016A11
	public override bool CanControlAim()
	{
		return false;
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00018814 File Offset: 0x00016A14
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00018817 File Offset: 0x00016A17
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0001881A File Offset: 0x00016A1A
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00018820 File Offset: 0x00016A20
	private void Awake()
	{
		this.interactable.OnInteractTimeoutEvent.AddListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractTimeOut));
		this.interactable.finishWhenTimeOut = false;
		this.fishingHudCanvas.gameObject.SetActive(false);
		this.baitVisual.gameObject.SetActive(false);
		this.baitTrail.gameObject.SetActive(false);
		this.dropParticle.SetActive(false);
		this.bucketParticle.SetActive(false);
		this.gotFx.SetActive(false);
		this.SyncInteractable(CharacterMainControl.Main);
		CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Combine(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeItemAgent));
		this.TransToNon();
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x000188DD File Offset: 0x00016ADD
	private void OnMainCharacterChangeItemAgent(CharacterMainControl character, DuckovItemAgent agent)
	{
		this.SyncInteractable(character);
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x000188E8 File Offset: 0x00016AE8
	private void SyncInteractable(CharacterMainControl character)
	{
		if (!character)
		{
			this.interactable.gameObject.SetActive(false);
			return;
		}
		DuckovItemAgent currentHoldItemAgent = character.CurrentHoldItemAgent;
		if (!currentHoldItemAgent)
		{
			this.interactable.gameObject.SetActive(false);
			return;
		}
		FishingRod component = currentHoldItemAgent.GetComponent<FishingRod>();
		this.interactable.gameObject.SetActive(component != null);
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00018950 File Offset: 0x00016B50
	private void SetWaveEmissionRate(float rate)
	{
		this.waveParticle.emission.rateOverTime = rate;
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00018978 File Offset: 0x00016B78
	private void OnDestroy()
	{
		if (this.interactable)
		{
			this.interactable.OnInteractTimeoutEvent.RemoveListener(new UnityAction<CharacterMainControl, InteractableBase>(this.OnInteractTimeOut));
		}
		CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent = (Action<CharacterMainControl, DuckovItemAgent>)Delegate.Remove(CharacterMainControl.OnMainCharacterChangeHoldItemAgentEvent, new Action<CharacterMainControl, DuckovItemAgent>(this.OnMainCharacterChangeItemAgent));
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x000189CE File Offset: 0x00016BCE
	public void TryCatch()
	{
		Debug.Log("TryCatch");
		if (this.fishingState == Action_FishingV2.FishingStates.waiting || this.fishingState == Action_FishingV2.FishingStates.ring)
		{
			this.catchInput = true;
		}
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x000189F3 File Offset: 0x00016BF3
	private void OnInteractTimeOut(CharacterMainControl target, InteractableBase interactable)
	{
		interactable.StopInteract();
		target.StartAction(this);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00018A03 File Offset: 0x00016C03
	public override bool IsReady()
	{
		return !base.Running;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00018A10 File Offset: 0x00016C10
	protected override bool OnStart()
	{
		if (this.characterController == null)
		{
			base.StopAction();
		}
		this.waitTime = UnityEngine.Random.Range(this.waitTimeRange.x, this.waitTimeRange.y);
		this.ringAnimator.SetInteger("State", 0);
		this.rodAgent = this.characterController.CurrentHoldItemAgent;
		if (!this.rodAgent)
		{
			this.characterController.PopText(this.noRodText.ToPlainText(), -1f);
			return false;
		}
		this.rod = this.rodAgent.GetComponent<FishingRod>();
		if (!this.rod)
		{
			this.characterController.PopText(this.noRodText.ToPlainText(), -1f);
			return false;
		}
		this.baitItem = this.rod.Bait;
		if (!this.baitItem)
		{
			this.characterController.PopText(this.noBaitText.ToPlainText(), -1f);
			return false;
		}
		this.characterController.characterModel.ForcePlayAttackAnimation();
		Vector3 direction = this.targetPoint.position - this.characterController.transform.position;
		direction.y = 0f;
		direction.Normalize();
		this.characterController.movementControl.ForceTurnTo(direction);
		this.fishingHudCanvas.worldCamera = Camera.main;
		this.fishingHudCanvas.gameObject.SetActive(true);
		this.hookStartPoint = this.rod.lineStart.position;
		this.TransToThrowing();
		return true;
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00018BA8 File Offset: 0x00016DA8
	protected override void OnStop()
	{
		this.TransToNon();
		this.fishingHudCanvas.gameObject.SetActive(false);
		this.ringAnimator.gameObject.SetActive(false);
		this.lineRenderer.gameObject.SetActive(false);
		this.baitVisual.gameObject.SetActive(false);
		this.gotFx.SetActive(false);
		this.SetWaveEmissionRate(0f);
		this.ringAnimator.SetInteger("State", 0);
		if (this.currentFish)
		{
			this.currentFish.DestroyTree();
			this.currentFish = null;
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00018C46 File Offset: 0x00016E46
	private void SpawnDropParticle()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.dropParticle, this.targetPoint).SetActive(true);
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00018C5F File Offset: 0x00016E5F
	private void SpawnBucketParticle()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.bucketParticle, this.bucketPoint).SetActive(true);
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x00018C78 File Offset: 0x00016E78
	private void OnDisable()
	{
		if (base.Running)
		{
			base.StopAction();
		}
		this.fishingHudCanvas.gameObject.SetActive(false);
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00018C9A File Offset: 0x00016E9A
	public override bool IsStopable()
	{
		return this.needStopAction;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00018CA4 File Offset: 0x00016EA4
	private Vector3 GetHookOutPos(float lerpValue)
	{
		lerpValue = Mathf.Clamp01(lerpValue);
		Vector3 vector = this.hookStartPoint;
		Vector3 position = this.targetPoint.position;
		Vector3 result = Vector3.Lerp(vector, position, lerpValue);
		float y = Mathf.LerpUnclamped(position.y, vector.y, this.outYCurve.Evaluate(lerpValue));
		result.y = y;
		return result;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x00018CFC File Offset: 0x00016EFC
	private Vector3 GetHookBackPos(float lerpValue)
	{
		lerpValue = Mathf.Clamp01(lerpValue);
		Vector3 position = this.rod.lineStart.position;
		Vector3 position2 = this.targetPoint.position;
		Vector3 result = Vector3.Lerp(position2, position, lerpValue);
		float y = Mathf.LerpUnclamped(position2.y, position.y, this.backYCurve.Evaluate(lerpValue));
		result.y = y;
		return result;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00018D5C File Offset: 0x00016F5C
	protected override void OnUpdateAction(float deltaTime)
	{
		if (!this.characterController || !this.rod)
		{
			this.needStopAction = true;
			base.StopAction();
			return;
		}
		this.lineRenderer.SetPosition(0, this.rod.lineStart.position);
		Vector3 position = this.rod.lineStart.position;
		this.needStopAction = false;
		if (this.rod == null)
		{
			this.needStopAction = true;
			base.StopAction();
			return;
		}
		switch (this.fishingState)
		{
		case Action_FishingV2.FishingStates.throwing:
			if (!this.baitItem || this.catchInput)
			{
				this.TransToCancleBack();
			}
			else if (this.stateTimer < this.throwStartTime)
			{
				this.hookStartPoint = this.rod.lineStart.position;
				position = this.hookStartPoint;
				this.baitTrail.Clear();
			}
			else if (this.stateTimer < this.outTime)
			{
				position = this.GetHookOutPos((this.stateTimer - this.throwStartTime) / (this.outTime - this.throwStartTime));
				if (!this.baitVisual.gameObject.activeInHierarchy)
				{
					this.baitVisual.gameObject.SetActive(true);
					this.baitTrail.gameObject.SetActive(true);
				}
				this.baitVisual.transform.position = position;
				this.baitTrail.transform.position = position;
			}
			else
			{
				this.TransToWaiting();
			}
			break;
		case Action_FishingV2.FishingStates.waiting:
			if (this.catchInput)
			{
				this.TransToCancleBack();
			}
			else
			{
				position = this.targetPoint.position;
				this.baitVisual.transform.position = position;
				this.baitTrail.transform.position = position;
				if (this.stateTimer >= this.waitTime)
				{
					if (this.currentFish != null)
					{
						this.TransToRing();
					}
					else
					{
						this.characterController.PopText("Error:Spawn fish failed", -1f);
						this.TransToCancleBack();
					}
				}
				if (this.waitTime - this.stateTimer < 0.25f && !this.hookFxSpawned)
				{
					this.hookFxSpawned = true;
					this.SpawnHookFx();
				}
			}
			break;
		case Action_FishingV2.FishingStates.ring:
		{
			position = this.targetPoint.position;
			float num = Mathf.Lerp(this.scaleRange.y, this.scaleRange.x, 1f - this.stateTimer / this.scaleTime);
			this.scaleRing.localScale = Vector3.one * num;
			if (this.catchInput)
			{
				if (num < this.successRange.x || num > this.successRange.y)
				{
					this.TransToFailBack();
					break;
				}
				this.TransToSuccessback();
			}
			if (this.stateTimer > this.scaleTime)
			{
				this.TransToFailBack();
			}
			break;
		}
		case Action_FishingV2.FishingStates.cancleBack:
			position = this.GetHookBackPos(this.stateTimer / this.backTime);
			this.baitVisual.transform.position = position;
			this.baitTrail.transform.position = position;
			if (this.stateTimer > this.backTime)
			{
				this.needStopAction = true;
			}
			break;
		case Action_FishingV2.FishingStates.successBack:
		{
			float num2 = 0.2f;
			if (this.stateTimer >= num2)
			{
				position = this.GetHookBackPos((this.stateTimer - num2) / this.backTime);
				this.baitVisual.transform.position = position;
				this.baitTrail.transform.position = position;
				if (this.stateTimer - num2 > this.backTime)
				{
					this.needStopAction = true;
				}
			}
			break;
		}
		case Action_FishingV2.FishingStates.failBack:
			position = this.GetHookBackPos(this.stateTimer / this.backTime);
			this.baitVisual.transform.position = position;
			this.baitTrail.transform.position = position;
			if (this.stateTimer > this.backTime)
			{
				NotificationText.Push(this.failText.ToPlainText());
				this.needStopAction = true;
			}
			break;
		}
		this.lineRenderer.SetPosition(1, position);
		this.catchInput = false;
		this.stateTimer += deltaTime;
		if (this.needStopAction)
		{
			this.baitVisual.gameObject.SetActive(false);
			this.baitTrail.gameObject.SetActive(false);
			this.baitTrail.Clear();
			base.StopAction();
		}
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x000191C7 File Offset: 0x000173C7
	private void TransToNon()
	{
		this.fishingState = Action_FishingV2.FishingStates.non;
		this.SetWaveEmissionRate(0f);
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x000191DC File Offset: 0x000173DC
	private void TransToThrowing()
	{
		AudioManager.Post(this.throwSoundKey, base.gameObject);
		this.stateTimer = 0f;
		this.lineRenderer.gameObject.SetActive(true);
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, this.rod.lineStart.position);
		this.lineRenderer.SetPosition(1, this.rod.lineStart.position);
		this.ringAnimator.gameObject.SetActive(true);
		this.ringAnimator.SetInteger("State", 0);
		this.fishingState = Action_FishingV2.FishingStates.throwing;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00019284 File Offset: 0x00017484
	private void TransToWaiting()
	{
		if (this.baitItem == null)
		{
			this.needStopAction = true;
			base.StopAction();
		}
		AudioManager.Post(this.startFishingSoundKey, this.targetPoint.gameObject);
		this.hookFxSpawned = false;
		this.SpawnDropParticle();
		this.SetWaveEmissionRate(1.5f);
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 0);
		this.luck = this.characterController.CharacterItem.GetStatValue(this.fishingQualityFactorHash);
		this.SpawnFish(this.luck).Forget();
		this.fishingState = Action_FishingV2.FishingStates.waiting;
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00019330 File Offset: 0x00017530
	private void SpawnHookFx()
	{
		if (this.hookFx == null)
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(this.hookFx, this.targetPoint.position + Vector3.up * 3f, Quaternion.identity);
		AudioManager.Post(this.baitSoundKey, this.targetPoint.gameObject);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00019394 File Offset: 0x00017594
	private void TransToRing()
	{
		this.scaleTime = this.characterController.CharacterItem.GetStatValue(this.fishingTimeHash) * this.scaleTimeFactor;
		this.scaleTime = Mathf.Max(0.01f, this.scaleTime);
		float num = this.currentFish.GetStatValue(this.fishingDifficultyHash);
		if (num < 0.02f)
		{
			num = 1f;
		}
		this.scaleTime /= num;
		if (this.scaleTime > 7f)
		{
			this.scaleTime = 7f;
		}
		this.stateTimer = 0f;
		this.catchInput = false;
		this.fishingState = Action_FishingV2.FishingStates.ring;
		this.ringAnimator.SetInteger("State", 1);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0001944C File Offset: 0x0001764C
	private void TransToCancleBack()
	{
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 0);
		this.fishingState = Action_FishingV2.FishingStates.cancleBack;
		this.SetWaveEmissionRate(0f);
		this.SpawnDropParticle();
		this.fishingHudCanvas.gameObject.SetActive(false);
		AudioManager.Post(this.pulloutSoundKey, this.targetPoint.gameObject);
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x000194B8 File Offset: 0x000176B8
	private void TransToSuccessback()
	{
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 2);
		AudioManager.Post(this.successSoundKey, this.targetPoint.gameObject);
		this.fishingState = Action_FishingV2.FishingStates.successBack;
		this.SetWaveEmissionRate(0f);
		this.SpawnDropParticle();
		this.gotFx.SetActive(true);
		this.fishingHudCanvas.gameObject.SetActive(false);
		this.CatchFish().Forget();
		RequireHasFished.SetHasFished();
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00019540 File Offset: 0x00017740
	private void TransToFailBack()
	{
		this.stateTimer = 0f;
		this.ringAnimator.SetInteger("State", 3);
		AudioManager.Post(this.failSoundKey, this.targetPoint.gameObject);
		this.fishingState = Action_FishingV2.FishingStates.failBack;
		this.SetWaveEmissionRate(0f);
		this.SpawnDropParticle();
		this.fishingHudCanvas.gameObject.SetActive(false);
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x000195AC File Offset: 0x000177AC
	private UniTaskVoid SpawnFish(float luck)
	{
		Action_FishingV2.<SpawnFish>d__85 <SpawnFish>d__;
		<SpawnFish>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnFish>d__.<>4__this = this;
		<SpawnFish>d__.luck = luck;
		<SpawnFish>d__.<>1__state = -1;
		<SpawnFish>d__.<>t__builder.Start<Action_FishingV2.<SpawnFish>d__85>(ref <SpawnFish>d__);
		return <SpawnFish>d__.<>t__builder.Task;
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x000195F8 File Offset: 0x000177F8
	private UniTaskVoid CatchFish()
	{
		Action_FishingV2.<CatchFish>d__86 <CatchFish>d__;
		<CatchFish>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CatchFish>d__.<>4__this = this;
		<CatchFish>d__.<>1__state = -1;
		<CatchFish>d__.<>t__builder.Start<Action_FishingV2.<CatchFish>d__86>(ref <CatchFish>d__);
		return <CatchFish>d__.<>t__builder.Task;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0001963B File Offset: 0x0001783B
	public override bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x040004E9 RID: 1257
	public InteractableBase interactable;

	// Token: 0x040004EA RID: 1258
	public Transform baitVisual;

	// Token: 0x040004EB RID: 1259
	public TrailRenderer baitTrail;

	// Token: 0x040004EC RID: 1260
	public Canvas fishingHudCanvas;

	// Token: 0x040004ED RID: 1261
	public Transform targetPoint;

	// Token: 0x040004EE RID: 1262
	public Transform bucketPoint;

	// Token: 0x040004EF RID: 1263
	[LocalizationKey("Default")]
	public string noRodText = "Pop_NoRod";

	// Token: 0x040004F0 RID: 1264
	[LocalizationKey("Default")]
	public string noBaitText = "Pop_NoBait";

	// Token: 0x040004F1 RID: 1265
	[LocalizationKey("Default")]
	public string gotFishText = "Notify_GotFish";

	// Token: 0x040004F2 RID: 1266
	[LocalizationKey("Default")]
	public string failText = "Notify_FishRunAway";

	// Token: 0x040004F3 RID: 1267
	private FishingRod rod;

	// Token: 0x040004F4 RID: 1268
	private ItemAgent rodAgent;

	// Token: 0x040004F5 RID: 1269
	private Item baitItem;

	// Token: 0x040004F6 RID: 1270
	public Animator ringAnimator;

	// Token: 0x040004F7 RID: 1271
	public Vector2 waitTimeRange = new Vector2(3f, 9f);

	// Token: 0x040004F8 RID: 1272
	private float waitTime;

	// Token: 0x040004F9 RID: 1273
	public Vector2 scaleRange = new Vector2(0.5f, 3f);

	// Token: 0x040004FA RID: 1274
	public Vector2 successRange = new Vector2(0.75f, 1.1f);

	// Token: 0x040004FB RID: 1275
	private float ringScaling = 2.5f;

	// Token: 0x040004FC RID: 1276
	private float stateTimer;

	// Token: 0x040004FD RID: 1277
	private bool catchInput;

	// Token: 0x040004FE RID: 1278
	public Transform scaleRing;

	// Token: 0x040004FF RID: 1279
	public LineRenderer lineRenderer;

	// Token: 0x04000500 RID: 1280
	public float throwStartTime = 0.1f;

	// Token: 0x04000501 RID: 1281
	public float outTime;

	// Token: 0x04000502 RID: 1282
	public AnimationCurve outYCurve;

	// Token: 0x04000503 RID: 1283
	public ParticleSystem waveParticle;

	// Token: 0x04000504 RID: 1284
	public GameObject dropParticle;

	// Token: 0x04000505 RID: 1285
	public GameObject bucketParticle;

	// Token: 0x04000506 RID: 1286
	public InteractableLootbox lootbox;

	// Token: 0x04000507 RID: 1287
	private bool hookFxSpawned;

	// Token: 0x04000508 RID: 1288
	public GameObject hookFx;

	// Token: 0x04000509 RID: 1289
	public float backTime;

	// Token: 0x0400050A RID: 1290
	public AnimationCurve backYCurve;

	// Token: 0x0400050B RID: 1291
	private Vector3 hookStartPoint;

	// Token: 0x0400050C RID: 1292
	public GameObject gotFx;

	// Token: 0x0400050D RID: 1293
	public FishSpawner lootSpawner;

	// Token: 0x0400050E RID: 1294
	private Item currentFish;

	// Token: 0x0400050F RID: 1295
	private float luck = 1f;

	// Token: 0x04000510 RID: 1296
	private float scaleTime;

	// Token: 0x04000511 RID: 1297
	private float scaleTimeFactor = 1.25f;

	// Token: 0x04000512 RID: 1298
	private int fishingTimeHash = "FishingTime".GetHashCode();

	// Token: 0x04000513 RID: 1299
	private int fishingDifficultyHash = "FishingDifficulty".GetHashCode();

	// Token: 0x04000514 RID: 1300
	private int fishingQualityFactorHash = "FishingQualityFactor".GetHashCode();

	// Token: 0x04000515 RID: 1301
	private Slot characterMeleeWeaponSlot;

	// Token: 0x04000516 RID: 1302
	private string currentStateInfo;

	// Token: 0x04000517 RID: 1303
	private string throwSoundKey = "SFX/Actions/Fishing_Throw";

	// Token: 0x04000518 RID: 1304
	private string startFishingSoundKey = "SFX/Actions/Fishing_Start";

	// Token: 0x04000519 RID: 1305
	private string pulloutSoundKey = "SFX/Actions/Fishing_PullOut";

	// Token: 0x0400051A RID: 1306
	private string baitSoundKey = "SFX/Actions/Fishing_Bait";

	// Token: 0x0400051B RID: 1307
	private string successSoundKey = "SFX/Actions/Fishing_Success";

	// Token: 0x0400051C RID: 1308
	private string failSoundKey = "SFX/Actions/Fishing_Failed";

	// Token: 0x0400051D RID: 1309
	public Action_FishingV2.FishingStates fishingState = Action_FishingV2.FishingStates.waiting;

	// Token: 0x0400051E RID: 1310
	private bool needStopAction;

	// Token: 0x0200045B RID: 1115
	public enum FishingStates
	{
		// Token: 0x04001B18 RID: 6936
		non,
		// Token: 0x04001B19 RID: 6937
		throwing,
		// Token: 0x04001B1A RID: 6938
		waiting,
		// Token: 0x04001B1B RID: 6939
		ring,
		// Token: 0x04001B1C RID: 6940
		cancleBack,
		// Token: 0x04001B1D RID: 6941
		successBack,
		// Token: 0x04001B1E RID: 6942
		failBack
	}
}
