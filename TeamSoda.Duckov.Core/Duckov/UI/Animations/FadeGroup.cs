using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003DE RID: 990
	public class FadeGroup : MonoBehaviour
	{
		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x06002426 RID: 9254 RVA: 0x0007E7E4 File Offset: 0x0007C9E4
		// (remove) Token: 0x06002427 RID: 9255 RVA: 0x0007E81C File Offset: 0x0007CA1C
		public event Action<FadeGroup> OnFadeComplete;

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x06002428 RID: 9256 RVA: 0x0007E854 File Offset: 0x0007CA54
		// (remove) Token: 0x06002429 RID: 9257 RVA: 0x0007E88C File Offset: 0x0007CA8C
		public event Action<FadeGroup> OnShowComplete;

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x0600242A RID: 9258 RVA: 0x0007E8C4 File Offset: 0x0007CAC4
		// (remove) Token: 0x0600242B RID: 9259 RVA: 0x0007E8FC File Offset: 0x0007CAFC
		public event Action<FadeGroup> OnHideComplete;

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x0007E931 File Offset: 0x0007CB31
		public bool IsHidingInProgress
		{
			get
			{
				return this.isHidingInProgress;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x0007E939 File Offset: 0x0007CB39
		public bool IsShowingInProgress
		{
			get
			{
				return this.isShowingInProgress;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x0007E941 File Offset: 0x0007CB41
		public bool IsShown
		{
			get
			{
				return this.isShown;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x0007E949 File Offset: 0x0007CB49
		public bool IsHidden
		{
			get
			{
				return !this.isShown;
			}
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0007E954 File Offset: 0x0007CB54
		private void Start()
		{
			if (this.skipHideOnStart)
			{
				this.SkipHide();
			}
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x0007E964 File Offset: 0x0007CB64
		private void OnEnable()
		{
			if (this.showOnEnable)
			{
				this.Show();
			}
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x0007E974 File Offset: 0x0007CB74
		[ContextMenu("Show")]
		public void Show()
		{
			if (this.debug)
			{
				Debug.Log("Fadegroup SHOW " + base.name);
			}
			this.skipHideOnStart = false;
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			this.ShowTask().Forget();
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0007E9C4 File Offset: 0x0007CBC4
		[ContextMenu("Hide")]
		public void Hide()
		{
			if (this.debug)
			{
				Debug.Log("Fadegroup HIDE " + base.name, base.gameObject);
			}
			this.HideTask().Forget();
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0007E9F4 File Offset: 0x0007CBF4
		public void Toggle()
		{
			if (this.IsShown)
			{
				this.Hide();
				return;
			}
			if (this.IsHidden)
			{
				this.Show();
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0007EA13 File Offset: 0x0007CC13
		public UniTask ShowAndReturnTask()
		{
			if (this.skipHideBeforeShow)
			{
				this.SkipHide();
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
			return this.ShowTask();
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0007EA3D File Offset: 0x0007CC3D
		public UniTask HideAndReturnTask()
		{
			return this.HideTask();
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0007EA45 File Offset: 0x0007CC45
		private int CacheNewTaskToken()
		{
			this.activeTaskToken = UnityEngine.Random.Range(0, int.MaxValue);
			return this.activeTaskToken;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0007EA60 File Offset: 0x0007CC60
		public UniTask ShowTask()
		{
			FadeGroup.<ShowTask>d__35 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<FadeGroup.<ShowTask>d__35>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0007EAA4 File Offset: 0x0007CCA4
		public UniTask HideTask()
		{
			FadeGroup.<HideTask>d__36 <HideTask>d__;
			<HideTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<HideTask>d__.<>4__this = this;
			<HideTask>d__.<>1__state = -1;
			<HideTask>d__.<>t__builder.Start<FadeGroup.<HideTask>d__36>(ref <HideTask>d__);
			return <HideTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0007EAE7 File Offset: 0x0007CCE7
		private void ShowComplete()
		{
			this.isShowingInProgress = false;
			Action<FadeGroup> onFadeComplete = this.OnFadeComplete;
			if (onFadeComplete != null)
			{
				onFadeComplete(this);
			}
			Action<FadeGroup> onShowComplete = this.OnShowComplete;
			if (onShowComplete == null)
			{
				return;
			}
			onShowComplete(this);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0007EB14 File Offset: 0x0007CD14
		private void HideComplete()
		{
			this.isHidingInProgress = false;
			Action<FadeGroup> onFadeComplete = this.OnFadeComplete;
			if (onFadeComplete != null)
			{
				onFadeComplete(this);
			}
			Action<FadeGroup> onHideComplete = this.OnHideComplete;
			if (onHideComplete != null)
			{
				onHideComplete(this);
			}
			if (this == null)
			{
				return;
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0007EB6C File Offset: 0x0007CD6C
		public void SkipHide()
		{
			foreach (FadeElement fadeElement in this.fadeElements)
			{
				if (fadeElement == null)
				{
					Debug.LogWarning("Element in fade group " + base.name + " is null");
				}
				else
				{
					fadeElement.SkipHide();
				}
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x0007EBF8 File Offset: 0x0007CDF8
		public bool IsFading
		{
			get
			{
				return this.fadeElements.Any((FadeElement e) => e != null && e.IsFading);
			}
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0007EC24 File Offset: 0x0007CE24
		internal void SkipShow()
		{
			foreach (FadeElement fadeElement in this.fadeElements)
			{
				if (fadeElement == null)
				{
					Debug.LogWarning("Element in fade group " + base.name + " is null");
				}
				else
				{
					fadeElement.SkipShow();
				}
			}
			if (this.manageGameObjectActive)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x0400187D RID: 6269
		[SerializeField]
		private List<FadeElement> fadeElements = new List<FadeElement>();

		// Token: 0x0400187E RID: 6270
		[SerializeField]
		private bool skipHideOnStart = true;

		// Token: 0x0400187F RID: 6271
		[SerializeField]
		private bool showOnEnable;

		// Token: 0x04001880 RID: 6272
		[SerializeField]
		private bool skipHideBeforeShow = true;

		// Token: 0x04001884 RID: 6276
		public bool manageGameObjectActive;

		// Token: 0x04001885 RID: 6277
		private bool isHidingInProgress;

		// Token: 0x04001886 RID: 6278
		private bool isShowingInProgress;

		// Token: 0x04001887 RID: 6279
		private bool isShown;

		// Token: 0x04001888 RID: 6280
		public bool debug;

		// Token: 0x04001889 RID: 6281
		private int activeTaskToken;
	}
}
