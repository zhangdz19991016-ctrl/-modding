using System;
using Saves;
using UnityEngine;

// Token: 0x02000134 RID: 308
public class BaseSceneUtilities : MonoBehaviour
{
	// Token: 0x060009FF RID: 2559 RVA: 0x0002B5A7 File Offset: 0x000297A7
	private void Save()
	{
		LevelManager.Instance.SaveMainCharacter();
		SavesSystem.CollectSaveData();
		SavesSystem.SaveFile(true);
		this.lastTimeSaved = Time.realtimeSinceStartup;
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0002B5C9 File Offset: 0x000297C9
	private float TimeSinceLastSave
	{
		get
		{
			return Time.realtimeSinceStartup - this.lastTimeSaved;
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0002B5D7 File Offset: 0x000297D7
	private void Awake()
	{
		this.lastTimeSaved = Time.realtimeSinceStartup;
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0002B5E4 File Offset: 0x000297E4
	private void Update()
	{
		if (!LevelManager.LevelInited)
		{
			return;
		}
		if (this.TimeSinceLastSave > this.saveInterval)
		{
			this.Save();
		}
	}

	// Token: 0x040008DC RID: 2268
	[SerializeField]
	private float saveInterval = 5f;

	// Token: 0x040008DD RID: 2269
	private float lastTimeSaved = float.MinValue;
}
