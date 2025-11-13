using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Buildings.UI
{
	// Token: 0x0200031F RID: 799
	public class BuildingBtnEntry : MonoBehaviour
	{
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x0006108A File Offset: 0x0005F28A
		private string TokenFormat
		{
			get
			{
				return this.tokenFormatKey.ToPlainText();
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x00061097 File Offset: 0x0005F297
		public BuildingInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x0006109F File Offset: 0x0005F29F
		public bool CostEnough
		{
			get
			{
				return this.info.TokenAmount > 0 || this.info.cost.Enough;
			}
		}

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06001AB2 RID: 6834 RVA: 0x000610C8 File Offset: 0x0005F2C8
		// (remove) Token: 0x06001AB3 RID: 6835 RVA: 0x00061100 File Offset: 0x0005F300
		public event Action<BuildingBtnEntry> onButtonClicked;

		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06001AB4 RID: 6836 RVA: 0x00061138 File Offset: 0x0005F338
		// (remove) Token: 0x06001AB5 RID: 6837 RVA: 0x00061170 File Offset: 0x0005F370
		public event Action<BuildingBtnEntry> onRecycleRequested;

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000611A5 File Offset: 0x0005F3A5
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			this.recycleButton.onPressFullfilled.AddListener(new UnityAction(this.OnRecycleButtonTriggered));
		}

		// Token: 0x06001AB7 RID: 6839 RVA: 0x000611DF File Offset: 0x0005F3DF
		private void OnRecycleButtonTriggered()
		{
			Action<BuildingBtnEntry> action = this.onRecycleRequested;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000611F2 File Offset: 0x0005F3F2
		private void OnEnable()
		{
			BuildingManager.OnBuildingListChanged += this.Refresh;
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00061205 File Offset: 0x0005F405
		private void OnDisable()
		{
			BuildingManager.OnBuildingListChanged -= this.Refresh;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00061218 File Offset: 0x0005F418
		private void OnButtonClicked()
		{
			Action<BuildingBtnEntry> action = this.onButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x0006122B File Offset: 0x0005F42B
		internal void Setup(BuildingInfo buildingInfo)
		{
			this.info = buildingInfo;
			this.Refresh();
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0006123C File Offset: 0x0005F43C
		private void Refresh()
		{
			int tokenAmount = this.info.TokenAmount;
			this.nameText.text = this.info.DisplayName;
			this.descriptionText.text = this.info.Description;
			this.tokenText.text = this.TokenFormat.Format(new
			{
				tokenAmount
			});
			this.icon.sprite = this.info.iconReference;
			this.costDisplay.Setup(this.info.cost, 1);
			this.costDisplay.gameObject.SetActive(tokenAmount <= 0);
			bool reachedAmountLimit = this.info.ReachedAmountLimit;
			this.amountText.text = ((this.info.maxAmount > 0) ? string.Format("{0}/{1}", this.info.CurrentAmount, this.info.maxAmount) : string.Format("{0}/∞", this.info.CurrentAmount));
			this.reachedAmountLimitationIndicator.SetActive(reachedAmountLimit);
			bool flag = !this.info.ReachedAmountLimit && this.CostEnough;
			this.backGround.color = (flag ? this.avaliableColor : this.normalColor);
			this.recycleButton.gameObject.SetActive(this.info.CurrentAmount > 0);
		}

		// Token: 0x0400131A RID: 4890
		[SerializeField]
		private Button button;

		// Token: 0x0400131B RID: 4891
		[SerializeField]
		private Image icon;

		// Token: 0x0400131C RID: 4892
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x0400131D RID: 4893
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x0400131E RID: 4894
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x0400131F RID: 4895
		[SerializeField]
		private LongPressButton recycleButton;

		// Token: 0x04001320 RID: 4896
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x04001321 RID: 4897
		[SerializeField]
		[LocalizationKey("Default")]
		private string tokenFormatKey;

		// Token: 0x04001322 RID: 4898
		[SerializeField]
		private TextMeshProUGUI tokenText;

		// Token: 0x04001323 RID: 4899
		[SerializeField]
		private GameObject reachedAmountLimitationIndicator;

		// Token: 0x04001324 RID: 4900
		[SerializeField]
		private Image backGround;

		// Token: 0x04001325 RID: 4901
		[SerializeField]
		private Color normalColor;

		// Token: 0x04001326 RID: 4902
		[SerializeField]
		private Color avaliableColor;

		// Token: 0x04001327 RID: 4903
		private BuildingInfo info;
	}
}
