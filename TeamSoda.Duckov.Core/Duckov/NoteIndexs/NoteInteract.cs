using System;
using Duckov.UI;

namespace Duckov.NoteIndexs
{
	// Token: 0x0200026C RID: 620
	public class NoteInteract : InteractableBase
	{
		// Token: 0x0600136A RID: 4970 RVA: 0x00048C17 File Offset: 0x00046E17
		protected override void Start()
		{
			base.Start();
			if (NoteIndex.GetNoteUnlocked(this.noteKey))
			{
				base.gameObject.SetActive(false);
			}
			this.finishWhenTimeOut = true;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00048C3F File Offset: 0x00046E3F
		protected override void OnInteractFinished()
		{
			NoteIndex.SetNoteUnlocked(this.noteKey);
			NoteIndexView.ShowNote(this.noteKey, true);
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00048C64 File Offset: 0x00046E64
		private void OnValidate()
		{
			this.noteTitle = "Note_" + this.noteKey + "_Title";
			this.noteContent = "Note_" + this.noteKey + "_Content";
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00048C9C File Offset: 0x00046E9C
		public void ReName()
		{
			base.gameObject.name = "Note_" + this.noteKey;
		}

		// Token: 0x04000E77 RID: 3703
		public string noteKey;

		// Token: 0x04000E78 RID: 3704
		[LocalizationKey("Default")]
		public string noteTitle;

		// Token: 0x04000E79 RID: 3705
		[LocalizationKey("Default")]
		public string noteContent;
	}
}
