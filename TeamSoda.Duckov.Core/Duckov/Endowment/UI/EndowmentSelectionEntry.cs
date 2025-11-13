using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Endowment.UI
{
	// Token: 0x020002FC RID: 764
	public class EndowmentSelectionEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x0005B4EF File Offset: 0x000596EF
		public string DisplayName
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.DisplayName;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x0005B510 File Offset: 0x00059710
		public string Description
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.Description;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060018F9 RID: 6393 RVA: 0x0005B531 File Offset: 0x00059731
		public string DescriptionAndEffects
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.DescriptionAndEffects;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x0005B552 File Offset: 0x00059752
		public EndowmentIndex Index
		{
			get
			{
				if (this.Target == null)
				{
					return EndowmentIndex.None;
				}
				return this.Target.Index;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x0005B56F File Offset: 0x0005976F
		// (set) Token: 0x060018FC RID: 6396 RVA: 0x0005B577 File Offset: 0x00059777
		public EndowmentEntry Target { get; private set; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x0005B580 File Offset: 0x00059780
		// (set) Token: 0x060018FE RID: 6398 RVA: 0x0005B588 File Offset: 0x00059788
		public bool Selected { get; private set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x0005B591 File Offset: 0x00059791
		public bool Unlocked
		{
			get
			{
				return EndowmentManager.GetEndowmentUnlocked(this.Index);
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x0005B59E File Offset: 0x0005979E
		public bool Locked
		{
			get
			{
				return !this.Unlocked;
			}
		}

		// Token: 0x06001901 RID: 6401 RVA: 0x0005B5AC File Offset: 0x000597AC
		public void Setup(EndowmentEntry target)
		{
			this.Target = target;
			if (this.Target == null)
			{
				return;
			}
			this.displayNameText.text = this.Target.DisplayName;
			this.icon.sprite = this.Target.Icon;
			this.requirementText.text = "- " + this.Target.RequirementText + " -";
			this.Refresh();
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x0005B626 File Offset: 0x00059826
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.selectedIndicator.SetActive(this.Selected);
			this.lockedIndcator.SetActive(this.Locked);
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x0005B659 File Offset: 0x00059859
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.Locked)
			{
				return;
			}
			Action<EndowmentSelectionEntry, PointerEventData> action = this.onClicked;
			if (action == null)
			{
				return;
			}
			action(this, eventData);
		}

		// Token: 0x06001904 RID: 6404 RVA: 0x0005B676 File Offset: 0x00059876
		internal void SetSelection(bool value)
		{
			this.Selected = value;
			this.Refresh();
		}

		// Token: 0x04001222 RID: 4642
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001223 RID: 4643
		[SerializeField]
		private Image icon;

		// Token: 0x04001224 RID: 4644
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001225 RID: 4645
		[SerializeField]
		private GameObject lockedIndcator;

		// Token: 0x04001226 RID: 4646
		[SerializeField]
		private TextMeshProUGUI requirementText;

		// Token: 0x04001227 RID: 4647
		public Action<EndowmentSelectionEntry, PointerEventData> onClicked;
	}
}
