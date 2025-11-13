using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.UI
{
	// Token: 0x020003CA RID: 970
	public class FollowCursor : MonoBehaviour
	{
		// Token: 0x06002377 RID: 9079 RVA: 0x0007C81B File Offset: 0x0007AA1B
		private void Awake()
		{
			this.parentRectTransform = (base.transform.parent as RectTransform);
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0007C844 File Offset: 0x0007AA44
		private unsafe void Update()
		{
			Vector2 screenPoint = *Mouse.current.position.value;
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, screenPoint, null, out v);
			this.rectTransform.localPosition = v;
		}

		// Token: 0x04001814 RID: 6164
		private RectTransform parentRectTransform;

		// Token: 0x04001815 RID: 6165
		private RectTransform rectTransform;
	}
}
