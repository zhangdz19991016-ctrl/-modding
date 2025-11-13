using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000095 RID: 149
[RequireComponent(typeof(Points))]
public class RandomCharacterSpawner : CharacterSpawnerComponentBase
{
	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x000174D8 File Offset: 0x000156D8
	private float minDistanceToMainCharacter
	{
		get
		{
			return this.spawnerRoot.minDistanceToPlayer;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000525 RID: 1317 RVA: 0x000174E5 File Offset: 0x000156E5
	private int scene
	{
		get
		{
			return this.spawnerRoot.RelatedScene;
		}
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x000174F2 File Offset: 0x000156F2
	private void ShowGizmo()
	{
		RandomCharacterSpawner.currentGizmosTag = this.gizmosTag;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00017500 File Offset: 0x00015700
	public override void Init(CharacterSpawnerRoot root)
	{
		this.spawnerRoot = root;
		if (this.spawnPoints == null)
		{
			this.spawnPoints = base.GetComponent<Points>();
		}
		this.spawnCountRange = new Vector2Int(Mathf.RoundToInt((float)this.spawnCountRange.x * LevelManager.enemySpawnCountFactor), Mathf.RoundToInt((float)this.spawnCountRange.y * LevelManager.enemySpawnCountFactor));
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00017567 File Offset: 0x00015767
	private void OnDestroy()
	{
		this.destroied = true;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00017570 File Offset: 0x00015770
	private CharacterRandomPresetInfo GetAPresetByWeight()
	{
		if (this.totalWeight < 0f)
		{
			this.totalWeight = 0f;
			for (int i = 0; i < this.randomPresetInfos.Count; i++)
			{
				if (this.randomPresetInfos[i].randomPreset == null)
				{
					this.randomPresetInfos.RemoveAt(i);
					i--;
					Debug.Log("Null preset");
				}
				else
				{
					this.totalWeight += this.randomPresetInfos[i].weight;
				}
			}
		}
		float num = UnityEngine.Random.Range(0f, this.totalWeight);
		float num2 = 0f;
		for (int j = 0; j < this.randomPresetInfos.Count; j++)
		{
			num2 += this.randomPresetInfos[j].weight;
			if (num < num2)
			{
				return this.randomPresetInfos[j];
			}
		}
		Debug.LogError("权重计算错误", base.gameObject);
		return this.randomPresetInfos[this.randomPresetInfos.Count - 1];
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00017678 File Offset: 0x00015878
	public override void StartSpawn()
	{
		this.CreateAsync().Forget();
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00017694 File Offset: 0x00015894
	private UniTaskVoid CreateAsync()
	{
		RandomCharacterSpawner.<CreateAsync>d__25 <CreateAsync>d__;
		<CreateAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateAsync>d__.<>4__this = this;
		<CreateAsync>d__.<>1__state = -1;
		<CreateAsync>d__.<>t__builder.Start<RandomCharacterSpawner.<CreateAsync>d__25>(ref <CreateAsync>d__);
		return <CreateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x000176D8 File Offset: 0x000158D8
	private UniTask<CharacterMainControl> CreateAt(Vector3 point, int scene, CharacterSpawnerGroup group, bool isLeader)
	{
		RandomCharacterSpawner.<CreateAt>d__26 <CreateAt>d__;
		<CreateAt>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateAt>d__.<>4__this = this;
		<CreateAt>d__.point = point;
		<CreateAt>d__.scene = scene;
		<CreateAt>d__.group = group;
		<CreateAt>d__.isLeader = isLeader;
		<CreateAt>d__.<>1__state = -1;
		<CreateAt>d__.<>t__builder.Start<RandomCharacterSpawner.<CreateAt>d__26>(ref <CreateAt>d__);
		return <CreateAt>d__.<>t__builder.Task;
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0001773C File Offset: 0x0001593C
	private void OnDrawGizmos()
	{
		if (RandomCharacterSpawner.currentGizmosTag != this.gizmosTag)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		if (this.spawnPoints && this.spawnPoints.points.Count > 0)
		{
			Vector3 point = this.spawnPoints.GetPoint(0);
			Vector3 vector = point + Vector3.up * 20f;
			Gizmos.DrawWireSphere(point, 10f);
			Gizmos.DrawLine(point, vector);
			Gizmos.DrawSphere(vector, 3f);
		}
	}

	// Token: 0x0400049F RID: 1183
	public Points spawnPoints;

	// Token: 0x040004A0 RID: 1184
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x040004A1 RID: 1185
	public CharacterSpawnerGroup masterGroup;

	// Token: 0x040004A2 RID: 1186
	public List<CharacterRandomPresetInfo> randomPresetInfos;

	// Token: 0x040004A3 RID: 1187
	private float delayTime = 1f;

	// Token: 0x040004A4 RID: 1188
	public Vector2Int spawnCountRange;

	// Token: 0x040004A5 RID: 1189
	private float totalWeight = -1f;

	// Token: 0x040004A6 RID: 1190
	public bool isStaticTarget;

	// Token: 0x040004A7 RID: 1191
	public static string currentGizmosTag;

	// Token: 0x040004A8 RID: 1192
	public bool firstIsLeader;

	// Token: 0x040004A9 RID: 1193
	private bool firstCreateStarted;

	// Token: 0x040004AA RID: 1194
	public UnityEvent OnStartCreateEvent;

	// Token: 0x040004AB RID: 1195
	private int targetSpawnCount;

	// Token: 0x040004AC RID: 1196
	private int currentSpawnedCount;

	// Token: 0x040004AD RID: 1197
	private bool destroied;

	// Token: 0x040004AE RID: 1198
	public string gizmosTag;
}
