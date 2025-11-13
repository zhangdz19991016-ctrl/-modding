using System;
using Saves;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class SaveDataBoolProxy : MonoBehaviour
{
	// Token: 0x060005CC RID: 1484 RVA: 0x0001A110 File Offset: 0x00018310
	public void Save()
	{
		SavesSystem.Save<bool>(this.key, this.value);
		Debug.Log(string.Format("SetSaveData:{0} to {1}", this.key, this.value));
	}

	// Token: 0x0400054C RID: 1356
	public string key;

	// Token: 0x0400054D RID: 1357
	public bool value;
}
