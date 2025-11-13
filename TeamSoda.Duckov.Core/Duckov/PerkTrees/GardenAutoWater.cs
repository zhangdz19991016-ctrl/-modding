using System;
using Duckov.Crops;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000253 RID: 595
	public class GardenAutoWater : PerkBehaviour, IGardenAutoWaterProvider
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x000472A2 File Offset: 0x000454A2
		public override string Description
		{
			get
			{
				return this.descriptionKey.ToPlainText();
			}
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x000472AF File Offset: 0x000454AF
		protected override void OnUnlocked()
		{
			Garden.Register(this);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000472B7 File Offset: 0x000454B7
		protected override void OnOnDestroy()
		{
			Garden.Unregister(this);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x000472BF File Offset: 0x000454BF
		public bool TakeEffect(string gardenID)
		{
			return gardenID == this.gardenID;
		}

		// Token: 0x04000E3D RID: 3645
		[SerializeField]
		[LocalizationKey("Default")]
		private string descriptionKey = "PerkBehaviour_GardenAutoWater";

		// Token: 0x04000E3E RID: 3646
		[SerializeField]
		private string gardenID = "Default";
	}
}
