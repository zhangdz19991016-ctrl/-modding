using System;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200008A RID: 138
[MenuPath("Health/一段时间没受伤")]
public class NotHurtForSeconds : EffectFilter
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0001640B File Offset: 0x0001460B
	public override string DisplayName
	{
		get
		{
			return this.time.ToString() + "秒内没受伤";
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00016422 File Offset: 0x00014622
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

	// Token: 0x060004EA RID: 1258 RVA: 0x0001645C File Offset: 0x0001465C
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		if (!this.binded && this.MainControl)
		{
			this.MainControl.Health.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.binded = true;
		}
		return Time.time - this.lastHurtTime > this.time;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000164BA File Offset: 0x000146BA
	private void OnDestroy()
	{
		if (this.MainControl)
		{
			this.MainControl.Health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x000164EA File Offset: 0x000146EA
	private void OnHurt(DamageInfo dmgInfo)
	{
		this.lastHurtTime = Time.time;
	}

	// Token: 0x0400041F RID: 1055
	public float time;

	// Token: 0x04000420 RID: 1056
	private float lastHurtTime = -9999f;

	// Token: 0x04000421 RID: 1057
	private bool binded;

	// Token: 0x04000422 RID: 1058
	private CharacterMainControl _mainControl;
}
