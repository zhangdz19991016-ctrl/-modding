using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class AISpecialAttachment_SpawnItemOnCritKill : AISpecialAttachmentBase
{
	// Token: 0x060004F3 RID: 1267 RVA: 0x0001654C File Offset: 0x0001474C
	protected override void OnInited()
	{
		this.character.BeforeCharacterSpawnLootOnDead += this.BeforeCharacterSpawnLootOnDead;
		this.SpawnItem().Forget();
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00016580 File Offset: 0x00014780
	private UniTaskVoid SpawnItem()
	{
		AISpecialAttachment_SpawnItemOnCritKill.<SpawnItem>d__5 <SpawnItem>d__;
		<SpawnItem>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnItem>d__.<>4__this = this;
		<SpawnItem>d__.<>1__state = -1;
		<SpawnItem>d__.<>t__builder.Start<AISpecialAttachment_SpawnItemOnCritKill.<SpawnItem>d__5>(ref <SpawnItem>d__);
		return <SpawnItem>d__.<>t__builder.Task;
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000165C3 File Offset: 0x000147C3
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.BeforeCharacterSpawnLootOnDead -= this.BeforeCharacterSpawnLootOnDead;
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x000165EC File Offset: 0x000147EC
	private void BeforeCharacterSpawnLootOnDead(DamageInfo dmgInfo)
	{
		this.hasDead = true;
		Debug.Log(string.Format("Die crit:{0}", dmgInfo.crit));
		bool flag = dmgInfo.crit > 0;
		if (this.inverse == flag || this.character == null)
		{
			if (this.itemInstance != null)
			{
				UnityEngine.Object.Destroy(this.itemInstance.gameObject);
			}
			return;
		}
		Debug.Log("pick up on crit");
		if (this.itemInstance != null)
		{
			this.character.CharacterItem.Inventory.AddAndMerge(this.itemInstance, 0);
		}
	}

	// Token: 0x04000426 RID: 1062
	[ItemTypeID]
	public int itemToSpawn;

	// Token: 0x04000427 RID: 1063
	private Item itemInstance;

	// Token: 0x04000428 RID: 1064
	private bool hasDead;

	// Token: 0x04000429 RID: 1065
	public bool inverse;
}
