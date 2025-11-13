using System;
using Duckov.Buildings;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000357 RID: 855
	public class QuestTask_ConstructBuilding : Task
	{
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x0006AD5E File Offset: 0x00068F5E
		[LocalizationKey("Default")]
		private string descriptionFormatKey
		{
			get
			{
				return "Task_ConstructBuilding";
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0006AD65 File Offset: 0x00068F65
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x0006AD72 File Offset: 0x00068F72
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					BuildingName = Building.GetDisplayName(this.buildingID)
				});
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0006AD8F File Offset: 0x00068F8F
		public override object GenerateSaveData()
		{
			return null;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0006AD92 File Offset: 0x00068F92
		protected override bool CheckFinished()
		{
			return BuildingManager.Any(this.buildingID, false);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0006ADA0 File Offset: 0x00068FA0
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x040014A5 RID: 5285
		[SerializeField]
		private string buildingID;
	}
}
