using System;
using Duckov.UI;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class CameraMode : MonoBehaviour
{
	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00034E20 File Offset: 0x00033020
	// (set) Token: 0x06000C77 RID: 3191 RVA: 0x00034E27 File Offset: 0x00033027
	public static CameraMode Instance { get; private set; }

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00034E2F File Offset: 0x0003302F
	public static bool Active
	{
		get
		{
			return !(CameraMode.Instance == null) && CameraMode.Instance.active;
		}
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x00034E4C File Offset: 0x0003304C
	private void Awake()
	{
		if (CameraMode.Instance != null)
		{
			Debug.LogError("检测到多个Camera Mode", base.gameObject);
			return;
		}
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		CameraMode.Instance = this;
		UIInputManager.OnToggleCameraMode += this.OnToggleCameraMode;
		UIInputManager.OnCancel += this.OnUICancel;
		ManagedUIElement.onOpen += this.OnViewOpen;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00034EC0 File Offset: 0x000330C0
	private void OnDestroy()
	{
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		UIInputManager.OnToggleCameraMode -= this.OnToggleCameraMode;
		UIInputManager.OnCancel -= this.OnUICancel;
		ManagedUIElement.onOpen -= this.OnViewOpen;
		Shader.SetGlobalFloat("CameraModeOn", 0f);
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00034F1E File Offset: 0x0003311E
	private void OnViewOpen(ManagedUIElement element)
	{
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
		}
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00034F2C File Offset: 0x0003312C
	private void OnUICancel(UIInputEventData data)
	{
		if (data.Used)
		{
			return;
		}
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
			data.Use();
		}
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00034F49 File Offset: 0x00033149
	private void OnToggleCameraMode(UIInputEventData data)
	{
		if (CameraMode.Active)
		{
			CameraMode.Deactivate();
		}
		else
		{
			CameraMode.Activate();
		}
		data.Use();
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00034F64 File Offset: 0x00033164
	private void MActivate()
	{
		if (View.ActiveView != null)
		{
			return;
		}
		this.active = true;
		Shader.SetGlobalFloat("CameraModeOn", 1f);
		Action onCameraModeActivated = CameraMode.OnCameraModeActivated;
		if (onCameraModeActivated != null)
		{
			onCameraModeActivated();
		}
		Action<bool> onCameraModeChanged = CameraMode.OnCameraModeChanged;
		if (onCameraModeChanged == null)
		{
			return;
		}
		onCameraModeChanged(this.active);
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x00034FBA File Offset: 0x000331BA
	private void MDeactivate()
	{
		this.active = false;
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		Action onCameraModeDeactivated = CameraMode.OnCameraModeDeactivated;
		if (onCameraModeDeactivated != null)
		{
			onCameraModeDeactivated();
		}
		Action<bool> onCameraModeChanged = CameraMode.OnCameraModeChanged;
		if (onCameraModeChanged == null)
		{
			return;
		}
		onCameraModeChanged(this.active);
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x00034FF7 File Offset: 0x000331F7
	public static void Activate()
	{
		if (CameraMode.Instance == null)
		{
			return;
		}
		Shader.SetGlobalFloat("CameraModeOn", 1f);
		CameraMode.Instance.MActivate();
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00035020 File Offset: 0x00033220
	public static void Deactivate()
	{
		Shader.SetGlobalFloat("CameraModeOn", 0f);
		if (CameraMode.Instance == null)
		{
			return;
		}
		CameraMode.Instance.MDeactivate();
	}

	// Token: 0x04000AD2 RID: 2770
	public static Action OnCameraModeActivated;

	// Token: 0x04000AD3 RID: 2771
	public static Action OnCameraModeDeactivated;

	// Token: 0x04000AD4 RID: 2772
	public static Action<bool> OnCameraModeChanged;

	// Token: 0x04000AD5 RID: 2773
	private bool active;
}
