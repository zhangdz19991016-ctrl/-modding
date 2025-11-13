using System;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class LanguageOptionsProvider : OptionsProviderBase
{
	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00038BAF File Offset: 0x00036DAF
	public override string Key
	{
		get
		{
			return "Language";
		}
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00038BB6 File Offset: 0x00036DB6
	public override string GetCurrentOption()
	{
		return LocalizationManager.CurrentLanguageDisplayName;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x00038BC0 File Offset: 0x00036DC0
	public override string[] GetOptions()
	{
		LocalizationDatabase instance = LocalizationDatabase.Instance;
		if (instance == null)
		{
			return new string[]
			{
				"?"
			};
		}
		string[] languageDisplayNameList = instance.GetLanguageDisplayNameList();
		this.cache = languageDisplayNameList;
		return languageDisplayNameList;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00038BFA File Offset: 0x00036DFA
	public override void Set(int index)
	{
		if (this.cache == null)
		{
			this.GetOptions();
		}
		if (index < 0 || index >= this.cache.Length)
		{
			Debug.LogError("语言越界");
			return;
		}
		LocalizationManager.SetLanguage(index);
	}

	// Token: 0x04000BB2 RID: 2994
	private string[] cache;
}
