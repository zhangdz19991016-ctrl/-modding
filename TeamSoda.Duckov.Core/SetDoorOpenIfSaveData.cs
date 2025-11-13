using System;
using Saves;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class SetDoorOpenIfSaveData : MonoBehaviour
{
	// Token: 0x06000736 RID: 1846 RVA: 0x00020882 File Offset: 0x0001EA82
	private void Start()
	{
		if (LevelManager.LevelInited)
		{
			this.OnSet();
			return;
		}
		LevelManager.OnLevelInitialized += this.OnSet;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x000208A3 File Offset: 0x0001EAA3
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.OnSet;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000208B8 File Offset: 0x0001EAB8
	private void OnSet()
	{
		bool flag = SavesSystem.Load<bool>(this.key);
		Debug.Log(string.Format("Load door data:{0}  {1}", this.key, flag));
		this.door.ForceSetClosed(flag != this.openIfDataTure, false);
	}

	// Token: 0x040006DB RID: 1755
	public Door door;

	// Token: 0x040006DC RID: 1756
	public string key;

	// Token: 0x040006DD RID: 1757
	public bool openIfDataTure = true;
}
