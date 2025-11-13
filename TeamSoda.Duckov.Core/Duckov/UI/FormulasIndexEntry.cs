using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038B RID: 907
	public class FormulasIndexEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x0006ECF8 File Offset: 0x0006CEF8
		public CraftingFormula Formula
		{
			get
			{
				return this.formula;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x0006ED00 File Offset: 0x0006CF00
		private int ItemID
		{
			get
			{
				return this.formula.result.id;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x0006ED12 File Offset: 0x0006CF12
		private ItemMetaData Meta
		{
			get
			{
				return ItemAssetsCollection.GetMetaData(this.ItemID);
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x0006ED1F File Offset: 0x0006CF1F
		private bool Unlocked
		{
			get
			{
				return CraftingManager.IsFormulaUnlocked(this.formula.id);
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x0006ED31 File Offset: 0x0006CF31
		public bool Valid
		{
			get
			{
				return this.ItemID >= 0;
			}
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0006ED3F File Offset: 0x0006CF3F
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.OnEntryClicked(this);
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0006ED4D File Offset: 0x0006CF4D
		internal void Setup(FormulasIndexView master, CraftingFormula formula)
		{
			this.master = master;
			this.formula = formula;
			this.Refresh();
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0006ED64 File Offset: 0x0006CF64
		public void Refresh()
		{
			ItemMetaData meta = this.Meta;
			if (!this.Valid)
			{
				this.displayNameText.text = "! " + this.formula.id + " !";
				this.image.sprite = this.lockedImage;
				return;
			}
			if (this.Unlocked)
			{
				this.displayNameText.text = string.Format("{0} x{1}", meta.DisplayName, this.formula.result.amount);
				this.image.sprite = meta.icon;
				return;
			}
			this.displayNameText.text = this.lockedText;
			this.image.sprite = this.lockedImage;
		}

		// Token: 0x04001582 RID: 5506
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001583 RID: 5507
		[SerializeField]
		private Image image;

		// Token: 0x04001584 RID: 5508
		[SerializeField]
		private string lockedText = "???";

		// Token: 0x04001585 RID: 5509
		[SerializeField]
		private Sprite lockedImage;

		// Token: 0x04001586 RID: 5510
		private FormulasIndexView master;

		// Token: 0x04001587 RID: 5511
		private CraftingFormula formula;
	}
}
