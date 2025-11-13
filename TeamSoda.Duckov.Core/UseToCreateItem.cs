using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class UseToCreateItem : UsageBehavior
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x00025E5C File Offset: 0x0002405C
	public override UsageBehavior.DisplaySettingsData DisplaySettings
	{
		get
		{
			return new UsageBehavior.DisplaySettingsData
			{
				display = true,
				description = this.descKey.ToPlainText()
			};
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00025E8C File Offset: 0x0002408C
	public override bool CanBeUsed(Item item, object user)
	{
		return user as CharacterMainControl;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00025EA0 File Offset: 0x000240A0
	protected override void OnUse(Item item, object user)
	{
		CharacterMainControl characterMainControl = user as CharacterMainControl;
		if (!characterMainControl)
		{
			return;
		}
		if (this.entries.entries.Count == 0)
		{
			return;
		}
		UseToCreateItem.Entry random = this.entries.GetRandom(0f);
		this.Generate(random.itemTypeID, characterMainControl).Forget();
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00025EF4 File Offset: 0x000240F4
	private UniTask Generate(int typeID, CharacterMainControl character)
	{
		UseToCreateItem.<Generate>d__9 <Generate>d__;
		<Generate>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Generate>d__.<>4__this = this;
		<Generate>d__.typeID = typeID;
		<Generate>d__.character = character;
		<Generate>d__.<>1__state = -1;
		<Generate>d__.<>t__builder.Start<UseToCreateItem.<Generate>d__9>(ref <Generate>d__);
		return <Generate>d__.<>t__builder.Task;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00025F47 File Offset: 0x00024147
	private void OnValidate()
	{
		this.entries.RefreshPercent();
	}

	// Token: 0x040007B3 RID: 1971
	[SerializeField]
	private RandomContainer<UseToCreateItem.Entry> entries;

	// Token: 0x040007B4 RID: 1972
	[LocalizationKey("Items")]
	public string descKey;

	// Token: 0x040007B5 RID: 1973
	[LocalizationKey("Default")]
	public string notificationKey;

	// Token: 0x040007B6 RID: 1974
	private bool running;

	// Token: 0x0200048A RID: 1162
	[Serializable]
	private struct Entry
	{
		// Token: 0x04001BC7 RID: 7111
		[ItemTypeID]
		[SerializeField]
		public int itemTypeID;
	}
}
