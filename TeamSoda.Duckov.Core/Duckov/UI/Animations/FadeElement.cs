using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DD RID: 989
	public abstract class FadeElement : MonoBehaviour
	{
		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x0007E61E File Offset: 0x0007C81E
		public UniTask ActiveTask
		{
			get
			{
				return this.activeTask;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x0007E626 File Offset: 0x0007C826
		protected int ActiveTaskToken
		{
			get
			{
				return this.activeTaskToken;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x0007E62E File Offset: 0x0007C82E
		protected bool ManageGameObjectActive
		{
			get
			{
				return this.manageGameObjectActive;
			}
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0007E636 File Offset: 0x0007C836
		private void CacheNewTaskToken()
		{
			this.activeTaskToken = UnityEngine.Random.Range(1, int.MaxValue);
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x0007E649 File Offset: 0x0007C849
		// (set) Token: 0x0600241A RID: 9242 RVA: 0x0007E651 File Offset: 0x0007C851
		public bool IsFading { get; private set; }

		// Token: 0x0600241B RID: 9243 RVA: 0x0007E65C File Offset: 0x0007C85C
		public UniTask Show(float delay = 0f)
		{
			FadeElement.<Show>d__18 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.<>4__this = this;
			<Show>d__.delay = delay;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<FadeElement.<Show>d__18>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0007E6A8 File Offset: 0x0007C8A8
		public UniTask Hide()
		{
			FadeElement.<Hide>d__19 <Hide>d__;
			<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Hide>d__.<>4__this = this;
			<Hide>d__.<>1__state = -1;
			<Hide>d__.<>t__builder.Start<FadeElement.<Hide>d__19>(ref <Hide>d__);
			return <Hide>d__.<>t__builder.Task;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x0007E6EC File Offset: 0x0007C8EC
		private UniTask WrapShowTask(int token, float delay = 0f)
		{
			FadeElement.<WrapShowTask>d__20 <WrapShowTask>d__;
			<WrapShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WrapShowTask>d__.<>4__this = this;
			<WrapShowTask>d__.token = token;
			<WrapShowTask>d__.delay = delay;
			<WrapShowTask>d__.<>1__state = -1;
			<WrapShowTask>d__.<>t__builder.Start<FadeElement.<WrapShowTask>d__20>(ref <WrapShowTask>d__);
			return <WrapShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x0007E740 File Offset: 0x0007C940
		private UniTask WrapHideTask(int token, float delay = 0f)
		{
			FadeElement.<WrapHideTask>d__21 <WrapHideTask>d__;
			<WrapHideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WrapHideTask>d__.<>4__this = this;
			<WrapHideTask>d__.token = token;
			<WrapHideTask>d__.delay = delay;
			<WrapHideTask>d__.<>1__state = -1;
			<WrapHideTask>d__.<>t__builder.Start<FadeElement.<WrapHideTask>d__21>(ref <WrapHideTask>d__);
			return <WrapHideTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600241F RID: 9247
		protected abstract UniTask ShowTask(int token);

		// Token: 0x06002420 RID: 9248
		protected abstract UniTask HideTask(int token);

		// Token: 0x06002421 RID: 9249
		protected abstract void OnSkipHide();

		// Token: 0x06002422 RID: 9250
		protected abstract void OnSkipShow();

		// Token: 0x06002423 RID: 9251 RVA: 0x0007E793 File Offset: 0x0007C993
		public void SkipHide()
		{
			this.activeTaskToken = 0;
			this.OnSkipHide();
			if (this.ManageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0007E7B6 File Offset: 0x0007C9B6
		internal void SkipShow()
		{
			this.activeTaskToken = 0;
			this.OnSkipShow();
			if (this.ManageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x04001875 RID: 6261
		protected UniTask activeTask;

		// Token: 0x04001876 RID: 6262
		private int activeTaskToken;

		// Token: 0x04001877 RID: 6263
		[SerializeField]
		private bool manageGameObjectActive;

		// Token: 0x04001878 RID: 6264
		[SerializeField]
		private float delay;

		// Token: 0x04001879 RID: 6265
		[SerializeField]
		private string sfx_Show;

		// Token: 0x0400187A RID: 6266
		[SerializeField]
		private string sfx_Hide;

		// Token: 0x0400187C RID: 6268
		private bool isShown;
	}
}
