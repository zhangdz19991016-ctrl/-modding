using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class DynamicItemDebugger : MonoBehaviour
{
	// Token: 0x060004BE RID: 1214 RVA: 0x00015C2C File Offset: 0x00013E2C
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.Add();
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00015C40 File Offset: 0x00013E40
	private void Add()
	{
		foreach (Item prefab in this.prefabs)
		{
			ItemAssetsCollection.AddDynamicEntry(prefab);
		}
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00015C94 File Offset: 0x00013E94
	private void CreateCorresponding()
	{
		this.CreateTask().Forget();
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00015CA4 File Offset: 0x00013EA4
	private UniTask CreateTask()
	{
		DynamicItemDebugger.<CreateTask>d__4 <CreateTask>d__;
		<CreateTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateTask>d__.<>4__this = this;
		<CreateTask>d__.<>1__state = -1;
		<CreateTask>d__.<>t__builder.Start<DynamicItemDebugger.<CreateTask>d__4>(ref <CreateTask>d__);
		return <CreateTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000403 RID: 1027
	[SerializeField]
	private List<Item> prefabs;
}
