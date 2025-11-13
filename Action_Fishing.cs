using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public class Action_Fishing : CharacterActionBase
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06000560 RID: 1376 RVA: 0x0001819C File Offset: 0x0001639C
	// (remove) Token: 0x06000561 RID: 1377 RVA: 0x000181D0 File Offset: 0x000163D0
	public static event Action<Action_Fishing, ICollection<Item>, Func<Item, bool>> OnPlayerStartSelectBait;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06000562 RID: 1378 RVA: 0x00018204 File Offset: 0x00016404
	// (remove) Token: 0x06000563 RID: 1379 RVA: 0x00018238 File Offset: 0x00016438
	public static event Action<Action_Fishing> OnPlayerStartFishing;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000564 RID: 1380 RVA: 0x0001826C File Offset: 0x0001646C
	// (remove) Token: 0x06000565 RID: 1381 RVA: 0x000182A0 File Offset: 0x000164A0
	public static event Action<Action_Fishing, float, Func<float>> OnPlayerStartCatching;

	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06000566 RID: 1382 RVA: 0x000182D4 File Offset: 0x000164D4
	// (remove) Token: 0x06000567 RID: 1383 RVA: 0x00018308 File Offset: 0x00016508
	public static event Action<Action_Fishing, Item, Action<bool>> OnPlayerStopCatching;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06000568 RID: 1384 RVA: 0x0001833C File Offset: 0x0001653C
	// (remove) Token: 0x06000569 RID: 1385 RVA: 0x00018370 File Offset: 0x00016570
	public static event Action<Action_Fishing> OnPlayerStopFishing;

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x0600056A RID: 1386 RVA: 0x000183A3 File Offset: 0x000165A3
	public Action_Fishing.FishingStates FishingState
	{
		get
		{
			return this.fishingState;
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x000183AB File Offset: 0x000165AB
	private void Awake()
	{
		this.fishingCamera.gameObject.SetActive(false);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x000183BE File Offset: 0x000165BE
	public override bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000183C1 File Offset: 0x000165C1
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Fishing;
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x000183C4 File Offset: 0x000165C4
	protected override bool OnStart()
	{
		if (!this.characterController)
		{
			return false;
		}
		this.fishingCamera.gameObject.SetActive(true);
		this.fishingRod = this.characterController.CurrentHoldItemAgent.GetComponent<FishingRod>();
		bool result = this.fishingRod != null;
		this.currentTask = this.Fishing();
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		InputManager.OnInteractButtonDown = (Action)Delegate.Combine(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
		UIInputManager.OnCancel += this.UIOnCancle;
		return result;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00018481 File Offset: 0x00016681
	private void OnCatchButton()
	{
		if (this.fishingState != Action_Fishing.FishingStates.catching)
		{
			return;
		}
		this.catchInput = true;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00018494 File Offset: 0x00016694
	private void UIOnCancle(UIInputEventData data)
	{
		data.Use();
		this.Quit();
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x000184A4 File Offset: 0x000166A4
	protected override void OnStop()
	{
		base.OnStop();
		this.fishingState = Action_Fishing.FishingStates.notStarted;
		Action<Action_Fishing> onPlayerStopFishing = Action_Fishing.OnPlayerStopFishing;
		if (onPlayerStopFishing != null)
		{
			onPlayerStopFishing(this);
		}
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
		this.fishingCamera.gameObject.SetActive(false);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00018511 File Offset: 0x00016711
	public override bool CanControlAim()
	{
		return false;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00018514 File Offset: 0x00016714
	public override bool CanMove()
	{
		return false;
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00018517 File Offset: 0x00016717
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0001851A File Offset: 0x0001671A
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001851D File Offset: 0x0001671D
	public override bool IsReady()
	{
		return true;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00018520 File Offset: 0x00016720
	private int NewToken()
	{
		this.fishingTaskToken++;
		this.fishingTaskToken %= 1000;
		return this.fishingTaskToken;
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00018548 File Offset: 0x00016748
	private UniTask Fishing()
	{
		Action_Fishing.<Fishing>d__48 <Fishing>d__;
		<Fishing>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Fishing>d__.<>4__this = this;
		<Fishing>d__.<>1__state = -1;
		<Fishing>d__.<>t__builder.Start<Action_Fishing.<Fishing>d__48>(ref <Fishing>d__);
		return <Fishing>d__.<>t__builder.Task;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0001858C File Offset: 0x0001678C
	private UniTask SingleFishingLoop(Func<bool> IsTaskValid)
	{
		Action_Fishing.<SingleFishingLoop>d__49 <SingleFishingLoop>d__;
		<SingleFishingLoop>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<SingleFishingLoop>d__.<>4__this = this;
		<SingleFishingLoop>d__.IsTaskValid = IsTaskValid;
		<SingleFishingLoop>d__.<>1__state = -1;
		<SingleFishingLoop>d__.<>t__builder.Start<Action_Fishing.<SingleFishingLoop>d__49>(ref <SingleFishingLoop>d__);
		return <SingleFishingLoop>d__.<>t__builder.Task;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x000185D7 File Offset: 0x000167D7
	private void ResultConfirm(bool _continueFishing)
	{
		this.resultConfirmed = true;
		this.continueFishing = _continueFishing;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x000185E8 File Offset: 0x000167E8
	private UniTask<bool> Catching(Func<bool> IsTaskValid)
	{
		Action_Fishing.<Catching>d__51 <Catching>d__;
		<Catching>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<Catching>d__.<>4__this = this;
		<Catching>d__.IsTaskValid = IsTaskValid;
		<Catching>d__.<>1__state = -1;
		<Catching>d__.<>t__builder.Start<Action_Fishing.<Catching>d__51>(ref <Catching>d__);
		return <Catching>d__.<>t__builder.Task;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00018634 File Offset: 0x00016834
	private UniTask<bool> WaitForSelectBait()
	{
		Action_Fishing.<WaitForSelectBait>d__52 <WaitForSelectBait>d__;
		<WaitForSelectBait>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<WaitForSelectBait>d__.<>4__this = this;
		<WaitForSelectBait>d__.<>1__state = -1;
		<WaitForSelectBait>d__.<>t__builder.Start<Action_Fishing.<WaitForSelectBait>d__52>(ref <WaitForSelectBait>d__);
		return <WaitForSelectBait>d__.<>t__builder.Task;
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00018678 File Offset: 0x00016878
	public List<Item> GetAllBaits()
	{
		List<Item> list = new List<Item>();
		if (!this.characterController)
		{
			return list;
		}
		foreach (Item item in this.characterController.CharacterItem.Inventory)
		{
			if (item.Tags.Contains(GameplayDataSettings.Tags.Bait))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x000186FC File Offset: 0x000168FC
	public void CatchButton()
	{
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x000186FE File Offset: 0x000168FE
	public void Quit()
	{
		Debug.Log("Quit");
		this.quit = true;
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00018714 File Offset: 0x00016914
	private bool SelectBaitAndStartFishing(Item _bait)
	{
		if (_bait == null)
		{
			Debug.Log("鱼饵选了个null, 退出");
			this.Quit();
			return false;
		}
		if (!_bait.Tags.Contains(GameplayDataSettings.Tags.Bait))
		{
			this.Quit();
			return false;
		}
		this.bait = _bait;
		return true;
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00018764 File Offset: 0x00016964
	private void OnDestroy()
	{
		if (base.Running)
		{
			Action<Action_Fishing> onPlayerStopFishing = Action_Fishing.OnPlayerStopFishing;
			if (onPlayerStopFishing != null)
			{
				onPlayerStopFishing(this);
			}
		}
		InputManager.OnInteractButtonDown = (Action)Delegate.Remove(InputManager.OnInteractButtonDown, new Action(this.OnCatchButton));
		UIInputManager.OnCancel -= this.UIOnCancle;
	}

	// Token: 0x040004D3 RID: 1235
	[SerializeField]
	private CinemachineVirtualCamera fishingCamera;

	// Token: 0x040004D4 RID: 1236
	private FishingRod fishingRod;

	// Token: 0x040004D5 RID: 1237
	[SerializeField]
	private FishingPoint fishingPoint;

	// Token: 0x040004D6 RID: 1238
	[SerializeField]
	private float introTime = 0.2f;

	// Token: 0x040004D7 RID: 1239
	private float fishingWaitTime = 2f;

	// Token: 0x040004D8 RID: 1240
	private float catchTime = 0.5f;

	// Token: 0x040004D9 RID: 1241
	private Item bait;

	// Token: 0x040004DA RID: 1242
	private Transform socket;

	// Token: 0x040004DB RID: 1243
	[SerializeField]
	[ItemTypeID]
	private int testCatchItem;

	// Token: 0x040004DC RID: 1244
	private Item catchedItem;

	// Token: 0x040004DD RID: 1245
	private bool quit;

	// Token: 0x040004DE RID: 1246
	private UniTask currentTask;

	// Token: 0x040004DF RID: 1247
	private bool catchInput;

	// Token: 0x040004E0 RID: 1248
	private bool resultConfirmed;

	// Token: 0x040004E1 RID: 1249
	private bool continueFishing;

	// Token: 0x040004E7 RID: 1255
	private Action_Fishing.FishingStates fishingState;

	// Token: 0x040004E8 RID: 1256
	private int fishingTaskToken;

	// Token: 0x02000454 RID: 1108
	public enum FishingStates
	{
		// Token: 0x04001AF6 RID: 6902
		notStarted,
		// Token: 0x04001AF7 RID: 6903
		intro,
		// Token: 0x04001AF8 RID: 6904
		selectingBait,
		// Token: 0x04001AF9 RID: 6905
		fishing,
		// Token: 0x04001AFA RID: 6906
		catching,
		// Token: 0x04001AFB RID: 6907
		over
	}
}
