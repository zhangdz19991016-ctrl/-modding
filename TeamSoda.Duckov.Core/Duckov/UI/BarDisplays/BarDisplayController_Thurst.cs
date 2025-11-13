using System;

namespace Duckov.UI.BarDisplays
{
	// Token: 0x020003D8 RID: 984
	public class BarDisplayController_Thurst : BarDisplayController
	{
		// Token: 0x060023D9 RID: 9177 RVA: 0x0007DB14 File Offset: 0x0007BD14
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

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x0007DB55 File Offset: 0x0007BD55
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

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060023DB RID: 9179 RVA: 0x0007DB76 File Offset: 0x0007BD76
		protected override float Current
		{
			get
			{
				if (this.Target == null)
				{
					return base.Current;
				}
				return this.Target.CurrentWater;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x0007DB98 File Offset: 0x0007BD98
		protected override float Max
		{
			get
			{
				if (this.Target == null)
				{
					return base.Max;
				}
				return this.Target.MaxWater;
			}
		}

		// Token: 0x04001852 RID: 6226
		private CharacterMainControl _target;

		// Token: 0x04001853 RID: 6227
		private float displayingCurrent = -1f;

		// Token: 0x04001854 RID: 6228
		private float displayingMax = -1f;
	}
}
