using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000B2 RID: 178
public class SoulCollector : MonoBehaviour
{
	// Token: 0x060005E6 RID: 1510 RVA: 0x0001A6DA File Offset: 0x000188DA
	private void Awake()
	{
		Health.OnDead += this.OnCharacterDie;
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001A6ED File Offset: 0x000188ED
	private void OnDestroy()
	{
		Health.OnDead -= this.OnCharacterDie;
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001A700 File Offset: 0x00018900
	private void Update()
	{
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0001A704 File Offset: 0x00018904
	private void OnCharacterDie(Health health, DamageInfo dmgInfo)
	{
		if (!health)
		{
			return;
		}
		if (!health.hasSoul)
		{
			return;
		}
		if (!this.selfCharacter && this.selfAgent.Item)
		{
			this.selfCharacter = this.selfAgent.Item.GetCharacterMainControl();
		}
		if (!this.selfCharacter)
		{
			return;
		}
		if (Vector3.Distance(health.transform.position, this.selfCharacter.transform.position) > 40f)
		{
			return;
		}
		int num = Mathf.RoundToInt(health.MaxHealth / 15f);
		if (num < 1)
		{
			num = 1;
		}
		if (LevelManager.Rule.AdvancedDebuffMode)
		{
			num *= 3;
		}
		this.SpawnCubes(health.transform.position + Vector3.up * 0.75f, num).Forget();
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0001A7E4 File Offset: 0x000189E4
	private UniTaskVoid SpawnCubes(Vector3 startPoint, int times)
	{
		SoulCollector.<SpawnCubes>d__10 <SpawnCubes>d__;
		<SpawnCubes>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnCubes>d__.<>4__this = this;
		<SpawnCubes>d__.startPoint = startPoint;
		<SpawnCubes>d__.times = times;
		<SpawnCubes>d__.<>1__state = -1;
		<SpawnCubes>d__.<>t__builder.Start<SoulCollector.<SpawnCubes>d__10>(ref <SpawnCubes>d__);
		return <SpawnCubes>d__.<>t__builder.Task;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001A838 File Offset: 0x00018A38
	public void AddCube()
	{
		this.AddCubeAsync().Forget();
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0001A854 File Offset: 0x00018A54
	private UniTaskVoid AddCubeAsync()
	{
		SoulCollector.<AddCubeAsync>d__12 <AddCubeAsync>d__;
		<AddCubeAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<AddCubeAsync>d__.<>4__this = this;
		<AddCubeAsync>d__.<>1__state = -1;
		<AddCubeAsync>d__.<>t__builder.Start<SoulCollector.<AddCubeAsync>d__12>(ref <AddCubeAsync>d__);
		return <AddCubeAsync>d__.<>t__builder.Task;
	}

	// Token: 0x04000565 RID: 1381
	public DuckovItemAgent selfAgent;

	// Token: 0x04000566 RID: 1382
	private CharacterMainControl selfCharacter;

	// Token: 0x04000567 RID: 1383
	[ItemTypeID]
	public int soulCubeID = 1165;

	// Token: 0x04000568 RID: 1384
	private Slot cubeSlot;

	// Token: 0x04000569 RID: 1385
	public GameObject addFx;

	// Token: 0x0400056A RID: 1386
	public SoulCube cubePfb;
}
