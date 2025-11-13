using System;
using Duckov.PerkTrees;
using TMPro;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C3 RID: 963
	public class PerkLineEntry : MonoBehaviour
	{
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x0007B2A7 File Offset: 0x000794A7
		public RectTransform RectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x0007B2C9 File Offset: 0x000794C9
		internal void Setup(PerkTreeView perkTreeView, PerkLevelLineNode cur)
		{
			this.target = cur;
			this.label.text = this.target.DisplayName;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0007B2E8 File Offset: 0x000794E8
		internal Vector2 GetLayoutPosition()
		{
			if (this.target == null)
			{
				return Vector2.zero;
			}
			return this.target.cachedPosition;
		}

		// Token: 0x040017E1 RID: 6113
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x040017E2 RID: 6114
		private RectTransform _rectTransform;

		// Token: 0x040017E3 RID: 6115
		private PerkLevelLineNode target;
	}
}
