using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003D6 RID: 982
	public class BarDisplayController_Hunger : BarDisplayController
	{
		// Token: 0x060023CF RID: 9167 RVA: 0x0007D98C File Offset: 0x0007BB8C
		private void Update()
		{
			float num = this.Current;
			float max = this.Max;
			if (this.displayingCurrent != num || this.displayingMax != max)
			{
				base.Refresh();
				this.displayingCurrent = num;
				this.displayingMax = max;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x0007D9CD File Offset: 0x0007BBCD
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

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x0007D9EE File Offset: 0x0007BBEE
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentEnergy;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x0007DA10 File Offset: 0x0007BC10
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxEnergy;
			}
		}

		// Token: 0x0400184C RID: 6220
		private CharacterMainControl _target;

		// Token: 0x0400184D RID: 6221
		private float displayingCurrent = -1f;

		// Token: 0x0400184E RID: 6222
		private float displayingMax = -1f;
	}
}
