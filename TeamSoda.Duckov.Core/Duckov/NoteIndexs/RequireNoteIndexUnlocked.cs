using System;
using Duckov.Quests;

namespace Duckov.NoteIndexs
{
	// Token: 0x0200026D RID: 621
	public class RequireNoteIndexUnlocked : Condition
	{
		// Token: 0x0600136F RID: 4975 RVA: 0x00048CC1 File Offset: 0x00046EC1
		public override bool Evaluate()
		{
			return NoteIndex.GetNoteUnlocked(this.key);
		}

		// Token: 0x04000E7A RID: 3706
		public string key;
	}
}
