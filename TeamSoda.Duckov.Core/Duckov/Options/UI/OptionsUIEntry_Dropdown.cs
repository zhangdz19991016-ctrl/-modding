using System;
using System.Collections.Generic;
using System.Linq;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Duckov.Options.UI
{
	// Token: 0x02000263 RID: 611
	public class OptionsUIEntry_Dropdown : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x00048278 File Offset: 0x00046478
		private string optionKey
		{
			get
			{
				if (this.provider == null)
				{
					return "";
				}
				return this.provider.Key;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x00048299 File Offset: 0x00046499
		// (set) Token: 0x0600131A RID: 4890 RVA: 0x000482AB File Offset: 0x000464AB
		[LocalizationKey("Options")]
		public string LabelKey
		{
			get
			{
				return "Options_" + this.optionKey;
			}
			set
			{
			}
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000482AD File Offset: 0x000464AD
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.SetupDropdown();
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x000482B8 File Offset: 0x000464B8
		private void SetupDropdown()
		{
			if (!this.provider)
			{
				return;
			}
			List<string> list = this.provider.GetOptions().ToList<string>();
			string currentOption = this.provider.GetCurrentOption();
			int num = list.IndexOf(currentOption);
			if (num < 0)
			{
				list.Insert(0, currentOption);
				num = 0;
			}
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(list.ToList<string>());
			this.dropdown.SetValueWithoutNotify(num);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00048330 File Offset: 0x00046530
		private void Awake()
		{
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
			this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnDropdownValueChanged));
			this.label.text = this.LabelKey.ToPlainText();
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00048380 File Offset: 0x00046580
		private void Start()
		{
			this.SetupDropdown();
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00048388 File Offset: 0x00046588
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0004839B File Offset: 0x0004659B
		private void OnSetLanguage(SystemLanguage language)
		{
			this.SetupDropdown();
			this.label.text = this.LabelKey.ToPlainText();
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x000483BC File Offset: 0x000465BC
		private void OnDropdownValueChanged(int index)
		{
			if (!this.provider)
			{
				return;
			}
			int num = this.provider.GetOptions().ToList<string>().IndexOf(this.dropdown.options[index].text);
			if (num >= 0)
			{
				this.provider.Set(num);
			}
			this.SetupDropdown();
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00048419 File Offset: 0x00046619
		private void OnValidate()
		{
			if (this.label)
			{
				this.label.text = this.LabelKey.ToPlainText();
			}
		}

		// Token: 0x04000E59 RID: 3673
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000E5A RID: 3674
		[SerializeField]
		private OptionsProviderBase provider;

		// Token: 0x04000E5B RID: 3675
		[SerializeField]
		private TMP_Dropdown dropdown;
	}
}
