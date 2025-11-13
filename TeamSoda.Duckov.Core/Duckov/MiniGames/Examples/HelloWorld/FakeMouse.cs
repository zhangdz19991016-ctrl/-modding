using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.HelloWorld
{
	// Token: 0x020002D3 RID: 723
	public class FakeMouse : MiniGameBehaviour
	{
		// Token: 0x060016FD RID: 5885 RVA: 0x00054955 File Offset: 0x00052B55
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
			this.parentRectTransform = (base.transform.parent as RectTransform);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00054980 File Offset: 0x00052B80
		protected override void OnUpdate(float deltaTime)
		{
			Vector3 vector = this.rectTransform.localPosition;
			vector += base.Game.GetAxis(1) * this.sensitivity;
			Rect rect = this.parentRectTransform.rect;
			vector.x = Mathf.Clamp(vector.x, rect.xMin, rect.xMax);
			vector.y = Mathf.Clamp(vector.y, rect.yMin, rect.yMax);
			this.rectTransform.localPosition = vector;
		}

		// Token: 0x040010C0 RID: 4288
		[SerializeField]
		private float sensitivity = 1f;

		// Token: 0x040010C1 RID: 4289
		private RectTransform rectTransform;

		// Token: 0x040010C2 RID: 4290
		private RectTransform parentRectTransform;
	}
}
