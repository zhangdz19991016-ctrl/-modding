using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityTemplateProjects
{
	// Token: 0x02000225 RID: 549
	public class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x06001091 RID: 4241 RVA: 0x000407D4 File Offset: 0x0003E9D4
		private void Start()
		{
			InputActionMap map = new InputActionMap("Simple Camera Controller");
			this.lookAction = map.AddAction("look", InputActionType.Value, "<Mouse>/delta", null, null, null, null);
			this.movementAction = map.AddAction("move", InputActionType.Value, "<Gamepad>/leftStick", null, null, null, null);
			this.verticalMovementAction = map.AddAction("Vertical Movement", InputActionType.Value, null, null, null, null, null);
			this.boostFactorAction = map.AddAction("Boost Factor", InputActionType.Value, "<Mouse>/scroll", null, null, null, null);
			this.lookAction.AddBinding("<Gamepad>/rightStick", null, null, null).WithProcessor("scaleVector2(x=15, y=15)");
			this.movementAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/w", null, null).With("Up", "<Keyboard>/upArrow", null, null).With("Down", "<Keyboard>/s", null, null).With("Down", "<Keyboard>/downArrow", null, null).With("Left", "<Keyboard>/a", null, null).With("Left", "<Keyboard>/leftArrow", null, null).With("Right", "<Keyboard>/d", null, null).With("Right", "<Keyboard>/rightArrow", null, null);
			this.verticalMovementAction.AddCompositeBinding("Dpad", null, null).With("Up", "<Keyboard>/pageUp", null, null).With("Down", "<Keyboard>/pageDown", null, null).With("Up", "<Keyboard>/e", null, null).With("Down", "<Keyboard>/q", null, null).With("Up", "<Gamepad>/rightshoulder", null, null).With("Down", "<Gamepad>/leftshoulder", null, null);
			this.boostFactorAction.AddBinding("<Gamepad>/Dpad", null, null, null).WithProcessor("scaleVector2(x=1, y=4)");
			this.movementAction.Enable();
			this.lookAction.Enable();
			this.verticalMovementAction.Enable();
			this.boostFactorAction.Enable();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00040A00 File Offset: 0x0003EC00
		private void OnEnable()
		{
			this.m_TargetCameraState.SetFromTransform(base.transform);
			this.m_InterpolatingCameraState.SetFromTransform(base.transform);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00040A24 File Offset: 0x0003EC24
		private Vector3 GetInputTranslationDirection()
		{
			Vector3 zero = Vector3.zero;
			Vector2 vector = this.movementAction.ReadValue<Vector2>();
			zero.x = vector.x;
			zero.z = vector.y;
			zero.y = this.verticalMovementAction.ReadValue<Vector2>().y;
			return zero;
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00040A78 File Offset: 0x0003EC78
		private void Update()
		{
			if (this.IsEscapePressed())
			{
				Application.Quit();
			}
			if (this.IsRightMouseButtonDown())
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
			if (this.IsRightMouseButtonUp())
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			if (this.IsCameraRotationAllowed())
			{
				Vector2 vector = this.GetInputLookRotation() * Time.deltaTime * 5f;
				if (this.invertY)
				{
					vector.y = -vector.y;
				}
				float num = this.mouseSensitivityCurve.Evaluate(vector.magnitude);
				this.m_TargetCameraState.yaw += vector.x * num;
				this.m_TargetCameraState.pitch += vector.y * num;
			}
			Vector3 vector2 = this.GetInputTranslationDirection() * Time.deltaTime;
			if (this.IsBoostPressed())
			{
				vector2 *= 10f;
			}
			this.boost += this.GetBoostFactor();
			vector2 *= Mathf.Pow(2f, this.boost);
			this.m_TargetCameraState.Translate(vector2);
			float positionLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.positionLerpTime * Time.deltaTime);
			float rotationLerpPct = 1f - Mathf.Exp(Mathf.Log(0.00999999f) / this.rotationLerpTime * Time.deltaTime);
			this.m_InterpolatingCameraState.LerpTowards(this.m_TargetCameraState, positionLerpPct, rotationLerpPct);
			this.m_InterpolatingCameraState.UpdateTransform(base.transform);
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00040BFC File Offset: 0x0003EDFC
		private float GetBoostFactor()
		{
			return this.boostFactorAction.ReadValue<Vector2>().y * 0.01f;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00040C14 File Offset: 0x0003EE14
		private Vector2 GetInputLookRotation()
		{
			return this.lookAction.ReadValue<Vector2>();
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00040C21 File Offset: 0x0003EE21
		private bool IsBoostPressed()
		{
			return (Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed) | (Gamepad.current != null && Gamepad.current.xButton.isPressed);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00040C56 File Offset: 0x0003EE56
		private bool IsEscapePressed()
		{
			return Keyboard.current != null && Keyboard.current.escapeKey.isPressed;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00040C70 File Offset: 0x0003EE70
		private bool IsCameraRotationAllowed()
		{
			return (Mouse.current != null && Mouse.current.rightButton.isPressed) | (Gamepad.current != null && Gamepad.current.rightStick.ReadValue().magnitude > 0f);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00040CBF File Offset: 0x0003EEBF
		private bool IsRightMouseButtonDown()
		{
			return Mouse.current != null && Mouse.current.rightButton.isPressed;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00040CD9 File Offset: 0x0003EED9
		private bool IsRightMouseButtonUp()
		{
			return Mouse.current != null && !Mouse.current.rightButton.isPressed;
		}

		// Token: 0x04000D38 RID: 3384
		private SimpleCameraController.CameraState m_TargetCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000D39 RID: 3385
		private SimpleCameraController.CameraState m_InterpolatingCameraState = new SimpleCameraController.CameraState();

		// Token: 0x04000D3A RID: 3386
		[Header("Movement Settings")]
		[Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
		public float boost = 3.5f;

		// Token: 0x04000D3B RID: 3387
		[Tooltip("Time it takes to interpolate camera position 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float positionLerpTime = 0.2f;

		// Token: 0x04000D3C RID: 3388
		[Header("Rotation Settings")]
		[Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
		public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f, 0f, 5f),
			new Keyframe(1f, 2.5f, 0f, 0f)
		});

		// Token: 0x04000D3D RID: 3389
		[Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target.")]
		[Range(0.001f, 1f)]
		public float rotationLerpTime = 0.01f;

		// Token: 0x04000D3E RID: 3390
		[Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
		public bool invertY;

		// Token: 0x04000D3F RID: 3391
		private InputAction movementAction;

		// Token: 0x04000D40 RID: 3392
		private InputAction verticalMovementAction;

		// Token: 0x04000D41 RID: 3393
		private InputAction lookAction;

		// Token: 0x04000D42 RID: 3394
		private InputAction boostFactorAction;

		// Token: 0x04000D43 RID: 3395
		private bool mouseRightButtonPressed;

		// Token: 0x0200050F RID: 1295
		private class CameraState
		{
			// Token: 0x060027DA RID: 10202 RVA: 0x00091E6C File Offset: 0x0009006C
			public void SetFromTransform(Transform t)
			{
				this.pitch = t.eulerAngles.x;
				this.yaw = t.eulerAngles.y;
				this.roll = t.eulerAngles.z;
				this.x = t.position.x;
				this.y = t.position.y;
				this.z = t.position.z;
			}

			// Token: 0x060027DB RID: 10203 RVA: 0x00091EE0 File Offset: 0x000900E0
			public void Translate(Vector3 translation)
			{
				Vector3 vector = Quaternion.Euler(this.pitch, this.yaw, this.roll) * translation;
				this.x += vector.x;
				this.y += vector.y;
				this.z += vector.z;
			}

			// Token: 0x060027DC RID: 10204 RVA: 0x00091F44 File Offset: 0x00090144
			public void LerpTowards(SimpleCameraController.CameraState target, float positionLerpPct, float rotationLerpPct)
			{
				this.yaw = Mathf.Lerp(this.yaw, target.yaw, rotationLerpPct);
				this.pitch = Mathf.Lerp(this.pitch, target.pitch, rotationLerpPct);
				this.roll = Mathf.Lerp(this.roll, target.roll, rotationLerpPct);
				this.x = Mathf.Lerp(this.x, target.x, positionLerpPct);
				this.y = Mathf.Lerp(this.y, target.y, positionLerpPct);
				this.z = Mathf.Lerp(this.z, target.z, positionLerpPct);
			}

			// Token: 0x060027DD RID: 10205 RVA: 0x00091FE1 File Offset: 0x000901E1
			public void UpdateTransform(Transform t)
			{
				t.eulerAngles = new Vector3(this.pitch, this.yaw, this.roll);
				t.position = new Vector3(this.x, this.y, this.z);
			}

			// Token: 0x04001E04 RID: 7684
			public float yaw;

			// Token: 0x04001E05 RID: 7685
			public float pitch;

			// Token: 0x04001E06 RID: 7686
			public float roll;

			// Token: 0x04001E07 RID: 7687
			public float x;

			// Token: 0x04001E08 RID: 7688
			public float y;

			// Token: 0x04001E09 RID: 7689
			public float z;
		}
	}
}
