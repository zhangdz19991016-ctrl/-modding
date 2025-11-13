using System;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class SetActiveByChance : MonoBehaviour
{
	// Token: 0x060005CE RID: 1486 RVA: 0x0001A14C File Offset: 0x0001834C
	private void Awake()
	{
		bool flag = UnityEngine.Random.Range(0f, 1f) < this.activeChange;
		if (this.saveInLevel && MultiSceneCore.Instance)
		{
			object obj;
			if (MultiSceneCore.Instance.inLevelData.TryGetValue(this.keyCached, out obj) && obj is bool)
			{
				bool flag2 = (bool)obj;
				Debug.Log(string.Format("存在门存档信息：{0}", flag2));
				flag = flag2;
			}
			MultiSceneCore.Instance.inLevelData[this.keyCached] = flag;
		}
		base.gameObject.SetActive(flag);
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0001A1EC File Offset: 0x000183EC
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int x = Mathf.RoundToInt(vector.x);
		int y = Mathf.RoundToInt(vector.y);
		int z = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int = new Vector3Int(x, y, z);
		return string.Format("Door_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x0400054E RID: 1358
	public bool saveInLevel;

	// Token: 0x0400054F RID: 1359
	private int keyCached;

	// Token: 0x04000550 RID: 1360
	[Range(0f, 1f)]
	public float activeChange = 0.5f;
}
