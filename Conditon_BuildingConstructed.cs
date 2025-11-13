using System;
using Duckov.Buildings;
using Duckov.Quests;

// Token: 0x0200011C RID: 284
public class Conditon_BuildingConstructed : Condition
{
	// Token: 0x06000992 RID: 2450 RVA: 0x0002A2B4 File Offset: 0x000284B4
	public override bool Evaluate()
	{
		bool flag = BuildingManager.Any(this.buildingID, false);
		if (this.not)
		{
			flag = !flag;
		}
		return flag;
	}

	// Token: 0x0400087F RID: 2175
	public string buildingID;

	// Token: 0x04000880 RID: 2176
	public bool not;
}
