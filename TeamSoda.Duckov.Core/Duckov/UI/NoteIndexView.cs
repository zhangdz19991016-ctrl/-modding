using System;
using Duckov.NoteIndexs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x0200038F RID: 911
	public class NoteIndexView : View
	{
		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0006F874 File Offset: 0x0006DA74
		private PrefabPool<NoteIndexView_Entry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<NoteIndexView_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0006F8AD File Offset: 0x0006DAAD
		private void OnEnable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Combine(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0006F8CF File Offset: 0x0006DACF
		private void OnDisable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Remove(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0006F8F1 File Offset: 0x0006DAF1
		private void Update()
		{
			if (this.needFocus)
			{
				this.needFocus = false;
				this.MoveScrollViewToActiveEntry();
			}
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0006F908 File Offset: 0x0006DB08
		private void OnNoteStatusChanged(string noteKey)
		{
			this.RefreshEntries();
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0006F910 File Offset: 0x0006DB10
		public void DoOpen()
		{
			base.Open(null);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0006F919 File Offset: 0x0006DB19
		protected override void OnOpen()
		{
			base.OnOpen();
			this.mainFadeGroup.Show();
			this.RefreshEntries();
			this.SetDisplayTargetNote(this.displayingNote);
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0006F93E File Offset: 0x0006DB3E
		protected override void OnClose()
		{
			base.OnClose();
			this.mainFadeGroup.Hide();
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0006F951 File Offset: 0x0006DB51
		protected override void OnCancel()
		{
			base.Close();
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0006F95C File Offset: 0x0006DB5C
		private void RefreshNoteCount()
		{
			int totalNoteCount = NoteIndex.GetTotalNoteCount();
			int unlockedNoteCount = NoteIndex.GetUnlockedNoteCount();
			this.noteCountText.text = string.Format("{0} / {1}", unlockedNoteCount, totalNoteCount);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0006F998 File Offset: 0x0006DB98
		private void RefreshEntries()
		{
			this.RefreshNoteCount();
			this.Pool.ReleaseAll();
			if (NoteIndex.Instance == null)
			{
				return;
			}
			int num = 0;
			foreach (string key in NoteIndex.GetAllNotes(false))
			{
				Note note = NoteIndex.GetNote(key);
				if (note != null)
				{
					NoteIndexView_Entry noteIndexView_Entry = this.Pool.Get(null);
					num++;
					noteIndexView_Entry.Setup(note, new Action<NoteIndexView_Entry>(this.OnEntryClicked), new Func<string>(this.GetDisplayingNote), num);
				}
			}
			this.noEntryIndicator.SetActive(num <= 0);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0006FA48 File Offset: 0x0006DC48
		private string GetDisplayingNote()
		{
			return this.displayingNote;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0006FA50 File Offset: 0x0006DC50
		public void SetDisplayTargetNote(string noteKey)
		{
			Note note = null;
			if (!string.IsNullOrWhiteSpace(noteKey))
			{
				note = NoteIndex.GetNote(noteKey);
			}
			if (note == null)
			{
				this.displayingNote = null;
			}
			else
			{
				this.displayingNote = note.key;
			}
			foreach (NoteIndexView_Entry noteIndexView_Entry in this.Pool.ActiveEntries)
			{
				noteIndexView_Entry.NotifySelectedDisplayingNoteChanged(this.displayingNote);
			}
			this.inspector.Setup(note);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0006FADC File Offset: 0x0006DCDC
		private void OnEntryClicked(NoteIndexView_Entry entry)
		{
			string key = entry.key;
			if (!NoteIndex.GetNoteUnlocked(key))
			{
				this.SetDisplayTargetNote("");
				return;
			}
			this.SetDisplayTargetNote(key);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0006FB0C File Offset: 0x0006DD0C
		public static void ShowNote(string noteKey, bool unlock = true)
		{
			NoteIndexView viewInstance = View.GetViewInstance<NoteIndexView>();
			if (viewInstance == null)
			{
				return;
			}
			if (unlock)
			{
				NoteIndex.SetNoteUnlocked(noteKey);
			}
			if (!(View.ActiveView is NoteIndexView))
			{
				viewInstance.Open(null);
			}
			viewInstance.SetDisplayTargetNote(noteKey);
			viewInstance.needFocus = true;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0006FB54 File Offset: 0x0006DD54
		private void MoveScrollViewToActiveEntry()
		{
			NoteIndexView_Entry displayingEntry = this.GetDisplayingEntry();
			if (displayingEntry == null)
			{
				return;
			}
			RectTransform rectTransform = displayingEntry.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			float num = -rectTransform.anchoredPosition.y;
			float height = this.indexScrollView.content.rect.height;
			float verticalNormalizedPosition = 1f - num / height;
			this.indexScrollView.verticalNormalizedPosition = verticalNormalizedPosition;
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0006FBC8 File Offset: 0x0006DDC8
		private NoteIndexView_Entry GetDisplayingEntry()
		{
			foreach (NoteIndexView_Entry noteIndexView_Entry in this.Pool.ActiveEntries)
			{
				if (noteIndexView_Entry.key == this.displayingNote)
				{
					return noteIndexView_Entry;
				}
			}
			return null;
		}

		// Token: 0x040015A5 RID: 5541
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x040015A6 RID: 5542
		[SerializeField]
		private GameObject noEntryIndicator;

		// Token: 0x040015A7 RID: 5543
		[SerializeField]
		private NoteIndexView_Entry entryTemplate;

		// Token: 0x040015A8 RID: 5544
		[SerializeField]
		private NoteIndexView_Inspector inspector;

		// Token: 0x040015A9 RID: 5545
		[SerializeField]
		private TextMeshProUGUI noteCountText;

		// Token: 0x040015AA RID: 5546
		[SerializeField]
		private ScrollRect indexScrollView;

		// Token: 0x040015AB RID: 5547
		private PrefabPool<NoteIndexView_Entry> _pool;

		// Token: 0x040015AC RID: 5548
		private string displayingNote;

		// Token: 0x040015AD RID: 5549
		private bool needFocus;
	}
}
