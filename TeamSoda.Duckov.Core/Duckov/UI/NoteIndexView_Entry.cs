using System;
using Duckov.NoteIndexs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x02000390 RID: 912
	public class NoteIndexView_Entry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x0006FC38 File Offset: 0x0006DE38
		public string key
		{
			get
			{
				return this.note.key;
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0006FC45 File Offset: 0x0006DE45
		private void OnEnable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Combine(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0006FC67 File Offset: 0x0006DE67
		private void OnDisable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Remove(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0006FC89 File Offset: 0x0006DE89
		private void OnNoteStatusChanged(string key)
		{
			if (key != this.note.key)
			{
				return;
			}
			this.RefreshNotReadIndicator();
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0006FCA5 File Offset: 0x0006DEA5
		private void RefreshNotReadIndicator()
		{
			this.notReadIndicator.SetActive(NoteIndex.GetNoteUnlocked(this.key) && !NoteIndex.GetNoteRead(this.key));
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0006FCD0 File Offset: 0x0006DED0
		internal void NotifySelectedDisplayingNoteChanged(string displayingNote)
		{
			this.RefreshHighlight();
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0006FCD8 File Offset: 0x0006DED8
		private void RefreshHighlight()
		{
			bool active = false;
			if (this.getDisplayingNote != null)
			{
				Func<string> func = this.getDisplayingNote;
				active = (((func != null) ? func() : null) == this.key);
			}
			this.highlightIndicator.SetActive(active);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x0006FD1C File Offset: 0x0006DF1C
		internal void Setup(Note note, Action<NoteIndexView_Entry> onClicked, Func<string> getDisplayingNote, int index)
		{
			bool noteUnlocked = NoteIndex.GetNoteUnlocked(note.key);
			this.note = note;
			this.titleText.text = (noteUnlocked ? note.Title : "???");
			this.onClicked = onClicked;
			this.getDisplayingNote = getDisplayingNote;
			if (index > 0)
			{
				this.indexText.text = index.ToString("000");
			}
			this.RefreshNotReadIndicator();
			this.RefreshHighlight();
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0006FD8D File Offset: 0x0006DF8D
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<NoteIndexView_Entry> action = this.onClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x040015AE RID: 5550
		[SerializeField]
		private GameObject highlightIndicator;

		// Token: 0x040015AF RID: 5551
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x040015B0 RID: 5552
		[SerializeField]
		private TextMeshProUGUI indexText;

		// Token: 0x040015B1 RID: 5553
		[SerializeField]
		private GameObject notReadIndicator;

		// Token: 0x040015B2 RID: 5554
		private Note note;

		// Token: 0x040015B3 RID: 5555
		private Action<NoteIndexView_Entry> onClicked;

		// Token: 0x040015B4 RID: 5556
		private Func<string> getDisplayingNote;
	}
}
