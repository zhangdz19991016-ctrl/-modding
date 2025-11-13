using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003EB RID: 1003
	public class ToggleAnimation : MonoBehaviour
	{
		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x0600246C RID: 9324 RVA: 0x0007F4C0 File Offset: 0x0007D6C0
		// (remove) Token: 0x0600246D RID: 9325 RVA: 0x0007F4F8 File Offset: 0x0007D6F8
		public event Action<ToggleAnimation, bool> onSetToggle;

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0007F52D File Offset: 0x0007D72D
		// (set) Token: 0x0600246F RID: 9327 RVA: 0x0007F535 File Offset: 0x0007D735
		public bool Status
		{
			get
			{
				return this.status;
			}
			protected set
			{
				this.SetToggle(value);
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0007F53E File Offset: 0x0007D73E
		public void SetToggle(bool value)
		{
			this.status = value;
			if (!Application.isPlaying)
			{
				return;
			}
			this.OnSetToggle(this.Status);
			Action<ToggleAnimation, bool> action = this.onSetToggle;
			if (action == null)
			{
				return;
			}
			action(this, value);
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0007F56D File Offset: 0x0007D76D
		protected virtual void OnSetToggle(bool value)
		{
		}

		// Token: 0x040018B6 RID: 6326
		[SerializeField]
		[HideInInspector]
		private bool status;
	}
}
