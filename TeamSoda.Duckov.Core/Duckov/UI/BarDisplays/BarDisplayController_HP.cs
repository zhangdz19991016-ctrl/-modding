using System;
using UnityEngine.Events;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003D5 RID: 981
	public class BarDisplayController_HP : BarDisplayController
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x0007D892 File Offset: 0x0007BA92
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.Health.CurrentHealth;
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x0007D8B8 File Offset: 0x0007BAB8
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return 0f;
				}
				return this.Target.Health.MaxHealth;
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x0007D8DE File Offset: 0x0007BADE
		private void OnEnable()
		{
			base.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x0007D8EC File Offset: 0x0007BAEC
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0007D8F4 File Offset: 0x0007BAF4
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.Health.OnHealthChange.AddListener(new UnityAction<Health>(this.OnHealthChange));
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0007D926 File Offset: 0x0007BB26
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.Health.OnHealthChange.RemoveListener(new UnityAction<Health>(this.OnHealthChange));
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x0007D958 File Offset: 0x0007BB58
		private CharacterMainControl Target
		{
			get
			{
				if (this._target == null)
				{
					this._target = CharacterMainControl.Main;
				}
				return this._target;
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x0007D979 File Offset: 0x0007BB79
		private void OnHealthChange(Health health)
		{
			base.Refresh();
		}

		// Token: 0x0400184B RID: 6219
		private CharacterMainControl _target;
	}
}
