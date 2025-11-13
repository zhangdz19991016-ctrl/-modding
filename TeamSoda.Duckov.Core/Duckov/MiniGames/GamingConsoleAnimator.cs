using System;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000285 RID: 645
	public class GamingConsoleAnimator : MonoBehaviour
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0004D380 File Offset: 0x0004B580
		[SerializeField]
		private MiniGame Game
		{
			get
			{
				if (this.console == null)
				{
					return null;
				}
				return this.console.Game;
			}
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0004D39D File Offset: 0x0004B59D
		private void Update()
		{
			this.Tick();
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004D3A8 File Offset: 0x0004B5A8
		private void Tick()
		{
			if (this.Game == null)
			{
				this.Clear();
				return;
			}
			if (CameraMode.Active)
			{
				return;
			}
			this.joyStick_Target = this.Game.GetAxis(0);
			this.joyStick_Current = Vector2.Lerp(this.joyStick_Current, this.joyStick_Target, 0.25f);
			Vector2 vector = this.joyStick_Current;
			this.animator.SetFloat("AxisX", vector.x);
			this.animator.SetFloat("AxisY", vector.y);
			this.animator.SetBool("ButtonA", this.Game.GetButton(MiniGame.Button.A));
			this.animator.SetBool("ButtonB", this.Game.GetButton(MiniGame.Button.B));
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004D46C File Offset: 0x0004B66C
		private void Clear()
		{
			this.animator.SetBool("ButtonA", false);
			this.animator.SetBool("ButtonB", false);
			this.animator.SetFloat("AxisX", 0f);
			this.animator.SetFloat("AxisY", 0f);
		}

		// Token: 0x04000F2D RID: 3885
		[SerializeField]
		private Animator animator;

		// Token: 0x04000F2E RID: 3886
		[SerializeField]
		private GamingConsole console;

		// Token: 0x04000F2F RID: 3887
		private Vector2 joyStick_Current;

		// Token: 0x04000F30 RID: 3888
		private Vector2 joyStick_Target;
	}
}
