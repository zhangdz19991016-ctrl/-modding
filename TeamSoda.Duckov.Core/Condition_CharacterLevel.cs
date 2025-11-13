using System;
using Duckov;
using Duckov.Quests;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class Condition_CharacterLevel : Condition
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x0002A0B8 File Offset: 0x000282B8
	[LocalizationKey("Default")]
	private string DisplayTextFormatKey
	{
		get
		{
			switch (this.relation)
			{
			case Condition_CharacterLevel.Relation.LessThan:
				return "Condition_CharacterLevel_LessThan";
			case Condition_CharacterLevel.Relation.Equals:
				return "Condition_CharacterLevel_Equals";
			case Condition_CharacterLevel.Relation.GreaterThan:
				return "Condition_CharacterLevel_GreaterThan";
			}
			return "";
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000985 RID: 2437 RVA: 0x0002A0FD File Offset: 0x000282FD
	private string DisplayTextFormat
	{
		get
		{
			return this.DisplayTextFormatKey.ToPlainText();
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000986 RID: 2438 RVA: 0x0002A10A File Offset: 0x0002830A
	public override string DisplayText
	{
		get
		{
			return this.DisplayTextFormat.Format(new
			{
				this.level
			});
		}
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0002A124 File Offset: 0x00028324
	public override bool Evaluate()
	{
		int num = EXPManager.Level;
		switch (this.relation)
		{
		case Condition_CharacterLevel.Relation.LessThan:
			return num <= this.level;
		case Condition_CharacterLevel.Relation.Equals:
			return num == this.level;
		case Condition_CharacterLevel.Relation.GreaterThan:
			return num >= this.level;
		}
		return false;
	}

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private Condition_CharacterLevel.Relation relation;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private int level;

	// Token: 0x0200049F RID: 1183
	private enum Relation
	{
		// Token: 0x04001C20 RID: 7200
		LessThan = 1,
		// Token: 0x04001C21 RID: 7201
		Equals,
		// Token: 0x04001C22 RID: 7202
		GreaterThan = 4
	}
}
