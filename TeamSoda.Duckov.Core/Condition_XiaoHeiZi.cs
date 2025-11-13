using System;
using Duckov.Quests;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class Condition_XiaoHeiZi : Condition
{
	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x0002A215 File Offset: 0x00028415
	public override string DisplayText
	{
		get
		{
			return "看看你是不是小黑子";
		}
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0002A21C File Offset: 0x0002841C
	public override bool Evaluate()
	{
		if (CharacterMainControl.Main == null)
		{
			return false;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		CharacterModel characterModel = main.characterModel;
		if (!characterModel)
		{
			return false;
		}
		CustomFaceInstance customFace = characterModel.CustomFace;
		if (!customFace)
		{
			return false;
		}
		if (customFace.ConvertToSaveData().hairID != this.hairID)
		{
			return false;
		}
		Item armorItem = main.GetArmorItem();
		return !(armorItem == null) && armorItem.TypeID == this.armorID;
	}

	// Token: 0x0400087D RID: 2173
	[SerializeField]
	private int hairID = 6;

	// Token: 0x0400087E RID: 2174
	[ItemTypeID]
	[SerializeField]
	private int armorID = 379;
}
