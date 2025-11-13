using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class EnemyCreator : MonoBehaviour
{
	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0002BA9C File Offset: 0x00029C9C
	private int characterItemTypeID
	{
		get
		{
			return GameplayDataSettings.ItemAssets.DefaultCharacterItemTypeID;
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0002BAA8 File Offset: 0x00029CA8
	private void Start()
	{
		Debug.LogError("This scripts shouldn't exist!", this);
		if (LevelManager.LevelInited)
		{
			this.StartCreate();
			return;
		}
		LevelManager.OnLevelInitialized += this.StartCreate;
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0002BAD4 File Offset: 0x00029CD4
	private void OnDestroy()
	{
		LevelManager.OnLevelInitialized -= this.StartCreate;
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0002BAE8 File Offset: 0x00029CE8
	private void StartCreate()
	{
		int creatorID = this.GetCreatorID();
		if (MultiSceneCore.Instance != null)
		{
			if (MultiSceneCore.Instance.usedCreatorIds.Contains(creatorID))
			{
				return;
			}
			MultiSceneCore.Instance.usedCreatorIds.Add(creatorID);
		}
		this.CreateCharacterAsync();
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0002BB34 File Offset: 0x00029D34
	private UniTaskVoid CreateCharacterAsync()
	{
		EnemyCreator.<CreateCharacterAsync>d__11 <CreateCharacterAsync>d__;
		<CreateCharacterAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateCharacterAsync>d__.<>4__this = this;
		<CreateCharacterAsync>d__.<>1__state = -1;
		<CreateCharacterAsync>d__.<>t__builder.Start<EnemyCreator.<CreateCharacterAsync>d__11>(ref <CreateCharacterAsync>d__);
		return <CreateCharacterAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0002BB78 File Offset: 0x00029D78
	private void PlugAccessories()
	{
		Slot slot = this.character.PrimWeaponSlot();
		Item item = (slot != null) ? slot.Content : null;
		if (item == null)
		{
			return;
		}
		CharacterMainControl characterMainControl = this.character;
		Inventory inventory;
		if (characterMainControl == null)
		{
			inventory = null;
		}
		else
		{
			Item characterItem = characterMainControl.CharacterItem;
			inventory = ((characterItem != null) ? characterItem.Inventory : null);
		}
		Inventory inventory2 = inventory;
		if (inventory2 == null)
		{
			return;
		}
		foreach (Item item2 in inventory2)
		{
			if (!(item2 == null))
			{
				item.TryPlug(item2, true, null, 0);
			}
		}
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0002BC18 File Offset: 0x00029E18
	private UniTask AddBullet()
	{
		EnemyCreator.<AddBullet>d__13 <AddBullet>d__;
		<AddBullet>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<AddBullet>d__.<>4__this = this;
		<AddBullet>d__.<>1__state = -1;
		<AddBullet>d__.<>t__builder.Start<EnemyCreator.<AddBullet>d__13>(ref <AddBullet>d__);
		return <AddBullet>d__.<>t__builder.Task;
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0002BC5C File Offset: 0x00029E5C
	private UniTask<List<Item>> GenerateItems()
	{
		EnemyCreator.<GenerateItems>d__14 <GenerateItems>d__;
		<GenerateItems>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<Item>>.Create();
		<GenerateItems>d__.<>4__this = this;
		<GenerateItems>d__.<>1__state = -1;
		<GenerateItems>d__.<>t__builder.Start<EnemyCreator.<GenerateItems>d__14>(ref <GenerateItems>d__);
		return <GenerateItems>d__.<>t__builder.Task;
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0002BCA0 File Offset: 0x00029EA0
	private UniTask<Item> LoadOrCreateCharacterItemInstance()
	{
		EnemyCreator.<LoadOrCreateCharacterItemInstance>d__15 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.<>4__this = this;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<EnemyCreator.<LoadOrCreateCharacterItemInstance>d__15>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0002BCE4 File Offset: 0x00029EE4
	private int GetCreatorID()
	{
		Transform parent = base.transform.parent;
		string text = base.transform.GetSiblingIndex().ToString();
		while (parent != null)
		{
			text = string.Format("{0}/{1}", parent.GetSiblingIndex(), text);
			parent = parent.parent;
		}
		text = string.Format("{0}/{1}", base.gameObject.scene.buildIndex, text);
		return text.GetHashCode();
	}

	// Token: 0x040008E7 RID: 2279
	private CharacterMainControl character;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	private List<RandomItemGenerateDescription> itemsToGenerate;

	// Token: 0x040008E9 RID: 2281
	[SerializeField]
	private ItemFilter bulletFilter;

	// Token: 0x040008EA RID: 2282
	[SerializeField]
	private AudioManager.VoiceType voiceType;

	// Token: 0x040008EB RID: 2283
	[SerializeField]
	private CharacterModel characterModel;

	// Token: 0x040008EC RID: 2284
	[SerializeField]
	private AICharacterController aiController;
}
