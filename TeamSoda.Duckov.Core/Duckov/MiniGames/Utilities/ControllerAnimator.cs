using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Duckov.MiniGames.Utilities
{
	// Token: 0x0200028A RID: 650
	public class ControllerAnimator : MonoBehaviour
	{
		// Token: 0x060014F0 RID: 5360 RVA: 0x0004DE1E File Offset: 0x0004C01E
		private void OnEnable()
		{
			MiniGame.OnInput += this.OnMiniGameInput;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0004DE31 File Offset: 0x0004C031
		private void OnDisable()
		{
			MiniGame.OnInput -= this.OnMiniGameInput;
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0004DE44 File Offset: 0x0004C044
		private void OnMiniGameInput(MiniGame game, MiniGame.MiniGameInputEventContext context)
		{
			if (this.master == null)
			{
				return;
			}
			if (this.master.Game != game)
			{
				return;
			}
			this.HandleInput(context);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0004DE70 File Offset: 0x0004C070
		private void HandleInput(MiniGame.MiniGameInputEventContext context)
		{
			if (context.isButtonEvent)
			{
				this.HandleButtonEvent(context);
				return;
			}
			if (context.isAxisEvent)
			{
				this.HandleAxisEvent(context);
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0004DE91 File Offset: 0x0004C091
		private void HandleAxisEvent(MiniGame.MiniGameInputEventContext context)
		{
			if (context.axisIndex != 0)
			{
				return;
			}
			this.SetAxis(context.axisValue);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0004DEA8 File Offset: 0x0004C0A8
		private void HandleButtonEvent(MiniGame.MiniGameInputEventContext context)
		{
			switch (context.button)
			{
			case MiniGame.Button.A:
				this.HandleBtnPushRest(this.btn_A, context.pressing);
				break;
			case MiniGame.Button.B:
				this.HandleBtnPushRest(this.btn_B, context.pressing);
				break;
			case MiniGame.Button.Start:
				this.HandleBtnPushRest(this.btn_Start, context.pressing);
				break;
			case MiniGame.Button.Select:
				this.HandleBtnPushRest(this.btn_Select, context.pressing);
				break;
			case MiniGame.Button.Left:
			case MiniGame.Button.Right:
			case MiniGame.Button.Up:
			case MiniGame.Button.Down:
				this.PlayAxisPressReleaseFX(context.button, context.pressing);
				break;
			}
			if (context.pressing)
			{
				switch (context.button)
				{
				case MiniGame.Button.None:
					break;
				case MiniGame.Button.A:
					this.ApplyTorque(1f, -0.5f);
					return;
				case MiniGame.Button.B:
					this.ApplyTorque(1f, --0f);
					return;
				case MiniGame.Button.Start:
					this.ApplyTorque(0.5f, -0.5f);
					return;
				case MiniGame.Button.Select:
					this.ApplyTorque(-0.5f, -0.5f);
					return;
				case MiniGame.Button.Left:
					this.ApplyTorque(-1f, 0f);
					return;
				case MiniGame.Button.Right:
					this.ApplyTorque(-0.5f, 0f);
					return;
				case MiniGame.Button.Up:
					this.ApplyTorque(-1f, 0.5f);
					return;
				case MiniGame.Button.Down:
					this.ApplyTorque(-1f, -0.5f);
					return;
				default:
					return;
				}
			}
			else
			{
				this.ApplyTorque(UnityEngine.Random.insideUnitCircle * 0.25f);
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0004E024 File Offset: 0x0004C224
		private void PlayAxisPressReleaseFX(MiniGame.Button button, bool pressing)
		{
			Transform transform = null;
			switch (button)
			{
			case MiniGame.Button.Left:
				transform = this.fxPos_Left;
				break;
			case MiniGame.Button.Right:
				transform = this.fxPos_Right;
				break;
			case MiniGame.Button.Up:
				transform = this.fxPos_Up;
				break;
			case MiniGame.Button.Down:
				transform = this.fxPos_Down;
				break;
			}
			if (transform == null)
			{
				return;
			}
			if (pressing)
			{
				FXPool.Play(this.buttonPressFX, transform.position, transform.rotation);
				return;
			}
			FXPool.Play(this.buttonRestFX, transform.position, transform.rotation);
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0004E0B0 File Offset: 0x0004C2B0
		private void ApplyTorque(float x, float y)
		{
			if (this.mainTransform == null)
			{
				return;
			}
			this.mainTransform.DOKill(false);
			Vector3 punch = new Vector3(-y, -x, 0f) * this.torqueStrength;
			this.mainTransform.localRotation = Quaternion.identity;
			this.mainTransform.DOPunchRotation(punch, this.torqueDuration, this.torqueVibrato, this.torqueElasticity);
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x0004E122 File Offset: 0x0004C322
		private void ApplyTorque(Vector2 torque)
		{
			this.ApplyTorque(torque.x, torque.y);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0004E136 File Offset: 0x0004C336
		private void HandleBtnPushRest(Transform btnTrans, bool pressed)
		{
			if (pressed)
			{
				this.Push(btnTrans);
				return;
			}
			this.Rest(btnTrans);
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004E14A File Offset: 0x0004C34A
		internal void SetConsole(GamingConsole master)
		{
			this.master = master;
			this.RefreshAll();
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004E15C File Offset: 0x0004C35C
		private void RefreshAll()
		{
			this.RestAll();
			if (this.master == null)
			{
				return;
			}
			MiniGame game = this.master.Game;
			if (game == null)
			{
				return;
			}
			if (game.GetButton(MiniGame.Button.A))
			{
				this.Push(this.btn_A);
			}
			if (game.GetButton(MiniGame.Button.B))
			{
				this.Push(this.btn_B);
			}
			if (game.GetButton(MiniGame.Button.Select))
			{
				this.Push(this.btn_Select);
			}
			if (game.GetButton(MiniGame.Button.Start))
			{
				this.Push(this.btn_Start);
			}
			this.SetAxis(game.GetAxis(0));
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004E1F8 File Offset: 0x0004C3F8
		private void RestAll()
		{
			this.Rest(this.btn_A);
			this.Rest(this.btn_B);
			this.Rest(this.btn_Start);
			this.Rest(this.btn_Select);
			this.Rest(this.btn_Axis);
			this.SetAxis(Vector2.zero);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0004E24C File Offset: 0x0004C44C
		private void SetAxis(Vector2 axis)
		{
			if (this.btn_Axis == null)
			{
				return;
			}
			axis = axis.normalized;
			Vector3 euler = new Vector3(0f, -axis.x * this.axisAmp, axis.y * this.axisAmp);
			Quaternion localRotation = this.btn_Axis.localRotation;
			Quaternion quaternion = Quaternion.Euler(euler);
			quaternion * Quaternion.Inverse(localRotation);
			this.btn_Axis.localRotation = quaternion;
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004E2C4 File Offset: 0x0004C4C4
		private void Push(Transform btnTransform)
		{
			if (btnTransform == null)
			{
				return;
			}
			btnTransform.DOKill(false);
			btnTransform.DOLocalMoveX(-this.btnDepth, this.transitionDuration, false).SetEase(Ease.OutElastic);
			if (this.buttonPressFX)
			{
				FXPool.Play(this.buttonPressFX, btnTransform.position, btnTransform.rotation);
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0004E324 File Offset: 0x0004C524
		private void Rest(Transform btnTransform)
		{
			if (btnTransform == null)
			{
				return;
			}
			btnTransform.DOKill(false);
			btnTransform.DOLocalMoveX(0f, this.transitionDuration, false).SetEase(Ease.OutElastic);
			if (this.buttonRestFX)
			{
				FXPool.Play(this.buttonRestFX, btnTransform.position, btnTransform.rotation);
			}
		}

		// Token: 0x04000F47 RID: 3911
		private GamingConsole master;

		// Token: 0x04000F48 RID: 3912
		public Transform mainTransform;

		// Token: 0x04000F49 RID: 3913
		public Transform btn_A;

		// Token: 0x04000F4A RID: 3914
		public Transform btn_B;

		// Token: 0x04000F4B RID: 3915
		public Transform btn_Start;

		// Token: 0x04000F4C RID: 3916
		public Transform btn_Select;

		// Token: 0x04000F4D RID: 3917
		public Transform btn_Axis;

		// Token: 0x04000F4E RID: 3918
		public Transform fxPos_Up;

		// Token: 0x04000F4F RID: 3919
		public Transform fxPos_Right;

		// Token: 0x04000F50 RID: 3920
		public Transform fxPos_Down;

		// Token: 0x04000F51 RID: 3921
		public Transform fxPos_Left;

		// Token: 0x04000F52 RID: 3922
		[SerializeField]
		private float transitionDuration = 0.2f;

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		private float axisAmp = 10f;

		// Token: 0x04000F54 RID: 3924
		[SerializeField]
		private float btnDepth = 0.003f;

		// Token: 0x04000F55 RID: 3925
		[SerializeField]
		private float torqueStrength = 5f;

		// Token: 0x04000F56 RID: 3926
		[SerializeField]
		private float torqueDuration = 0.5f;

		// Token: 0x04000F57 RID: 3927
		[SerializeField]
		private int torqueVibrato = 1;

		// Token: 0x04000F58 RID: 3928
		[SerializeField]
		private float torqueElasticity = 1f;

		// Token: 0x04000F59 RID: 3929
		[SerializeField]
		private ParticleSystem buttonPressFX;

		// Token: 0x04000F5A RID: 3930
		[SerializeField]
		private ParticleSystem buttonRestFX;
	}
}
