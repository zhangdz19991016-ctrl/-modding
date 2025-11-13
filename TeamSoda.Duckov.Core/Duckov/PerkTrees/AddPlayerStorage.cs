using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000252 RID: 594
	public class AddPlayerStorage : PerkBehaviour
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x00047221 File Offset: 0x00045421
		private string DescriptionFormat
		{
			get
			{
				return "PerkBehaviour_AddPlayerStorage".ToPlainText();
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0004722D File Offset: 0x0004542D
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.addCapacity
				});
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00047245 File Offset: 0x00045445
		protected override void OnAwake()
		{
			PlayerStorage.OnRecalculateStorageCapacity += this.OnRecalculatePlayerStorage;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00047258 File Offset: 0x00045458
		protected override void OnOnDestroy()
		{
			PlayerStorage.OnRecalculateStorageCapacity -= this.OnRecalculatePlayerStorage;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0004726B File Offset: 0x0004546B
		private void OnRecalculatePlayerStorage(PlayerStorage.StorageCapacityCalculationHolder holder)
		{
			if (base.Master.Unlocked)
			{
				holder.capacity += this.addCapacity;
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0004728D File Offset: 0x0004548D
		protected override void OnUnlocked()
		{
			base.OnUnlocked();
			PlayerStorage.NotifyCapacityDirty();
		}

		// Token: 0x04000E3C RID: 3644
		[SerializeField]
		private int addCapacity;
	}
}
