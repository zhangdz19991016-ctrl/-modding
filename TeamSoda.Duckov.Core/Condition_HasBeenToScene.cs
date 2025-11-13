using System;
using Duckov.Quests;
using Duckov.Scenes;

// Token: 0x02000118 RID: 280
public class Condition_HasBeenToScene : Condition
{
	// Token: 0x06000989 RID: 2441 RVA: 0x0002A185 File Offset: 0x00028385
	public override bool Evaluate()
	{
		return MultiSceneCore.GetVisited(this.sceneID);
	}

	// Token: 0x0400087A RID: 2170
	[SceneID]
	public string sceneID;
}
