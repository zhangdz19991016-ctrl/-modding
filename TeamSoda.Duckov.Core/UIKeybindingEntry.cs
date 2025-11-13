using System;
using Cysharp.Threading.Tasks;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020001E1 RID: 481
public class UIKeybindingEntry : MonoBehaviour
{
	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0003A738 File Offset: 0x00038938
	// (set) Token: 0x06000E61 RID: 3681 RVA: 0x0003A787 File Offset: 0x00038987
	[LocalizationKey("UIText")]
	private string displayNameKey
	{
		get
		{
			if (!string.IsNullOrEmpty(this.overrideDisplayNameKey))
			{
				return this.overrideDisplayNameKey;
			}
			if (this.actionRef == null)
			{
				return "?";
			}
			return "Input_" + this.actionRef.action.name;
		}
		set
		{
		}
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0003A78C File Offset: 0x0003898C
	private void Awake()
	{
		this.rebindButton.onClick.AddListener(new UnityAction(this.OnButtonClick));
		this.clearButton.onClick.AddListener(new UnityAction(this.OnClearButtonClick));
		this.Setup();
		LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x0003A7E8 File Offset: 0x000389E8
	private void OnClearButtonClick()
	{
		InputRebinder.ClearRebind(this.actionRef.action.name);
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x0003A7FF File Offset: 0x000389FF
	private void OnDestroy()
	{
		LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x0003A812 File Offset: 0x00038A12
	private void OnLanguageChanged(SystemLanguage language)
	{
		this.label.text = this.displayNameKey.ToPlainText();
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x0003A82A File Offset: 0x00038A2A
	private void OnButtonClick()
	{
		InputRebinder.RebindAsync(this.actionRef.action.name, this.index, this.excludes, true).Forget<bool>();
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x0003A853 File Offset: 0x00038A53
	private void OnValidate()
	{
		this.Setup();
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x0003A85B File Offset: 0x00038A5B
	private void Setup()
	{
		this.indicator.Setup(this.actionRef, this.index);
		this.label.text = this.displayNameKey.ToPlainText();
	}

	// Token: 0x04000BED RID: 3053
	[SerializeField]
	private InputActionReference actionRef;

	// Token: 0x04000BEE RID: 3054
	[SerializeField]
	private int index;

	// Token: 0x04000BEF RID: 3055
	[SerializeField]
	private string overrideDisplayNameKey;

	// Token: 0x04000BF0 RID: 3056
	private string[] excludes = new string[]
	{
		"<Mouse>/leftButton",
		"<Mouse>/rightButton",
		"<Pointer>/position",
		"<Pointer>/delta",
		"<Pointer>/press",
		"<Mouse>/scroll"
	};

	// Token: 0x04000BF1 RID: 3057
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04000BF2 RID: 3058
	[SerializeField]
	private Button rebindButton;

	// Token: 0x04000BF3 RID: 3059
	[SerializeField]
	private Button clearButton;

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private InputIndicator indicator;
}
