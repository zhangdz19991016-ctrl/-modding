using System;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x0200038C RID: 908
	public class FormulasIndexView : View, ISingleSelectionMenu<FormulasIndexEntry>
	{
		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x0006EE38 File Offset: 0x0006D038
		private PrefabPool<FormulasIndexEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<FormulasIndexEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0006EE71 File Offset: 0x0006D071
		public FormulasIndexEntry GetSelection()
		{
			return this.selectedEntry;
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x0006EE79 File Offset: 0x0006D079
		public static FormulasIndexView Instance
		{
			get
			{
				return View.GetViewInstance<FormulasIndexView>();
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0006EE80 File Offset: 0x0006D080
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0006EE88 File Offset: 0x0006D088
		public static void Show()
		{
			if (FormulasIndexView.Instance == null)
			{
				return;
			}
			FormulasIndexView.Instance.Open(null);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0006EEA3 File Offset: 0x0006D0A3
		public bool SetSelection(FormulasIndexEntry selection)
		{
			this.selectedEntry = selection;
			return true;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0006EEB0 File Offset: 0x0006D0B0
		protected override void OnOpen()
		{
			base.OnOpen();
			this.selectedEntry = null;
			this.Pool.ReleaseAll();
			foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
			{
				if (!craftingFormula.hideInIndex && (!GameMetaData.Instance.IsDemo || !craftingFormula.lockInDemo))
				{
					this.Pool.Get(null).Setup(this, craftingFormula);
				}
			}
			this.RefreshDetails();
			this.fadeGroup.Show();
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0006EF54 File Offset: 0x0006D154
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0006EF68 File Offset: 0x0006D168
		internal void OnEntryClicked(FormulasIndexEntry entry)
		{
			FormulasIndexEntry formulasIndexEntry = this.selectedEntry;
			this.selectedEntry = entry;
			this.selectedEntry.Refresh();
			if (formulasIndexEntry)
			{
				formulasIndexEntry.Refresh();
			}
			this.RefreshDetails();
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0006EFA4 File Offset: 0x0006D1A4
		private void RefreshDetails()
		{
			if (this.selectedEntry && this.selectedEntry.Valid)
			{
				this.detailsDisplay.Setup(new CraftingFormula?(this.selectedEntry.Formula));
				return;
			}
			this.detailsDisplay.Setup(null);
		}

		// Token: 0x04001588 RID: 5512
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001589 RID: 5513
		[SerializeField]
		private FormulasIndexEntry entryTemplate;

		// Token: 0x0400158A RID: 5514
		[SerializeField]
		private FormulasDetailsDisplay detailsDisplay;

		// Token: 0x0400158B RID: 5515
		private PrefabPool<FormulasIndexEntry> _pool;

		// Token: 0x0400158C RID: 5516
		private FormulasIndexEntry selectedEntry;
	}
}
