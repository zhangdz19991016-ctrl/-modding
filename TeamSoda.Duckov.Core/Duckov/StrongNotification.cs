using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov
{
	// Token: 0x02000244 RID: 580
	public class StrongNotification : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x00045EF4 File Offset: 0x000440F4
		// (set) Token: 0x06001227 RID: 4647 RVA: 0x00045EFB File Offset: 0x000440FB
		public static StrongNotification Instance { get; private set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x00045F03 File Offset: 0x00044103
		private bool showing
		{
			get
			{
				return this.showingTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x00045F13 File Offset: 0x00044113
		public static bool Showing
		{
			get
			{
				return !(StrongNotification.Instance == null) && StrongNotification.Instance.showing;
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00045F30 File Offset: 0x00044130
		private void Awake()
		{
			if (StrongNotification.Instance == null)
			{
				StrongNotification.Instance = this;
			}
			UIInputManager.OnConfirm += this.OnConfirm;
			UIInputManager.OnCancel += this.OnCancel;
			View.OnActiveViewChanged += this.View_OnActiveViewChanged;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00045F83 File Offset: 0x00044183
		private void OnDestroy()
		{
			UIInputManager.OnConfirm -= this.OnConfirm;
			UIInputManager.OnCancel -= this.OnCancel;
			View.OnActiveViewChanged -= this.View_OnActiveViewChanged;
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00045FB8 File Offset: 0x000441B8
		private void View_OnActiveViewChanged()
		{
			this.confirmed = true;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00045FC1 File Offset: 0x000441C1
		private void OnCancel(UIInputEventData data)
		{
			this.confirmed = true;
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00045FCA File Offset: 0x000441CA
		private void OnConfirm(UIInputEventData data)
		{
			this.confirmed = true;
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00045FD3 File Offset: 0x000441D3
		private void Update()
		{
			if (!this.showing && StrongNotification.pending.Count > 0)
			{
				this.BeginShow();
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00045FF0 File Offset: 0x000441F0
		private void BeginShow()
		{
			this.showingTask = this.ShowTask();
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00046000 File Offset: 0x00044200
		private UniTask ShowTask()
		{
			StrongNotification.<ShowTask>d__23 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<StrongNotification.<ShowTask>d__23>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00046044 File Offset: 0x00044244
		private UniTask DisplayContent(StrongNotificationContent cur)
		{
			StrongNotification.<DisplayContent>d__24 <DisplayContent>d__;
			<DisplayContent>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DisplayContent>d__.<>4__this = this;
			<DisplayContent>d__.cur = cur;
			<DisplayContent>d__.<>1__state = -1;
			<DisplayContent>d__.<>t__builder.Start<StrongNotification.<DisplayContent>d__24>(ref <DisplayContent>d__);
			return <DisplayContent>d__.<>t__builder.Task;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0004608F File Offset: 0x0004428F
		public void OnPointerClick(PointerEventData eventData)
		{
			this.confirmed = true;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00046098 File Offset: 0x00044298
		public static void Push(StrongNotificationContent content)
		{
			StrongNotification.pending.Add(content);
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000460A5 File Offset: 0x000442A5
		public static void Push(string mainText, string subText = "")
		{
			StrongNotification.pending.Add(new StrongNotificationContent(mainText, subText, null));
		}

		// Token: 0x04000DEF RID: 3567
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04000DF0 RID: 3568
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x04000DF1 RID: 3569
		[SerializeField]
		private TextMeshProUGUI textMain;

		// Token: 0x04000DF2 RID: 3570
		[SerializeField]
		private TextMeshProUGUI textSub;

		// Token: 0x04000DF3 RID: 3571
		[SerializeField]
		private Image image;

		// Token: 0x04000DF4 RID: 3572
		[SerializeField]
		private float contentDelay = 0.5f;

		// Token: 0x04000DF5 RID: 3573
		private static List<StrongNotificationContent> pending = new List<StrongNotificationContent>();

		// Token: 0x04000DF7 RID: 3575
		private UniTask showingTask;

		// Token: 0x04000DF8 RID: 3576
		private bool confirmed;
	}
}
