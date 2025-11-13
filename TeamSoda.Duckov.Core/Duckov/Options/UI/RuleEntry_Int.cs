using System;
using System.Reflection;
using Duckov.Rules;
using Duckov.UI;
using UnityEngine;

namespace Duckov.Options.UI
{
	// Token: 0x02000267 RID: 615
	public class RuleEntry_Int : MonoBehaviour
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x000487AC File Offset: 0x000469AC
		private void Awake()
		{
			SliderWithTextField sliderWithTextField = this.slider;
			sliderWithTextField.onValueChanged = (Action<float>)Delegate.Combine(sliderWithTextField.onValueChanged, new Action<float>(this.OnValueChanged));
			GameRulesManager.OnRuleChanged += this.OnRuleChanged;
			Type typeFromHandle = typeof(Ruleset);
			this.field = typeFromHandle.GetField(this.fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			this.RefreshValue();
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00048816 File Offset: 0x00046A16
		private void OnRuleChanged()
		{
			this.RefreshValue();
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x00048820 File Offset: 0x00046A20
		private void OnValueChanged(float value)
		{
			if (GameRulesManager.SelectedRuleIndex != RuleIndex.Custom)
			{
				this.RefreshValue();
				return;
			}
			Ruleset ruleset = GameRulesManager.Current;
			this.SetValue(ruleset, (int)value);
			GameRulesManager.NotifyRuleChanged();
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x00048850 File Offset: 0x00046A50
		public void RefreshValue()
		{
			float valueWithoutNotify = (float)this.GetValue(GameRulesManager.Current);
			this.slider.SetValueWithoutNotify(valueWithoutNotify);
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x00048876 File Offset: 0x00046A76
		protected void SetValue(Ruleset ruleset, int value)
		{
			this.field.SetValue(ruleset, value);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0004888A File Offset: 0x00046A8A
		protected int GetValue(Ruleset ruleset)
		{
			return (int)this.field.GetValue(ruleset);
		}

		// Token: 0x04000E69 RID: 3689
		[SerializeField]
		private SliderWithTextField slider;

		// Token: 0x04000E6A RID: 3690
		[SerializeField]
		private string fieldName = "damageFactor_ToPlayer";

		// Token: 0x04000E6B RID: 3691
		private FieldInfo field;
	}
}
