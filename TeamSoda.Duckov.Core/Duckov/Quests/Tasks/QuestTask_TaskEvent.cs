using System;
using SodaCraft.Localizations;
using UnityEngine;

namespace Duckov.Quests.Tasks
{
	// Token: 0x0200035C RID: 860
	public class QuestTask_TaskEvent : Task
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0006B675 File Offset: 0x00069875
		public string EventKey
		{
			get
			{
				return this.eventKey;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001E22 RID: 7714 RVA: 0x0006B67D File Offset: 0x0006987D
		public override string Description
		{
			get
			{
				return this.description.ToPlainText();
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0006B68A File Offset: 0x0006988A
		private void OnTaskEvent(string _key)
		{
			if (_key == this.eventKey)
			{
				this.finished = true;
				this.SetMapElementVisable(false);
				base.ReportStatusChanged();
			}
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0006B6AE File Offset: 0x000698AE
		protected override void OnInit()
		{
			base.OnInit();
			TaskEvent.OnTaskEvent += this.OnTaskEvent;
			this.SetMapElementVisable(!base.IsFinished());
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0006B6D6 File Offset: 0x000698D6
		private void OnDisable()
		{
			TaskEvent.OnTaskEvent -= this.OnTaskEvent;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0006B6E9 File Offset: 0x000698E9
		protected override bool CheckFinished()
		{
			return this.finished;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x0006B6F1 File Offset: 0x000698F1
		public override object GenerateSaveData()
		{
			return this.finished;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0006B700 File Offset: 0x00069900
		public override void SetupSaveData(object data)
		{
			if (data is bool)
			{
				bool flag = (bool)data;
				this.finished = flag;
			}
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0006B724 File Offset: 0x00069924
		private void SetMapElementVisable(bool visable)
		{
			if (!this.mapElement)
			{
				return;
			}
			if (!this.mapElement.enabled)
			{
				return;
			}
			if (visable)
			{
				this.mapElement.name = base.Master.DisplayName;
			}
			this.mapElement.SetVisibility(visable);
		}

		// Token: 0x040014BC RID: 5308
		[SerializeField]
		private string eventKey;

		// Token: 0x040014BD RID: 5309
		[SerializeField]
		[LocalizationKey("Quests")]
		private string description;

		// Token: 0x040014BE RID: 5310
		private bool finished;

		// Token: 0x040014BF RID: 5311
		[SerializeField]
		private MapElementForTask mapElement;
	}
}
