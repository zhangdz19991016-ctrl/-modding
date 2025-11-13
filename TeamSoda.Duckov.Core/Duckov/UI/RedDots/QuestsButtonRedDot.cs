using System;
using Duckov.Quests;
using UnityEngine;

namespace Duckov.UI.RedDots
{
	// Token: 0x020003EF RID: 1007
	public class QuestsButtonRedDot : MonoBehaviour
	{
		// Token: 0x0600248C RID: 9356 RVA: 0x0007F9E7 File Offset: 0x0007DBE7
		private void Awake()
		{
			Quest.onQuestNeedInspectionChanged += this.OnQuestNeedInspectionChanged;
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x0007F9FA File Offset: 0x0007DBFA
		private void OnDestroy()
		{
			Quest.onQuestNeedInspectionChanged -= this.OnQuestNeedInspectionChanged;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0007FA0D File Offset: 0x0007DC0D
		private void OnQuestNeedInspectionChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0007FA15 File Offset: 0x0007DC15
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x0007FA1D File Offset: 0x0007DC1D
		private void Refresh()
		{
			this.dot.SetActive(QuestManager.AnyQuestNeedsInspection);
		}

		// Token: 0x040018CA RID: 6346
		public GameObject dot;
	}
}
