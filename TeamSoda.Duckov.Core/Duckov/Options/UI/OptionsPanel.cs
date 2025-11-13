using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Options.UI
{
	// Token: 0x02000262 RID: 610
	public class OptionsPanel : UIPanel, ISingleSelectionMenu<OptionsPanel_TabButton>
	{
		// Token: 0x06001311 RID: 4881 RVA: 0x00048150 File Offset: 0x00046350
		private void Start()
		{
			this.Setup();
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00048158 File Offset: 0x00046358
		private void Setup()
		{
			foreach (OptionsPanel_TabButton optionsPanel_TabButton in this.tabButtons)
			{
				optionsPanel_TabButton.onClicked = (Action<OptionsPanel_TabButton, PointerEventData>)Delegate.Combine(optionsPanel_TabButton.onClicked, new Action<OptionsPanel_TabButton, PointerEventData>(this.OnTabButtonClicked));
			}
			if (this.selection == null)
			{
				this.selection = this.tabButtons[0];
			}
			this.SetSelection(this.selection);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x000481F4 File Offset: 0x000463F4
		private void OnTabButtonClicked(OptionsPanel_TabButton button, PointerEventData data)
		{
			data.Use();
			this.SetSelection(button);
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00048204 File Offset: 0x00046404
		protected override void OnOpen()
		{
			base.OnOpen();
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0004820C File Offset: 0x0004640C
		public OptionsPanel_TabButton GetSelection()
		{
			return this.selection;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00048214 File Offset: 0x00046414
		public bool SetSelection(OptionsPanel_TabButton selection)
		{
			this.selection = selection;
			foreach (OptionsPanel_TabButton optionsPanel_TabButton in this.tabButtons)
			{
				optionsPanel_TabButton.NotifySelectionChanged(this, selection);
			}
			return true;
		}

		// Token: 0x04000E57 RID: 3671
		[SerializeField]
		private List<OptionsPanel_TabButton> tabButtons;

		// Token: 0x04000E58 RID: 3672
		private OptionsPanel_TabButton selection;
	}
}
