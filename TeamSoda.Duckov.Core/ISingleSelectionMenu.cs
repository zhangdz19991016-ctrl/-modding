using System;

// Token: 0x02000160 RID: 352
public interface ISingleSelectionMenu<EntryType> where EntryType : class
{
	// Token: 0x06000AD0 RID: 2768
	EntryType GetSelection();

	// Token: 0x06000AD1 RID: 2769
	bool SetSelection(EntryType selection);
}
