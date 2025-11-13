using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000287 RID: 647
	public class MiniGame : MonoBehaviour
	{
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0004D541 File Offset: 0x0004B741
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0004D549 File Offset: 0x0004B749
		public Camera Camera
		{
			get
			{
				return this.camera;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0004D551 File Offset: 0x0004B751
		public Camera UICamera
		{
			get
			{
				return this.uiCamera;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0004D559 File Offset: 0x0004B759
		public RenderTexture RenderTexture
		{
			get
			{
				return this.renderTexture;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0004D561 File Offset: 0x0004B761
		public GamingConsole Console
		{
			get
			{
				return this.console;
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x060014C5 RID: 5317 RVA: 0x0004D56C File Offset: 0x0004B76C
		// (remove) Token: 0x060014C6 RID: 5318 RVA: 0x0004D5A0 File Offset: 0x0004B7A0
		public static event Action<MiniGame, MiniGame.MiniGameInputEventContext> OnInput;

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004D5D3 File Offset: 0x0004B7D3
		public void SetRenderTexture(RenderTexture texture)
		{
			this.camera.targetTexture = texture;
			if (this.uiCamera)
			{
				this.uiCamera.targetTexture = texture;
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0004D5FC File Offset: 0x0004B7FC
		public RenderTexture CreateAndSetRenderTexture(int width, int height)
		{
			RenderTexture result = new RenderTexture(width, height, 32);
			this.SetRenderTexture(result);
			return result;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004D61B File Offset: 0x0004B81B
		private void Awake()
		{
			if (this.renderTexture != null)
			{
				this.SetRenderTexture(this.renderTexture);
			}
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004D638 File Offset: 0x0004B838
		public void SetInputAxis(Vector2 axis, int index = 0)
		{
			Vector2 vector = this.inputAxis_0;
			if (index == 0)
			{
				this.inputAxis_0 = axis;
			}
			if (index == 1)
			{
				this.inputAxis_1 = axis;
			}
			if (index == 0)
			{
				bool flag = axis.x < -0.1f;
				bool flag2 = axis.x > 0.1f;
				bool flag3 = axis.y > 0.1f;
				bool flag4 = axis.y < -0.1f;
				bool flag5 = vector.x < -0.1f;
				bool flag6 = vector.x > 0.1f;
				bool flag7 = vector.y > 0.1f;
				bool flag8 = vector.y < -0.1f;
				if (flag != flag5)
				{
					this.SetButton(MiniGame.Button.Left, flag);
				}
				if (flag2 != flag6)
				{
					this.SetButton(MiniGame.Button.Right, flag2);
				}
				if (flag3 != flag7)
				{
					this.SetButton(MiniGame.Button.Up, flag3);
				}
				if (flag4 != flag8)
				{
					this.SetButton(MiniGame.Button.Down, flag4);
				}
			}
			Action<MiniGame, MiniGame.MiniGameInputEventContext> onInput = MiniGame.OnInput;
			if (onInput == null)
			{
				return;
			}
			onInput(this, new MiniGame.MiniGameInputEventContext
			{
				isAxisEvent = true,
				axisIndex = index,
				axisValue = axis
			});
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0004D744 File Offset: 0x0004B944
		public void SetButton(MiniGame.Button button, bool down)
		{
			MiniGame.ButtonStatus buttonStatus;
			if (!this.buttons.TryGetValue(button, out buttonStatus))
			{
				buttonStatus = new MiniGame.ButtonStatus();
				this.buttons[button] = buttonStatus;
			}
			if (down)
			{
				buttonStatus.justPressed = true;
				buttonStatus.pressed = true;
			}
			else
			{
				buttonStatus.pressed = false;
				buttonStatus.justReleased = true;
			}
			this.buttons[button] = buttonStatus;
			Action<MiniGame, MiniGame.MiniGameInputEventContext> onInput = MiniGame.OnInput;
			if (onInput == null)
			{
				return;
			}
			onInput(this, new MiniGame.MiniGameInputEventContext
			{
				isButtonEvent = true,
				button = button,
				pressing = buttonStatus.pressed,
				buttonDown = buttonStatus.justPressed,
				buttonUp = buttonStatus.justReleased
			});
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0004D7F4 File Offset: 0x0004B9F4
		public bool GetButton(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.pressed;
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0004D81C File Offset: 0x0004BA1C
		public bool GetButtonDown(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.justPressed;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0004D844 File Offset: 0x0004BA44
		public bool GetButtonUp(MiniGame.Button button)
		{
			MiniGame.ButtonStatus buttonStatus;
			return this.buttons.TryGetValue(button, out buttonStatus) && buttonStatus.justReleased;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004D86C File Offset: 0x0004BA6C
		public Vector2 GetAxis(int index = 0)
		{
			if (index == 0)
			{
				return this.inputAxis_0;
			}
			if (index == 1)
			{
				return this.inputAxis_1;
			}
			return default(Vector2);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0004D897 File Offset: 0x0004BA97
		private void Tick(float deltaTime)
		{
			this.UpdateLogic(deltaTime);
			this.Cleanup();
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0004D8A6 File Offset: 0x0004BAA6
		private void UpdateLogic(float deltaTime)
		{
			Action<MiniGame, float> action = MiniGame.onUpdateLogic;
			if (action == null)
			{
				return;
			}
			action(this, deltaTime);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0004D8BC File Offset: 0x0004BABC
		private void Cleanup()
		{
			foreach (MiniGame.ButtonStatus buttonStatus in this.buttons.Values)
			{
				buttonStatus.justPressed = false;
				buttonStatus.justReleased = false;
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0004D91C File Offset: 0x0004BB1C
		private void Update()
		{
			if (this.tickTiming == MiniGame.TickTiming.Update)
			{
				this.Tick(Time.deltaTime);
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0004D932 File Offset: 0x0004BB32
		private void FixedUpdate()
		{
			if (this.tickTiming == MiniGame.TickTiming.FixedUpdate)
			{
				this.Tick(Time.fixedDeltaTime);
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0004D948 File Offset: 0x0004BB48
		private void LateUpdate()
		{
			if (this.tickTiming == MiniGame.TickTiming.FixedUpdate)
			{
				this.Tick(Time.deltaTime);
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0004D960 File Offset: 0x0004BB60
		public void ClearInput()
		{
			foreach (MiniGame.ButtonStatus buttonStatus in this.buttons.Values)
			{
				if (buttonStatus.pressed)
				{
					buttonStatus.justReleased = true;
				}
				buttonStatus.pressed = false;
			}
			this.SetInputAxis(default(Vector2), 0);
			this.SetInputAxis(default(Vector2), 1);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0004D9E8 File Offset: 0x0004BBE8
		internal void SetConsole(GamingConsole console)
		{
			this.console = console;
		}

		// Token: 0x04000F33 RID: 3891
		[SerializeField]
		private string id;

		// Token: 0x04000F34 RID: 3892
		public MiniGame.TickTiming tickTiming;

		// Token: 0x04000F35 RID: 3893
		[SerializeField]
		private Camera camera;

		// Token: 0x04000F36 RID: 3894
		[SerializeField]
		private Camera uiCamera;

		// Token: 0x04000F37 RID: 3895
		[SerializeField]
		private RenderTexture renderTexture;

		// Token: 0x04000F38 RID: 3896
		public static Action<MiniGame, float> onUpdateLogic;

		// Token: 0x04000F39 RID: 3897
		private GamingConsole console;

		// Token: 0x04000F3A RID: 3898
		private Vector2 inputAxis_0;

		// Token: 0x04000F3B RID: 3899
		private Vector2 inputAxis_1;

		// Token: 0x04000F3C RID: 3900
		private Dictionary<MiniGame.Button, MiniGame.ButtonStatus> buttons = new Dictionary<MiniGame.Button, MiniGame.ButtonStatus>();

		// Token: 0x0200055F RID: 1375
		public enum TickTiming
		{
			// Token: 0x04001F2E RID: 7982
			Manual,
			// Token: 0x04001F2F RID: 7983
			Update,
			// Token: 0x04001F30 RID: 7984
			FixedUpdate,
			// Token: 0x04001F31 RID: 7985
			LateUpdate
		}

		// Token: 0x02000560 RID: 1376
		public enum Button
		{
			// Token: 0x04001F33 RID: 7987
			None,
			// Token: 0x04001F34 RID: 7988
			A,
			// Token: 0x04001F35 RID: 7989
			B,
			// Token: 0x04001F36 RID: 7990
			Start,
			// Token: 0x04001F37 RID: 7991
			Select,
			// Token: 0x04001F38 RID: 7992
			Left,
			// Token: 0x04001F39 RID: 7993
			Right,
			// Token: 0x04001F3A RID: 7994
			Up,
			// Token: 0x04001F3B RID: 7995
			Down
		}

		// Token: 0x02000561 RID: 1377
		public class ButtonStatus
		{
			// Token: 0x04001F3C RID: 7996
			public bool pressed;

			// Token: 0x04001F3D RID: 7997
			public bool justPressed;

			// Token: 0x04001F3E RID: 7998
			public bool justReleased;
		}

		// Token: 0x02000562 RID: 1378
		public struct MiniGameInputEventContext
		{
			// Token: 0x04001F3F RID: 7999
			public bool isButtonEvent;

			// Token: 0x04001F40 RID: 8000
			public MiniGame.Button button;

			// Token: 0x04001F41 RID: 8001
			public bool pressing;

			// Token: 0x04001F42 RID: 8002
			public bool buttonDown;

			// Token: 0x04001F43 RID: 8003
			public bool buttonUp;

			// Token: 0x04001F44 RID: 8004
			public bool isAxisEvent;

			// Token: 0x04001F45 RID: 8005
			public int axisIndex;

			// Token: 0x04001F46 RID: 8006
			public Vector2 axisValue;
		}
	}
}
