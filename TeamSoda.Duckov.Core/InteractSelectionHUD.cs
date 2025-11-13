using System;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000C4 RID: 196
public class InteractSelectionHUD : MonoBehaviour
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000640 RID: 1600 RVA: 0x0001C40E File Offset: 0x0001A60E
	public InteractableBase InteractTarget
	{
		get
		{
			return this.interactable;
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001C416 File Offset: 0x0001A616
	public void SetInteractable(InteractableBase _interactable, bool _hasUpDown)
	{
		this.interactable = _interactable;
		this.text.text = this.interactable.GetInteractName();
		this.UpdateRequireItem(this.interactable);
		this.selectionPoint.SetActive(_hasUpDown);
		this.hasUpDown = _hasUpDown;
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0001C454 File Offset: 0x0001A654
	private void UpdateRequireItem(InteractableBase interactable)
	{
		if (!interactable || !interactable.requireItem)
		{
			this.requireCanvasGroup.alpha = 0f;
			return;
		}
		this.requireCanvasGroup.alpha = 1f;
		CharacterMainControl mainCharacter = LevelManager.Instance.MainCharacter;
		bool flag = interactable.whenToUseRequireItem > InteractableBase.WhenToUseRequireItemTypes.None;
		string str = flag ? this.requirUseItemTextKey.ToPlainText() : this.requirItemTextKey.ToPlainText();
		this.requireText.text = str + " " + interactable.GetRequiredItemName();
		if (flag)
		{
			TextMeshProUGUI textMeshProUGUI = this.requireText;
			textMeshProUGUI.text += " x1";
		}
		this.requirementIcon.sprite = interactable.GetRequireditemIcon();
		if (interactable.TryGetRequiredItem(mainCharacter).Item1)
		{
			this.requireItemBackgroundImage.color = this.hasRequireItemColor;
			return;
		}
		this.requireItemBackgroundImage.color = this.noRequireItemColor;
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0001C540 File Offset: 0x0001A740
	public void SetSelection(bool _select)
	{
		this.selecting = _select;
		this.selectIndicator.SetActive(this.selecting);
		this.upDownIndicator.SetActive(this.selecting && this.hasUpDown);
		this.selectionPoint.SetActive(!this.selecting && this.hasUpDown);
		if (_select)
		{
			UnityEvent onSelectedEvent = this.OnSelectedEvent;
			if (onSelectedEvent != null)
			{
				onSelectedEvent.Invoke();
			}
			this.background.color = this.selectedColor;
			return;
		}
		this.background.color = this.unselectedColor;
	}

	// Token: 0x040005EE RID: 1518
	private InteractableBase interactable;

	// Token: 0x040005EF RID: 1519
	public GameObject selectIndicator;

	// Token: 0x040005F0 RID: 1520
	public TextMeshProUGUI text;

	// Token: 0x040005F1 RID: 1521
	public ProceduralImage background;

	// Token: 0x040005F2 RID: 1522
	public Color selectedColor;

	// Token: 0x040005F3 RID: 1523
	public Color unselectedColor;

	// Token: 0x040005F4 RID: 1524
	public CanvasGroup requireCanvasGroup;

	// Token: 0x040005F5 RID: 1525
	public ProceduralImage requireItemBackgroundImage;

	// Token: 0x040005F6 RID: 1526
	public TextMeshProUGUI requireText;

	// Token: 0x040005F7 RID: 1527
	[LocalizationKey("UI")]
	public string requirItemTextKey = "UI_RequireItem";

	// Token: 0x040005F8 RID: 1528
	[LocalizationKey("UI")]
	public string requirUseItemTextKey = "UI_RequireUseItem";

	// Token: 0x040005F9 RID: 1529
	public Image requirementIcon;

	// Token: 0x040005FA RID: 1530
	public Color hasRequireItemColor;

	// Token: 0x040005FB RID: 1531
	public Color noRequireItemColor;

	// Token: 0x040005FC RID: 1532
	private bool selecting;

	// Token: 0x040005FD RID: 1533
	public UnityEvent OnSelectedEvent;

	// Token: 0x040005FE RID: 1534
	public GameObject selectionPoint;

	// Token: 0x040005FF RID: 1535
	public GameObject upDownIndicator;

	// Token: 0x04000600 RID: 1536
	private bool hasUpDown;
}
