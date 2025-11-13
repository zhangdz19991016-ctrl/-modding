using System;
using System.Reflection;
using Duckov.Rules;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class RuleEntry_Bool : OptionsProviderBase
{
	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0003A925 File Offset: 0x00038B25
	public override string Key
	{
		get
		{
			return this.fieldName;
		}
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x0003A92D File Offset: 0x00038B2D
	private void Awake()
	{
		this.field = typeof(Ruleset).GetField(this.fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x0003A94C File Offset: 0x00038B4C
	public override string GetCurrentOption()
	{
		Ruleset obj = GameRulesManager.Current;
		if ((bool)this.field.GetValue(obj))
		{
			return "Options_On".ToPlainText();
		}
		return "Options_Off".ToPlainText();
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x0003A987 File Offset: 0x00038B87
	public override string[] GetOptions()
	{
		return new string[]
		{
			"Options_Off".ToPlainText(),
			"Options_On".ToPlainText()
		};
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x0003A9AC File Offset: 0x00038BAC
	public override void Set(int index)
	{
		bool flag = index > 0;
		Ruleset obj = GameRulesManager.Current;
		this.field.SetValue(obj, flag);
	}

	// Token: 0x04000BF8 RID: 3064
	[SerializeField]
	private string fieldName;

	// Token: 0x04000BF9 RID: 3065
	private FieldInfo field;
}
