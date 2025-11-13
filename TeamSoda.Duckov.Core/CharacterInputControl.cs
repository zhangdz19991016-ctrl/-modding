using System;
using System.Collections.Generic;
using System.Reflection;
using Dialogues;
using Duckov;
using Duckov.MiniMaps.UI;
using Duckov.Quests.UI;
using Duckov.UI;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000078 RID: 120
public class CharacterInputControl : MonoBehaviour
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000468 RID: 1128 RVA: 0x000145CD File Offset: 0x000127CD
	// (set) Token: 0x06000469 RID: 1129 RVA: 0x000145D4 File Offset: 0x000127D4
	public static CharacterInputControl Instance { get; private set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x0600046A RID: 1130 RVA: 0x000145DC File Offset: 0x000127DC
	private PlayerInput PlayerInput
	{
		get
		{
			return GameManager.MainPlayerInput;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x000145E3 File Offset: 0x000127E3
	private bool usingMouseAndKeyboard
	{
		get
		{
			return InputManager.InputDevice == InputManager.InputDevices.mouseKeyboard;
		}
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x000145ED File Offset: 0x000127ED
	private void Awake()
	{
		CharacterInputControl.Instance = this;
		this.inputActions = new CharacterInputControl.InputActionReferences(this.PlayerInput);
		this.RegisterEvents();
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001460C File Offset: 0x0001280C
	private void OnDestroy()
	{
		this.UnregisterEvent();
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00014614 File Offset: 0x00012814
	private void RegisterEvents()
	{
		this.Bind(this.inputActions.MoveAxis, new Action<InputAction.CallbackContext>(this.OnPlayerMoveInput));
		this.Bind(this.inputActions.Run, new Action<InputAction.CallbackContext>(this.OnPlayerRunInput));
		this.Bind(this.inputActions.MousePos, new Action<InputAction.CallbackContext>(this.OnPlayerMouseMove));
		this.Bind(this.inputActions.Skill_1_StartAim, new Action<InputAction.CallbackContext>(this.OnStartCharacterSkillAim));
		this.Bind(this.inputActions.Reload, new Action<InputAction.CallbackContext>(this.OnReloadInput));
		this.Bind(this.inputActions.Interact, new Action<InputAction.CallbackContext>(this.OnInteractInput));
		this.Bind(this.inputActions.Quack, new Action<InputAction.CallbackContext>(this.OnQuackInput));
		this.Bind(this.inputActions.ScrollWheel, new Action<InputAction.CallbackContext>(this.OnMouseScollerInput));
		this.Bind(this.inputActions.SwitchWeapon, new Action<InputAction.CallbackContext>(this.OnSwitchWeaponInput));
		this.Bind(this.inputActions.SwitchInteractAndBulletType, new Action<InputAction.CallbackContext>(this.OnSwitchInteractAndBulletTypeInput));
		this.Bind(this.inputActions.Trigger, new Action<InputAction.CallbackContext>(this.OnPlayerTriggerInputUsingMouseKeyboard));
		this.Bind(this.inputActions.ToggleView, new Action<InputAction.CallbackContext>(this.OnToggleViewInput));
		this.Bind(this.inputActions.ToggleNightVision, new Action<InputAction.CallbackContext>(this.OnToggleNightVisionInput));
		this.Bind(this.inputActions.CancelSkill, new Action<InputAction.CallbackContext>(this.OnCancelSkillInput));
		this.Bind(this.inputActions.Dash, new Action<InputAction.CallbackContext>(this.OnDashInput));
		this.Bind(this.inputActions.ItemShortcut1, new Action<InputAction.CallbackContext>(this.OnPlayerSwitchItemAgent1));
		this.Bind(this.inputActions.ItemShortcut2, new Action<InputAction.CallbackContext>(this.OnPlayerSwitchItemAgent2));
		this.Bind(this.inputActions.ItemShortcut3, new Action<InputAction.CallbackContext>(this.OnShortCutInput3));
		this.Bind(this.inputActions.ItemShortcut4, new Action<InputAction.CallbackContext>(this.OnShortCutInput4));
		this.Bind(this.inputActions.ItemShortcut5, new Action<InputAction.CallbackContext>(this.OnShortCutInput5));
		this.Bind(this.inputActions.ItemShortcut6, new Action<InputAction.CallbackContext>(this.OnShortCutInput6));
		this.Bind(this.inputActions.ItemShortcut7, new Action<InputAction.CallbackContext>(this.OnShortCutInput7));
		this.Bind(this.inputActions.ItemShortcut8, new Action<InputAction.CallbackContext>(this.OnShortCutInput8));
		this.Bind(this.inputActions.ADS, new Action<InputAction.CallbackContext>(this.OnPlayerAdsInput));
		this.Bind(this.inputActions.UI_Inventory, new Action<InputAction.CallbackContext>(this.OnUIInventoryInput));
		this.Bind(this.inputActions.UI_Map, new Action<InputAction.CallbackContext>(this.OnUIMapInput));
		this.Bind(this.inputActions.UI_Quest, new Action<InputAction.CallbackContext>(this.OnUIQuestViewInput));
		this.Bind(this.inputActions.StopAction, new Action<InputAction.CallbackContext>(this.OnPlayerStopAction));
		this.Bind(this.inputActions.PutAway, new Action<InputAction.CallbackContext>(this.OnPutAwayInput));
		this.Bind(this.inputActions.ItemShortcut_Melee, new Action<InputAction.CallbackContext>(this.OnPlayerSwitchItemAgentMelee));
		this.Bind(this.inputActions.MouseDelta, new Action<InputAction.CallbackContext>(this.OnPlayerMouseDelta));
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000149A4 File Offset: 0x00012BA4
	private void UnregisterEvent()
	{
		while (this.unbindCommands.Count > 0)
		{
			this.unbindCommands.Dequeue()();
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000149C8 File Offset: 0x00012BC8
	private void Bind(InputAction action, Action<InputAction.CallbackContext> method)
	{
		action.performed += method;
		action.started += method;
		action.canceled += method;
		this.unbindCommands.Enqueue(delegate
		{
			this.Unbind(action, method);
		});
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00014A3A File Offset: 0x00012C3A
	private void Unbind(InputAction action, Action<InputAction.CallbackContext> method)
	{
		action.performed -= method;
		action.started -= method;
		action.canceled -= method;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x00014A54 File Offset: 0x00012C54
	private void Update()
	{
		if (!this.character)
		{
			this.character = CharacterMainControl.Main;
			if (!this.character)
			{
				return;
			}
		}
		if (this.usingMouseAndKeyboard)
		{
			this.inputManager.SetMousePosition(this.mousePos);
			this.inputManager.SetAimInputUsingMouse(this.mouseDelta);
			this.inputManager.SetTrigger(this.mouseKeyboardTriggerInput, this.mouseKeyboardTriggerInputThisFrame, this.mouseKeyboardTriggerReleaseThisFrame);
			if (this.character.skillAction.holdItemSkillKeeper.CheckSkillAndBinding())
			{
				this.inputManager.SetAimType(AimTypes.handheldSkill);
				if (this.mouseKeyboardTriggerInputThisFrame)
				{
					this.inputManager.StartItemSkillAim();
				}
				else if (this.mouseKeyboardTriggerReleaseThisFrame)
				{
					Debug.Log("Release");
					this.inputManager.ReleaseItemSkill();
				}
			}
			else
			{
				this.inputManager.SetAimType(AimTypes.normalAim);
			}
			this.UpdateScollerInput();
		}
		this.mouseKeyboardTriggerInputThisFrame = false;
		this.mouseKeyboardTriggerReleaseThisFrame = false;
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x00014B48 File Offset: 0x00012D48
	public void OnPlayerMoveInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Vector2 moveInput = context.ReadValue<Vector2>();
			this.inputManager.SetMoveInput(moveInput);
		}
		if (context.canceled)
		{
			this.inputManager.SetMoveInput(Vector2.zero);
		}
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00014B8C File Offset: 0x00012D8C
	public void OnPlayerRunInput(InputAction.CallbackContext context)
	{
		this.runInput = false;
		if (context.started)
		{
			this.inputManager.SetRunInput(true);
			this.runInput = true;
		}
		if (context.canceled)
		{
			this.inputManager.SetRunInput(false);
			this.runInput = false;
		}
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00014BD8 File Offset: 0x00012DD8
	public void OnPlayerAdsInput(InputAction.CallbackContext context)
	{
		this.adsInput = false;
		if (context.started)
		{
			this.inputManager.SetAdsInput(true);
			this.adsInput = true;
		}
		if (context.canceled)
		{
			this.inputManager.SetAdsInput(false);
			this.adsInput = false;
		}
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x00014C24 File Offset: 0x00012E24
	public void OnToggleViewInput(InputAction.CallbackContext context)
	{
		if (GameManager.Paused)
		{
			return;
		}
		if (context.started)
		{
			this.inputManager.ToggleView();
		}
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x00014C42 File Offset: 0x00012E42
	public void OnToggleNightVisionInput(InputAction.CallbackContext context)
	{
		if (GameManager.Paused)
		{
			return;
		}
		if (context.started)
		{
			this.inputManager.ToggleNightVision();
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00014C60 File Offset: 0x00012E60
	public void OnPlayerTriggerInputUsingMouseKeyboard(InputAction.CallbackContext context)
	{
		if (InputManager.InputDevice != InputManager.InputDevices.mouseKeyboard)
		{
			return;
		}
		if (context.started)
		{
			this.mouseKeyboardTriggerInputThisFrame = true;
			this.mouseKeyboardTriggerInput = true;
			this.mouseKeyboardTriggerReleaseThisFrame = false;
			return;
		}
		if (context.canceled)
		{
			this.mouseKeyboardTriggerInputThisFrame = false;
			this.mouseKeyboardTriggerInput = false;
			this.mouseKeyboardTriggerReleaseThisFrame = true;
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00014CB2 File Offset: 0x00012EB2
	public void OnPlayerMouseMove(InputAction.CallbackContext context)
	{
		this.mousePos = context.ReadValue<Vector2>();
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00014CC1 File Offset: 0x00012EC1
	public void OnPlayerMouseDelta(InputAction.CallbackContext context)
	{
		this.mouseDelta = context.ReadValue<Vector2>();
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00014CD0 File Offset: 0x00012ED0
	public void OnPlayerStopAction(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.StopAction();
		}
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00014CE6 File Offset: 0x00012EE6
	public void OnPlayerSwitchItemAgent1(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.SwitchItemAgent(1);
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00014CFD File Offset: 0x00012EFD
	public void OnPlayerSwitchItemAgent2(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.SwitchItemAgent(2);
		}
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00014D14 File Offset: 0x00012F14
	public void OnPlayerSwitchItemAgentMelee(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.SwitchItemAgent(3);
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00014D2B File Offset: 0x00012F2B
	public void OnStartCharacterSkillAim(InputAction.CallbackContext context)
	{
		this.inputManager.StartCharacterSkillAim();
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00014D38 File Offset: 0x00012F38
	public void OnCharacterSkillRelease()
	{
		this.inputManager.ReleaseCharacterSkill();
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00014D45 File Offset: 0x00012F45
	public void OnReloadInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.TryToReload(null);
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00014D64 File Offset: 0x00012F64
	public void OnUIInventoryInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (GameManager.Paused)
		{
			return;
		}
		if (DialogueUI.Active)
		{
			return;
		}
		if (SceneLoader.IsSceneLoading)
		{
			return;
		}
		if (!(View.ActiveView == null))
		{
			View.ActiveView.TryQuit();
			return;
		}
		if (LevelManager.Instance.IsBaseLevel)
		{
			PlayerStorage.Instance.InteractableLootBox.InteractWithMainCharacter();
			return;
		}
		InventoryView.Show();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00014DCC File Offset: 0x00012FCC
	public void OnUIQuestViewInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (GameManager.Paused)
		{
			return;
		}
		if (DialogueUI.Active)
		{
			return;
		}
		if (View.ActiveView == null)
		{
			QuestView.Show();
			return;
		}
		if (View.ActiveView is QuestView)
		{
			View.ActiveView.TryQuit();
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00014E1C File Offset: 0x0001301C
	public void OnDashInput(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.Dash();
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00014E34 File Offset: 0x00013034
	public void OnUIMapInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (GameManager.Paused)
		{
			return;
		}
		if (SceneLoader.IsSceneLoading)
		{
			return;
		}
		if (View.ActiveView == null)
		{
			MiniMapView.Show();
			return;
		}
		MiniMapView miniMapView = View.ActiveView as MiniMapView;
		if (miniMapView != null)
		{
			miniMapView.Close();
		}
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00014E82 File Offset: 0x00013082
	public void OnCancelSkillInput(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			this.inputManager.CancleSkill();
		}
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00014E99 File Offset: 0x00013099
	public void OnInteractInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.inputManager.Interact();
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00014EB0 File Offset: 0x000130B0
	public void OnQuackInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.inputManager.Quack();
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00014EC7 File Offset: 0x000130C7
	public void OnPutAwayInput(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.inputManager.PutAway();
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00014EDE File Offset: 0x000130DE
	public void OnMouseScollerInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			this.scollY = context.ReadValue<Vector2>().y;
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00014EFC File Offset: 0x000130FC
	private void UpdateScollerInput()
	{
		float num = 1f;
		if (Mathf.Abs(this.scollY) > 0.5f && (float)this.scollYZeroFrames > num)
		{
			if (ScrollWheelBehaviour.CurrentBehaviour == ScrollWheelBehaviour.Behaviour.AmmoAndInteract)
			{
				this.inputManager.SetSwitchInteractInput((this.scollY > 0f) ? 1 : -1);
				this.inputManager.SetSwitchBulletTypeInput((this.scollY > 0f) ? 1 : -1);
			}
			else
			{
				this.inputManager.SetSwitchWeaponInput((this.scollY > 0f) ? 1 : -1);
			}
		}
		if (Mathf.Abs(this.scollY) < 0.5f)
		{
			this.scollYZeroFrames++;
			return;
		}
		this.scollYZeroFrames = 0;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00014FB0 File Offset: 0x000131B0
	public void OnSwitchWeaponInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			float num = context.ReadValue<float>();
			this.inputManager.SetSwitchWeaponInput((num > 0f) ? -1 : 1);
		}
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00014FE8 File Offset: 0x000131E8
	public void OnSwitchInteractAndBulletTypeInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			float num = context.ReadValue<float>();
			this.inputManager.SetSwitchInteractInput((num > 0f) ? -1 : 1);
			this.inputManager.SetSwitchBulletTypeInput((num > 0f) ? -1 : 1);
		}
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00015034 File Offset: 0x00013234
	private void ShortCutInput(int index)
	{
		if (View.ActiveView != null)
		{
			UIInputManager.NotifyShortcutInput(index - 3);
			return;
		}
		Item item = ItemShortcut.Get(index - 3);
		if (item == null)
		{
			return;
		}
		if (!this.character)
		{
			return;
		}
		if (item && item.UsageUtilities && item.UsageUtilities.IsUsable(item, this.character))
		{
			this.character.UseItem(item);
			return;
		}
		if (item && item.GetBool("IsSkill", false))
		{
			this.character.ChangeHoldItem(item);
			return;
		}
		if (item && item.HasHandHeldAgent)
		{
			Debug.Log("has hand held");
			this.character.ChangeHoldItem(item);
		}
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x000150F9 File Offset: 0x000132F9
	public void OnShortCutInput3(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(3);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001510C File Offset: 0x0001330C
	public void OnShortCutInput4(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(4);
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0001511F File Offset: 0x0001331F
	public void OnShortCutInput5(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(5);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00015132 File Offset: 0x00013332
	public void OnShortCutInput6(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(6);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00015145 File Offset: 0x00013345
	public void OnShortCutInput7(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(7);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00015158 File Offset: 0x00013358
	public void OnShortCutInput8(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		this.ShortCutInput(8);
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001516C File Offset: 0x0001336C
	internal static InputAction GetInputAction(string name)
	{
		if (CharacterInputControl.Instance == null)
		{
			return null;
		}
		InputAction result;
		try
		{
			result = CharacterInputControl.Instance.PlayerInput.actions[name];
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			Debug.LogError("查找 Input Action " + name + " 时发生错误, 返回null");
			result = null;
		}
		return result;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000151D0 File Offset: 0x000133D0
	public static bool GetChangeBulletTypeWasPressed()
	{
		return CharacterInputControl.Instance.inputActions.SwitchBulletType.WasPressedThisFrame();
	}

	// Token: 0x040003D2 RID: 978
	public InputManager inputManager;

	// Token: 0x040003D3 RID: 979
	private bool runInput;

	// Token: 0x040003D4 RID: 980
	private bool adsInput;

	// Token: 0x040003D5 RID: 981
	private bool aimDown;

	// Token: 0x040003D6 RID: 982
	private Vector2 mousePos;

	// Token: 0x040003D7 RID: 983
	private Vector2 mouseDelta;

	// Token: 0x040003D8 RID: 984
	private bool mouseKeyboardTriggerInput;

	// Token: 0x040003D9 RID: 985
	private bool mouseKeyboardTriggerReleaseThisFrame;

	// Token: 0x040003DA RID: 986
	private bool mouseKeyboardTriggerInputThisFrame;

	// Token: 0x040003DB RID: 987
	private CharacterMainControl character;

	// Token: 0x040003DC RID: 988
	private CharacterInputControl.InputActionReferences inputActions;

	// Token: 0x040003DD RID: 989
	private Queue<Action> unbindCommands = new Queue<Action>();

	// Token: 0x040003DE RID: 990
	private float scollY;

	// Token: 0x040003DF RID: 991
	private int scollYZeroFrames;

	// Token: 0x02000442 RID: 1090
	private class InputActionReferences
	{
		// Token: 0x06002685 RID: 9861 RVA: 0x00085748 File Offset: 0x00083948
		public InputActionReferences(PlayerInput playerInput)
		{
			InputActionAsset actions = playerInput.actions;
			Type typeFromHandle = typeof(CharacterInputControl.InputActionReferences);
			Type typeFromHandle2 = typeof(InputAction);
			FieldInfo[] fields = typeFromHandle.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.FieldType != typeFromHandle2)
				{
					Debug.LogError(fieldInfo.FieldType.Name);
				}
				else
				{
					InputAction inputAction = actions[fieldInfo.Name];
					if (inputAction == null)
					{
						Debug.LogError("找不到名为 " + fieldInfo.Name + " 的input action");
					}
					else
					{
						fieldInfo.SetValue(this, inputAction);
					}
				}
			}
			foreach (FieldInfo fieldInfo2 in fields)
			{
				if (!(fieldInfo2.FieldType != typeFromHandle2))
				{
					fieldInfo2.GetValue(this);
				}
			}
		}

		// Token: 0x04001A84 RID: 6788
		public InputAction MoveAxis;

		// Token: 0x04001A85 RID: 6789
		public InputAction Run;

		// Token: 0x04001A86 RID: 6790
		public InputAction Aim;

		// Token: 0x04001A87 RID: 6791
		public InputAction MousePos;

		// Token: 0x04001A88 RID: 6792
		public InputAction ItemShortcut1;

		// Token: 0x04001A89 RID: 6793
		public InputAction ItemShortcut2;

		// Token: 0x04001A8A RID: 6794
		public InputAction Skill_1_StartAim;

		// Token: 0x04001A8B RID: 6795
		public InputAction Reload;

		// Token: 0x04001A8C RID: 6796
		public InputAction UI_Inventory;

		// Token: 0x04001A8D RID: 6797
		public InputAction UI_Map;

		// Token: 0x04001A8E RID: 6798
		public InputAction Interact;

		// Token: 0x04001A8F RID: 6799
		public InputAction ScrollWheel;

		// Token: 0x04001A90 RID: 6800
		public InputAction SwitchWeapon;

		// Token: 0x04001A91 RID: 6801
		public InputAction SwitchInteractAndBulletType;

		// Token: 0x04001A92 RID: 6802
		public InputAction Trigger;

		// Token: 0x04001A93 RID: 6803
		public InputAction ToggleView;

		// Token: 0x04001A94 RID: 6804
		public InputAction ToggleNightVision;

		// Token: 0x04001A95 RID: 6805
		public InputAction CancelSkill;

		// Token: 0x04001A96 RID: 6806
		public InputAction Dash;

		// Token: 0x04001A97 RID: 6807
		public InputAction ItemShortcut3;

		// Token: 0x04001A98 RID: 6808
		public InputAction ItemShortcut4;

		// Token: 0x04001A99 RID: 6809
		public InputAction ItemShortcut5;

		// Token: 0x04001A9A RID: 6810
		public InputAction ItemShortcut6;

		// Token: 0x04001A9B RID: 6811
		public InputAction ItemShortcut7;

		// Token: 0x04001A9C RID: 6812
		public InputAction ItemShortcut8;

		// Token: 0x04001A9D RID: 6813
		public InputAction Quack;

		// Token: 0x04001A9E RID: 6814
		public InputAction ADS;

		// Token: 0x04001A9F RID: 6815
		public InputAction UI_Quest;

		// Token: 0x04001AA0 RID: 6816
		public InputAction StopAction;

		// Token: 0x04001AA1 RID: 6817
		public InputAction PutAway;

		// Token: 0x04001AA2 RID: 6818
		public InputAction ItemShortcut_Melee;

		// Token: 0x04001AA3 RID: 6819
		public InputAction MouseDelta;

		// Token: 0x04001AA4 RID: 6820
		public InputAction SwitchBulletType;
	}
}
