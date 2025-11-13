using System;
using ItemStatsSystem;

// Token: 0x02000089 RID: 137
[MenuPath("弱属性")]
public class ElementFactorFilter : EffectFilter
{
	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00016324 File Offset: 0x00014524
	public override string DisplayName
	{
		get
		{
			return string.Format("如果{0}系数{1}{2}", this.element, (this.type == ElementFactorFilter.ElementFactorFilterTypes.GreaterThan) ? "大于" : "小于", this.compareTo);
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060004E4 RID: 1252 RVA: 0x0001635A File Offset: 0x0001455A
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00016394 File Offset: 0x00014594
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		if (!this.MainControl)
		{
			return false;
		}
		if (!this.MainControl.Health)
		{
			return false;
		}
		float num = this.MainControl.Health.ElementFactor(this.element);
		if (this.type != ElementFactorFilter.ElementFactorFilterTypes.GreaterThan)
		{
			return num < this.compareTo;
		}
		return num > this.compareTo;
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x000163F6 File Offset: 0x000145F6
	private void OnDestroy()
	{
	}

	// Token: 0x0400041B RID: 1051
	public ElementFactorFilter.ElementFactorFilterTypes type;

	// Token: 0x0400041C RID: 1052
	public float compareTo = 1f;

	// Token: 0x0400041D RID: 1053
	public ElementTypes element;

	// Token: 0x0400041E RID: 1054
	private CharacterMainControl _mainControl;

	// Token: 0x02000447 RID: 1095
	public enum ElementFactorFilterTypes
	{
		// Token: 0x04001AB6 RID: 6838
		GreaterThan,
		// Token: 0x04001AB7 RID: 6839
		LessThan
	}
}
