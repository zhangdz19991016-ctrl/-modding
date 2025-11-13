using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Buffs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200037F RID: 895
	public class BuffsDisplayEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06001F18 RID: 7960 RVA: 0x0006D980 File Offset: 0x0006BB80
		// (remove) Token: 0x06001F19 RID: 7961 RVA: 0x0006D9B4 File Offset: 0x0006BBB4
		public static event Action<BuffsDisplayEntry, PointerEventData> OnBuffsDisplayEntryClicked;

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001F1A RID: 7962 RVA: 0x0006D9E7 File Offset: 0x0006BBE7
		public Image Icon
		{
			get
			{
				return this.icon;
			}
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0006D9F0 File Offset: 0x0006BBF0
		public void Setup(BuffsDisplay master, Buff target)
		{
			this.master = master;
			this.target = target;
			this.icon.sprite = target.Icon;
			if (this.displayName)
			{
				this.displayName.text = target.DisplayName;
			}
			this.fadeGroup.Show();
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0006DA45 File Offset: 0x0006BC45
		private void Update()
		{
			this.Refresh();
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0006DA50 File Offset: 0x0006BC50
		private void Refresh()
		{
			if (this.target == null)
			{
				this.Release();
				return;
			}
			if (this.target.LimitedLifeTime)
			{
				this.remainingTimeText.text = string.Format(this.timeFormat, this.target.RemainingTime);
			}
			else
			{
				this.remainingTimeText.text = "";
			}
			if (this.target.MaxLayers > 1)
			{
				this.layersText.text = this.target.CurrentLayers.ToString();
				return;
			}
			this.layersText.text = "";
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0006DAF4 File Offset: 0x0006BCF4
		public void Release()
		{
			if (this.releasing)
			{
				return;
			}
			this.releasing = true;
			this.ReleaseTask().Forget();
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0006DB14 File Offset: 0x0006BD14
		private UniTask ReleaseTask()
		{
			BuffsDisplayEntry.<ReleaseTask>d__19 <ReleaseTask>d__;
			<ReleaseTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ReleaseTask>d__.<>4__this = this;
			<ReleaseTask>d__.<>1__state = -1;
			<ReleaseTask>d__.<>t__builder.Start<BuffsDisplayEntry.<ReleaseTask>d__19>(ref <ReleaseTask>d__);
			return <ReleaseTask>d__.<>t__builder.Task;
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x0006DB57 File Offset: 0x0006BD57
		public Buff Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0006DB5F File Offset: 0x0006BD5F
		public void NotifyPooled()
		{
			this.pooled = true;
			this.releasing = false;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0006DB6F File Offset: 0x0006BD6F
		public void NotifyReleased()
		{
			this.pooled = false;
			this.target = null;
			this.releasing = false;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0006DB86 File Offset: 0x0006BD86
		public void OnPointerClick(PointerEventData eventData)
		{
			PunchReceiver punchReceiver = this.punchReceiver;
			if (punchReceiver != null)
			{
				punchReceiver.Punch();
			}
			Action<BuffsDisplayEntry, PointerEventData> onBuffsDisplayEntryClicked = BuffsDisplayEntry.OnBuffsDisplayEntryClicked;
			if (onBuffsDisplayEntryClicked == null)
			{
				return;
			}
			onBuffsDisplayEntryClicked(this, eventData);
		}

		// Token: 0x04001533 RID: 5427
		[SerializeField]
		private Image icon;

		// Token: 0x04001534 RID: 5428
		[SerializeField]
		private TextMeshProUGUI remainingTimeText;

		// Token: 0x04001535 RID: 5429
		[SerializeField]
		private TextMeshProUGUI layersText;

		// Token: 0x04001536 RID: 5430
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001537 RID: 5431
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001538 RID: 5432
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x04001539 RID: 5433
		[SerializeField]
		private string timeFormat = "{0:0}s";

		// Token: 0x0400153A RID: 5434
		private BuffsDisplay master;

		// Token: 0x0400153B RID: 5435
		private Buff target;

		// Token: 0x0400153C RID: 5436
		private bool releasing;

		// Token: 0x0400153D RID: 5437
		private bool pooled;
	}
}
