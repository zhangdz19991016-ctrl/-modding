using System;
using Duckov.Economy;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B6 RID: 438
public class FormulasDetailsDisplay : MonoBehaviour
{
	// Token: 0x06000D0C RID: 3340 RVA: 0x00036BD7 File Offset: 0x00034DD7
	private void SetupEmpty()
	{
		this.contentFadeGroup.Hide();
		this.placeHolderFadeGroup.Show();
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x00036BEF File Offset: 0x00034DEF
	private void SetupFormula(CraftingFormula formula)
	{
		this.formula = formula;
		this.RefreshContent();
		this.contentFadeGroup.Show();
		this.placeHolderFadeGroup.Hide();
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x00036C14 File Offset: 0x00034E14
	private void RefreshContent()
	{
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.formula.result.id);
		this.nameText.text = metaData.DisplayName;
		this.descriptionText.text = metaData.Description;
		this.image.sprite = metaData.icon;
		this.costDisplay.Setup(this.formula.cost, 1);
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00036C83 File Offset: 0x00034E83
	public void Setup(CraftingFormula? formula)
	{
		if (formula == null)
		{
			this.SetupEmpty();
			return;
		}
		if (!CraftingManager.IsFormulaUnlocked(formula.Value.id))
		{
			this.SetupUnknown();
			return;
		}
		this.SetupFormula(formula.Value);
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00036CBC File Offset: 0x00034EBC
	private void SetupUnknown()
	{
		this.nameText.text = "???";
		this.descriptionText.text = "???";
		this.image.sprite = this.unknownImage;
		this.contentFadeGroup.Show();
		this.placeHolderFadeGroup.Hide();
		this.costDisplay.Setup(default(Cost), 1);
	}

	// Token: 0x04000B48 RID: 2888
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000B49 RID: 2889
	[SerializeField]
	private Image image;

	// Token: 0x04000B4A RID: 2890
	[SerializeField]
	private TextMeshProUGUI descriptionText;

	// Token: 0x04000B4B RID: 2891
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	private FadeGroup contentFadeGroup;

	// Token: 0x04000B4D RID: 2893
	[SerializeField]
	private FadeGroup placeHolderFadeGroup;

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private Sprite unknownImage;

	// Token: 0x04000B4F RID: 2895
	private CraftingFormula formula;
}
