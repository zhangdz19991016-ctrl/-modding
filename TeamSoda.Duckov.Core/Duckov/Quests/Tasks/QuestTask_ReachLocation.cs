using System;
using Duckov.Scenes;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200035A RID: 858
	public class QuestTask_ReachLocation : Task
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x0006B392 File Offset: 0x00069592
		public string descriptionFormatkey
		{
			get
			{
				return "Task_ReachLocation";
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x0006B399 File Offset: 0x00069599
		public string DescriptionFormat
		{
			get
			{
				return this.descriptionFormatkey.ToPlainText();
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x0006B3A6 File Offset: 0x000695A6
		public string TargetLocationDisplayName
		{
			get
			{
				return this.location.GetDisplayName();
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0006B3B3 File Offset: 0x000695B3
		public override string Description
		{
			get
			{
				return this.DescriptionFormat.Format(new
				{
					this.TargetLocationDisplayName
				});
			}
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0006B3CB File Offset: 0x000695CB
		private void OnEnable()
		{
			SceneLoader.onFinishedLoadingScene += this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x0006B3EF File Offset: 0x000695EF
		private void Start()
		{
			this.CacheLocation();
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x0006B3F7 File Offset: 0x000695F7
		private void OnDisable()
		{
			SceneLoader.onFinishedLoadingScene -= this.OnFinishedLoadingScene;
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0006B41B File Offset: 0x0006961B
		protected override void OnInit()
		{
			base.OnInit();
			if (!base.IsFinished())
			{
				this.SetMapElementVisable(true);
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0006B432 File Offset: 0x00069632
		private void OnFinishedLoadingScene(SceneLoadingContext context)
		{
			this.CacheLocation();
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0006B43A File Offset: 0x0006963A
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Reach location task caching";
			this.CacheLocation();
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0006B44C File Offset: 0x0006964C
		private void CacheLocation()
		{
			this.target = this.location.GetLocationTransform();
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0006B460 File Offset: 0x00069660
		private void Update()
		{
			if (this.finished)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			if ((main.transform.position - this.target.position).magnitude <= this.radius)
			{
				this.finished = true;
				this.SetMapElementVisable(false);
			}
			base.ReportStatusChanged();
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0006B4D4 File Offset: 0x000696D4
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0006B4E1 File Offset: 0x000696E1
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0006B4EC File Offset: 0x000696EC
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0006B510 File Offset: 0x00069710
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.locations.Clear();
				this.mapElement.locations.Add(this.location);
				this.mapElement.range = this.radius;
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x040014B3 RID: 5299
		[SerializeField]
		private MultiSceneLocation location;

		// Token: 0x040014B4 RID: 5300
		[SerializeField]
		private float radius = 1f;

		// Token: 0x040014B5 RID: 5301
		[SerializeField]
		private bool finished;

		// Token: 0x040014B6 RID: 5302
		[SerializeField]
		private Transform target;

		// Token: 0x040014B7 RID: 5303
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
