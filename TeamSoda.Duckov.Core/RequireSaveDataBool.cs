using System;
using Duckov.Quests;
using Saves;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class RequireSaveDataBool : Condition
{
	// Token: 0x0600099B RID: 2459 RVA: 0x0002A3E8 File Offset: 0x000285E8
	public override bool Evaluate()
	{
		bool flag = SavesSystem.Load<bool>(this.key);
		Debug.Log(string.Format("Load bool:{0}  value:{1}", this.key, flag));
		return flag == this.requireValue;
	}

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	private string key;

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	private bool requireValue;
}
