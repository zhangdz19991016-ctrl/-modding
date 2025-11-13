using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class CostTakerHUD_Entry : MonoBehaviour
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
	// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
	public CostTaker Target { get; private set; }

	// Token: 0x060006B3 RID: 1715 RVA: 0x0001E2C9 File Offset: 0x0001C4C9
	private void Awake()
	{
		this.rectTransform = (base.transform as RectTransform);
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0001E2DC File Offset: 0x0001C4DC
	private void LateUpdate()
	{
		this.UpdatePosition();
		this.UpdateFadeGroup();
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0001E2EA File Offset: 0x0001C4EA
	internal void Setup(CostTaker cur)
	{
		this.Target = cur;
		this.nameText.text = cur.InteractName;
		this.costDisplay.Setup(cur.Cost, 1);
		this.UpdatePosition();
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x0001E31C File Offset: 0x0001C51C
	private void UpdatePosition()
	{
		this.directionPositive = this.rectTransform.MatchWorldPosition(this.Target.transform.TransformPoint(this.Target.interactMarkerOffset), Vector3.up * 0.5f);
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x0001E35C File Offset: 0x0001C55C
	private void UpdateFadeGroup()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		bool flag = false;
		if (this.directionPositive && !(this.Target == null) && !(main == null))
		{
			Vector3 vector = main.transform.position - this.Target.transform.position;
			if (Mathf.Abs(vector.y) <= 2.5f && vector.magnitude <= 10f)
			{
				flag = true;
			}
		}
		if (flag && !this.fadeGroup.IsShown)
		{
			this.fadeGroup.Show();
			return;
		}
		if (!flag && this.fadeGroup.IsShown)
		{
			this.fadeGroup.Hide();
		}
	}

	// Token: 0x04000673 RID: 1651
	private RectTransform rectTransform;

	// Token: 0x04000674 RID: 1652
	[SerializeField]
	private TextMeshProUGUI nameText;

	// Token: 0x04000675 RID: 1653
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000676 RID: 1654
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000677 RID: 1655
	private const float HideDistance = 10f;

	// Token: 0x04000678 RID: 1656
	private const float HideDistanceYLimit = 2.5f;

	// Token: 0x04000679 RID: 1657
	private bool directionPositive;
}
