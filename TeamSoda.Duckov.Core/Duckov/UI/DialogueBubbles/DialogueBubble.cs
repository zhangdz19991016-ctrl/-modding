using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI.DialogueBubbles
{
	// Token: 0x020003ED RID: 1005
	public class DialogueBubble : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x0007F5FA File Offset: 0x0007D7FA
		public Transform Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x0007F602 File Offset: 0x0007D802
		private float YOffset
		{
			get
			{
				if (this._yOffset < 0f)
				{
					return this.defaultYOffset;
				}
				return this._yOffset;
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0007F61E File Offset: 0x0007D81E
		private void LateUpdate()
		{
			this.UpdatePosition();
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0007F628 File Offset: 0x0007D828
		private void UpdatePosition()
		{
			if (this.target == null)
			{
				return;
			}
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			Vector3 vector = this.target.position + Vector3.up * this.YOffset;
			if (Vector3.Dot(vector - main.transform.position, main.transform.forward) < 0f)
			{
				base.transform.localPosition = Vector3.one * 1000000f;
				return;
			}
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, vector);
			screenPoint.y += this.screenYOffset * (float)Screen.height;
			Vector2 v;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform.parent as RectTransform, screenPoint, null, out v))
			{
				return;
			}
			base.transform.localPosition = v;
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0007F70C File Offset: 0x0007D90C
		public UniTask Show(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			this.task = this.ShowTask(text, target, yOffset, needInteraction, skippable, speed, duration);
			return this.task;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0007F738 File Offset: 0x0007D938
		public UniTask ShowTask(string text, Transform target, float yOffset = -1f, bool needInteraction = false, bool skippable = false, float speed = -1f, float duration = 2f)
		{
			DialogueBubble.<ShowTask>d__20 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.text = text;
			<ShowTask>d__.target = target;
			<ShowTask>d__.yOffset = yOffset;
			<ShowTask>d__.needInteraction = needInteraction;
			<ShowTask>d__.skippable = skippable;
			<ShowTask>d__.speed = speed;
			<ShowTask>d__.duration = duration;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<DialogueBubble.<ShowTask>d__20>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0007F7B8 File Offset: 0x0007D9B8
		private UniTask WaitForInteraction(int currentToken)
		{
			DialogueBubble.<WaitForInteraction>d__21 <WaitForInteraction>d__;
			<WaitForInteraction>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForInteraction>d__.<>4__this = this;
			<WaitForInteraction>d__.currentToken = currentToken;
			<WaitForInteraction>d__.<>1__state = -1;
			<WaitForInteraction>d__.<>t__builder.Start<DialogueBubble.<WaitForInteraction>d__21>(ref <WaitForInteraction>d__);
			return <WaitForInteraction>d__.<>t__builder.Task;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0007F803 File Offset: 0x0007DA03
		public void Interact()
		{
			this.interacted = true;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x0007F80C File Offset: 0x0007DA0C
		private UniTask Hide()
		{
			DialogueBubble.<Hide>d__23 <Hide>d__;
			<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Hide>d__.<>4__this = this;
			<Hide>d__.<>1__state = -1;
			<Hide>d__.<>t__builder.Start<DialogueBubble.<Hide>d__23>(ref <Hide>d__);
			return <Hide>d__.<>t__builder.Task;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0007F84F File Offset: 0x0007DA4F
		public void OnPointerClick(PointerEventData eventData)
		{
			this.Interact();
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0007F857 File Offset: 0x0007DA57
		private void Awake()
		{
			DialogueBubblesManager.onPointerClick += this.OnPointerClick;
		}

		// Token: 0x040018B8 RID: 6328
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040018B9 RID: 6329
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x040018BA RID: 6330
		[SerializeField]
		private float defaultSpeed = 10f;

		// Token: 0x040018BB RID: 6331
		[SerializeField]
		private float sustainDuration = 2f;

		// Token: 0x040018BC RID: 6332
		[SerializeField]
		private float defaultYOffset = 2f;

		// Token: 0x040018BD RID: 6333
		[SerializeField]
		private GameObject interactIndicator;

		// Token: 0x040018BE RID: 6334
		private bool interacted;

		// Token: 0x040018BF RID: 6335
		private bool animating;

		// Token: 0x040018C0 RID: 6336
		private int taskToken;

		// Token: 0x040018C1 RID: 6337
		private Transform target;

		// Token: 0x040018C2 RID: 6338
		private float _yOffset;

		// Token: 0x040018C3 RID: 6339
		private float screenYOffset = 0.06f;

		// Token: 0x040018C4 RID: 6340
		private UniTask task;
	}
}
