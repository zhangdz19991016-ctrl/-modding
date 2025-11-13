using System;
using ItemStatsSystem;

// Token: 0x02000088 RID: 136
[MenuPath("角色/角色正在奔跑")]
public class CharacterIsRunning : EffectFilter
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060004DE RID: 1246 RVA: 0x000162CC File Offset: 0x000144CC
	public override string DisplayName
	{
		get
		{
			return "角色正在奔跑";
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060004DF RID: 1247 RVA: 0x000162D3 File Offset: 0x000144D3
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

	// Token: 0x060004E0 RID: 1248 RVA: 0x0001630D File Offset: 0x0001450D
	protected override bool OnEvaluate(EffectTriggerEventContext context)
	{
		return this.MainControl.Running;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001631A File Offset: 0x0001451A
	private void OnDestroy()
	{
	}

	// Token: 0x0400041A RID: 1050
	private CharacterMainControl _mainControl;
}
