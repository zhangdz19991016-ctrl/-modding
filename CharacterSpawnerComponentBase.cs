using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
public abstract class CharacterSpawnerComponentBase : MonoBehaviour
{
	// Token: 0x06000505 RID: 1285
	public abstract void Init(CharacterSpawnerRoot root);

	// Token: 0x06000506 RID: 1286
	public abstract void StartSpawn();
}
