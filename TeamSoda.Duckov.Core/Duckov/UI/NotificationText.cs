using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x02000380 RID: 896
	public class NotificationText : MonoBehaviour
	{
		// Token: 0x06001F25 RID: 7973 RVA: 0x0006DBBD File Offset: 0x0006BDBD
		public static void Push(string text)
		{
			if (NotificationText.pendingTexts.Count > 0 && NotificationText.pendingTexts.Peek() == text)
			{
				return;
			}
			NotificationText.pendingTexts.Enqueue(text);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0006DBEA File Offset: 0x0006BDEA
		private static string Pop()
		{
			return NotificationText.pendingTexts.Dequeue();
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x0006DBF6 File Offset: 0x0006BDF6
		private int PendingCount
		{
			get
			{
				return NotificationText.pendingTexts.Count;
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0006DC02 File Offset: 0x0006BE02
		private void Update()
		{
			if (!this.showing && this.PendingCount > 0)
			{
				this.ShowNext().Forget();
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0006DC20 File Offset: 0x0006BE20
		private UniTask ShowNext()
		{
			NotificationText.<ShowNext>d__11 <ShowNext>d__;
			<ShowNext>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowNext>d__.<>4__this = this;
			<ShowNext>d__.<>1__state = -1;
			<ShowNext>d__.<>t__builder.Start<NotificationText.<ShowNext>d__11>(ref <ShowNext>d__);
			return <ShowNext>d__.<>t__builder.Task;
		}

		// Token: 0x0400153E RID: 5438
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400153F RID: 5439
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001540 RID: 5440
		[SerializeField]
		private float duration = 1.2f;

		// Token: 0x04001541 RID: 5441
		[SerializeField]
		private float durationIfPending = 0.65f;

		// Token: 0x04001542 RID: 5442
		private static Queue<string> pendingTexts = new Queue<string>();

		// Token: 0x04001543 RID: 5443
		private bool showing;
	}
}
