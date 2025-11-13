using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003CD RID: 973
	[RequireComponent(typeof(ScrollRect))]
	[ExecuteInEditMode]
	public class ScrollViewMaxHeight : UIBehaviour, ILayoutElement
	{
		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x0007CB94 File Offset: 0x0007AD94
		public float preferredHeight
		{
			get
			{
				float y = this.scrollRect.content.sizeDelta.y;
				float num = this.maxHeight;
				if (this.useTargetParentSize)
				{
					float num2 = 0f;
					foreach (RectTransform rectTransform in this.siblings)
					{
						num2 += rectTransform.rect.height;
					}
					num = this.targetParentHeight - num2 - this.parentLayoutMargin;
				}
				if (y > num)
				{
					return num;
				}
				return y;
			}
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x0007CC38 File Offset: 0x0007AE38
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x0007CC3A File Offset: 0x0007AE3A
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600238B RID: 9099 RVA: 0x0007CC3C File Offset: 0x0007AE3C
		public virtual float minWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x0007CC43 File Offset: 0x0007AE43
		public virtual float minHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600238D RID: 9101 RVA: 0x0007CC4A File Offset: 0x0007AE4A
		public virtual float preferredWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x0007CC51 File Offset: 0x0007AE51
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600238F RID: 9103 RVA: 0x0007CC58 File Offset: 0x0007AE58
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0007CC5F File Offset: 0x0007AE5F
		public virtual int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0007CC67 File Offset: 0x0007AE67
		private void OnContentRectChange(RectTransform rectTransform)
		{
			this.SetDirty();
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x0007CC6F File Offset: 0x0007AE6F
		private RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x0007CC98 File Offset: 0x0007AE98
		protected override void OnEnable()
		{
			if (this.scrollRect == null)
			{
				this.scrollRect = base.GetComponent<ScrollRect>();
			}
			if (this.contentRectChangeEventEmitter == null)
			{
				this.contentRectChangeEventEmitter = this.scrollRect.content.GetComponent<RectTransformChangeEventEmitter>();
			}
			if (this.contentRectChangeEventEmitter == null)
			{
				this.contentRectChangeEventEmitter = this.scrollRect.content.gameObject.AddComponent<RectTransformChangeEventEmitter>();
			}
			base.OnEnable();
			this.contentRectChangeEventEmitter.OnRectTransformChange += this.OnContentRectChange;
			this.SetDirty();
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x0007CD2F File Offset: 0x0007AF2F
		protected override void OnDisable()
		{
			this.contentRectChangeEventEmitter.OnRectTransformChange -= this.OnContentRectChange;
			this.SetDirty();
			base.OnDisable();
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x0007CD54 File Offset: 0x0007AF54
		private void Update()
		{
			if (this.preferredHeight != this.rectTransform.rect.height)
			{
				this.SetDirty();
			}
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x0007CD82 File Offset: 0x0007AF82
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.transform as RectTransform);
		}

		// Token: 0x0400181F RID: 6175
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x04001820 RID: 6176
		[SerializeField]
		private RectTransformChangeEventEmitter contentRectChangeEventEmitter;

		// Token: 0x04001821 RID: 6177
		[SerializeField]
		private int m_layoutPriority = 1;

		// Token: 0x04001822 RID: 6178
		[SerializeField]
		private bool useTargetParentSize;

		// Token: 0x04001823 RID: 6179
		[SerializeField]
		private float targetParentHeight = 935f;

		// Token: 0x04001824 RID: 6180
		[SerializeField]
		private List<RectTransform> siblings = new List<RectTransform>();

		// Token: 0x04001825 RID: 6181
		[SerializeField]
		private float parentLayoutMargin = 16f;

		// Token: 0x04001826 RID: 6182
		[SerializeField]
		private float maxHeight = 100f;

		// Token: 0x04001827 RID: 6183
		private RectTransform _rectTransform;
	}
}
