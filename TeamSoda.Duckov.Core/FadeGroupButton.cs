using System;
using Duckov.UI.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000168 RID: 360
public class FadeGroupButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000AFD RID: 2813 RVA: 0x0002F963 File Offset: 0x0002DB63
	private void OnEnable()
	{
		UIInputManager.OnCancel += this.OnCancel;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0002F976 File Offset: 0x0002DB76
	private void OnDisable()
	{
		UIInputManager.OnCancel -= this.OnCancel;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0002F989 File Offset: 0x0002DB89
	private void OnCancel(UIInputEventData data)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (data.Used)
		{
			return;
		}
		if (!this.triggerWhenCancel)
		{
			return;
		}
		this.Execute();
		data.Use();
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0002F9B2 File Offset: 0x0002DBB2
	public void OnPointerClick(PointerEventData eventData)
	{
		this.Execute();
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0002F9BA File Offset: 0x0002DBBA
	private void Execute()
	{
		if (this.closeOnClick)
		{
			this.closeOnClick.Hide();
		}
		if (this.openOnClick)
		{
			this.openOnClick.Show();
		}
	}

	// Token: 0x04000987 RID: 2439
	[SerializeField]
	private FadeGroup closeOnClick;

	// Token: 0x04000988 RID: 2440
	[SerializeField]
	private FadeGroup openOnClick;

	// Token: 0x04000989 RID: 2441
	[SerializeField]
	private bool triggerWhenCancel;
}
