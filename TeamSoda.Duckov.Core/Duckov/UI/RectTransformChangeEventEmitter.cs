using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003CB RID: 971
	[ExecuteInEditMode]
	public class RectTransformChangeEventEmitter : UIBehaviour
	{
		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x0600237A RID: 9082 RVA: 0x0007C890 File Offset: 0x0007AA90
		// (remove) Token: 0x0600237B RID: 9083 RVA: 0x0007C8C8 File Offset: 0x0007AAC8
		public event Action<RectTransform> OnRectTransformChange;

		// Token: 0x0600237C RID: 9084 RVA: 0x0007C8FD File Offset: 0x0007AAFD
		private void SetDirty()
		{
			Action<RectTransform> onRectTransformChange = this.OnRectTransformChange;
			if (onRectTransformChange == null)
			{
				return;
			}
			onRectTransformChange(base.transform as RectTransform);
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0007C91A File Offset: 0x0007AB1A
		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0007C922 File Offset: 0x0007AB22
		protected override void OnEnable()
		{
			this.SetDirty();
		}
	}
}
