using System;
using Duckov.Buffs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x0200037D RID: 893
	public class BuffDetailsOverlay : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x0006D4DD File Offset: 0x0006B6DD
		public Buff Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0006D4E5 File Offset: 0x0006B6E5
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			BuffsDisplayEntry.OnBuffsDisplayEntryClicked += this.OnBuffsDisplayEntryClicked;
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0006D509 File Offset: 0x0006B709
		private void OnDestroy()
		{
			BuffsDisplayEntry.OnBuffsDisplayEntryClicked -= this.OnBuffsDisplayEntryClicked;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0006D51C File Offset: 0x0006B71C
		private void OnBuffsDisplayEntryClicked(BuffsDisplayEntry entry, PointerEventData eventData)
		{
			if (this.fadeGroup.IsShown && this.target == entry.Target)
			{
				this.fadeGroup.Hide();
				this.punchReceiver.Punch();
				return;
			}
			this.Setup(entry);
			this.Show();
			this.punchReceiver.Punch();
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0006D578 File Offset: 0x0006B778
		public void Setup(Buff target)
		{
			this.target = target;
			if (target == null)
			{
				return;
			}
			this.text_BuffName.text = target.DisplayName;
			this.text_BuffDescription.text = target.Description;
			this.RefreshCountDown();
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0006D5B4 File Offset: 0x0006B7B4
		private void Update()
		{
			if (this.fadeGroup.IsShown || this.fadeGroup.IsShowingInProgress)
			{
				if (this.target != null)
				{
					this.RefreshCountDown();
				}
				else
				{
					this.fadeGroup.Hide();
				}
				if (this.TimeSinceShowStarted > this.disappearAfterSeconds)
				{
					this.fadeGroup.Hide();
				}
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0006D618 File Offset: 0x0006B818
		public void Setup(BuffsDisplayEntry target)
		{
			if (target == null)
			{
				return;
			}
			this.Setup((target != null) ? target.Target : null);
			RectTransform rectTransform = target.Icon.rectTransform;
			Vector3 position = rectTransform.TransformPoint(rectTransform.rect.max);
			this.rectTransform.pivot = Vector2.up;
			this.rectTransform.position = position;
			this.rectTransform.SetAsLastSibling();
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0006D68C File Offset: 0x0006B88C
		private void RefreshCountDown()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.target.LimitedLifeTime)
			{
				float remainingTime = this.target.RemainingTime;
				this.text_CountDown.text = string.Format("{0:0.0}s", remainingTime);
				return;
			}
			this.text_CountDown.text = "";
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x0006D6ED File Offset: 0x0006B8ED
		private float TimeSinceShowStarted
		{
			get
			{
				return Time.unscaledTime - this.timeWhenShowStarted;
			}
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0006D6FB File Offset: 0x0006B8FB
		public void Show()
		{
			this.fadeGroup.Show();
			this.timeWhenShowStarted = Time.unscaledTime;
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0006D713 File Offset: 0x0006B913
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.fadeGroup.IsShown || this.fadeGroup.IsShowingInProgress)
			{
				this.punchReceiver.Punch();
				this.fadeGroup.Hide();
			}
		}

		// Token: 0x04001525 RID: 5413
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001526 RID: 5414
		[SerializeField]
		private TextMeshProUGUI text_BuffName;

		// Token: 0x04001527 RID: 5415
		[SerializeField]
		private TextMeshProUGUI text_BuffDescription;

		// Token: 0x04001528 RID: 5416
		[SerializeField]
		private TextMeshProUGUI text_CountDown;

		// Token: 0x04001529 RID: 5417
		[SerializeField]
		private PunchReceiver punchReceiver;

		// Token: 0x0400152A RID: 5418
		[SerializeField]
		private float disappearAfterSeconds = 5f;

		// Token: 0x0400152B RID: 5419
		private RectTransform rectTransform;

		// Token: 0x0400152C RID: 5420
		private Buff target;

		// Token: 0x0400152D RID: 5421
		private float timeWhenShowStarted;
	}
}
