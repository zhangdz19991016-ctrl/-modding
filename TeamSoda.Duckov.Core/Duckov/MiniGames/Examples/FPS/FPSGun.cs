using System;
using DG.Tweening;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002DA RID: 730
	public class FPSGun : MiniGameBehaviour
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x00054D2C File Offset: 0x00052F2C
		public float ScatterAngle
		{
			get
			{
				return Mathf.Lerp(this.minScatterAngle, this.maxScatterAngle, this.scatterStatus);
			}
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00054D48 File Offset: 0x00052F48
		private void Fire()
		{
			this.coolDown = 1f / this.fireRate;
			this.DoCast();
			this.muzzleFlash.Play();
			this.DoFireAnimation();
			this.scatterStatus = Mathf.MoveTowards(this.scatterStatus, 1f, this.scatterIncrementPerShot);
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00054D9C File Offset: 0x00052F9C
		private void DoFireAnimation()
		{
			this.graphicsTransform.DOKill(true);
			this.graphicsTransform.localPosition = Vector3.zero;
			this.graphicsTransform.localRotation = Quaternion.identity;
			this.graphicsTransform.DOPunchPosition(Vector3.back * 0.2f, 0.2f, 10, 1f, false);
			this.graphicsTransform.DOShakeRotation(0.5f, -Vector3.right * 10f, 10, 90f, true);
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00054E2C File Offset: 0x0005302C
		private void DoCast()
		{
			Ray ray = this.mainCamera.ViewportPointToRay(Vector3.one * 0.5f);
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.ScatterAngle / 2f;
			Vector3 vector2 = Quaternion.Euler(vector.y, vector.x, 0f) * Vector3.forward;
			Vector3 direction = this.mainCamera.transform.localToWorldMatrix.MultiplyVector(vector2);
			ray.direction = direction;
			RaycastHit castInfo;
			Physics.Raycast(ray, out castInfo, 100f, this.castLayers);
			this.HandleBulletTracer(castInfo);
			if (castInfo.collider == null)
			{
				return;
			}
			FPSDamageInfo fpsdamageInfo = new FPSDamageInfo
			{
				source = this,
				amount = 1f,
				point = castInfo.point,
				normal = castInfo.normal
			};
			FPSDamageReceiver component = castInfo.collider.GetComponent<FPSDamageReceiver>();
			if (component)
			{
				component.CastDamage(fpsdamageInfo);
				return;
			}
			this.HandleNormalHit(fpsdamageInfo);
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00054F4C File Offset: 0x0005314C
		private void HandleBulletTracer(RaycastHit castInfo)
		{
			if (this.bulletTracer == null)
			{
				return;
			}
			if (!true)
			{
				return;
			}
			Vector3 position = this.muzzle.transform.position;
			Vector3 vector = this.muzzle.transform.forward;
			if (castInfo.collider != null)
			{
				vector = castInfo.point - position;
				if ((castInfo.point - position).magnitude < 5f)
				{
					this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -vector);
					this.bulletTracer.transform.position = castInfo.point;
				}
				else
				{
					this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector);
					this.bulletTracer.transform.position = this.muzzle.position;
				}
			}
			else
			{
				this.bulletTracer.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vector);
				this.bulletTracer.transform.position = this.muzzle.position;
			}
			this.bulletTracer.Emit(1);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x0005507D File Offset: 0x0005327D
		private void HandleNormalHit(FPSDamageInfo info)
		{
			FXPool.Play(this.normalHitFXPrefab, info.point, Quaternion.FromToRotation(Vector3.forward, info.normal));
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000550A1 File Offset: 0x000532A1
		internal void SetTrigger(bool value)
		{
			this.trigger = value;
			if (value)
			{
				this.justPressedTrigger = true;
			}
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x000550B4 File Offset: 0x000532B4
		internal void Setup(Camera mainCamera, Transform gunParent)
		{
			base.transform.SetParent(gunParent, false);
			base.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			this.mainCamera = mainCamera;
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000550E0 File Offset: 0x000532E0
		protected override void OnUpdate(float deltaTime)
		{
			if (this.coolDown > 0f)
			{
				this.coolDown -= deltaTime;
				this.coolDown = Mathf.Max(0f, this.coolDown);
			}
			if (this.coolDown <= 0f && this.trigger && (this.auto || this.justPressedTrigger))
			{
				this.Fire();
			}
			this.justPressedTrigger = false;
			this.scatterStatus = Mathf.MoveTowards(this.scatterStatus, 0f, this.scatterDecayRate * deltaTime);
			this.UpdateGunPhysicsStatus(deltaTime);
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x00055175 File Offset: 0x00053375
		private void UpdateGunPhysicsStatus(float deltaTime)
		{
		}

		// Token: 0x040010D5 RID: 4309
		[SerializeField]
		private float fireRate = 1f;

		// Token: 0x040010D6 RID: 4310
		[SerializeField]
		private bool auto;

		// Token: 0x040010D7 RID: 4311
		[SerializeField]
		private Transform muzzle;

		// Token: 0x040010D8 RID: 4312
		[SerializeField]
		private ParticleSystem muzzleFlash;

		// Token: 0x040010D9 RID: 4313
		[SerializeField]
		private ParticleSystem bulletTracer;

		// Token: 0x040010DA RID: 4314
		[SerializeField]
		private LayerMask castLayers = -1;

		// Token: 0x040010DB RID: 4315
		[SerializeField]
		private ParticleSystem normalHitFXPrefab;

		// Token: 0x040010DC RID: 4316
		[SerializeField]
		private float minScatterAngle;

		// Token: 0x040010DD RID: 4317
		[SerializeField]
		private float maxScatterAngle;

		// Token: 0x040010DE RID: 4318
		[SerializeField]
		private float scatterIncrementPerShot;

		// Token: 0x040010DF RID: 4319
		[SerializeField]
		private float scatterDecayRate;

		// Token: 0x040010E0 RID: 4320
		[SerializeField]
		private Transform graphicsTransform;

		// Token: 0x040010E1 RID: 4321
		[SerializeField]
		private FPSGun.Pose idlePose;

		// Token: 0x040010E2 RID: 4322
		[SerializeField]
		private FPSGun.Pose recoilPose;

		// Token: 0x040010E3 RID: 4323
		private float scatterStatus;

		// Token: 0x040010E4 RID: 4324
		private float coolDown;

		// Token: 0x040010E5 RID: 4325
		private Camera mainCamera;

		// Token: 0x040010E6 RID: 4326
		private bool trigger;

		// Token: 0x040010E7 RID: 4327
		private bool justPressedTrigger;

		// Token: 0x0200057B RID: 1403
		[Serializable]
		public struct Pose
		{
			// Token: 0x060028BF RID: 10431 RVA: 0x00096B34 File Offset: 0x00094D34
			public static FPSGun.Pose Extraterpolate(FPSGun.Pose poseA, FPSGun.Pose poseB, float t)
			{
				return new FPSGun.Pose
				{
					localPosition = Vector3.LerpUnclamped(poseA.localPosition, poseB.localPosition, t),
					localRotation = Quaternion.LerpUnclamped(poseA.localRotation, poseB.localRotation, t)
				};
			}

			// Token: 0x060028C0 RID: 10432 RVA: 0x00096B7C File Offset: 0x00094D7C
			public Pose(Transform fromTransform)
			{
				this.localPosition = fromTransform.localPosition;
				this.localRotation = fromTransform.localRotation;
			}

			// Token: 0x04001FBD RID: 8125
			[SerializeField]
			private Vector3 localPosition;

			// Token: 0x04001FBE RID: 8126
			[SerializeField]
			private Quaternion localRotation;
		}
	}
}
