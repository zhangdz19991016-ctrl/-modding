using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class CharacterSpawnerGroupSelector : CharacterSpawnerComponentBase
{
	// Token: 0x06000510 RID: 1296 RVA: 0x00016CE0 File Offset: 0x00014EE0
	public void Collect()
	{
		this.groups = base.GetComponentsInChildren<CharacterSpawnerGroup>().ToList<CharacterSpawnerGroup>();
		foreach (CharacterSpawnerGroup characterSpawnerGroup in this.groups)
		{
			characterSpawnerGroup.Collect();
		}
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00016D44 File Offset: 0x00014F44
	public override void Init(CharacterSpawnerRoot root)
	{
		foreach (CharacterSpawnerGroup characterSpawnerGroup in this.groups)
		{
			if (characterSpawnerGroup == null)
			{
				Debug.LogError("生成器引用为空");
			}
			else
			{
				characterSpawnerGroup.Init(root);
			}
		}
		this.spawnerRoot = root;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00016DB4 File Offset: 0x00014FB4
	public override void StartSpawn()
	{
		if (this.spawnGroupCountRange.y > this.groups.Count)
		{
			this.spawnGroupCountRange.y = this.groups.Count;
		}
		if (this.spawnGroupCountRange.x > this.groups.Count)
		{
			this.spawnGroupCountRange.x = this.groups.Count;
		}
		int count = UnityEngine.Random.Range(this.spawnGroupCountRange.x, this.spawnGroupCountRange.y);
		this.finalCount = count;
		this.RandomSpawn(count);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00016E47 File Offset: 0x00015047
	private void OnValidate()
	{
		if (this.groups.Count < 0)
		{
			return;
		}
		if (this.spawnGroupCountRange.x > this.spawnGroupCountRange.y)
		{
			this.spawnGroupCountRange.y = this.spawnGroupCountRange.x;
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00016E88 File Offset: 0x00015088
	public void RandomSpawn(int count)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.groups.Count; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < count; j++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			int index2 = list[index];
			list.RemoveAt(index);
			CharacterSpawnerGroup characterSpawnerGroup = this.groups[index2];
			if (characterSpawnerGroup)
			{
				characterSpawnerGroup.StartSpawn();
			}
		}
	}

	// Token: 0x04000481 RID: 1153
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x04000482 RID: 1154
	public List<CharacterSpawnerGroup> groups;

	// Token: 0x04000483 RID: 1155
	public Vector2Int spawnGroupCountRange = new Vector2Int(1, 1);

	// Token: 0x04000484 RID: 1156
	private int finalCount;
}
