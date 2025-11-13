using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000392 RID: 914
	public class SliderWithTextField : MonoBehaviour
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x0006FE80 File Offset: 0x0006E080
		// (set) Token: 0x06001FE6 RID: 8166 RVA: 0x0006FE88 File Offset: 0x0006E088
		[LocalizationKey("Default")]
		public string LabelKey
		{
			get
			{
				return this._labelKey;
			}
			set
			{
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x0006FE8A File Offset: 0x0006E08A
		// (set) Token: 0x06001FE8 RID: 8168 RVA: 0x0006FE92 File Offset: 0x0006E092
		public float Value
		{
			get
			{
				return this.GetValue();
			}
			set
			{
				this.SetValue(value);
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0006FE9B File Offset: 0x0006E09B
		public void SetValueWithoutNotify(float value)
		{
			this.value = value;
			this.RefreshValues();
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0006FEAA File Offset: 0x0006E0AA
		public void SetValue(float value)
		{
			this.SetValueWithoutNotify(value);
			Action<float> action = this.onValueChanged;
			if (action == null)
			{
				return;
			}
			action(value);
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0006FEC4 File Offset: 0x0006E0C4
		public float GetValue()
		{
			return this.value;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x0006FECC File Offset: 0x0006E0CC
		private void Awake()
		{
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.valueField.onEndEdit.AddListener(new UnityAction<string>(this.OnFieldEndEdit));
			this.RefreshLable();
			LocalizationManager.OnSetLanguage += this.OnLanguageChanged;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x0006FF28 File Offset: 0x0006E128
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= this.OnLanguageChanged;
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x0006FF3B File Offset: 0x0006E13B
		private void OnLanguageChanged(SystemLanguage language)
		{
			this.RefreshLable();
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x0006FF43 File Offset: 0x0006E143
		private void RefreshLable()
		{
			if (this.label)
			{
				this.label.text = this.LabelKey.ToPlainText();
			}
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0006FF68 File Offset: 0x0006E168
		private void OnFieldEndEdit(string arg0)
		{
			float num;
			if (float.TryParse(arg0, out num))
			{
				if (this.isPercentage)
				{
					num /= 100f;
				}
				num = Mathf.Clamp(num, this.slider.minValue, this.slider.maxValue);
				this.Value = num;
			}
			this.RefreshValues();
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0006FFB9 File Offset: 0x0006E1B9
		private void OnEnable()
		{
			this.RefreshValues();
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0006FFC1 File Offset: 0x0006E1C1
		private void OnSliderValueChanged(float value)
		{
			this.Value = value;
			this.RefreshValues();
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0006FFD0 File Offset: 0x0006E1D0
		private void RefreshValues()
		{
			this.valueField.SetTextWithoutNotify(this.Value.ToString(this.valueFormat));
			this.slider.SetValueWithoutNotify(this.Value);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0007000D File Offset: 0x0006E20D
		private void OnValidate()
		{
			this.RefreshLable();
		}

		// Token: 0x040015BB RID: 5563
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x040015BC RID: 5564
		[SerializeField]
		private Slider slider;

		// Token: 0x040015BD RID: 5565
		[SerializeField]
		private TMP_InputField valueField;

		// Token: 0x040015BE RID: 5566
		[SerializeField]
		private string valueFormat = "0";

		// Token: 0x040015BF RID: 5567
		[SerializeField]
		private bool isPercentage;

		// Token: 0x040015C0 RID: 5568
		[SerializeField]
		private string _labelKey = "?";

		// Token: 0x040015C1 RID: 5569
		[SerializeField]
		private float value;

		// Token: 0x040015C2 RID: 5570
		public Action<float> onValueChanged;
	}
}
