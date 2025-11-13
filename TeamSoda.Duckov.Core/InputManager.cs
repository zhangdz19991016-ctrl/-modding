using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.Options;
using Duckov.UI;
using Duckov.UI.DialogueBubbles;
using Duckov.Utilities;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000106 RID: 262
public class InputManager : MonoBehaviour
{
	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00026DF0 File Offset: 0x00024FF0
	public static InputManager.InputDevices InputDevice
	{
		get
		{
			return InputManager.inputDevice;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00026DF7 File Offset: 0x00024FF7
	public Vector3 WorldMoveInput
	{
		get
		{
			return this.worldMoveInput;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00026DFF File Offset: 0x00024FFF
	public Transform AimTarget
	{
		get
		{
			return this.aimTargetCol;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00026E07 File Offset: 0x00025007
	public Vector2 MoveAxisInput
	{
		get
		{
			return this.moveAxisInput;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00026E0F File Offset: 0x0002500F
	public Vector2 AimScreenPoint
	{
		get
		{
			return this.aimScreenPoint;
		}
	}

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060008A5 RID: 2213 RVA: 0x00026E18 File Offset: 0x00025018
	// (remove) Token: 0x060008A6 RID: 2214 RVA: 0x00026E4C File Offset: 0x0002504C
	public static event Action OnInputDeviceChanged;

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060008A7 RID: 2215 RVA: 0x00026E7F File Offset: 0x0002507F
	public Vector3 InputAimPoint
	{
		get
		{
			return this.inputAimPoint;
		}
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060008A8 RID: 2216 RVA: 0x00026E88 File Offset: 0x00025088
	// (remove) Token: 0x060008A9 RID: 2217 RVA: 0x00026EBC File Offset: 0x000250BC
	public static event Action<int> OnSwitchBulletTypeInput;

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x060008AA RID: 2218 RVA: 0x00026EF0 File Offset: 0x000250F0
	// (remove) Token: 0x060008AB RID: 2219 RVA: 0x00026F24 File Offset: 0x00025124
	public static event Action<int> OnSwitchWeaponInput;

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060008AC RID: 2220 RVA: 0x00026F57 File Offset: 0x00025157
	private static InputManager instance
	{
		get
		{
			if (LevelManager.Instance == null)
			{
				return null;
			}
			return LevelManager.Instance.InputManager;
		}
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00026F72 File Offset: 0x00025172
	private void OnDestroy()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x060008AE RID: 2222 RVA: 0x00026F80 File Offset: 0x00025180
	public static bool InputActived
	{
		get
		{
			return InputManager.instance && !GameManager.Paused && !CameraMode.Active && LevelManager.LevelInited && CharacterMainControl.Main && !CharacterMainControl.Main.Health.IsDead && InputManager.instance.inputActiveCoolCounter <= 0;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x00026FE5 File Offset: 0x000251E5
	public Vector2 MousePos
	{
		get
		{
			return this.inputMousePosition;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00026FED File Offset: 0x000251ED
	public bool TriggerInput
	{
		get
		{
			return this.triggerInput;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060008B1 RID: 2225 RVA: 0x00026FF5 File Offset: 0x000251F5
	// (set) Token: 0x060008B2 RID: 2226 RVA: 0x00027028 File Offset: 0x00025228
	private Vector2 AimMousePosition
	{
		get
		{
			if (!this.aimMousePosFirstSynced)
			{
				this.aimMousePosFirstSynced = true;
				if (Mouse.current != null)
				{
					this._aimMousePosCache = Mouse.current.position.ReadValue();
				}
			}
			return this._aimMousePosCache;
		}
		set
		{
			if (!this.aimMousePosFirstSynced)
			{
				this.aimMousePosFirstSynced = true;
				if (Mouse.current != null)
				{
					this._aimMousePosCache = Mouse.current.position.ReadValue();
				}
			}
			this._aimMousePosCache = value;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0002705C File Offset: 0x0002525C
	public bool AimingEnemyHead
	{
		get
		{
			return this.aimingEnemyHead;
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00027064 File Offset: 0x00025264
	private void Start()
	{
		this.obsticleHits = new RaycastHit[3];
		this.obsticleLayers = (GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask);
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0002709C File Offset: 0x0002529C
	private void OnApplicationFocus(bool hasFocus)
	{
		this.currentFocus = hasFocus;
		if (!this.currentFocus)
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000270B3 File Offset: 0x000252B3
	private void Awake()
	{
		if (this.blockInputSources == null)
		{
			this.blockInputSources = new HashSet<GameObject>();
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000270C8 File Offset: 0x000252C8
	public static void DisableInput(GameObject source)
	{
		if (source == null)
		{
			return;
		}
		if (InputManager.instance == null)
		{
			return;
		}
		InputManager.instance.inputActiveCoolCounter = 2;
		InputManager.instance.blockInputSources.Add(source);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x000270FE File Offset: 0x000252FE
	public static void ActiveInput(GameObject source)
	{
		if (source == null)
		{
			return;
		}
		InputManager.instance.blockInputSources.Remove(source);
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x0002711B File Offset: 0x0002531B
	public static void SetInputDevice(InputManager.InputDevices _inputDevice)
	{
		Action onInputDeviceChanged = InputManager.OnInputDeviceChanged;
		if (onInputDeviceChanged == null)
		{
			return;
		}
		onInputDeviceChanged();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x0002712C File Offset: 0x0002532C
	private void UpdateCursor()
	{
		if (LevelManager.Instance == null || this.characterMainControl == null || !this.characterMainControl.gameObject.activeInHierarchy)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			return;
		}
		bool flag = !this.characterMainControl || this.characterMainControl.Health.IsDead;
		bool flag2 = true;
		if (InputManager.InputActived && !flag)
		{
			flag2 = false;
		}
		if (CameraMode.Active)
		{
			flag2 = false;
		}
		if (View.ActiveView != null)
		{
			flag2 = true;
		}
		if (!Application.isFocused)
		{
			flag2 = true;
		}
		if (this.cursorVisable != flag2)
		{
			this.cursorVisable = !this.cursorVisable;
		}
		if (this.cursorVisable)
		{
			this.recoilNeedToRecover = Vector2.zero;
			if (Mouse.current != null)
			{
				this.AimMousePosition = Mouse.current.position.ReadValue();
			}
		}
		if (Application.isFocused)
		{
			Cursor.visible = this.cursorVisable;
		}
		else
		{
			Cursor.visible = true;
		}
		bool flag3 = false;
		if (CameraMode.Active)
		{
			flag3 = true;
		}
		if (this.currentFocus)
		{
			Cursor.lockState = (flag3 ? CursorLockMode.Locked : CursorLockMode.Confined);
			return;
		}
		Cursor.lockState = CursorLockMode.None;
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0002724C File Offset: 0x0002544C
	private void Update()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!this.mainCam)
		{
			this.mainCam = LevelManager.Instance.GameCamera.renderCamera;
			return;
		}
		this.UpdateInputActived();
		this.UpdateCursor();
		if (this.runInput)
		{
			if (this.runInptutThisFrame)
			{
				this.runInputBuffer = !this.runInputBuffer;
			}
		}
		else if (this.moveAxisInput.magnitude < 0.1f)
		{
			this.runInputBuffer = false;
		}
		else if (this.adsInput)
		{
			this.runInputBuffer = false;
		}
		this.characterMainControl.SetRunInput(InputManager.useRunInputBuffer ? this.runInputBuffer : this.runInput);
		this.SetMoveInput(this.moveAxisInput);
		if (InputManager.InputDevice == InputManager.InputDevices.touch)
		{
			this.UpdateJoystickAim();
			this.UpdateAimWhileUsingTouch();
		}
		if (this.checkGunDurabilityCoolTimer <= this.checkGunDurabilityCoolTime)
		{
			this.checkGunDurabilityCoolTimer += Time.deltaTime;
		}
		this.runInptutThisFrame = false;
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00027348 File Offset: 0x00025548
	private void UpdateInputActived()
	{
		this.blockInputSources.RemoveWhere((GameObject x) => x == null || !x.activeInHierarchy);
		if (this.blockInputSources.Count > 0)
		{
			InputManager.instance.inputActiveCoolCounter = 2;
			return;
		}
		if (InputManager.instance.inputActiveCoolCounter > 0)
		{
			InputManager.instance.inputActiveCoolCounter--;
		}
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x000273B9 File Offset: 0x000255B9
	private void UpdateAimWhileUsingTouch()
	{
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x000273BC File Offset: 0x000255BC
	public void SetTrigger(bool trigger, bool triggerThisFrame, bool releaseThisFrame)
	{
		this.triggerInput = false;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.Trigger(false, false, false);
			return;
		}
		this.triggerInput = trigger;
		this.characterMainControl.Trigger(trigger, triggerThisFrame, releaseThisFrame);
		if (trigger)
		{
			this.CheckGunDurability();
		}
		if (triggerThisFrame)
		{
			this.runInputBuffer = false;
			this.characterMainControl.Attack();
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00027428 File Offset: 0x00025628
	private void CheckAttack()
	{
		if (InputManager.InputDevice != InputManager.InputDevices.touch)
		{
			return;
		}
		if (this.characterMainControl.CurrentAction && this.characterMainControl.CurrentAction.Running)
		{
			return;
		}
		ItemAgent_MeleeWeapon meleeWeapon = this.characterMainControl.GetMeleeWeapon();
		if (meleeWeapon == null)
		{
			return;
		}
		if (meleeWeapon.AttackableTargetInRange())
		{
			this.characterMainControl.Attack();
		}
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00027490 File Offset: 0x00025690
	private void CheckGunDurability()
	{
		if (this.checkGunDurabilityCoolTimer <= this.checkGunDurabilityCoolTime)
		{
			return;
		}
		ItemAgent_Gun gun = this.characterMainControl.GetGun();
		if (gun != null && gun.Item.Durability <= 0f)
		{
			DialogueBubblesManager.Show("Pop_GunBroken".ToPlainText(), this.characterMainControl.transform, 2.5f, false, false, -1f, 2f).Forget();
		}
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00027504 File Offset: 0x00025704
	private Vector3 TrnasAxisInputToWorld(Vector2 axisInput)
	{
		Vector3 result = Vector3.zero;
		if (!this.mainCam)
		{
			return result;
		}
		if (!this.characterMainControl)
		{
			return result;
		}
		if (MoveDirectionOptions.MoveViaCharacterDirection)
		{
			Vector3 vector = this.inputAimPoint - this.characterMainControl.transform.position;
			vector.y = 0f;
			if (vector.magnitude < 1f)
			{
				return this.characterMainControl.transform.forward;
			}
			vector.Normalize();
			Vector3 a = Quaternion.Euler(0f, 90f, 0f) * vector;
			result = axisInput.x * a + axisInput.y * vector;
		}
		else
		{
			Vector3 right = this.mainCam.transform.right;
			right.y = 0f;
			right.Normalize();
			Vector3 forward = this.mainCam.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			result = axisInput.x * right + axisInput.y * forward;
		}
		return result;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00027631 File Offset: 0x00025831
	public void SetSwitchBulletTypeInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Action<int> onSwitchBulletTypeInput = InputManager.OnSwitchBulletTypeInput;
		if (onSwitchBulletTypeInput == null)
		{
			return;
		}
		onSwitchBulletTypeInput(dir);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00027659 File Offset: 0x00025859
	public void SetSwitchWeaponInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		Action<int> onSwitchWeaponInput = InputManager.OnSwitchWeaponInput;
		if (onSwitchWeaponInput != null)
		{
			onSwitchWeaponInput(dir);
		}
		this.characterMainControl.SwitchWeapon(dir);
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0002768E File Offset: 0x0002588E
	public void SetSwitchInteractInput(int dir)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.SwitchInteractSelection((dir > 0) ? -1 : 1);
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x000276BC File Offset: 0x000258BC
	public void SetMoveInput(Vector2 axisInput)
	{
		this.moveAxisInput = axisInput;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.SetMoveInput(Vector3.zero);
			return;
		}
		this.worldMoveInput = this.TrnasAxisInputToWorld(axisInput);
		Vector3 normalized = this.worldMoveInput;
		if (normalized.magnitude > 0.02f)
		{
			normalized = normalized.normalized;
		}
		this.characterMainControl.SetMoveInput(normalized);
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0002772C File Offset: 0x0002592C
	public void SetRunInput(bool run)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.runInput = false;
			this.runInptutThisFrame = false;
			this.characterMainControl.SetRunInput(false);
			return;
		}
		this.runInptutThisFrame = (!this.runInput && run);
		this.runInput = run;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00027781 File Offset: 0x00025981
	public void SetAdsInput(bool ads)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.characterMainControl.SetAdsInput(false);
			this.adsInput = false;
			return;
		}
		this.adsInput = ads;
		this.characterMainControl.SetAdsInput(ads);
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000277BF File Offset: 0x000259BF
	public void ToggleView()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		CameraArm.ToggleView();
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x000277DC File Offset: 0x000259DC
	public void ToggleNightVision()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ToggleNightVision();
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x000277FF File Offset: 0x000259FF
	public void SetAimInputUsingJoystick(Vector2 _joystickAxisInput)
	{
		if (InputManager.InputDevice == InputManager.InputDevices.mouseKeyboard)
		{
			return;
		}
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			this.joystickAxisInput = Vector3.zero;
			return;
		}
		this.joystickAxisInput = _joystickAxisInput;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00027836 File Offset: 0x00025A36
	private void UpdateJoystickAim()
	{
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00027838 File Offset: 0x00025A38
	public void SetAimType(AimTypes aimType)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		SkillBase currentRunningSkill = this.characterMainControl.GetCurrentRunningSkill();
		if (aimType != this.characterMainControl.AimType && currentRunningSkill != null)
		{
			Debug.Log("skill is running:" + currentRunningSkill.name);
			return;
		}
		this.characterMainControl.SetAimType(aimType);
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x000278A0 File Offset: 0x00025AA0
	public void SetMousePosition(Vector2 mousePosition)
	{
		this.inputMousePosition = mousePosition;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x000278AC File Offset: 0x00025AAC
	public void SetAimInputUsingMouse(Vector2 mouseDelta)
	{
		this.aimingEnemyHead = false;
		this.AimMousePosition += mouseDelta * OptionsManager.MouseSensitivity / 10f;
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		ItemAgent_Gun gun = this.characterMainControl.GetGun();
		if (gun)
		{
			this.AimMousePosition = this.ProcessMousePosViaRecoil(this.AimMousePosition, mouseDelta, gun);
		}
		Vector2 vector = default(Vector2);
		if (Application.isFocused && InputManager.InputActived && !Application.isEditor)
		{
			Vector2 aimMousePosition = this.AimMousePosition;
			this.ClampMousePosInWindow(ref aimMousePosition, ref vector);
			this.AimMousePosition = aimMousePosition;
		}
		this.aimScreenPoint = this.AimMousePosition;
		this.characterMainControl.GetCurrentRunningSkill();
		Ray ray = LevelManager.Instance.GameCamera.renderCamera.ScreenPointToRay(this.aimScreenPoint);
		Plane plane = new Plane(Vector3.up, Vector3.up * (this.characterMainControl.transform.position.y + 0.5f));
		float d = 0f;
		plane.Raycast(ray, out d);
		Vector3 vector2 = ray.origin + ray.direction * d;
		Debug.DrawLine(vector2, vector2 + Vector3.up * 3f, Color.yellow);
		Vector3 aimPoint = vector2;
		if (gun && this.characterMainControl.CanControlAim())
		{
			if (Physics.Raycast(ray, out this.hittedHead, 100f, 1 << LayerMask.NameToLayer("HeadCollider")))
			{
				this.aimingEnemyHead = true;
			}
			Vector3 position = this.characterMainControl.transform.position;
			if (gun)
			{
				position = gun.muzzle.transform.position;
			}
			Vector3 vector3 = vector2 - position;
			vector3.y = 0f;
			vector3.Normalize();
			Vector3 axis = Vector3.Cross(vector3, ray.direction);
			this.aimCheckLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
			int num = 0;
			while ((float)num < 45f)
			{
				int num2 = num;
				if (num > 23)
				{
					num2 = -(num - 23);
				}
				float d2 = 1.5f;
				Vector3 vector4 = Quaternion.AngleAxis(-2f * (float)num2, axis) * vector3;
				Ray ray2 = new Ray(position + d2 * vector4, vector4);
				if (Physics.SphereCast(ray2, 0.02f, out this.hittedCharacterDmgReceiverInfo, gun.BulletDistance, this.aimCheckLayers, QueryTriggerInteraction.Ignore) && this.hittedCharacterDmgReceiverInfo.distance > 0.1f && !Physics.SphereCast(ray2, 0.1f, out this.hittedObsticleInfo, this.hittedCharacterDmgReceiverInfo.distance, this.obsticleLayers, QueryTriggerInteraction.Ignore))
				{
					aimPoint = this.hittedCharacterDmgReceiverInfo.point;
					break;
				}
				num++;
			}
		}
		if (this.aimingEnemyHead)
		{
			Vector3 direction = ray.direction;
			Vector3 rhs = this.hittedHead.collider.transform.position - this.hittedHead.point;
			float d3 = Vector3.Dot(direction, rhs);
			aimPoint = this.hittedHead.point + direction * d3 * 0.5f;
		}
		this.inputAimPoint = vector2;
		this.characterMainControl.SetAimPoint(aimPoint);
		if (Application.isFocused && this.currentFocus && InputManager.InputActived)
		{
			Mouse.current.WarpCursorPosition(this.AimMousePosition);
		}
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00027C44 File Offset: 0x00025E44
	private Vector2 ProcessMousePosViaCameraChange(Vector2 inputMousePos)
	{
		Camera renderCamera = LevelManager.Instance.GameCamera.renderCamera;
		if (this.fovCache < 0f)
		{
			this.fovCache = renderCamera.fieldOfView;
			return inputMousePos;
		}
		float fieldOfView = renderCamera.fieldOfView;
		Vector2 a = new Vector2(inputMousePos.x / (float)Screen.width * 2f - 1f, inputMousePos.y / (float)Screen.height * 2f - 1f);
		float d = Mathf.Tan(this.fovCache * 0.017453292f / 2f) / Mathf.Tan(fieldOfView * 0.017453292f / 2f);
		Vector2 vector = a * d;
		Vector2 result = new Vector2((vector.x + 1f) * 0.5f * (float)Screen.width, (vector.y + 1f) * 0.5f * (float)Screen.height);
		this.fovCache = fieldOfView;
		return result;
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00027D2C File Offset: 0x00025F2C
	private void ClampMousePosInWindow(ref Vector2 mousePosition, ref Vector2 deltaValue)
	{
		Vector2 zero = Vector2.zero;
		zero.x = Mathf.Clamp(mousePosition.x, 0f, (float)Screen.width);
		zero.y = Mathf.Clamp(mousePosition.y, 0f, (float)Screen.height);
		deltaValue = zero - mousePosition;
		mousePosition = zero;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x00027D92 File Offset: 0x00025F92
	public void Interact()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.Interact();
		Action onInteractButtonDown = InputManager.OnInteractButtonDown;
		if (onInteractButtonDown == null)
		{
			return;
		}
		onInteractButtonDown();
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00027DC4 File Offset: 0x00025FC4
	public void PutAway()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ChangeHoldItem(null);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00027DE9 File Offset: 0x00025FE9
	public void Quack()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.Quack();
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x00027E0C File Offset: 0x0002600C
	public void SwitchItemAgent(int index)
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		switch (index)
		{
		case 1:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.PrimaryWeaponSlotHash);
			return;
		case 2:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.SecondaryWeaponSlotHash);
			return;
		case 3:
			this.characterMainControl.SwitchHoldAgentInSlot(InputManager.MeleeWeaponSlotHash);
			return;
		default:
			return;
		}
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x00027E76 File Offset: 0x00026076
	public void StopAction()
	{
		if (InputManager.InputActived && this.characterMainControl.CurrentAction && this.characterMainControl.CurrentAction.IsStopable())
		{
			this.characterMainControl.CurrentAction.StopAction();
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00027EB4 File Offset: 0x000260B4
	private bool CheckInAimAngleAndNoObsticle()
	{
		if (!this.characterMainControl)
		{
			return false;
		}
		if (this.aimTarget == null || this.characterMainControl.CurrentUsingAimSocket == null)
		{
			return false;
		}
		Vector3 position = this.characterMainControl.CurrentUsingAimSocket.position;
		position.y = 0f;
		Vector3 position2 = this.aimTarget.position;
		position2.y = 0f;
		Vector3 vector = position2 - position;
		float magnitude = vector.magnitude;
		vector.Normalize();
		float num = Mathf.Atan(0.25f / magnitude) * 57.29578f;
		if (Vector3.Angle(this.characterMainControl.CurrentAimDirection, vector) >= num)
		{
			return false;
		}
		Vector3 vector2 = position + Vector3.up * this.characterMainControl.CurrentUsingAimSocket.position.y;
		Vector3 vector3 = vector;
		Debug.DrawLine(vector2, vector2 + vector3 * magnitude);
		return Physics.SphereCastNonAlloc(vector2, 0.1f, vector3, this.obsticleHits, magnitude, this.obsticleLayers, QueryTriggerInteraction.Ignore) <= 0;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00027FCD File Offset: 0x000261CD
	public void ReleaseItemSkill()
	{
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ReleaseSkill(SkillTypes.itemSkill);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00027FE4 File Offset: 0x000261E4
	public void ReleaseCharacterSkill()
	{
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.ReleaseSkill(SkillTypes.characterSkill);
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00027FFB File Offset: 0x000261FB
	public bool CancleSkill()
	{
		return this.characterMainControl && this.characterMainControl.CancleSkill();
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00028017 File Offset: 0x00026217
	public void Dash()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		this.characterMainControl.TryCatchFishInput();
		this.characterMainControl.Dash();
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00028048 File Offset: 0x00026248
	public void StartCharacterSkillAim()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		if (this.characterMainControl.skillAction.characterSkillKeeper.Skill == null)
		{
			return;
		}
		if (this.characterMainControl.StartSkillAim(SkillTypes.characterSkill) && this.characterMainControl.skillAction.CurrentRunningSkill && this.characterMainControl.skillAction.CurrentRunningSkill.SkillContext.releaseOnStartAim)
		{
			this.characterMainControl.ReleaseSkill(SkillTypes.characterSkill);
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x000280D8 File Offset: 0x000262D8
	public void StartItemSkillAim()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		if (!InputManager.InputActived)
		{
			return;
		}
		if (!this.characterMainControl.agentHolder.Skill)
		{
			return;
		}
		if (this.characterMainControl.StartSkillAim(SkillTypes.itemSkill) && this.characterMainControl.skillAction.CurrentRunningSkill && this.characterMainControl.skillAction.CurrentRunningSkill.SkillContext.releaseOnStartAim)
		{
			this.characterMainControl.ReleaseSkill(SkillTypes.itemSkill);
		}
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00028164 File Offset: 0x00026364
	public void AddRecoil(ItemAgent_Gun gun)
	{
		if (!gun)
		{
			return;
		}
		this.recoilGun = gun;
		float recoilMultiplier = LevelManager.Rule.RecoilMultiplier;
		this.recoilV = UnityEngine.Random.Range(gun.RecoilVMin, gun.RecoilVMax) * gun.RecoilScaleV * (1f / gun.CharacterRecoilControl) * recoilMultiplier;
		this.recoilH = UnityEngine.Random.Range(gun.RecoilHMin, gun.RecoilHMax) * gun.RecoilScaleH * (1f / gun.CharacterRecoilControl) * recoilMultiplier;
		this.recoilRecover = gun.RecoilRecover;
		this.recoilTime = Mathf.Min(gun.RecoilTime, 1f / gun.ShootSpeed);
		this.recoilRecoverTime = gun.RecoilRecoverTime;
		this.recoilTimer = 0f;
		this.newRecoil = true;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00028230 File Offset: 0x00026430
	private Vector2 ProcessMousePosViaRecoil(Vector2 mousePos, Vector2 mouseDelta, ItemAgent_Gun gun)
	{
		if (!gun || this.recoilGun != gun)
		{
			this.newRecoil = false;
			this.recoilNeedToRecover = Vector2.zero;
			return mousePos;
		}
		Vector3 position = this.characterMainControl.transform.position;
		if (this.newRecoil)
		{
			Vector2 b = LevelManager.Instance.GameCamera.renderCamera.WorldToScreenPoint(position);
			Vector2 normalized = (mousePos - b).normalized;
			this.recoilThisShot = normalized * this.recoilV + this.recoilH * -Vector2.Perpendicular(normalized);
		}
		Vector3.Distance(this.InputAimPoint, position);
		float num = Time.deltaTime;
		if (this.recoilTimer + num >= this.recoilTime)
		{
			num = this.recoilTime - this.recoilTimer;
		}
		if (num > 0f)
		{
			Vector2 b2 = this.recoilThisShot * num / this.recoilTime * (float)Screen.height / 1440f;
			mousePos += b2;
			this.recoilNeedToRecover += b2;
			Vector2 zero = Vector2.zero;
			this.ClampMousePosInWindow(ref mousePos, ref zero);
			this.recoilNeedToRecover += zero;
		}
		if (num <= 0f && this.recoilTimer > this.recoilRecoverTime && this.recoilNeedToRecover.magnitude > 0f)
		{
			float num2 = Time.deltaTime;
			if (this.recoilTimer - num2 < this.recoilRecoverTime)
			{
				num2 = this.recoilTimer - this.recoilRecoverTime;
			}
			Vector2 a = Vector2.MoveTowards(this.recoilNeedToRecover, Vector2.zero, num2 * this.recoilRecover * (float)Screen.height / 1440f);
			mousePos += a - this.recoilNeedToRecover;
			this.recoilNeedToRecover = a;
		}
		float num3 = Vector2.Dot(-this.recoilNeedToRecover.normalized, mouseDelta);
		if (num3 > 0f)
		{
			this._oppositeDelta = 0f;
			this.recoilNeedToRecover = Vector2.MoveTowards(this.recoilNeedToRecover, Vector2.zero, num3);
		}
		else
		{
			this._oppositeDelta += mouseDelta.magnitude;
			if (this._oppositeDelta > 15f * (float)Screen.height / 1440f)
			{
				this._oppositeDelta = 0f;
				this.recoilNeedToRecover = Vector3.zero;
			}
		}
		this.recoilTimer += Time.deltaTime;
		this.newRecoil = false;
		return mousePos;
	}

	// Token: 0x040007E5 RID: 2021
	private static InputManager.InputDevices inputDevice = InputManager.InputDevices.mouseKeyboard;

	// Token: 0x040007E6 RID: 2022
	public CharacterMainControl characterMainControl;

	// Token: 0x040007E7 RID: 2023
	public AimTargetFinder aimTargetFinder;

	// Token: 0x040007E8 RID: 2024
	public float runThreshold = 0.85f;

	// Token: 0x040007E9 RID: 2025
	private Vector3 worldMoveInput;

	// Token: 0x040007EA RID: 2026
	public static Action OnInteractButtonDown;

	// Token: 0x040007EB RID: 2027
	private Transform aimTargetCol;

	// Token: 0x040007EC RID: 2028
	private LayerMask obsticleLayers;

	// Token: 0x040007ED RID: 2029
	private RaycastHit[] obsticleHits;

	// Token: 0x040007EE RID: 2030
	private RaycastHit hittedCharacterDmgReceiverInfo;

	// Token: 0x040007EF RID: 2031
	private RaycastHit hittedObsticleInfo;

	// Token: 0x040007F0 RID: 2032
	private RaycastHit hittedHead;

	// Token: 0x040007F1 RID: 2033
	private LayerMask aimCheckLayers;

	// Token: 0x040007F2 RID: 2034
	private CharacterMainControl foundCharacter;

	// Token: 0x040007F3 RID: 2035
	public static readonly int PrimaryWeaponSlotHash = "PrimaryWeapon".GetHashCode();

	// Token: 0x040007F4 RID: 2036
	public static readonly int SecondaryWeaponSlotHash = "SecondaryWeapon".GetHashCode();

	// Token: 0x040007F5 RID: 2037
	public static readonly int MeleeWeaponSlotHash = "MeleeWeapon".GetHashCode();

	// Token: 0x040007F6 RID: 2038
	private Camera mainCam;

	// Token: 0x040007F7 RID: 2039
	private float checkGunDurabilityCoolTimer;

	// Token: 0x040007F8 RID: 2040
	private float checkGunDurabilityCoolTime = 2f;

	// Token: 0x040007F9 RID: 2041
	private Transform aimTarget;

	// Token: 0x040007FA RID: 2042
	private Vector2 joystickAxisInput;

	// Token: 0x040007FB RID: 2043
	private Vector2 moveAxisInput;

	// Token: 0x040007FC RID: 2044
	private Vector2 aimScreenPoint;

	// Token: 0x040007FE RID: 2046
	private Vector3 inputAimPoint;

	// Token: 0x040007FF RID: 2047
	public static bool useRunInputBuffer = false;

	// Token: 0x04000800 RID: 2048
	private HashSet<GameObject> blockInputSources = new HashSet<GameObject>();

	// Token: 0x04000803 RID: 2051
	private int inputActiveCoolCounter;

	// Token: 0x04000804 RID: 2052
	private bool adsInput;

	// Token: 0x04000805 RID: 2053
	private bool runInputBuffer;

	// Token: 0x04000806 RID: 2054
	private bool runInput;

	// Token: 0x04000807 RID: 2055
	private bool runInptutThisFrame;

	// Token: 0x04000808 RID: 2056
	private bool newRecoil;

	// Token: 0x04000809 RID: 2057
	private ItemAgent_Gun recoilGun;

	// Token: 0x0400080A RID: 2058
	private float recoilV;

	// Token: 0x0400080B RID: 2059
	private float recoilH;

	// Token: 0x0400080C RID: 2060
	private float recoilRecover;

	// Token: 0x0400080D RID: 2061
	private bool triggerInput;

	// Token: 0x0400080E RID: 2062
	private Vector2 recoilNeedToRecover;

	// Token: 0x0400080F RID: 2063
	private Vector2 inputMousePosition;

	// Token: 0x04000810 RID: 2064
	private Vector2 _aimMousePosCache;

	// Token: 0x04000811 RID: 2065
	private bool aimMousePosFirstSynced;

	// Token: 0x04000812 RID: 2066
	private bool cursorVisable = true;

	// Token: 0x04000813 RID: 2067
	private bool aimingEnemyHead;

	// Token: 0x04000814 RID: 2068
	private bool currentFocus = true;

	// Token: 0x04000815 RID: 2069
	private float fovCache = -1f;

	// Token: 0x04000816 RID: 2070
	private float _oppositeDelta;

	// Token: 0x04000817 RID: 2071
	private float recoilTimer;

	// Token: 0x04000818 RID: 2072
	private float recoilTime = 0.04f;

	// Token: 0x04000819 RID: 2073
	private float recoilRecoverTime = 0.1f;

	// Token: 0x0400081A RID: 2074
	private Vector2 recoilThisShot;

	// Token: 0x02000490 RID: 1168
	public enum InputDevices
	{
		// Token: 0x04001BE9 RID: 7145
		mouseKeyboard,
		// Token: 0x04001BEA RID: 7146
		touch
	}
}
