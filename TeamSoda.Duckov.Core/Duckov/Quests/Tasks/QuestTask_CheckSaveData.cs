using System;
using Saves;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000356 RID: 854
	public class QuestTask_CheckSaveData : Task
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x0006AD05 File Offset: 0x00068F05
		public string SaveDataKey
		{
			get
			{
				return this.saveDataKey;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x0006AD0D File Offset: 0x00068F0D
		// (set) Token: 0x06001DC2 RID: 7618 RVA: 0x0006AD1A File Offset: 0x00068F1A
		private bool SaveDataTrue
		{
			get
			{
				return SavesSystem.Load<bool>(this.saveDataKey);
			}
			set
			{
				SavesSystem.Save<bool>(this.saveDataKey, value);
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x0006AD28 File Offset: 0x00068F28
		public override string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0006AD35 File Offset: 0x00068F35
		protected override void OnInit()
		{
			base.OnInit();
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0006AD3D File Offset: 0x00068F3D
		private void OnDisable()
		{
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006AD3F File Offset: 0x00068F3F
		protected override bool CheckFinished()
		{
			return this.SaveDataTrue;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006AD47 File Offset: 0x00068F47
		public override object GenerateSaveData()
		{
			return this.SaveDataTrue;
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0006AD54 File Offset: 0x00068F54
		public override void SetupSaveData(object data)
		{
		}

		// Token: 0x040014A3 RID: 5283
		[SerializeField]
		private string saveDataKey;

		// Token: 0x040014A4 RID: 5284
		[SerializeField]
		[LocalizationKey("Quests")]
		private string description;
	}
}
