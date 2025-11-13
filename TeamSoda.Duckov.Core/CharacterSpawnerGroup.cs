using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class CharacterSpawnerGroup : CharacterSpawnerComponentBase
{
	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000508 RID: 1288 RVA: 0x00016AC9 File Offset: 0x00014CC9
	public AICharacterController LeaderAI
	{
		get
		{
			return this.leaderAI;
		}
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00016AD1 File Offset: 0x00014CD1
	public void Collect()
	{
		this.spawners = base.GetComponentsInChildren<RandomCharacterSpawner>().ToList<RandomCharacterSpawner>();
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00016AE4 File Offset: 0x00014CE4
	public override void Init(CharacterSpawnerRoot root)
	{
		foreach (RandomCharacterSpawner randomCharacterSpawner in this.spawners)
		{
			if (randomCharacterSpawner == null)
			{
				Debug.LogError("生成器引用为空：" + base.gameObject.name);
			}
			else
			{
				randomCharacterSpawner.Init(root);
			}
		}
		this.spawnerRoot = root;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00016B64 File Offset: 0x00014D64
	public void Awake()
	{
		this.characters = new List<AICharacterController>();
		if (this.hasLeader && UnityEngine.Random.Range(0f, 1f) > this.hasLeaderChance)
		{
			this.hasLeader = false;
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00016B98 File Offset: 0x00014D98
	private void Update()
	{
		if (this.hasLeader && this.leaderAI == null && this.characters.Count > 0)
		{
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (this.characters[i] == null)
				{
					this.characters.RemoveAt(i);
					i--;
				}
				else
				{
					this.leaderAI = this.characters[i];
				}
			}
		}
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00016C17 File Offset: 0x00014E17
	public void AddCharacterSpawned(AICharacterController _character, bool isLeader)
	{
		_character.group = this;
		if (isLeader)
		{
			this.leaderAI = _character;
		}
		else if (this.hasLeader && !this.leaderAI)
		{
			this.leaderAI = _character;
		}
		this.characters.Add(_character);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00016C54 File Offset: 0x00014E54
	public override void StartSpawn()
	{
		bool flag = true;
		foreach (RandomCharacterSpawner randomCharacterSpawner in this.spawners)
		{
			if (!(randomCharacterSpawner == null))
			{
				randomCharacterSpawner.masterGroup = this;
				if (flag && this.hasLeader)
				{
					randomCharacterSpawner.firstIsLeader = true;
				}
				flag = false;
				randomCharacterSpawner.StartSpawn();
			}
		}
	}

	// Token: 0x0400047B RID: 1147
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x0400047C RID: 1148
	public bool hasLeader;

	// Token: 0x0400047D RID: 1149
	[Range(0f, 1f)]
	public float hasLeaderChance = 1f;

	// Token: 0x0400047E RID: 1150
	public List<RandomCharacterSpawner> spawners;

	// Token: 0x0400047F RID: 1151
	private List<AICharacterController> characters;

	// Token: 0x04000480 RID: 1152
	private AICharacterController leaderAI;
}
