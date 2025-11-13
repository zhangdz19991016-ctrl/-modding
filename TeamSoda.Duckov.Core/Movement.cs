using System;
using ECM2;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class Movement : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001198B File Offset: 0x0000FB8B
	public float walkSpeed
	{
		get
		{
			return this.characterController.CharacterWalkSpeed * (this.characterController.IsInAdsInput ? this.characterController.AdsWalkSpeedMultiplier : 1f);
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000119B8 File Offset: 0x0000FBB8
	public float originWalkSpeed
	{
		get
		{
			return this.characterController.CharacterOriginWalkSpeed;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060003FA RID: 1018 RVA: 0x000119C5 File Offset: 0x0000FBC5
	public float runSpeed
	{
		get
		{
			return this.characterController.CharacterRunSpeed;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x000119D2 File Offset: 0x0000FBD2
	public float walkAcc
	{
		get
		{
			return this.characterController.CharacterWalkAcc;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060003FC RID: 1020 RVA: 0x000119DF File Offset: 0x0000FBDF
	public float runAcc
	{
		get
		{
			return this.characterController.CharacterRunAcc;
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x000119EC File Offset: 0x0000FBEC
	public float turnSpeed
	{
		get
		{
			return this.characterController.CharacterTurnSpeed;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060003FE RID: 1022 RVA: 0x000119F9 File Offset: 0x0000FBF9
	public float aimTurnSpeed
	{
		get
		{
			return this.characterController.CharacterAimTurnSpeed;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x00011A06 File Offset: 0x0000FC06
	public Vector3 MoveInput
	{
		get
		{
			return this.moveInput;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000400 RID: 1024 RVA: 0x00011A0E File Offset: 0x0000FC0E
	public bool Running
	{
		get
		{
			return this.running;
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x00011A16 File Offset: 0x0000FC16
	public bool Moving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000402 RID: 1026 RVA: 0x00011A1E File Offset: 0x0000FC1E
	public bool IsOnGround
	{
		get
		{
			return this.characterMovement.isOnGround;
		}
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x00011A2B File Offset: 0x0000FC2B
	public bool StandStill
	{
		get
		{
			return !this.moving && this.characterMovement.velocity.magnitude < 0.1f;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000404 RID: 1028 RVA: 0x00011A4E File Offset: 0x0000FC4E
	private bool checkCanMove
	{
		get
		{
			return this.characterController.CanMove();
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x00011A5B File Offset: 0x0000FC5B
	private bool checkCanRun
	{
		get
		{
			return this.characterController.CanRun();
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x00011A68 File Offset: 0x0000FC68
	public Vector3 CurrentMoveDirectionXZ
	{
		get
		{
			return this.currentMoveDirectionXZ;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00011A70 File Offset: 0x0000FC70
	public Transform rotationRoot
	{
		get
		{
			return this.characterController.modelRoot;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000408 RID: 1032 RVA: 0x00011A7D File Offset: 0x0000FC7D
	public unsafe Vector3 Velocity
	{
		get
		{
			return *this.characterMovement.velocity;
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00011A8F File Offset: 0x0000FC8F
	private void Awake()
	{
		this.characterMovement.constrainToGround = true;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00011A9D File Offset: 0x0000FC9D
	public void SetMoveInput(Vector3 _moveInput)
	{
		_moveInput.y = 0f;
		this.moveInput = _moveInput;
		this.moving = false;
		if (this.checkCanMove && this.moveInput.magnitude > 0.02f)
		{
			this.moving = true;
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00011ADA File Offset: 0x0000FCDA
	public void SetForceMoveVelocity(Vector3 _forceMoveVelocity)
	{
		this.forceMove = true;
		this.forceMoveVelocity = _forceMoveVelocity;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00011AEA File Offset: 0x0000FCEA
	public void SetAimDirection(Vector3 _aimDirection)
	{
		this.targetAimDirection = _aimDirection;
		this.targetAimDirection.y = 0f;
		this.targetAimDirection.Normalize();
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00011B10 File Offset: 0x0000FD10
	public void SetAimDirectionToTarget(Vector3 targetPoint, Transform aimHandler)
	{
		Vector3 position = base.transform.position;
		position.y = 0f;
		Vector3 position2 = aimHandler.position;
		position2.y = 0f;
		targetPoint.y = 0f;
		float num = Vector3.Distance(position, targetPoint);
		float num2 = Vector3.Distance(position, position2);
		if (num < num2 + 0.25f)
		{
			return;
		}
		float num3 = Mathf.Asin(num2 / num) * 57.29578f;
		this.targetAimDirection = Quaternion.Euler(0f, -num3, 0f) * (targetPoint - position).normalized;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00011BAC File Offset: 0x0000FDAC
	private void UpdateAiming()
	{
		Vector3 currentAimPoint = this.characterController.GetCurrentAimPoint();
		currentAimPoint.y = base.transform.position.y;
		if (Vector3.Distance(currentAimPoint, base.transform.position) > 0.6f && this.characterController.IsAiming() && this.characterController.CanControlAim())
		{
			this.SetAimDirectionToTarget(currentAimPoint, this.characterController.CurrentUsingAimSocket);
			return;
		}
		if (this.Moving)
		{
			this.SetAimDirection(this.CurrentMoveDirectionXZ);
		}
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00011C38 File Offset: 0x0000FE38
	public unsafe void UpdateMovement()
	{
		bool checkCanRun = this.checkCanRun;
		bool checkCanMove = this.checkCanMove;
		if (this.moveInput.magnitude <= 0.02f || !checkCanMove)
		{
			this.moving = false;
			this.running = false;
		}
		else
		{
			this.moving = true;
		}
		if (!checkCanRun)
		{
			this.running = false;
		}
		if (this.moving && checkCanRun)
		{
			this.running = true;
		}
		if (!this.forceMove)
		{
			this.UpdateNormalMove();
		}
		else
		{
			this.UpdateForceMove();
			this.forceMove = false;
		}
		this.UpdateAiming();
		this.UpdateRotation(Time.deltaTime);
		*this.characterMovement.velocity += Physics.gravity * Time.deltaTime;
		this.characterMovement.Move(*this.characterMovement.velocity, Time.deltaTime);
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00011D13 File Offset: 0x0000FF13
	private void Update()
	{
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00011D15 File Offset: 0x0000FF15
	public unsafe void ForceSetPosition(Vector3 Pos)
	{
		this.characterMovement.PauseGroundConstraint(1f);
		this.characterMovement.SetPosition(Pos, false);
		*this.characterMovement.velocity = Vector3.zero;
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00011D4C File Offset: 0x0000FF4C
	private unsafe void UpdateNormalMove()
	{
		Vector3 vector = *this.characterMovement.velocity;
		Vector3 target = Vector3.zero;
		float num = this.walkAcc;
		if (this.moving)
		{
			target = this.moveInput * (this.running ? this.runSpeed : this.walkSpeed);
			num = (this.running ? this.runAcc : this.walkAcc);
		}
		target.y = vector.y;
		vector = Vector3.MoveTowards(vector, target, num * Time.deltaTime);
		Vector3 vector2 = vector;
		vector2.y = 0f;
		if (vector2.magnitude > 0.02f)
		{
			this.currentMoveDirectionXZ = vector2.normalized;
		}
		*this.characterMovement.velocity = vector;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00011E10 File Offset: 0x00010010
	private unsafe void UpdateForceMove()
	{
		Vector3 vector = *this.characterMovement.velocity;
		Vector3 vector2 = this.forceMoveVelocity;
		float walkAcc = this.walkAcc;
		vector2.y = vector.y;
		vector = vector2;
		Vector3 vector3 = vector;
		vector3.y = 0f;
		if (vector3.magnitude > 0.02f)
		{
			this.currentMoveDirectionXZ = vector3.normalized;
		}
		*this.characterMovement.velocity = vector;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00011E88 File Offset: 0x00010088
	public void ForceTurnTo(Vector3 direction)
	{
		this.targetAimDirection = direction.normalized;
		Quaternion rotation = Quaternion.Euler(0f, Quaternion.LookRotation(this.targetAimDirection, Vector3.up).eulerAngles.y, 0f);
		this.rotationRoot.rotation = rotation;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00011EDC File Offset: 0x000100DC
	private void UpdateRotation(float deltaTime)
	{
		if (this.targetAimDirection.magnitude < 0.1f)
		{
			this.targetAimDirection = this.rotationRoot.forward;
		}
		float num = this.turnSpeed;
		if (this.characterController.IsAiming() && this.characterController.IsMainCharacter)
		{
			num = this.aimTurnSpeed;
		}
		if (this.targetAimDirection.magnitude > 0.1f)
		{
			Quaternion to = Quaternion.Euler(0f, Quaternion.LookRotation(this.targetAimDirection, Vector3.up).eulerAngles.y, 0f);
			this.rotationRoot.rotation = Quaternion.RotateTowards(this.rotationRoot.rotation, to, num * deltaTime);
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00011F92 File Offset: 0x00010192
	public void ForceSetAimDirectionToAimPoint()
	{
		this.UpdateRotation(99999f);
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x00011FA0 File Offset: 0x000101A0
	public float GetMoveAnimationValue()
	{
		float magnitude = this.characterMovement.velocity.magnitude;
		float num;
		if (this.moving && this.running)
		{
			num = Mathf.InverseLerp(this.walkSpeed, this.runSpeed, magnitude) + 1f;
			num *= this.walkSpeed / this.originWalkSpeed;
		}
		else
		{
			num = Mathf.Clamp01(magnitude / this.walkSpeed);
			num *= this.walkSpeed / this.originWalkSpeed;
		}
		if (this.walkSpeed <= 0f)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00012034 File Offset: 0x00010234
	public Vector2 GetLocalMoveDirectionAnimationValue()
	{
		Vector2 up = Vector2.up;
		if (!this.StandStill)
		{
			Vector3 direction = this.currentMoveDirectionXZ;
			Vector3 vector = this.rotationRoot.InverseTransformDirection(direction);
			up.x = vector.x;
			up.y = vector.z;
		}
		return up;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001207E File Offset: 0x0001027E
	private void FixedUpdate()
	{
	}

	// Token: 0x04000307 RID: 775
	public CharacterMainControl characterController;

	// Token: 0x04000308 RID: 776
	[SerializeField]
	private CharacterMovement characterMovement;

	// Token: 0x04000309 RID: 777
	public Vector3 targetAimDirection;

	// Token: 0x0400030A RID: 778
	private Vector3 moveInput;

	// Token: 0x0400030B RID: 779
	private bool running;

	// Token: 0x0400030C RID: 780
	private bool moving;

	// Token: 0x0400030D RID: 781
	private Vector3 currentMoveDirectionXZ;

	// Token: 0x0400030E RID: 782
	public bool forceMove;

	// Token: 0x0400030F RID: 783
	public Vector3 forceMoveVelocity;

	// Token: 0x04000310 RID: 784
	private const float movingInputThreshold = 0.02f;
}
