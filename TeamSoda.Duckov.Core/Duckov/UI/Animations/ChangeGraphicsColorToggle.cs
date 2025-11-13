using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI.Animations
{
	// Token: 0x020003E8 RID: 1000
	public class ChangeGraphicsColorToggle : ToggleComponent
	{
		// Token: 0x06002461 RID: 9313 RVA: 0x0007F283 File Offset: 0x0007D483
		protected override void OnSetToggle(ToggleAnimation master, bool value)
		{
			this.image.DOKill(false);
			this.image.DOColor(value ? this.trueColor : this.falseColor, this.duration);
		}

		// Token: 0x040018A5 RID: 6309
		[SerializeField]
		private Image image;

		// Token: 0x040018A6 RID: 6310
		[SerializeField]
		private Color trueColor;

		// Token: 0x040018A7 RID: 6311
		[SerializeField]
		private Color falseColor;

		// Token: 0x040018A8 RID: 6312
		[SerializeField]
		private float duration = 0.1f;
	}
}
