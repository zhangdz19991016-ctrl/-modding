using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class LevelManagerProxy : MonoBehaviour
{
	// Token: 0x0600096B RID: 2411 RVA: 0x00029F23 File Offset: 0x00028123
	public void NotifyEvacuated()
	{
		LevelManager instance = LevelManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.NotifyEvacuated(new EvacuationInfo(MultiSceneCore.ActiveSubSceneID, base.transform.position));
	}
}
