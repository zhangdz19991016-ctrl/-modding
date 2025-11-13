using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Quests.UI
{
	// Token: 0x0200034A RID: 842
	public class QuestEntry : MonoBehaviour, IPoolable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x00069194 File Offset: 0x00067394
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06001D22 RID: 7458 RVA: 0x0006919C File Offset: 0x0006739C
		// (remove) Token: 0x06001D23 RID: 7459 RVA: 0x000691D4 File Offset: 0x000673D4
		public event Action<QuestEntry, PointerEventData> onClick;

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001D24 RID: 7460 RVA: 0x00069209 File Offset: 0x00067409
		public bool Selected
		{
			get
			{
				return this.menu.GetSelection() == this;
			}
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0006921C File Offset: 0x0006741C
		public void NotifyPooled()
		{
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x0006921E File Offset: 0x0006741E
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x0006922D File Offset: 0x0006742D
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00069235 File Offset: 0x00067435
		internal void Setup(Quest quest)
		{
			this.UnregisterEvents();
			this.target = quest;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00069250 File Offset: 0x00067450
		internal void SetMenu(ISingleSelectionMenu<QuestEntry> menu)
		{
			this.menu = menu;
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00069259 File Offset: 0x00067459
		private void RegisterEvents()
		{
			if (this.target != null)
			{
				this.target.onStatusChanged += this.OnTargetStatusChanged;
				this.target.onNeedInspectionChanged += this.OnNeedInspectionChanged;
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00069297 File Offset: 0x00067497
		private void UnregisterEvents()
		{
			if (this.target != null)
			{
				this.target.onStatusChanged -= this.OnTargetStatusChanged;
				this.target.onNeedInspectionChanged -= this.OnNeedInspectionChanged;
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000692D5 File Offset: 0x000674D5
		private void OnNeedInspectionChanged(Quest obj)
		{
			this.Refresh();
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000692DD File Offset: 0x000674DD
		private void OnTargetStatusChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000692E5 File Offset: 0x000674E5
		private void OnMasterSelectionChanged(QuestView view, Quest oldSelection, Quest newSelection)
		{
			this.Refresh();
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000692F0 File Offset: 0x000674F0
		private void Refresh()
		{
			this.selectionIndicator.SetActive(this.Selected);
			this.displayName.text = this.target.DisplayName;
			this.questIDDisplay.text = string.Format("{0:0000}", this.target.ID);
			SceneInfoEntry requireSceneInfo = this.target.RequireSceneInfo;
			if (requireSceneInfo == null)
			{
				this.locationName.text = this.anyLocationKey.ToPlainText();
			}
			else
			{
				this.locationName.text = requireSceneInfo.DisplayName;
			}
			this.redDot.SetActive(this.target.NeedInspection);
			this.claimableIndicator.SetActive(this.target.Complete || this.target.AreTasksFinished());
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000693BD File Offset: 0x000675BD
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<QuestEntry, PointerEventData> action = this.onClick;
			if (action != null)
			{
				action(this, eventData);
			}
			this.menu.SetSelection(this);
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000693DF File Offset: 0x000675DF
		public void NotifyRefresh()
		{
			this.Refresh();
		}

		// Token: 0x04001432 RID: 5170
		private ISingleSelectionMenu<QuestEntry> menu;

		// Token: 0x04001433 RID: 5171
		private Quest target;

		// Token: 0x04001434 RID: 5172
		[SerializeField]
		private GameObject selectionIndicator;

		// Token: 0x04001435 RID: 5173
		[SerializeField]
		private TextMeshProUGUI displayName;

		// Token: 0x04001436 RID: 5174
		[SerializeField]
		private TextMeshProUGUI locationName;

		// Token: 0x04001437 RID: 5175
		[SerializeField]
		[LocalizationKey("Default")]
		private string anyLocationKey;

		// Token: 0x04001438 RID: 5176
		[SerializeField]
		private GameObject redDot;

		// Token: 0x04001439 RID: 5177
		[SerializeField]
		private GameObject claimableIndicator;

		// Token: 0x0400143A RID: 5178
		[SerializeField]
		private TextMeshProUGUI questIDDisplay;
	}
}
