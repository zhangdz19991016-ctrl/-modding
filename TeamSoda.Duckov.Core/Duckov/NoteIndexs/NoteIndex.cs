using System;
using System.Collections.Generic;
using System.Linq;
using Saves;
using Sirenix.Utilities;
using UnityEngine;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000268 RID: 616
	public class NoteIndex : MonoBehaviour
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x000488B0 File Offset: 0x00046AB0
		public static NoteIndex Instance
		{
			get
			{
				return GameManager.NoteIndex;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x000488B7 File Offset: 0x00046AB7
		public List<Note> Notes
		{
			get
			{
				return this.notes;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x000488BF File Offset: 0x00046ABF
		private Dictionary<string, Note> MDic
		{
			get
			{
				if (this._dic == null)
				{
					this.RebuildDic();
				}
				return this._dic;
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x000488D8 File Offset: 0x00046AD8
		private void RebuildDic()
		{
			if (this._dic == null)
			{
				this._dic = new Dictionary<string, Note>();
			}
			this._dic.Clear();
			foreach (Note note in this.notes)
			{
				this._dic[note.key] = note;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x00048954 File Offset: 0x00046B54
		public HashSet<string> UnlockedNotes
		{
			get
			{
				return this.unlockedNotes;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x0004895C File Offset: 0x00046B5C
		public HashSet<string> ReadNotes
		{
			get
			{
				return this.unlockedNotes;
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x00048964 File Offset: 0x00046B64
		public static IEnumerable<string> GetAllNotes(bool unlockedOnly = true)
		{
			if (NoteIndex.Instance == null)
			{
				yield break;
			}
			foreach (Note note in NoteIndex.Instance.notes)
			{
				string key = note.key;
				if (!unlockedOnly || NoteIndex.GetNoteUnlocked(key))
				{
					yield return note.key;
				}
			}
			List<Note>.Enumerator enumerator = default(List<Note>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x00048974 File Offset: 0x00046B74
		private void Awake()
		{
			SavesSystem.OnCollectSaveData += this.Save;
			SavesSystem.OnSetFile += this.Load;
			this.Load();
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0004899E File Offset: 0x00046B9E
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
			SavesSystem.OnSetFile -= this.Load;
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x000489C4 File Offset: 0x00046BC4
		private void Save()
		{
			NoteIndex.SaveData value = new NoteIndex.SaveData(this);
			SavesSystem.Save<NoteIndex.SaveData>("NoteIndexData", value);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x000489E4 File Offset: 0x00046BE4
		private void Load()
		{
			SavesSystem.Load<NoteIndex.SaveData>("NoteIndexData").Setup(this);
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00048A04 File Offset: 0x00046C04
		public void MSetEntryDynamic(Note note)
		{
			this.MDic[note.key] = note;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00048A18 File Offset: 0x00046C18
		public Note MGetNote(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				Debug.LogError("Trying to get note with an empty key.");
				return null;
			}
			Note result;
			if (!this.MDic.TryGetValue(key, out result))
			{
				Debug.LogError("Cannot find note: " + key);
				return null;
			}
			return result;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00048A5C File Offset: 0x00046C5C
		public static Note GetNote(string key)
		{
			if (NoteIndex.Instance == null)
			{
				return null;
			}
			return NoteIndex.Instance.MGetNote(key);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00048A78 File Offset: 0x00046C78
		public static bool SetNoteDynamic(Note note)
		{
			if (NoteIndex.Instance == null)
			{
				return false;
			}
			NoteIndex.Instance.MSetEntryDynamic(note);
			return true;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00048A95 File Offset: 0x00046C95
		public static bool GetNoteUnlocked(string noteKey)
		{
			return !(NoteIndex.Instance == null) && NoteIndex.Instance.unlockedNotes.Contains(noteKey);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00048AB6 File Offset: 0x00046CB6
		public static bool GetNoteRead(string noteKey)
		{
			return !(NoteIndex.Instance == null) && NoteIndex.Instance.readNotes.Contains(noteKey);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00048AD7 File Offset: 0x00046CD7
		public static void SetNoteUnlocked(string noteKey)
		{
			if (NoteIndex.Instance == null)
			{
				return;
			}
			NoteIndex.Instance.unlockedNotes.Add(noteKey);
			Action<string> action = NoteIndex.onNoteStatusChanged;
			if (action == null)
			{
				return;
			}
			action(noteKey);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00048B08 File Offset: 0x00046D08
		public static void SetNoteRead(string noteKey)
		{
			if (NoteIndex.Instance == null)
			{
				return;
			}
			NoteIndex.Instance.readNotes.Add(noteKey);
			Action<string> action = NoteIndex.onNoteStatusChanged;
			if (action == null)
			{
				return;
			}
			action(noteKey);
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00048B39 File Offset: 0x00046D39
		internal static int GetTotalNoteCount()
		{
			if (NoteIndex.Instance == null)
			{
				return 0;
			}
			return NoteIndex.Instance.Notes.Count<Note>();
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00048B59 File Offset: 0x00046D59
		internal static int GetUnlockedNoteCount()
		{
			if (NoteIndex.Instance == null)
			{
				return 0;
			}
			return NoteIndex.Instance.UnlockedNotes.Count;
		}

		// Token: 0x04000E6C RID: 3692
		[SerializeField]
		private List<Note> notes = new List<Note>();

		// Token: 0x04000E6D RID: 3693
		private Dictionary<string, Note> _dic;

		// Token: 0x04000E6E RID: 3694
		private HashSet<string> unlockedNotes = new HashSet<string>();

		// Token: 0x04000E6F RID: 3695
		private HashSet<string> readNotes = new HashSet<string>();

		// Token: 0x04000E70 RID: 3696
		public static Action<string> onNoteStatusChanged;

		// Token: 0x04000E71 RID: 3697
		private const string SaveKey = "NoteIndexData";

		// Token: 0x02000544 RID: 1348
		[Serializable]
		private struct SaveData
		{
			// Token: 0x06002842 RID: 10306 RVA: 0x00093AAF File Offset: 0x00091CAF
			public SaveData(NoteIndex from)
			{
				this.unlockedNotes = from.unlockedNotes.ToList<string>();
				this.readNotes = from.unlockedNotes.ToList<string>();
			}

			// Token: 0x06002843 RID: 10307 RVA: 0x00093AD4 File Offset: 0x00091CD4
			public void Setup(NoteIndex to)
			{
				to.unlockedNotes.Clear();
				if (this.unlockedNotes != null)
				{
					to.unlockedNotes.AddRange(this.unlockedNotes);
				}
				to.readNotes.Clear();
				if (this.readNotes != null)
				{
					to.readNotes.AddRange(this.readNotes);
				}
			}

			// Token: 0x04001ED5 RID: 7893
			public List<string> unlockedNotes;

			// Token: 0x04001ED6 RID: 7894
			public List<string> readNotes;
		}
	}
}
