using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using Saves;
using UnityEngine;

namespace Debugging
{
	// Token: 0x02000223 RID: 547
	public class InventorySaveLoad : MonoBehaviour
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x00040611 File Offset: 0x0003E811
		public void Save()
		{
			this.inventory.Save(this.key);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00040624 File Offset: 0x0003E824
		public UniTask Load()
		{
			InventorySaveLoad.<Load>d__4 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Load>d__.<>4__this = this;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<InventorySaveLoad.<Load>d__4>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00040667 File Offset: 0x0003E867
		private void OnLoadFinished()
		{
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00040669 File Offset: 0x0003E869
		public void BeginLoad()
		{
			this.Load().Forget();
		}

		// Token: 0x04000D31 RID: 3377
		public Inventory inventory;

		// Token: 0x04000D32 RID: 3378
		public string key = "helloInventory";

		// Token: 0x04000D33 RID: 3379
		private bool loading;
	}
}
