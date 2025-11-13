using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class CharacterCreator : MonoBehaviour
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000889 RID: 2185 RVA: 0x000265FC File Offset: 0x000247FC
	public CharacterMainControl characterPfb
	{
		get
		{
			return GameplayDataSettings.Prefabs.CharacterPrefab;
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00026608 File Offset: 0x00024808
	public UniTask<CharacterMainControl> CreateCharacter(Item itemInstance, CharacterModel modelPrefab, Vector3 pos, Quaternion rotation)
	{
		CharacterCreator.<CreateCharacter>d__2 <CreateCharacter>d__;
		<CreateCharacter>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateCharacter>d__.<>4__this = this;
		<CreateCharacter>d__.itemInstance = itemInstance;
		<CreateCharacter>d__.modelPrefab = modelPrefab;
		<CreateCharacter>d__.pos = pos;
		<CreateCharacter>d__.rotation = rotation;
		<CreateCharacter>d__.<>1__state = -1;
		<CreateCharacter>d__.<>t__builder.Start<CharacterCreator.<CreateCharacter>d__2>(ref <CreateCharacter>d__);
		return <CreateCharacter>d__.<>t__builder.Task;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0002666C File Offset: 0x0002486C
	public UniTask<Item> LoadOrCreateCharacterItemInstance(int itemTypeID)
	{
		CharacterCreator.<LoadOrCreateCharacterItemInstance>d__3 <LoadOrCreateCharacterItemInstance>d__;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
		<LoadOrCreateCharacterItemInstance>d__.itemTypeID = itemTypeID;
		<LoadOrCreateCharacterItemInstance>d__.<>1__state = -1;
		<LoadOrCreateCharacterItemInstance>d__.<>t__builder.Start<CharacterCreator.<LoadOrCreateCharacterItemInstance>d__3>(ref <LoadOrCreateCharacterItemInstance>d__);
		return <LoadOrCreateCharacterItemInstance>d__.<>t__builder.Task;
	}
}
