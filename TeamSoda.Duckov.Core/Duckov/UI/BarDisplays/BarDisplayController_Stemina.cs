using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003D7 RID: 983
	public class BarDisplayController_Stemina : BarDisplayController
	{
		// Token: 0x060023D4 RID: 9172 RVA: 0x0007DA50 File Offset: 0x0007BC50
		private void Update()
		{
			float num = this.Current;
			float max = this.Max;
			if (this.displayingStemina != num || this.displayingMaxStemina != max)
			{
				base.Refresh();
				this.displayingStemina = num;
				this.displayingMaxStemina = max;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x0007DA91 File Offset: 0x0007BC91
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

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x0007DAB2 File Offset: 0x0007BCB2
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentStamina;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x0007DAD4 File Offset: 0x0007BCD4
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxStamina;
			}
		}

		// Token: 0x0400184F RID: 6223
		private CharacterMainControl _target;

		// Token: 0x04001850 RID: 6224
		private float displayingStemina = -1f;

		// Token: 0x04001851 RID: 6225
		private float displayingMaxStemina = -1f;
	}
}
