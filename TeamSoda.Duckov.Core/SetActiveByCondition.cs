using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Quests;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class SetActiveByCondition : MonoBehaviour
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x0001A264 File Offset: 0x00018464
	private void Update()
	{
		if (!LevelManager.LevelInited && this.requireLevelInited)
		{
			return;
		}
		if (!this.autoCheck)
		{
			return;
		}
		this.Set();
		if (this.update)
		{
			this.CheckAndLoop().Forget();
		}
		base.enabled = false;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0001A2B0 File Offset: 0x000184B0
	public void Set()
	{
		if (this.targetObject)
		{
			bool flag = this.conditions.Satisfied();
			if (this.inverse)
			{
				flag = !flag;
			}
			this.targetObject.SetActive(flag);
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0001A2F0 File Offset: 0x000184F0
	private UniTaskVoid CheckAndLoop()
	{
		SetActiveByCondition.<CheckAndLoop>d__9 <CheckAndLoop>d__;
		<CheckAndLoop>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CheckAndLoop>d__.<>4__this = this;
		<CheckAndLoop>d__.<>1__state = -1;
		<CheckAndLoop>d__.<>t__builder.Start<SetActiveByCondition.<CheckAndLoop>d__9>(ref <CheckAndLoop>d__);
		return <CheckAndLoop>d__.<>t__builder.Task;
	}

	// Token: 0x04000551 RID: 1361
	public GameObject targetObject;

	// Token: 0x04000552 RID: 1362
	public bool inverse;

	// Token: 0x04000553 RID: 1363
	public List<Condition> conditions;

	// Token: 0x04000554 RID: 1364
	public bool autoCheck = true;

	// Token: 0x04000555 RID: 1365
	public bool update;

	// Token: 0x04000556 RID: 1366
	public bool requireLevelInited = true;

	// Token: 0x04000557 RID: 1367
	private float checkTimeSpace = 1f;
}
