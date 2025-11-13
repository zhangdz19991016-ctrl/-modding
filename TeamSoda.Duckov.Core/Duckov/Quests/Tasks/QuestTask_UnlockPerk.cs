using System;
using System.Collections.Generic;
using Duckov.PerkTrees;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200035D RID: 861
	public class QuestTask_UnlockPerk : Task
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x0006B77A File Offset: 0x0006997A
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001E2C RID: 7724 RVA: 0x0006B787 File Offset: 0x00069987
		private string PerkDisplayName
		{
			get
			{
				if (this.perk == null)
				{
					this.BindPerk();
				}
				if (this.perk == null)
				{
					return this.perkObjectName.ToPlainText();
				}
				return this.perk.DisplayName;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x0006B7C3 File Offset: 0x000699C3
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.PerkDisplayName
				});
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001E2E RID: 7726 RVA: 0x0006B7DB File Offset: 0x000699DB
		public override Sprite Icon
		{
			get
			{
				if (this.perk != null)
				{
					return this.perk.Icon;
				}
				return null;
			}
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0006B7F8 File Offset: 0x000699F8
		protected override void OnInit()
		{
			if (LevelManager.LevelInited)
			{
				this.BindPerk();
				return;
			}
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0006B81C File Offset: 0x00069A1C
		private bool BindPerk()
		{
			if (this.perk)
			{
				if (!this.unlocked && this.perk.Unlocked)
				{
					this.OnPerkUnlockStateChanged(this.perk, true);
				}
				return false;
			}
			PerkTree perkTree = PerkTreeManager.GetPerkTree(this.perkTreeID);
			if (perkTree)
			{
				using (List<Perk>.Enumerator enumerator = perkTree.perks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Perk perk = enumerator.Current;
						if (perk.gameObject.name == this.perkObjectName)
						{
							this.perk = perk;
							if (this.perk.Unlocked)
							{
								this.OnPerkUnlockStateChanged(this.perk, true);
							}
							this.perk.onUnlockStateChanged += this.OnPerkUnlockStateChanged;
							return true;
						}
					}
					goto IL_E6;
				}
			}
			Debug.LogError("PerkTree Not Found " + this.perkTreeID, base.gameObject);
			IL_E6:
			Debug.LogError("Perk Not Found: " + this.perkTreeID + "/" + this.perkObjectName, base.gameObject);
			return false;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0006B948 File Offset: 0x00069B48
		private void OnPerkUnlockStateChanged(Perk _perk, bool _unlocked)
		{
			if (base.Master.Complete)
			{
				return;
			}
			if (_unlocked)
			{
				this.unlocked = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0006B968 File Offset: 0x00069B68
		private void OnDestroy()
		{
			if (this.perk)
			{
				this.perk.onUnlockStateChanged -= this.OnPerkUnlockStateChanged;
			}
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0006B99F File Offset: 0x00069B9F
		private void OnLevelInitialized()
		{
			this.BindPerk();
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0006B9A8 File Offset: 0x00069BA8
		public override object GenerateSaveData()
		{
			return this.unlocked;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0006B9B5 File Offset: 0x00069BB5
		protected override bool CheckFinished()
		{
			return this.unlocked;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0006B9C0 File Offset: 0x00069BC0
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.unlocked = flag;
			}
		}

		// Token: 0x040014C0 RID: 5312
		[SerializeField]
		private string perkTreeID;

		// Token: 0x040014C1 RID: 5313
		[SerializeField]
		private string perkObjectName;

		// Token: 0x040014C2 RID: 5314
		private Perk perk;

		// Token: 0x040014C3 RID: 5315
		[NonSerialized]
		private bool unlocked;

		// Token: 0x040014C4 RID: 5316
		private string descriptionFormatKey = "Task_UnlockPerk";
	}
}
