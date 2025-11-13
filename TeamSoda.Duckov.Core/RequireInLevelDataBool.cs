using System;
using Duckov.Quests;
using Duckov.Scenes;

// Token: 0x0200011F RID: 287
public class RequireInLevelDataBool : Condition
{
	// Token: 0x06000998 RID: 2456 RVA: 0x0002A33C File Offset: 0x0002853C
	public override bool Evaluate()
	{
		if (!MultiSceneCore.Instance)
		{
			return false;
		}
		if (!this.keyHashInited)
		{
			this.InitKeyHash();
		}
		object obj;
		return !this.isEmptyString && (MultiSceneCore.Instance.inLevelData.TryGetValue(this.keyHash, out obj) && obj is bool) && (bool)obj;
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0002A398 File Offset: 0x00028598
	private void InitKeyHash()
	{
		if (this.keyString == "")
		{
			this.isEmptyString = true;
		}
		this.keyHash = this.keyString.GetHashCode();
		this.keyHashInited = true;
	}

	// Token: 0x04000885 RID: 2181
	public string keyString = "";

	// Token: 0x04000886 RID: 2182
	private int keyHash = -1;

	// Token: 0x04000887 RID: 2183
	private bool keyHashInited;

	// Token: 0x04000888 RID: 2184
	private bool isEmptyString;
}
