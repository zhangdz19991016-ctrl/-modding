using System;
using UnityEngine;

namespace Duckov.UI.Animations
{
	// Token: 0x020003EC RID: 1004
	public class ToggleComponent : MonoBehaviour
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x0007F577 File Offset: 0x0007D777
		private bool Status
		{
			get
			{
				return this.master && this.master.Status;
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0007F593 File Offset: 0x0007D793
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<ToggleAnimation>();
			}
			this.master.onSetToggle += this.OnSetToggle;
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0007F5C7 File Offset: 0x0007D7C7
		private void OnDestroy()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.onSetToggle -= this.OnSetToggle;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0007F5F0 File Offset: 0x0007D7F0
		protected virtual void OnSetToggle(ToggleAnimation master, bool value)
		{
		}

		// Token: 0x040018B7 RID: 6327
		[SerializeField]
		private ToggleAnimation master;
	}
}
