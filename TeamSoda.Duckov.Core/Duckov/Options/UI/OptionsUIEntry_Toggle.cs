using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Options.UI
{
	// Token: 0x02000265 RID: 613
	public class OptionsUIEntry_Toggle : MonoBehaviour
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001332 RID: 4914 RVA: 0x000485C8 File Offset: 0x000467C8
		// (set) Token: 0x06001333 RID: 4915 RVA: 0x000485DA File Offset: 0x000467DA
		[LocalizationKey("Default")]
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

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x000485DC File Offset: 0x000467DC
		// (set) Token: 0x06001335 RID: 4917 RVA: 0x000485EF File Offset: 0x000467EF
		public bool Value
		{
			get
			{
				return OptionsManager.Load<bool>(this.key, this.defaultValue);
			}
			set
			{
				OptionsManager.Save<bool>(this.key, value);
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001336 RID: 4918 RVA: 0x000485FD File Offset: 0x000467FD
		private int SliderValue
		{
			get
			{
				if (!this.Value)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0004860C File Offset: 0x0004680C
		private void Awake()
		{
			this.toggle.wholeNumbers = true;
			this.toggle.minValue = 0f;
			this.toggle.maxValue = 1f;
			this.toggle.onValueChanged.AddListener(new UnityAction<float>(this.OnToggleValueChanged));
			this.label.text = this.labelKey.ToPlainText();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x00048677 File Offset: 0x00046877
		private void OnEnable()
		{
			this.toggle.SetValueWithoutNotify((float)this.SliderValue);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0004868B File Offset: 0x0004688B
		private void OnToggleValueChanged(float value)
		{
			this.Value = (value > 0f);
		}

		// Token: 0x04000E62 RID: 3682
		[SerializeField]
		private string key;

		// Token: 0x04000E63 RID: 3683
		[SerializeField]
		private bool defaultValue;

		// Token: 0x04000E64 RID: 3684
		[Space]
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x04000E65 RID: 3685
		[SerializeField]
		private Slider toggle;
	}
}
