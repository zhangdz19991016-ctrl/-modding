using System;
using Duckov.MasterKeys.UI;

namespace Duckov.MasterKeys
{
	// Token: 0x020002E2 RID: 738
	public class MasterKeyRegisterViewInvoker : InteractableBase
	{
		// Token: 0x060017B5 RID: 6069 RVA: 0x00057842 File Offset: 0x00055A42
		protected override void Awake()
		{
			base.Awake();
			this.finishWhenTimeOut = true;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00057851 File Offset: 0x00055A51
		protected override void OnInteractFinished()
		{
			MasterKeysRegisterView.Show();
		}
	}
}
