using System;
using Duckov.UI;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x0200026B RID: 619
	public class NoteIndexProxy : MonoBehaviour
	{
		// Token: 0x06001367 RID: 4967 RVA: 0x00048BFE File Offset: 0x00046DFE
		public void UnlockNote(string key)
		{
			NoteIndex.SetNoteUnlocked(key);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00048C06 File Offset: 0x00046E06
		public void UnlockAndShowNote(string key)
		{
			NoteIndexView.ShowNote(key, true);
		}
	}
}
