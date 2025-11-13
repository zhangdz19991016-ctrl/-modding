using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using ItemStatsSystem;
using ItemStatsSystem.Data;
using ItemStatsSystem.Items;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000284 RID: 644
	public class GamingConsole : InteractableBase
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x0004CB93 File Offset: 0x0004AD93
		public MiniGame SelectedGame
		{
			get
			{
				if (this.CatridgeGameID == null)
				{
					return null;
				}
				return this.possibleGames.Find((MiniGame e) => e != null && e.ID == this.CatridgeGameID);
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0004CBB6 File Offset: 0x0004ADB6
		public MiniGame Game
		{
			get
			{
				return this.game;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x0004CBBE File Offset: 0x0004ADBE
		public Slot MonitorSlot
		{
			get
			{
				return this.mainItem.Slots["Monitor"];
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0004CBD5 File Offset: 0x0004ADD5
		public Slot ConsoleSlot
		{
			get
			{
				return this.mainItem.Slots["Console"];
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0004CBEC File Offset: 0x0004ADEC
		public bool controllerConnected
		{
			get
			{
				if (this.mainItem == null)
				{
					return false;
				}
				if (this.ConsoleSlot == null)
				{
					return false;
				}
				Item content = this.ConsoleSlot.Content;
				if (content == null)
				{
					return false;
				}
				Slot slot = content.Slots["FcController"];
				return slot != null && slot.Content != null;
			}
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06001499 RID: 5273 RVA: 0x0004CC4C File Offset: 0x0004AE4C
		// (remove) Token: 0x0600149A RID: 5274 RVA: 0x0004CC84 File Offset: 0x0004AE84
		public event Action<GamingConsole> onContentChanged;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x0600149B RID: 5275 RVA: 0x0004CCBC File Offset: 0x0004AEBC
		// (remove) Token: 0x0600149C RID: 5276 RVA: 0x0004CCF4 File Offset: 0x0004AEF4
		public event Action<GamingConsole> OnAfterAnimateIn;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x0600149D RID: 5277 RVA: 0x0004CD2C File Offset: 0x0004AF2C
		// (remove) Token: 0x0600149E RID: 5278 RVA: 0x0004CD64 File Offset: 0x0004AF64
		public event Action<GamingConsole> OnBeforeAnimateOut;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x0600149F RID: 5279 RVA: 0x0004CD9C File Offset: 0x0004AF9C
		// (remove) Token: 0x060014A0 RID: 5280 RVA: 0x0004CDD0 File Offset: 0x0004AFD0
		public static event Action<bool> OnGamingConsoleInteractChanged;

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0004CE03 File Offset: 0x0004B003
		public Item Monitor
		{
			get
			{
				if (this.MonitorSlot == null)
				{
					return null;
				}
				return this.MonitorSlot.Content;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060014A2 RID: 5282 RVA: 0x0004CE1A File Offset: 0x0004B01A
		public Item Console
		{
			get
			{
				if (this.ConsoleSlot == null)
				{
					return null;
				}
				return this.ConsoleSlot.Content;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0004CE34 File Offset: 0x0004B034
		public Item Cartridge
		{
			get
			{
				if (this.Console == null)
				{
					return null;
				}
				if (!this.Console.Slots)
				{
					Debug.LogError(this.Console.DisplayName + " has no catridge slot");
					return null;
				}
				Slot slot = this.Console.Slots["Cartridge"];
				if (slot == null)
				{
					Debug.LogError(this.Console.DisplayName + " has no catridge slot");
					return null;
				}
				return slot.Content;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060014A4 RID: 5284 RVA: 0x0004CEBA File Offset: 0x0004B0BA
		public string CatridgeGameID
		{
			get
			{
				if (this.Cartridge == null)
				{
					return null;
				}
				return this.Cartridge.Constants.GetString("GameID", null);
			}
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0004CEE4 File Offset: 0x0004B0E4
		private UniTask Load()
		{
			GamingConsole.<Load>d__50 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<GamingConsole.<Load>d__50>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0004CF28 File Offset: 0x0004B128
		private void Save()
		{
			if (this.loading)
			{
				return;
			}
			if (!this.loaded)
			{
				return;
			}
			GamingConsole.SaveData saveData = new GamingConsole.SaveData();
			if (this.Console != null)
			{
				saveData.consoleData = ItemTreeData.FromItem(this.Console);
			}
			if (this.Monitor != null)
			{
				saveData.monitorData = ItemTreeData.FromItem(this.Monitor);
			}
			SavesSystem.Save<GamingConsole.SaveData>(this.SaveKey, saveData);
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0004CF98 File Offset: 0x0004B198
		protected override void Awake()
		{
			base.Awake();
			UIInputManager.OnCancel += this.OnUICancel;
			SavesSystem.OnCollectSaveData += this.Save;
			this.inputHandler.enabled = false;
			this.mainItem.onItemTreeChanged += this.OnContentChanged;
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0004CFF0 File Offset: 0x0004B1F0
		protected override void OnDestroy()
		{
			base.OnDestroy();
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(false);
			}
			UIInputManager.OnCancel -= this.OnUICancel;
			SavesSystem.OnCollectSaveData -= this.Save;
			this.isBeingDestroyed = true;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0004D03D File Offset: 0x0004B23D
		private void OnDisable()
		{
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged == null)
			{
				return;
			}
			onGamingConsoleInteractChanged(false);
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0004D04F File Offset: 0x0004B24F
		protected override void Start()
		{
			base.Start();
			this.Load().Forget();
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0004D062 File Offset: 0x0004B262
		private void OnContentChanged(Item item)
		{
			Action<GamingConsole> action = this.onContentChanged;
			if (action != null)
			{
				action(this);
			}
			this.RefreshGame();
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0004D07C File Offset: 0x0004B27C
		private void OnUICancel(UIInputEventData data)
		{
			if (data.Used)
			{
				return;
			}
			if (base.Interacting)
			{
				base.StopInteract();
				data.Use();
			}
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004D09C File Offset: 0x0004B29C
		protected override void OnInteractStart(CharacterMainControl interactCharacter)
		{
			base.OnInteractStart(interactCharacter);
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(this);
			}
			if (this.Console == null || this.Monitor == null || this.Cartridge == null)
			{
				NotificationText.Push(this.incompleteNotificationText.ToPlainText());
				base.StopInteract();
				return;
			}
			if (this.SelectedGame == null)
			{
				NotificationText.Push(this.noGameNotificationText.ToPlainText());
				base.StopInteract();
				return;
			}
			this.RefreshGame();
			this.inputHandler.enabled = this.controllerConnected;
			this.AnimateCameraIn().Forget();
			HUDManager.RegisterHideToken(this);
			CharacterMainControl.Main.SetPosition(this.teleportToPositionWhenBegin.position);
			GamingConsoleHUD.Show();
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004D170 File Offset: 0x0004B370
		private UniTask AnimateCameraIn()
		{
			GamingConsole.<AnimateCameraIn>d__61 <AnimateCameraIn>d__;
			<AnimateCameraIn>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateCameraIn>d__.<>4__this = this;
			<AnimateCameraIn>d__.<>1__state = -1;
			<AnimateCameraIn>d__.<>t__builder.Start<GamingConsole.<AnimateCameraIn>d__61>(ref <AnimateCameraIn>d__);
			return <AnimateCameraIn>d__.<>t__builder.Task;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0004D1B4 File Offset: 0x0004B3B4
		private UniTask AnimateCameraOut()
		{
			GamingConsole.<AnimateCameraOut>d__62 <AnimateCameraOut>d__;
			<AnimateCameraOut>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<AnimateCameraOut>d__.<>4__this = this;
			<AnimateCameraOut>d__.<>1__state = -1;
			<AnimateCameraOut>d__.<>t__builder.Start<GamingConsole.<AnimateCameraOut>d__62>(ref <AnimateCameraOut>d__);
			return <AnimateCameraOut>d__.<>t__builder.Task;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x0004D1F7 File Offset: 0x0004B3F7
		protected override void OnInteractStop()
		{
			base.OnInteractStop();
			Action<bool> onGamingConsoleInteractChanged = GamingConsole.OnGamingConsoleInteractChanged;
			if (onGamingConsoleInteractChanged != null)
			{
				onGamingConsoleInteractChanged(false);
			}
			this.inputHandler.enabled = false;
			this.AnimateCameraOut().Forget();
			HUDManager.UnregisterHideToken(this);
			GamingConsoleHUD.Hide();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0004D234 File Offset: 0x0004B434
		private void RefreshGame()
		{
			if (this.game == null)
			{
				this.CreateGame(this.SelectedGame);
				return;
			}
			if (this.SelectedGame == null || this.SelectedGame.ID != this.game.ID)
			{
				this.CreateGame(this.SelectedGame);
			}
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0004D294 File Offset: 0x0004B494
		private void CreateGame(MiniGame prefab)
		{
			if (this.isBeingDestroyed)
			{
				return;
			}
			if (this.game != null)
			{
				UnityEngine.Object.Destroy(this.game.gameObject);
			}
			if (prefab == null)
			{
				return;
			}
			this.game = UnityEngine.Object.Instantiate<MiniGame>(prefab);
			this.game.transform.SetParent(base.transform, true);
			this.game.SetRenderTexture(this.rt);
			this.game.SetConsole(this);
			this.inputHandler.SetGame(this.game);
		}

		// Token: 0x04000F14 RID: 3860
		[SerializeField]
		private List<MiniGame> possibleGames;

		// Token: 0x04000F15 RID: 3861
		[SerializeField]
		private RenderTexture rt;

		// Token: 0x04000F16 RID: 3862
		[SerializeField]
		private MiniGameInputHandler inputHandler;

		// Token: 0x04000F17 RID: 3863
		[SerializeField]
		private CinemachineVirtualCamera virtualCamera;

		// Token: 0x04000F18 RID: 3864
		[SerializeField]
		private float transitionTime = 1f;

		// Token: 0x04000F19 RID: 3865
		[SerializeField]
		private Transform vcamEndPosition;

		// Token: 0x04000F1A RID: 3866
		[SerializeField]
		private Transform vcamLookTarget;

		// Token: 0x04000F1B RID: 3867
		[SerializeField]
		private AnimationCurve posCurve;

		// Token: 0x04000F1C RID: 3868
		[SerializeField]
		private AnimationCurve rotCurve;

		// Token: 0x04000F1D RID: 3869
		[SerializeField]
		private AnimationCurve fovCurve;

		// Token: 0x04000F1E RID: 3870
		[SerializeField]
		private float activeFov = 45f;

		// Token: 0x04000F1F RID: 3871
		[SerializeField]
		private Transform teleportToPositionWhenBegin;

		// Token: 0x04000F20 RID: 3872
		[SerializeField]
		private Item mainItem;

		// Token: 0x04000F21 RID: 3873
		[SerializeField]
		[LocalizationKey("Default")]
		private string incompleteNotificationText = "GamingConsole_Incomplete";

		// Token: 0x04000F22 RID: 3874
		[SerializeField]
		[LocalizationKey("Default")]
		private string noGameNotificationText = "GamingConsole_NoGame";

		// Token: 0x04000F23 RID: 3875
		private MiniGame game;

		// Token: 0x04000F28 RID: 3880
		private string SaveKey = "GamingConsoleData";

		// Token: 0x04000F29 RID: 3881
		private bool loading;

		// Token: 0x04000F2A RID: 3882
		private bool loaded;

		// Token: 0x04000F2B RID: 3883
		private bool isBeingDestroyed;

		// Token: 0x04000F2C RID: 3884
		private int animateToken;

		// Token: 0x0200055B RID: 1371
		[Serializable]
		private class SaveData
		{
			// Token: 0x04001F11 RID: 7953
			public ItemTreeData monitorData;

			// Token: 0x04001F12 RID: 7954
			public ItemTreeData consoleData;
		}
	}
}
