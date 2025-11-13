using System;
using CameraSystems;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x020001B9 RID: 441
public class FreeCameraController : MonoBehaviour
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000D1A RID: 3354 RVA: 0x00036F0C File Offset: 0x0003510C
	private Gamepad Gamepad
	{
		get
		{
			return Gamepad.current;
		}
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00036F13 File Offset: 0x00035113
	private void Awake()
	{
		if (!this.propertiesControl)
		{
			this.propertiesControl = base.GetComponent<CameraPropertiesControl>();
		}
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x00036F2E File Offset: 0x0003512E
	private void OnEnable()
	{
		this.SetRotation(base.transform.rotation);
		this.SnapToMainCamera();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x00036F48 File Offset: 0x00035148
	public void SetRotation(Quaternion rotation)
	{
		Vector3 eulerAngles = rotation.eulerAngles;
		this.yaw = eulerAngles.y;
		this.pitch = eulerAngles.x;
		this.yawTarget = this.yaw;
		this.pitchTarget = this.pitch;
		if (this.pitch > 180f)
		{
			this.pitch -= 360f;
		}
		if (this.pitch < -180f)
		{
			this.pitch += 360f;
		}
		this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x00037008 File Offset: 0x00035208
	private unsafe void Update()
	{
		if (this.Gamepad == null)
		{
			return;
		}
		bool isPressed = this.Gamepad.rightShoulder.isPressed;
		float num = this.moveSpeed * (float)(isPressed ? 2 : 1);
		CharacterMainControl main = CharacterMainControl.Main;
		Vector2 vector = *this.Gamepad.leftStick.value;
		float d = *this.Gamepad.rightTrigger.value - *this.Gamepad.leftTrigger.value;
		Vector3 vector2 = new Vector3(vector.x * num, 0f, vector.y * num) * Time.unscaledDeltaTime;
		Vector3 a = this.projectMovementOnXZPlane ? Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized : base.transform.forward;
		Vector3 a2 = this.projectMovementOnXZPlane ? Vector3.ProjectOnPlane(base.transform.right, Vector3.up).normalized : base.transform.right;
		Vector3 b = d * Vector3.up * num * 0.5f * Time.unscaledDeltaTime;
		Vector3 b2 = a * vector2.z + a2 * vector2.x + b;
		if (!this.followCharacter || main == null)
		{
			this.worldPosTarget += b2;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, this.worldPosTarget, ref this.velocityWorldSpace, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
			if (main == null)
			{
				this.followCharacter = false;
			}
		}
		else
		{
			this.offsetFromCharacter += b2;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, main.transform.position + this.offsetFromCharacter, ref this.velocityLocalSpace, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		}
		Vector3 vector3 = *this.Gamepad.rightStick.value * this.rotateSpeed * this.vCamera.m_Lens.FieldOfView / 60f;
		this.yawTarget += vector3.x * Time.unscaledDeltaTime;
		this.yaw = Mathf.SmoothDamp(this.yaw, this.yawTarget, ref this.yawVelocity, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		this.pitchTarget += -vector3.y * Time.unscaledDeltaTime;
		this.pitch = Mathf.SmoothDamp(this.pitch, this.pitchTarget, ref this.pitchVelocity, this.smoothTime, 20f, 10f * Time.unscaledDeltaTime);
		this.pitch = Mathf.Clamp(this.pitch, -89f, 89f);
		base.transform.rotation = Quaternion.Euler(this.pitch, this.yaw, 0f);
		if (this.Gamepad.buttonNorth.wasPressedThisFrame)
		{
			this.SnapToMainCamera();
		}
		if (this.Gamepad.buttonEast.wasPressedThisFrame)
		{
			this.ToggleFollowTarget();
		}
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0003737E File Offset: 0x0003557E
	private void OnDestroy()
	{
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x00037380 File Offset: 0x00035580
	private void ToggleFollowTarget()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		this.followCharacter = !this.followCharacter;
		if (this.followCharacter)
		{
			this.offsetFromCharacter = base.transform.position - main.transform.position;
		}
		this.worldPosTarget = base.transform.position;
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x000373E8 File Offset: 0x000355E8
	private void SnapToMainCamera()
	{
		if (GameCamera.Instance == null)
		{
			return;
		}
		Camera renderCamera = GameCamera.Instance.renderCamera;
		if (renderCamera == null)
		{
			return;
		}
		base.transform.position = renderCamera.transform.position;
		this.worldPosTarget = renderCamera.transform.position;
		this.vCamera.m_Lens.FieldOfView = renderCamera.fieldOfView;
		this.SetRotation(renderCamera.transform.rotation);
		CharacterMainControl main = CharacterMainControl.Main;
		if (main != null && this.followCharacter)
		{
			this.offsetFromCharacter = base.transform.position - main.transform.position;
		}
	}

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	private CameraPropertiesControl propertiesControl;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private float moveSpeed = 10f;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private float rotateSpeed = 180f;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private float smoothTime = 2f;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private Vector2 minMaxXRotation = new Vector2(-89f, 89f);

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	private bool projectMovementOnXZPlane;

	// Token: 0x04000B5E RID: 2910
	[Range(-180f, 180f)]
	private float yaw;

	// Token: 0x04000B5F RID: 2911
	[Range(-89f, 89f)]
	private float pitch;

	// Token: 0x04000B60 RID: 2912
	[SerializeField]
	private CinemachineVirtualCamera vCamera;

	// Token: 0x04000B61 RID: 2913
	private bool followCharacter;

	// Token: 0x04000B62 RID: 2914
	private Vector3 offsetFromCharacter;

	// Token: 0x04000B63 RID: 2915
	private Vector3 worldPosTarget;

	// Token: 0x04000B64 RID: 2916
	private Vector3 velocityWorldSpace;

	// Token: 0x04000B65 RID: 2917
	private Vector3 velocityLocalSpace;

	// Token: 0x04000B66 RID: 2918
	private float yawVelocity;

	// Token: 0x04000B67 RID: 2919
	private float pitchVelocity;

	// Token: 0x04000B68 RID: 2920
	private float yawTarget;

	// Token: 0x04000B69 RID: 2921
	private float pitchTarget;
}
