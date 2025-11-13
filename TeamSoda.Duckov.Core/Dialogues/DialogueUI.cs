using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using NodeCanvas.DialogueTrees;
using SodaCraft.Localizations;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialogues
{
	// Token: 0x0200021D RID: 541
	public class DialogueUI : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x0003FC17 File Offset: 0x0003DE17
		// (set) Token: 0x0600103D RID: 4157 RVA: 0x0003FC1E File Offset: 0x0003DE1E
		public static DialogueUI instance { get; private set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0003FC28 File Offset: 0x0003DE28
		private PrefabPool<DialogueUIChoice> ChoicePool
		{
			get
			{
				if (this._choicePool == null)
				{
					this._choicePool = new PrefabPool<DialogueUIChoice>(this.choiceTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._choicePool;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0003FC61 File Offset: 0x0003DE61
		public static bool Active
		{
			get
			{
				return !(DialogueUI.instance == null) && DialogueUI.instance.mainFadeGroup.IsShown;
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06001040 RID: 4160 RVA: 0x0003FC84 File Offset: 0x0003DE84
		// (remove) Token: 0x06001041 RID: 4161 RVA: 0x0003FCB8 File Offset: 0x0003DEB8
		public static event Action OnDialogueStatusChanged;

		// Token: 0x06001042 RID: 4162 RVA: 0x0003FCEB File Offset: 0x0003DEEB
		private void Awake()
		{
			DialogueUI.instance = this;
			this.choiceTemplate.gameObject.SetActive(false);
			this.RegisterEvents();
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0003FD0A File Offset: 0x0003DF0A
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0003FD12 File Offset: 0x0003DF12
		private void Update()
		{
			this.RefreshActorPositionIndicator();
			if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
			{
				this.Confirm();
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0003FD38 File Offset: 0x0003DF38
		private void OnEnable()
		{
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0003FD3A File Offset: 0x0003DF3A
		private void OnDisable()
		{
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0003FD3C File Offset: 0x0003DF3C
		private void RegisterEvents()
		{
			DialogueTree.OnDialogueStarted += this.OnDialogueStarted;
			DialogueTree.OnDialoguePaused += this.OnDialoguePaused;
			DialogueTree.OnDialogueFinished += this.OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest += this.OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest += this.OnMultipleChoiceRequest;
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0003FDA0 File Offset: 0x0003DFA0
		private void UnregisterEvents()
		{
			DialogueTree.OnDialogueStarted -= this.OnDialogueStarted;
			DialogueTree.OnDialoguePaused -= this.OnDialoguePaused;
			DialogueTree.OnDialogueFinished -= this.OnDialogueFinished;
			DialogueTree.OnSubtitlesRequest -= this.OnSubtitlesRequest;
			DialogueTree.OnMultipleChoiceRequest -= this.OnMultipleChoiceRequest;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0003FE02 File Offset: 0x0003E002
		private void OnMultipleChoiceRequest(MultipleChoiceRequestInfo info)
		{
			this.DoMultipleChoice(info).Forget();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0003FE10 File Offset: 0x0003E010
		private void OnSubtitlesRequest(SubtitlesRequestInfo info)
		{
			this.DoSubtitle(info).Forget();
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0003FE1E File Offset: 0x0003E01E
		public static void HideTextFadeGroup()
		{
			DialogueUI.instance.MHideTextFadeGroup();
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0003FE2A File Offset: 0x0003E02A
		private void MHideTextFadeGroup()
		{
			this.textAreaFadeGroup.Hide();
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0003FE37 File Offset: 0x0003E037
		private void OnDialogueFinished(DialogueTree tree)
		{
			this.textAreaFadeGroup.Hide();
			InputManager.ActiveInput(base.gameObject);
			this.mainFadeGroup.Hide();
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged == null)
			{
				return;
			}
			onDialogueStatusChanged();
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0003FE69 File Offset: 0x0003E069
		private void OnDialoguePaused(DialogueTree tree)
		{
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged == null)
			{
				return;
			}
			onDialogueStatusChanged();
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0003FE7A File Offset: 0x0003E07A
		private void OnDialogueStarted(DialogueTree tree)
		{
			InputManager.DisableInput(base.gameObject);
			this.mainFadeGroup.Show();
			Action onDialogueStatusChanged = DialogueUI.OnDialogueStatusChanged;
			if (onDialogueStatusChanged != null)
			{
				onDialogueStatusChanged();
			}
			this.actorNameFadeGroup.SkipHide();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0003FEB0 File Offset: 0x0003E0B0
		public UniTask DoSubtitle(SubtitlesRequestInfo info)
		{
			DialogueUI.<DoSubtitle>d__40 <DoSubtitle>d__;
			<DoSubtitle>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoSubtitle>d__.<>4__this = this;
			<DoSubtitle>d__.info = info;
			<DoSubtitle>d__.<>1__state = -1;
			<DoSubtitle>d__.<>t__builder.Start<DialogueUI.<DoSubtitle>d__40>(ref <DoSubtitle>d__);
			return <DoSubtitle>d__.<>t__builder.Task;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0003FEFC File Offset: 0x0003E0FC
		private void SetupActorInfo(IDialogueActor actor)
		{
			DuckovDialogueActor duckovDialogueActor = actor as DuckovDialogueActor;
			if (duckovDialogueActor == null)
			{
				this.actorNameFadeGroup.Hide();
				this.actorPortraitContainer.gameObject.SetActive(false);
				this.actorPositionIndicator.gameObject.SetActive(false);
				this.talkingActor = null;
				return;
			}
			this.talkingActor = duckovDialogueActor;
			Sprite portraitSprite = duckovDialogueActor.portraitSprite;
			string nameKey = duckovDialogueActor.NameKey;
			Transform transform = duckovDialogueActor.transform;
			this.actorNameText.text = nameKey.ToPlainText();
			this.actorNameFadeGroup.Show();
			this.actorPortraitContainer.SetActive(portraitSprite);
			this.actorPortraitDisplay.sprite = portraitSprite;
			if (this.talkingActor.transform != null)
			{
				this.actorPositionIndicator.gameObject.SetActive(true);
			}
			this.RefreshActorPositionIndicator();
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0003FFC8 File Offset: 0x0003E1C8
		private void RefreshActorPositionIndicator()
		{
			if (this.talkingActor == null)
			{
				this.actorPositionIndicator.gameObject.SetActive(false);
				return;
			}
			this.actorPositionIndicator.MatchWorldPosition(this.talkingActor.transform.position + this.talkingActor.Offset, default(Vector3));
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0004002C File Offset: 0x0003E22C
		private UniTask DoMultipleChoice(MultipleChoiceRequestInfo info)
		{
			DialogueUI.<DoMultipleChoice>d__43 <DoMultipleChoice>d__;
			<DoMultipleChoice>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoMultipleChoice>d__.<>4__this = this;
			<DoMultipleChoice>d__.info = info;
			<DoMultipleChoice>d__.<>1__state = -1;
			<DoMultipleChoice>d__.<>t__builder.Start<DialogueUI.<DoMultipleChoice>d__43>(ref <DoMultipleChoice>d__);
			return <DoMultipleChoice>d__.<>t__builder.Task;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00040078 File Offset: 0x0003E278
		private UniTask DisplayOptions(Dictionary<IStatement, int> options)
		{
			DialogueUI.<DisplayOptions>d__44 <DisplayOptions>d__;
			<DisplayOptions>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayOptions>d__.<>4__this = this;
			<DisplayOptions>d__.options = options;
			<DisplayOptions>d__.<>1__state = -1;
			<DisplayOptions>d__.<>t__builder.Start<DialogueUI.<DisplayOptions>d__44>(ref <DisplayOptions>d__);
			return <DisplayOptions>d__.<>t__builder.Task;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000400C3 File Offset: 0x0003E2C3
		internal void NotifyChoiceConfirmed(DialogueUIChoice choice)
		{
			this.confirmedChoice = choice.Index;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x000400D4 File Offset: 0x0003E2D4
		private UniTask<int> WaitForChoice()
		{
			DialogueUI.<WaitForChoice>d__48 <WaitForChoice>d__;
			<WaitForChoice>d__.<>t__builder = AsyncUniTaskMethodBuilder<int>.Create();
			<WaitForChoice>d__.<>4__this = this;
			<WaitForChoice>d__.<>1__state = -1;
			<WaitForChoice>d__.<>t__builder.Start<DialogueUI.<WaitForChoice>d__48>(ref <WaitForChoice>d__);
			return <WaitForChoice>d__.<>t__builder.Task;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00040117 File Offset: 0x0003E317
		public void Confirm()
		{
			this.confirmed = true;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00040120 File Offset: 0x0003E320
		private UniTask WaitForConfirm()
		{
			DialogueUI.<WaitForConfirm>d__51 <WaitForConfirm>d__;
			<WaitForConfirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForConfirm>d__.<>4__this = this;
			<WaitForConfirm>d__.<>1__state = -1;
			<WaitForConfirm>d__.<>t__builder.Start<DialogueUI.<WaitForConfirm>d__51>(ref <WaitForConfirm>d__);
			return <WaitForConfirm>d__.<>t__builder.Task;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00040163 File Offset: 0x0003E363
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Confirm();
		}

		// Token: 0x04000D0A RID: 3338
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000D0B RID: 3339
		[SerializeField]
		private FadeGroup textAreaFadeGroup;

		// Token: 0x04000D0C RID: 3340
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000D0D RID: 3341
		[SerializeField]
		private GameObject continueIndicator;

		// Token: 0x04000D0E RID: 3342
		[SerializeField]
		private float speed = 10f;

		// Token: 0x04000D0F RID: 3343
		[SerializeField]
		private RectTransform actorPositionIndicator;

		// Token: 0x04000D10 RID: 3344
		[SerializeField]
		private FadeGroup actorNameFadeGroup;

		// Token: 0x04000D11 RID: 3345
		[SerializeField]
		private TextMeshProUGUI actorNameText;

		// Token: 0x04000D12 RID: 3346
		[SerializeField]
		private GameObject actorPortraitContainer;

		// Token: 0x04000D13 RID: 3347
		[SerializeField]
		private Image actorPortraitDisplay;

		// Token: 0x04000D14 RID: 3348
		[SerializeField]
		private FadeGroup choiceListFadeGroup;

		// Token: 0x04000D15 RID: 3349
		[SerializeField]
		private Menu choiceMenu;

		// Token: 0x04000D16 RID: 3350
		[SerializeField]
		private DialogueUIChoice choiceTemplate;

		// Token: 0x04000D17 RID: 3351
		private PrefabPool<DialogueUIChoice> _choicePool;

		// Token: 0x04000D18 RID: 3352
		private DuckovDialogueActor talkingActor;

		// Token: 0x04000D1A RID: 3354
		private int confirmedChoice;

		// Token: 0x04000D1B RID: 3355
		private bool waitingForChoice;

		// Token: 0x04000D1C RID: 3356
		private bool confirmed;
	}
}
