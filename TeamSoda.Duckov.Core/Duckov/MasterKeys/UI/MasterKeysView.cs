using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002E8 RID: 744
	public class MasterKeysView : View, ISingleSelectionMenu<MasterKeysIndexEntry>
	{
		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x000585AD File Offset: 0x000567AD
		public static MasterKeysView Instance
		{
			get
			{
				return View.GetViewInstance<MasterKeysView>();
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000585B4 File Offset: 0x000567B4
		protected override void Awake()
		{
			base.Awake();
			this.listDisplay.onEntryPointerClicked += this.OnEntryClicked;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x000585D3 File Offset: 0x000567D3
		private void OnEntryClicked(MasterKeysIndexEntry entry)
		{
			this.RefreshInspectorDisplay();
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000585DB File Offset: 0x000567DB
		public MasterKeysIndexEntry GetSelection()
		{
			return this.listDisplay.GetSelection();
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000585E8 File Offset: 0x000567E8
		public bool SetSelection(MasterKeysIndexEntry selection)
		{
			this.listDisplay.GetSelection();
			return true;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000585F7 File Offset: 0x000567F7
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.SetSelection(null);
			this.RefreshListDisplay();
			this.RefreshInspectorDisplay();
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0005861E File Offset: 0x0005681E
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00058631 File Offset: 0x00056831
		private void RefreshListDisplay()
		{
			this.listDisplay.Refresh();
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00058640 File Offset: 0x00056840
		private void RefreshInspectorDisplay()
		{
			MasterKeysIndexEntry selection = this.GetSelection();
			this.inspector.Setup(selection);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00058660 File Offset: 0x00056860
		internal static void Show()
		{
			if (MasterKeysView.Instance == null)
			{
				Debug.Log(" Master keys view Instance is null");
				return;
			}
			MasterKeysView.Instance.Open(null);
		}

		// Token: 0x04001179 RID: 4473
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400117A RID: 4474
		[SerializeField]
		private MasterKeysIndexList listDisplay;

		// Token: 0x0400117B RID: 4475
		[SerializeField]
		private MasterKeysIndexInspector inspector;
	}
}
