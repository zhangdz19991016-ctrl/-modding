using System;
using Duckov.Scenes;
using Eflatun.SceneReference;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x02000358 RID: 856
	public class QuestTask_Evacuate : Task
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x0006ADAA File Offset: 0x00068FAA
		private SceneInfoEntry RequireSceneInfo
		{
			get
			{
				return SceneInfoCollection.GetSceneInfo(this.requireSceneID);
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x0006ADB8 File Offset: 0x00068FB8
		private SceneReference RequireScene
		{
			get
			{
				SceneInfoEntry requireSceneInfo = this.RequireSceneInfo;
				if (requireSceneInfo == null)
				{
					return null;
				}
				return requireSceneInfo.SceneReference;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x0006ADD7 File Offset: 0x00068FD7
		private string descriptionFormatKey
		{
			get
			{
				return "Task_Evacuate";
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x0006ADDE File Offset: 0x00068FDE
		private string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatKey.ToPlainText();
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001DD5 RID: 7637 RVA: 0x0006ADEC File Offset: 0x00068FEC
		private string TargetDisplayName
		{
			get
			{
				if (this.RequireScene != null && this.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.None)
				{
					return this.RequireSceneInfo.DisplayName;
				}
				if (base.Master.RequireScene != null && base.Master.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.None)
				{
					return base.Master.RequireSceneInfo.DisplayName;
				}
				return "Scene_Any".ToPlainText();
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0006AE54 File Offset: 0x00069054
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.TargetDisplayName
				});
			}
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x0006AE6C File Offset: 0x0006906C
		private void OnEnable()
		{
			LevelManager.OnEvacuated += this.OnEvacuated;
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x0006AE7F File Offset: 0x0006907F
		private void OnDisable()
		{
			LevelManager.OnEvacuated -= this.OnEvacuated;
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0006AE94 File Offset: 0x00069094
		private void OnEvacuated(EvacuationInfo info)
		{
			if (this.finished)
			{
				return;
			}
			if (this.RequireScene == null || this.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.Empty)
			{
				if (base.Master.SceneRequirementSatisfied)
				{
					this.finished = true;
					base.ReportStatusChanged();
					return;
				}
			}
			else if (this.RequireScene.UnsafeReason == SceneReferenceUnsafeReason.None && this.RequireScene.LoadedScene.isLoaded)
			{
				this.finished = true;
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0006AF0A File Offset: 0x0006910A
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0006AF17 File Offset: 0x00069117
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0006AF20 File Offset: 0x00069120
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x040014A6 RID: 5286
		[SerializeField]
		[SceneID]
		private string requireSceneID;

		// Token: 0x040014A7 RID: 5287
		[SerializeField]
		private bool finished;
	}
}
