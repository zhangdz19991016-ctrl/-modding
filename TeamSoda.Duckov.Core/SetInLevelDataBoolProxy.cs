using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class SetInLevelDataBoolProxy : MonoBehaviour
{
	// Token: 0x060005B0 RID: 1456 RVA: 0x000198C9 File Offset: 0x00017AC9
	public void SetToTarget()
	{
		this.SetTo(this.targetValue);
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x000198D8 File Offset: 0x00017AD8
	public void SetTo(bool target)
	{
		if (this.keyString == "")
		{
			return;
		}
		if (!this.keyInited)
		{
			this.InitKey();
		}
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.Instance.inLevelData[this.keyHash] = target;
		}
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0001992D File Offset: 0x00017B2D
	private void InitKey()
	{
		this.keyHash = this.keyString.GetHashCode();
		this.keyInited = true;
	}

	// Token: 0x0400052C RID: 1324
	public bool targetValue = true;

	// Token: 0x0400052D RID: 1325
	public string keyString = "";

	// Token: 0x0400052E RID: 1326
	private int keyHash;

	// Token: 0x0400052F RID: 1327
	private bool keyInited;
}
