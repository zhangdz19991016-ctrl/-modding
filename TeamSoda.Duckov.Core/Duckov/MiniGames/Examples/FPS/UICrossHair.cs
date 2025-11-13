using System;
using UnityEngine;

namespace Duckov.MiniGames.Examples.FPS
{
	// Token: 0x020002D5 RID: 725
	public class UICrossHair : MiniGameBehaviour
	{
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x00054B18 File Offset: 0x00052D18
		private float ScatterAngle
		{
			get
			{
				if (this.gunControl)
				{
					return this.gunControl.ScatterAngle;
				}
				return 0f;
			}
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00054B38 File Offset: 0x00052D38
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00054B54 File Offset: 0x00052D54
		protected override void OnUpdate(float deltaTime)
		{
			float scatterAngle = this.ScatterAngle;
			float fieldOfView = base.Game.Camera.fieldOfView;
			float y = this.canvasRectTransform.sizeDelta.y;
			float num = scatterAngle / fieldOfView;
			float d = (float)(Mathf.FloorToInt(y * num / 2f) * 2 + 1);
			this.rectTransform.sizeDelta = d * Vector2.one;
		}

		// Token: 0x040010C7 RID: 4295
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040010C8 RID: 4296
		[SerializeField]
		private RectTransform canvasRectTransform;

		// Token: 0x040010C9 RID: 4297
		[SerializeField]
		private FPSGunControl gunControl;
	}
}
