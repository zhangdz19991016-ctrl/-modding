using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class ItemPickerDebug : MonoBehaviour
{
	// Token: 0x06000AE1 RID: 2785 RVA: 0x0002F62B File Offset: 0x0002D82B
	public void PickPlayerInventoryAndLog()
	{
		this.Pick().Forget();
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0002F638 File Offset: 0x0002D838
	private UniTask Pick()
	{
		ItemPickerDebug.<Pick>d__1 <Pick>d__;
		<Pick>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Pick>d__.<>1__state = -1;
		<Pick>d__.<>t__builder.Start<ItemPickerDebug.<Pick>d__1>(ref <Pick>d__);
		return <Pick>d__.<>t__builder.Task;
	}
}
