using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Options.UI
{
	// Token: 0x02000264 RID: 612
	public class OptionsUIEntry_Slider : MonoBehaviour
	{
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x00048446 File Offset: 0x00046646
		// (set) Token: 0x06001325 RID: 4901 RVA: 0x00048458 File Offset: 0x00046658
		[LocalizationKey("Options")]
		private string labelKey
		{
			get
			{
				return "Options_" + this.key;
			}
			set
			{
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001326 RID: 4902 RVA: 0x0004845A File Offset: 0x0004665A
		// (set) Token: 0x06001327 RID: 4903 RVA: 0x0004846D File Offset: 0x0004666D
		public float Value
		{
			get
			{
				return OptionsManager.Load<float>(this.key, this.defaultValue);
			}
			set
			{
				OptionsManager.Save<float>(this.key, value);
			}
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0004847C File Offset: 0x0004667C
		private void Awake()
		{
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnFieldEndEdit));
			this.RefreshLable();
			LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x000484D8 File Offset: 0x000466D8
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000484EB File Offset: 0x000466EB
		private void OnLanguageChanged(SystemLanguage language)
		{
			this.RefreshLable();
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x000484F3 File Offset: 0x000466F3
		private void RefreshLable()
		{
			if (this.label)
			{
				this.label.text = this.labelKey.ToPlainText();
			}
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00048518 File Offset: 0x00046718
		private void OnFieldEndEdit(string arg0)
		{
			float value;
			if (float.TryParse(arg0, out value))
			{
				value = Mathf.Clamp(value, this.slider.minValue, this.slider.maxValue);
				this.Value = value;
			}
			this.RefreshValues();
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00048559 File Offset: 0x00046759
		private void OnEnable()
		{
			this.RefreshValues();
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00048561 File Offset: 0x00046761
		private void OnSliderValueChanged(float value)
		{
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00048570 File Offset: 0x00046770
		private void RefreshValues()
		{
			this.valueField.SetTextWithoutNotify(this.Value.ToString(this.valueFormat));
			this.slider.SetValueWithoutNotify(this.Value);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000485AD File Offset: 0x000467AD
		private void OnValidate()
		{
			this.RefreshLable();
		}

		// Token: 0x04000E5C RID: 3676
		[SerializeField]
		private string key;

		// Token: 0x04000E5D RID: 3677
		[Space]
		[SerializeField]
		private float defaultValue;

		// Token: 0x04000E5E RID: 3678
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000E5F RID: 3679
		[SerializeField]
		private Slider slider;

		// Token: 0x04000E60 RID: 3680
		[SerializeField]
		private TMP_InputField valueField;

		// Token: 0x04000E61 RID: 3681
		[SerializeField]
		private string valueFormat = "0";
	}
}
