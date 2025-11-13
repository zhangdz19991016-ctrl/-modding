using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Saves;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001BD RID: 445
public class InputRebinder : MonoBehaviour
{
	// Token: 0x06000D4A RID: 3402 RVA: 0x00037AD5 File Offset: 0x00035CD5
	public void Rebind()
	{
		InputRebinder.RebindAsync(this.action, this.index, this.excludes, false).Forget<bool>();
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00037AF4 File Offset: 0x00035CF4
	private static PlayerInput PlayerInput
	{
		get
		{
			return GameManager.MainPlayerInput;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00037AFB File Offset: 0x00035CFB
	private static bool OperationPending
	{
		get
		{
			return InputRebinder.operation.started && !InputRebinder.operation.canceled && !InputRebinder.operation.completed;
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x00037B26 File Offset: 0x00035D26
	private void Awake()
	{
		InputRebinder.Load();
		UIInputManager.OnCancelEarly += this.OnUICancel;
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x00037B3E File Offset: 0x00035D3E
	private void OnDestroy()
	{
		UIInputManager.OnCancelEarly -= this.OnUICancel;
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00037B51 File Offset: 0x00035D51
	private void OnUICancel(UIInputEventData data)
	{
		if (InputRebinder.OperationPending)
		{
			data.Use();
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x00037B60 File Offset: 0x00035D60
	public static void Load()
	{
		string text = SavesSystem.LoadGlobal<string>("InputBinding", null);
		string.IsNullOrEmpty(text);
		try
		{
			InputRebinder.PlayerInput.actions.LoadBindingOverridesFromJson(text, true);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			InputRebinder.PlayerInput.actions.RemoveAllBindingOverrides();
		}
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x00037BBC File Offset: 0x00035DBC
	public static void Save()
	{
		string text = InputRebinder.PlayerInput.actions.SaveBindingOverridesAsJson();
		SavesSystem.SaveGlobal<string>("InputBinding", text);
		Debug.Log(text);
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x00037BEA File Offset: 0x00035DEA
	public static void Clear()
	{
		InputRebinder.PlayerInput.actions.RemoveAllBindingOverrides();
		Action onBindingChanged = InputRebinder.OnBindingChanged;
		if (onBindingChanged != null)
		{
			onBindingChanged();
		}
		InputIndicator.NotifyBindingChanged();
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x00037C10 File Offset: 0x00035E10
	private static void Rebind(string name, int index, string[] excludes = null)
	{
		if (InputRebinder.OperationPending)
		{
			return;
		}
		InputAction inputAction = InputRebinder.PlayerInput.actions[name];
		if (inputAction == null)
		{
			Debug.LogError("找不到名为 " + name + " 的 action");
			return;
		}
		Action<InputAction> onRebindBegin = InputRebinder.OnRebindBegin;
		if (onRebindBegin != null)
		{
			onRebindBegin(inputAction);
		}
		Debug.Log("Resetting");
		InputRebinder.operation.Reset();
		Debug.Log("Settingup");
		inputAction.actionMap.Disable();
		InputRebinder.operation.WithCancelingThrough("<Keyboard>/escape").WithAction(inputAction).WithTargetBinding(index).OnComplete(new Action<InputActionRebindingExtensions.RebindingOperation>(InputRebinder.OnComplete)).OnCancel(new Action<InputActionRebindingExtensions.RebindingOperation>(InputRebinder.OnCancel));
		if (excludes != null)
		{
			foreach (string path in excludes)
			{
				InputRebinder.operation.WithControlsExcluding(path);
			}
		}
		Debug.Log("Starting");
		InputRebinder.operation.Start();
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x00037D00 File Offset: 0x00035F00
	public static UniTask<bool> RebindAsync(string name, int index, string[] excludes = null, bool save = false)
	{
		InputRebinder.<RebindAsync>d__20 <RebindAsync>d__;
		<RebindAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<RebindAsync>d__.name = name;
		<RebindAsync>d__.index = index;
		<RebindAsync>d__.excludes = excludes;
		<RebindAsync>d__.save = save;
		<RebindAsync>d__.<>1__state = -1;
		<RebindAsync>d__.<>t__builder.Start<InputRebinder.<RebindAsync>d__20>(ref <RebindAsync>d__);
		return <RebindAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x00037D5C File Offset: 0x00035F5C
	public static void ClearRebind(string name)
	{
		if (InputRebinder.OperationPending)
		{
			return;
		}
		InputAction inputAction = InputRebinder.PlayerInput.actions[name];
		if (inputAction == null)
		{
			Debug.LogError("找不到名为 " + name + " 的 action");
			return;
		}
		inputAction.RemoveAllBindingOverrides();
		InputIndicator.NotifyBindingChanged();
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x00037DA8 File Offset: 0x00035FA8
	private static void OnCancel(InputActionRebindingExtensions.RebindingOperation operation)
	{
		Debug.Log(operation.action.name + " binding canceled");
		operation.action.actionMap.Enable();
		Action<InputAction> onRebindComplete = InputRebinder.OnRebindComplete;
		if (onRebindComplete == null)
		{
			return;
		}
		onRebindComplete(operation.action);
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x00037DF4 File Offset: 0x00035FF4
	private static void OnComplete(InputActionRebindingExtensions.RebindingOperation operation)
	{
		Debug.Log(operation.action.name + " bind to " + operation.selectedControl.name);
		operation.action.actionMap.Enable();
		Action<InputAction> onRebindComplete = InputRebinder.OnRebindComplete;
		if (onRebindComplete != null)
		{
			onRebindComplete(operation.action);
		}
		Action onBindingChanged = InputRebinder.OnBindingChanged;
		if (onBindingChanged != null)
		{
			onBindingChanged();
		}
		InputIndicator.NotifyRebindComplete(operation.action);
	}

	// Token: 0x04000B74 RID: 2932
	[Header("Debug")]
	[SerializeField]
	private string action = "MoveAxis";

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private int index = 2;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private string[] excludes = new string[]
	{
		"<Mouse>/leftButton",
		"<Mouse>/rightButton",
		"<Pointer>/position",
		"<Pointer>/delta",
		"<Pointer>/Press"
	};

	// Token: 0x04000B77 RID: 2935
	public static Action<InputAction> OnRebindBegin;

	// Token: 0x04000B78 RID: 2936
	public static Action<InputAction> OnRebindComplete;

	// Token: 0x04000B79 RID: 2937
	public static Action OnBindingChanged;

	// Token: 0x04000B7A RID: 2938
	private static InputActionRebindingExtensions.RebindingOperation operation = new InputActionRebindingExtensions.RebindingOperation();

	// Token: 0x04000B7B RID: 2939
	private const string SaveKey = "InputBinding";
}
