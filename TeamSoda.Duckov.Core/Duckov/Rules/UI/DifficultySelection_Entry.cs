using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Rules.UI
{
	// Token: 0x020003FC RID: 1020
	public class DifficultySelection_Entry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060024FB RID: 9467 RVA: 0x00080A9F File Offset: 0x0007EC9F
		// (set) Token: 0x060024FC RID: 9468 RVA: 0x00080AA7 File Offset: 0x0007ECA7
		public DifficultySelection Master { get; private set; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060024FD RID: 9469 RVA: 0x00080AB0 File Offset: 0x0007ECB0
		// (set) Token: 0x060024FE RID: 9470 RVA: 0x00080AB8 File Offset: 0x0007ECB8
		public DifficultySelection.SettingEntry Setting { get; private set; }

		// Token: 0x060024FF RID: 9471 RVA: 0x00080AC1 File Offset: 0x0007ECC1
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.locked)
			{
				return;
			}
			this.Master.NotifySelected(this);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x00080AD8 File Offset: 0x0007ECD8
		public void OnPointerEnter(PointerEventData eventData)
		{
			DifficultySelection master = this.Master;
			if (master != null)
			{
				master.NotifyEntryPointerEnter(this);
			}
			Action<DifficultySelection_Entry> action = this.onPointerEnter;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x00080AFD File Offset: 0x0007ECFD
		public void OnPointerExit(PointerEventData eventData)
		{
			DifficultySelection master = this.Master;
			if (master != null)
			{
				master.NotifyEntryPointerExit(this);
			}
			Action<DifficultySelection_Entry> action = this.onPointerExit;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x00080B22 File Offset: 0x0007ED22
		internal void Refresh()
		{
			if (this.Master == null)
			{
				return;
			}
			this.selectedIndicator.SetActive(this.Master.SelectedRuleIndex == this.Setting.ruleIndex);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00080B58 File Offset: 0x0007ED58
		internal void Setup(DifficultySelection master, DifficultySelection.SettingEntry setting, bool locked)
		{
			this.Master = master;
			this.Setting = setting;
			this.title.text = setting.Title;
			this.icon.sprite = setting.icon;
			this.recommendationIndicator.SetActive(setting.recommended);
			this.locked = locked;
			this.lockedIndicator.SetActive(locked);
			this.Refresh();
		}

		// Token: 0x04001923 RID: 6435
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001924 RID: 6436
		[SerializeField]
		private Image icon;

		// Token: 0x04001925 RID: 6437
		[SerializeField]
		private GameObject recommendationIndicator;

		// Token: 0x04001926 RID: 6438
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001927 RID: 6439
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x04001928 RID: 6440
		internal Action<DifficultySelection_Entry> onPointerEnter;

		// Token: 0x04001929 RID: 6441
		internal Action<DifficultySelection_Entry> onPointerExit;

		// Token: 0x0400192C RID: 6444
		private bool locked;
	}
}
