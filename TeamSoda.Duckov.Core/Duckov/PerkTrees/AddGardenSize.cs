using System;
using Duckov.Crops;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000251 RID: 593
	public class AddGardenSize : PerkBehaviour, IGardenSizeAdder
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x0004719B File Offset: 0x0004539B
		public override string Description
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText().Format(new
				{
					addX = this.add.x,
					addY = this.add.y
				});
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x000471C8 File Offset: 0x000453C8
		protected override void OnUnlocked()
		{
			Garden.Register(this);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x000471D0 File Offset: 0x000453D0
		protected override void OnOnDestroy()
		{
			Garden.Unregister(this);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x000471D8 File Offset: 0x000453D8
		public Vector2Int GetValue(string gardenID)
		{
			if (gardenID != this.gardenID)
			{
				return default(Vector2Int);
			}
			return this.add;
		}

		// Token: 0x04000E39 RID: 3641
		[LocalizationKey("Default")]
		[SerializeField]
		private string descriptionFormatKey = "PerkBehaviour_AddGardenSize";

		// Token: 0x04000E3A RID: 3642
		[SerializeField]
		private string gardenID = "Default";

		// Token: 0x04000E3B RID: 3643
		[SerializeField]
		private Vector2Int add;
	}
}
