using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Token: 0x020001A5 RID: 421
public class CameraModeController : MonoBehaviour
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000C83 RID: 3203 RVA: 0x00035051 File Offset: 0x00033251
	private static string filePath
	{
		get
		{
			if (GameMetaData.Instance.Platform == Platform.WeGame)
			{
				return Application.streamingAssetsPath + "/ScreenShots";
			}
			return Application.persistentDataPath + "/ScreenShots";
		}
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00035080 File Offset: 0x00033280
	private void UpdateInput()
	{
		this.moveInput = this.inputActionAsset["CameraModeMove"].ReadValue<Vector2>();
		this.focusInput = this.inputActionAsset["CameraModeFocus"].IsPressed();
		this.upDownInput = this.inputActionAsset["CameraModeUpDown"].ReadValue<float>();
		this.fovInput = this.inputActionAsset["CameraModeFOV"].ReadValue<float>();
		this.aimInput = this.inputActionAsset["CameraModeAim"].ReadValue<Vector2>();
		this.captureInput = this.inputActionAsset["CameraModeCapture"].WasPressedThisFrame();
		this.fastInput = this.inputActionAsset["CameraModeFaster"].IsPressed();
		this.openFolderInput = this.inputActionAsset["CameraModeOpenFolder"].WasPressedThisFrame();
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x00035168 File Offset: 0x00033368
	private void Awake()
	{
		CameraMode.OnCameraModeActivated = (Action)Delegate.Combine(CameraMode.OnCameraModeActivated, new Action(this.OnCameraModeActivated));
		CameraMode.OnCameraModeDeactivated = (Action)Delegate.Combine(CameraMode.OnCameraModeDeactivated, new Action(this.OnCameraModeDeactivated));
		this.inputActionAsset.Enable();
		this.vCam.gameObject.SetActive(true);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x000351E0 File Offset: 0x000333E0
	private void Update()
	{
		if (!this.actived)
		{
			return;
		}
		this.UpdateInput();
		if (this.shootting)
		{
			return;
		}
		this.UpdateMove();
		this.UpdateLook();
		this.UpdateFov();
		if (this.captureInput)
		{
			this.Shot().Forget();
		}
		if (this.openFolderInput)
		{
			CameraModeController.OpenFolder();
			this.openFolderInput = false;
		}
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00035241 File Offset: 0x00033441
	private void LateUpdate()
	{
		this.UpdateFocus();
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0003524C File Offset: 0x0003344C
	private void UpdateMove()
	{
		Vector3 forward = this.vCam.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Vector3 right = this.vCam.transform.right;
		right.y = 0f;
		right.Normalize();
		Vector3 a = right * this.moveInput.x + forward * this.moveInput.y;
		a.Normalize();
		a += this.upDownInput * Vector3.up;
		this.vCam.transform.position += Time.unscaledDeltaTime * a * (this.fastInput ? this.fastMoveSpeed : this.moveSpeed);
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00035328 File Offset: 0x00033528
	private void UpdateLook()
	{
		this.pitch += -this.aimInput.y * this.aimSpeed * Time.unscaledDeltaTime;
		this.pitch = Mathf.Clamp(this.pitch, -89.9f, 89.9f);
		this.yaw += this.aimInput.x * this.aimSpeed * Time.unscaledDeltaTime;
		this.vCam.transform.localRotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x000353C4 File Offset: 0x000335C4
	private void UpdateFocus()
	{
		if (this.focusInput)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.vCam.transform.position, this.vCam.transform.forward, out raycastHit, 100f, this.dofLayerMask))
			{
				this.dofTargetPoint = raycastHit.point + this.vCam.transform.forward * -0.2f;
				this.dofTarget.position = this.dofTargetPoint;
			}
			this.focusMeshTimer = this.focusMeshAppearTime;
			if (!this.focusMesh.gameObject.activeSelf)
			{
				this.focusMesh.gameObject.SetActive(true);
			}
		}
		else if (this.focusMeshTimer > 0f)
		{
			this.focusMeshTimer -= Time.unscaledDeltaTime;
			if (this.focusMeshTimer <= 0f)
			{
				this.focusMeshTimer = 0f;
				this.focusMesh.gameObject.SetActive(false);
			}
		}
		if (this.focusMesh.gameObject.activeSelf)
		{
			this.focusMesh.transform.localScale = Vector3.one * this.focusMeshSize * this.focusMeshTimer / this.focusMeshAppearTime;
		}
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x00035514 File Offset: 0x00033714
	private void UpdateFov()
	{
		float num = this.vCam.m_Lens.FieldOfView;
		num += -this.fovChangeSpeed * this.fovInput;
		num = Mathf.Clamp(num, this.fovRange.x, this.fovRange.y);
		this.vCam.m_Lens.FieldOfView = num;
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x00035574 File Offset: 0x00033774
	private void OnDestroy()
	{
		CameraMode.OnCameraModeActivated = (Action)Delegate.Remove(CameraMode.OnCameraModeActivated, new Action(this.OnCameraModeActivated));
		CameraMode.OnCameraModeDeactivated = (Action)Delegate.Remove(CameraMode.OnCameraModeDeactivated, new Action(this.OnCameraModeDeactivated));
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x000355C4 File Offset: 0x000337C4
	private void OnCameraModeActivated()
	{
		GameCamera instance = GameCamera.Instance;
		if (instance != null)
		{
			CameraArm mianCameraArm = instance.mianCameraArm;
			this.yaw = mianCameraArm.yaw;
			this.pitch = mianCameraArm.pitch;
			this.vCam.transform.position = instance.renderCamera.transform.position;
			this.dofTargetPoint = instance.target.transform.position;
			this.actived = true;
			this.vCam.m_Lens.FieldOfView = instance.renderCamera.fieldOfView;
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x00035666 File Offset: 0x00033866
	public static void OpenFolder()
	{
		GUIUtility.systemCopyBuffer = CameraModeController.filePath;
		NotificationText.Push(CameraModeController.filePath ?? "");
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00035685 File Offset: 0x00033885
	private void OnCameraModeDeactivated()
	{
		this.actived = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x0003569C File Offset: 0x0003389C
	private UniTaskVoid Shot()
	{
		CameraModeController.<Shot>d__44 <Shot>d__;
		<Shot>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Shot>d__.<>4__this = this;
		<Shot>d__.<>1__state = -1;
		<Shot>d__.<>t__builder.Start<CameraModeController.<Shot>d__44>(ref <Shot>d__);
		return <Shot>d__.<>t__builder.Task;
	}

	// Token: 0x04000AD6 RID: 2774
	public CinemachineVirtualCamera vCam;

	// Token: 0x04000AD7 RID: 2775
	private bool actived;

	// Token: 0x04000AD8 RID: 2776
	public Transform dofTarget;

	// Token: 0x04000AD9 RID: 2777
	private Vector3 dofTargetPoint;

	// Token: 0x04000ADA RID: 2778
	public InputActionAsset inputActionAsset;

	// Token: 0x04000ADB RID: 2779
	public LayerMask dofLayerMask;

	// Token: 0x04000ADC RID: 2780
	private Vector2 moveInput;

	// Token: 0x04000ADD RID: 2781
	private float upDownInput;

	// Token: 0x04000ADE RID: 2782
	private bool focusInput;

	// Token: 0x04000ADF RID: 2783
	private bool captureInput;

	// Token: 0x04000AE0 RID: 2784
	private bool fastInput;

	// Token: 0x04000AE1 RID: 2785
	private bool openFolderInput;

	// Token: 0x04000AE2 RID: 2786
	public GameObject focusMesh;

	// Token: 0x04000AE3 RID: 2787
	public float focusMeshSize = 0.3f;

	// Token: 0x04000AE4 RID: 2788
	private float focusMeshCurrentSize = 0.3f;

	// Token: 0x04000AE5 RID: 2789
	public float focusMeshAppearTime = 1f;

	// Token: 0x04000AE6 RID: 2790
	private float focusMeshTimer = 0.3f;

	// Token: 0x04000AE7 RID: 2791
	private float fovInput;

	// Token: 0x04000AE8 RID: 2792
	private Vector2 aimInput;

	// Token: 0x04000AE9 RID: 2793
	public float moveSpeed;

	// Token: 0x04000AEA RID: 2794
	public float fastMoveSpeed;

	// Token: 0x04000AEB RID: 2795
	public float aimSpeed;

	// Token: 0x04000AEC RID: 2796
	private float yaw;

	// Token: 0x04000AED RID: 2797
	private float pitch;

	// Token: 0x04000AEE RID: 2798
	private bool shootting;

	// Token: 0x04000AEF RID: 2799
	public ColorPunch colorPunch;

	// Token: 0x04000AF0 RID: 2800
	public Vector2 fovRange = new Vector2(5f, 60f);

	// Token: 0x04000AF1 RID: 2801
	[Range(0.01f, 0.5f)]
	public float fovChangeSpeed = 10f;

	// Token: 0x04000AF2 RID: 2802
	public CanvasGroup indicatorGroup;

	// Token: 0x04000AF3 RID: 2803
	public UnityEvent OnCapturedEvent;
}
