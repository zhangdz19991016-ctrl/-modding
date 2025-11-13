using System;
using Duckov.UI;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000173 RID: 371
public class UIInputManager : MonoBehaviour
{
	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002FF01 File Offset: 0x0002E101
	public static UIInputManager Instance
	{
		get
		{
			return GameManager.UiInputManager;
		}
	}

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06000B31 RID: 2865 RVA: 0x0002FF08 File Offset: 0x0002E108
	// (remove) Token: 0x06000B32 RID: 2866 RVA: 0x0002FF3C File Offset: 0x0002E13C
	public static event Action<UIInputEventData> OnNavigate;

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06000B33 RID: 2867 RVA: 0x0002FF70 File Offset: 0x0002E170
	// (remove) Token: 0x06000B34 RID: 2868 RVA: 0x0002FFA4 File Offset: 0x0002E1A4
	public static event Action<UIInputEventData> OnConfirm;

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06000B35 RID: 2869 RVA: 0x0002FFD8 File Offset: 0x0002E1D8
	// (remove) Token: 0x06000B36 RID: 2870 RVA: 0x0003000C File Offset: 0x0002E20C
	public static event Action<UIInputEventData> OnToggleIndicatorHUD;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06000B37 RID: 2871 RVA: 0x00030040 File Offset: 0x0002E240
	// (remove) Token: 0x06000B38 RID: 2872 RVA: 0x00030074 File Offset: 0x0002E274
	public static event Action<UIInputEventData> OnCancelEarly;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06000B39 RID: 2873 RVA: 0x000300A8 File Offset: 0x0002E2A8
	// (remove) Token: 0x06000B3A RID: 2874 RVA: 0x000300DC File Offset: 0x0002E2DC
	public static event Action<UIInputEventData> OnCancel;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06000B3B RID: 2875 RVA: 0x00030110 File Offset: 0x0002E310
	// (remove) Token: 0x06000B3C RID: 2876 RVA: 0x00030144 File Offset: 0x0002E344
	public static event Action<UIInputEventData> OnFastPick;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06000B3D RID: 2877 RVA: 0x00030178 File Offset: 0x0002E378
	// (remove) Token: 0x06000B3E RID: 2878 RVA: 0x000301AC File Offset: 0x0002E3AC
	public static event Action<UIInputEventData> OnDropItem;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06000B3F RID: 2879 RVA: 0x000301E0 File Offset: 0x0002E3E0
	// (remove) Token: 0x06000B40 RID: 2880 RVA: 0x00030214 File Offset: 0x0002E414
	public static event Action<UIInputEventData> OnUseItem;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06000B41 RID: 2881 RVA: 0x00030248 File Offset: 0x0002E448
	// (remove) Token: 0x06000B42 RID: 2882 RVA: 0x0003027C File Offset: 0x0002E47C
	public static event Action<UIInputEventData> OnToggleCameraMode;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06000B43 RID: 2883 RVA: 0x000302B0 File Offset: 0x0002E4B0
	// (remove) Token: 0x06000B44 RID: 2884 RVA: 0x000302E4 File Offset: 0x0002E4E4
	public static event Action<UIInputEventData> OnWishlistHoveringItem;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06000B45 RID: 2885 RVA: 0x00030318 File Offset: 0x0002E518
	// (remove) Token: 0x06000B46 RID: 2886 RVA: 0x0003034C File Offset: 0x0002E54C
	public static event Action<UIInputEventData> OnNextPage;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06000B47 RID: 2887 RVA: 0x00030380 File Offset: 0x0002E580
	// (remove) Token: 0x06000B48 RID: 2888 RVA: 0x000303B4 File Offset: 0x0002E5B4
	public static event Action<UIInputEventData> OnPreviousPage;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06000B49 RID: 2889 RVA: 0x000303E8 File Offset: 0x0002E5E8
	// (remove) Token: 0x06000B4A RID: 2890 RVA: 0x0003041C File Offset: 0x0002E61C
	public static event Action<UIInputEventData> OnLockInventoryIndex;

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06000B4B RID: 2891 RVA: 0x00030450 File Offset: 0x0002E650
	// (remove) Token: 0x06000B4C RID: 2892 RVA: 0x00030484 File Offset: 0x0002E684
	public static event Action<UIInputEventData, int> OnShortcutInput;

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x06000B4D RID: 2893 RVA: 0x000304B8 File Offset: 0x0002E6B8
	// (remove) Token: 0x06000B4E RID: 2894 RVA: 0x000304EC File Offset: 0x0002E6EC
	public static event Action<InputAction.CallbackContext> OnInteractInputContext;

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0003051F File Offset: 0x0002E71F
	public static bool Ctrl
	{
		get
		{
			return Keyboard.current != null && Keyboard.current.ctrlKey.isPressed;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00030539 File Offset: 0x0002E739
	public static bool Alt
	{
		get
		{
			return Keyboard.current != null && Keyboard.current.altKey.isPressed;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00030553 File Offset: 0x0002E753
	public static bool Shift
	{
		get
		{
			return Keyboard.current != null && Keyboard.current.shiftKey.isPressed;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00030570 File Offset: 0x0002E770
	public static Vector2 Point
	{
		get
		{
			if (!Application.isPlaying)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance == null)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance.inputActionPoint == null)
			{
				return default(Vector2);
			}
			return UIInputManager.Instance.inputActionPoint.ReadValue<Vector2>();
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x000305CC File Offset: 0x0002E7CC
	public static Vector2 MouseDelta
	{
		get
		{
			if (!Application.isPlaying)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance == null)
			{
				return default(Vector2);
			}
			if (UIInputManager.Instance.inputActionMouseDelta == null)
			{
				return default(Vector2);
			}
			return UIInputManager.Instance.inputActionMouseDelta.ReadValue<Vector2>();
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00030626 File Offset: 0x0002E826
	public static bool WasClickedThisFrame
	{
		get
		{
			return Application.isPlaying && !(UIInputManager.Instance == null) && UIInputManager.Instance.inputActionMouseClick != null && UIInputManager.Instance.inputActionMouseClick.WasPressedThisFrame();
		}
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00030660 File Offset: 0x0002E860
	public static Ray GetPointRay()
	{
		if (UIInputManager.Instance == null)
		{
			return default(Ray);
		}
		GameCamera instance = GameCamera.Instance;
		if (instance == null)
		{
			return default(Ray);
		}
		return instance.renderCamera.ScreenPointToRay(UIInputManager.Point);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x000306B4 File Offset: 0x0002E8B4
	private void Awake()
	{
		if (UIInputManager.Instance != this)
		{
			return;
		}
		InputActionAsset actions = GameManager.MainPlayerInput.actions;
		this.inputActionNavigate = actions["UI_Navigate"];
		this.inputActionConfirm = actions["UI_Confirm"];
		this.inputActionCancel = actions["UI_Cancel"];
		this.inputActionPoint = actions["Point"];
		this.inputActionFastPick = actions["Interact"];
		this.inputActionDropItem = actions["UI_Item_Drop"];
		this.inputActionUseItem = actions["UI_Item_use"];
		this.inputActionToggleIndicatorHUD = actions["UI_ToggleIndicatorHUD"];
		this.inputActionToggleCameraMode = actions["UI_ToggleCameraMode"];
		this.inputActionWishlistHoveringItem = actions["UI_WishlistHoveringItem"];
		this.inputActionNextPage = actions["UI_NextPage"];
		this.inputActionPreviousPage = actions["UI_PreviousPage"];
		this.inputActionLockInventoryIndex = actions["UI_LockInventoryIndex"];
		this.inputActionMouseDelta = actions["MouseDelta"];
		this.inputActionMouseClick = actions["Click"];
		this.inputActionInteract = actions["Interact"];
		this.Bind(this.inputActionNavigate, new Action<InputAction.CallbackContext>(this.OnInputActionNavigate));
		this.Bind(this.inputActionConfirm, new Action<InputAction.CallbackContext>(this.OnInputActionConfirm));
		this.Bind(this.inputActionCancel, new Action<InputAction.CallbackContext>(this.OnInputActionCancel));
		this.Bind(this.inputActionFastPick, new Action<InputAction.CallbackContext>(this.OnInputActionFastPick));
		this.Bind(this.inputActionDropItem, new Action<InputAction.CallbackContext>(this.OnInputActionDropItem));
		this.Bind(this.inputActionUseItem, new Action<InputAction.CallbackContext>(this.OnInputActionUseItem));
		this.Bind(this.inputActionToggleIndicatorHUD, new Action<InputAction.CallbackContext>(this.OnInputActionToggleIndicatorHUD));
		this.Bind(this.inputActionToggleCameraMode, new Action<InputAction.CallbackContext>(this.OnInputActionToggleCameraMode));
		this.Bind(this.inputActionWishlistHoveringItem, new Action<InputAction.CallbackContext>(this.OnInputWishlistHoveringItem));
		this.Bind(this.inputActionNextPage, new Action<InputAction.CallbackContext>(this.OnInputActionNextPage));
		this.Bind(this.inputActionPreviousPage, new Action<InputAction.CallbackContext>(this.OnInputActionPrevioursPage));
		this.Bind(this.inputActionLockInventoryIndex, new Action<InputAction.CallbackContext>(this.OnInputActionLockInventoryIndex));
		this.Bind(this.inputActionInteract, new Action<InputAction.CallbackContext>(this.OnInputActionInteract));
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00030924 File Offset: 0x0002EB24
	private void OnDestroy()
	{
		this.UnBind(this.inputActionNavigate, new Action<InputAction.CallbackContext>(this.OnInputActionNavigate));
		this.UnBind(this.inputActionConfirm, new Action<InputAction.CallbackContext>(this.OnInputActionConfirm));
		this.UnBind(this.inputActionCancel, new Action<InputAction.CallbackContext>(this.OnInputActionCancel));
		this.UnBind(this.inputActionFastPick, new Action<InputAction.CallbackContext>(this.OnInputActionFastPick));
		this.UnBind(this.inputActionUseItem, new Action<InputAction.CallbackContext>(this.OnInputActionUseItem));
		this.UnBind(this.inputActionToggleIndicatorHUD, new Action<InputAction.CallbackContext>(this.OnInputActionToggleIndicatorHUD));
		this.UnBind(this.inputActionToggleCameraMode, new Action<InputAction.CallbackContext>(this.OnInputActionToggleCameraMode));
		this.UnBind(this.inputActionWishlistHoveringItem, new Action<InputAction.CallbackContext>(this.OnInputWishlistHoveringItem));
		this.UnBind(this.inputActionNextPage, new Action<InputAction.CallbackContext>(this.OnInputActionNextPage));
		this.UnBind(this.inputActionPreviousPage, new Action<InputAction.CallbackContext>(this.OnInputActionPrevioursPage));
		this.UnBind(this.inputActionLockInventoryIndex, new Action<InputAction.CallbackContext>(this.OnInputActionLockInventoryIndex));
		this.UnBind(this.inputActionInteract, new Action<InputAction.CallbackContext>(this.OnInputActionInteract));
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00030A51 File Offset: 0x0002EC51
	private void OnInputActionInteract(InputAction.CallbackContext context)
	{
		Action<InputAction.CallbackContext> onInteractInputContext = UIInputManager.OnInteractInputContext;
		if (onInteractInputContext == null)
		{
			return;
		}
		onInteractInputContext(context);
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00030A63 File Offset: 0x0002EC63
	private void OnInputActionLockInventoryIndex(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onLockInventoryIndex = UIInputManager.OnLockInventoryIndex;
			if (onLockInventoryIndex == null)
			{
				return;
			}
			onLockInventoryIndex(new UIInputEventData());
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00030A82 File Offset: 0x0002EC82
	private void OnInputActionNextPage(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onNextPage = UIInputManager.OnNextPage;
			if (onNextPage == null)
			{
				return;
			}
			onNextPage(new UIInputEventData());
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00030AA1 File Offset: 0x0002ECA1
	private void OnInputActionPrevioursPage(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onPreviousPage = UIInputManager.OnPreviousPage;
			if (onPreviousPage == null)
			{
				return;
			}
			onPreviousPage(new UIInputEventData());
		}
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00030AC0 File Offset: 0x0002ECC0
	private void OnInputWishlistHoveringItem(InputAction.CallbackContext context)
	{
		if (!context.started)
		{
			return;
		}
		Action<UIInputEventData> onWishlistHoveringItem = UIInputManager.OnWishlistHoveringItem;
		if (onWishlistHoveringItem == null)
		{
			return;
		}
		onWishlistHoveringItem(new UIInputEventData());
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00030AE0 File Offset: 0x0002ECE0
	private void OnInputActionToggleCameraMode(InputAction.CallbackContext context)
	{
		if (View.ActiveView != null)
		{
			return;
		}
		if (context.started)
		{
			Action<UIInputEventData> onToggleCameraMode = UIInputManager.OnToggleCameraMode;
			if (onToggleCameraMode == null)
			{
				return;
			}
			onToggleCameraMode(new UIInputEventData());
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00030B0D File Offset: 0x0002ED0D
	private void OnInputActionDropItem(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onDropItem = UIInputManager.OnDropItem;
			if (onDropItem == null)
			{
				return;
			}
			onDropItem(new UIInputEventData());
		}
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00030B2C File Offset: 0x0002ED2C
	private void OnInputActionUseItem(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onUseItem = UIInputManager.OnUseItem;
			if (onUseItem == null)
			{
				return;
			}
			onUseItem(new UIInputEventData());
		}
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00030B4B File Offset: 0x0002ED4B
	private void OnInputActionFastPick(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onFastPick = UIInputManager.OnFastPick;
			if (onFastPick == null)
			{
				return;
			}
			onFastPick(new UIInputEventData());
		}
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00030B6C File Offset: 0x0002ED6C
	private void OnInputActionCancel(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			UIInputEventData uiinputEventData = new UIInputEventData
			{
				cancel = true
			};
			Action<UIInputEventData> onCancelEarly = UIInputManager.OnCancelEarly;
			if (onCancelEarly != null)
			{
				onCancelEarly(uiinputEventData);
			}
			if (uiinputEventData.Used)
			{
				return;
			}
			Action<UIInputEventData> onCancel = UIInputManager.OnCancel;
			if (onCancel != null)
			{
				onCancel(uiinputEventData);
			}
			if (uiinputEventData.Used)
			{
				return;
			}
			if (LevelManager.Instance != null && View.ActiveView == null)
			{
				PauseMenu.Toggle();
			}
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00030BE2 File Offset: 0x0002EDE2
	private void OnInputActionConfirm(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onConfirm = UIInputManager.OnConfirm;
			if (onConfirm == null)
			{
				return;
			}
			onConfirm(new UIInputEventData
			{
				confirm = true
			});
		}
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00030C08 File Offset: 0x0002EE08
	private void OnInputActionNavigate(InputAction.CallbackContext context)
	{
		Vector2 vector = context.ReadValue<Vector2>();
		Action<UIInputEventData> onNavigate = UIInputManager.OnNavigate;
		if (onNavigate == null)
		{
			return;
		}
		onNavigate(new UIInputEventData
		{
			vector = vector
		});
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00030C38 File Offset: 0x0002EE38
	private void OnInputActionToggleIndicatorHUD(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Action<UIInputEventData> onToggleIndicatorHUD = UIInputManager.OnToggleIndicatorHUD;
			if (onToggleIndicatorHUD == null)
			{
				return;
			}
			onToggleIndicatorHUD(new UIInputEventData());
		}
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00030C57 File Offset: 0x0002EE57
	private void Bind(InputAction inputAction, Action<InputAction.CallbackContext> action)
	{
		inputAction.Enable();
		inputAction.started += action;
		inputAction.performed += action;
		inputAction.canceled += action;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00030C74 File Offset: 0x0002EE74
	private void UnBind(InputAction inputAction, Action<InputAction.CallbackContext> action)
	{
		if (inputAction != null)
		{
			inputAction.started -= action;
			inputAction.performed -= action;
			inputAction.canceled -= action;
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00030C8E File Offset: 0x0002EE8E
	internal static void NotifyShortcutInput(int index)
	{
		UIInputManager.OnShortcutInput(new UIInputEventData
		{
			confirm = true
		}, index);
	}

	// Token: 0x040009A4 RID: 2468
	private static bool instantiated;

	// Token: 0x040009A5 RID: 2469
	private InputAction inputActionNavigate;

	// Token: 0x040009A6 RID: 2470
	private InputAction inputActionConfirm;

	// Token: 0x040009A7 RID: 2471
	private InputAction inputActionCancel;

	// Token: 0x040009A8 RID: 2472
	private InputAction inputActionPoint;

	// Token: 0x040009A9 RID: 2473
	private InputAction inputActionMouseDelta;

	// Token: 0x040009AA RID: 2474
	private InputAction inputActionMouseClick;

	// Token: 0x040009AB RID: 2475
	private InputAction inputActionFastPick;

	// Token: 0x040009AC RID: 2476
	private InputAction inputActionDropItem;

	// Token: 0x040009AD RID: 2477
	private InputAction inputActionUseItem;

	// Token: 0x040009AE RID: 2478
	private InputAction inputActionToggleIndicatorHUD;

	// Token: 0x040009AF RID: 2479
	private InputAction inputActionToggleCameraMode;

	// Token: 0x040009B0 RID: 2480
	private InputAction inputActionWishlistHoveringItem;

	// Token: 0x040009B1 RID: 2481
	private InputAction inputActionNextPage;

	// Token: 0x040009B2 RID: 2482
	private InputAction inputActionPreviousPage;

	// Token: 0x040009B3 RID: 2483
	private InputAction inputActionLockInventoryIndex;

	// Token: 0x040009B4 RID: 2484
	private InputAction inputActionInteract;
}
