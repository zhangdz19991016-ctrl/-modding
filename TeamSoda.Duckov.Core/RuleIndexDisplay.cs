using System;
using Duckov.Rules;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class RuleIndexDisplay : MonoBehaviour
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x0002F316 File Offset: 0x0002D516
	private void Awake()
	{
		LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0002F329 File Offset: 0x0002D529
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0002F33C File Offset: 0x0002D53C
	private void OnLanguageChanged(SystemLanguage language)
	{
		this.Refresh();
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0002F344 File Offset: 0x0002D544
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0002F34C File Offset: 0x0002D54C
	private void Refresh()
	{
		this.text.text = string.Format("Rule_{0}", GameRulesManager.SelectedRuleIndex).ToPlainText();
	}

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	private TextMeshProUGUI text;
}
