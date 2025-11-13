using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.MiniMaps
{
	// Token: 0x0200027A RID: 634
	public interface IPointOfInterest
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0004AEEF File Offset: 0x000490EF
		int OverrideScene
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0004AEF2 File Offset: 0x000490F2
		Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0004AEF5 File Offset: 0x000490F5
		Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0004AEFC File Offset: 0x000490FC
		string DisplayName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0004AEFF File Offset: 0x000490FF
		Color ShadowColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0004AF06 File Offset: 0x00049106
		float ShadowDistance
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0004AF0D File Offset: 0x0004910D
		bool IsArea
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0004AF10 File Offset: 0x00049110
		float AreaRadius
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0004AF17 File Offset: 0x00049117
		bool HideIcon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0004AF1A File Offset: 0x0004911A
		float ScaleFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0004AF21 File Offset: 0x00049121
		void NotifyClicked(PointerEventData eventData)
		{
		}
	}
}
