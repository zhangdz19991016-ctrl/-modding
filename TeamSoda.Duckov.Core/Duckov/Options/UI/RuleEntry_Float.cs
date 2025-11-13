using System;
using System.Reflection;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;

namespace Duckov.Options.UI
{
	// Token: 0x02000266 RID: 614
	public class RuleEntry_Float : MonoBehaviour
	{
		// Token: 0x0600133B RID: 4923 RVA: 0x000486A8 File Offset: 0x000468A8
		private void Awake()
		{
			SliderWithTextField sliderWithTextField = this.slider;
			sliderWithTextField.onValueChanged = (Action<float>)Delegate.Combine(sliderWithTextField.onValueChanged, new Action<float>(this.OnValueChanged));
			GameRulesManager.OnRuleChanged += this.OnRuleChanged;
			Type typeFromHandle = typeof(Ruleset);
			this.field = typeFromHandle.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			this.RefreshValue();
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00048712 File Offset: 0x00046912
		private void OnRuleChanged()
		{
			this.RefreshValue();
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0004871C File Offset: 0x0004691C
		private void OnValueChanged(float value)
		{
			if (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom)
			{
				this.RefreshValue();
				return;
			}
			Ruleset ruleset = GameRulesManager.Current;
			this.SetValue(ruleset, value);
			GameRulesManager.NotifyRuleChanged();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0004874C File Offset: 0x0004694C
		public void RefreshValue()
		{
			float value = this.GetValue(GameRulesManager.Current);
			this.slider.SetValueWithoutNotify(value);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00048771 File Offset: 0x00046971
		protected void SetValue(Ruleset ruleset, float value)
		{
			this.field.SetValue(ruleset, value);
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x00048785 File Offset: 0x00046985
		protected float GetValue(Ruleset ruleset)
		{
			return (float)this.field.GetValue(ruleset);
		}

		// Token: 0x04000E66 RID: 3686
		[SerializeField]
		private SliderWithTextField slider;

		// Token: 0x04000E67 RID: 3687
		[SerializeField]
		private string fieldName = "damageFactor_ToPlayer";

		// Token: 0x04000E68 RID: 3688
		private FieldInfo field;
	}
}
