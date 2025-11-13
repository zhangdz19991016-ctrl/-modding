using System;
using Duckov.NoteIndexs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000391 RID: 913
	public class NoteIndexView_Inspector : MonoBehaviour
	{
		// Token: 0x06001FE1 RID: 8161 RVA: 0x0006FDA8 File Offset: 0x0006DFA8
		private void Awake()
		{
			this.placeHolder.Show();
			this.content.SkipHide();
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x0006FDC0 File Offset: 0x0006DFC0
		internal void Setup(Note value)
		{
			if (value == null)
			{
				this.placeHolder.Show();
				this.content.Hide();
				return;
			}
			this.note = value;
			this.SetupContent(this.note);
			this.placeHolder.Hide();
			this.content.Show();
			NoteIndex.SetNoteRead(value.key);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0006FE1C File Offset: 0x0006E01C
		private void SetupContent(Note value)
		{
			this.textTitle.text = value.Title;
			this.textContent.text = value.Content;
			this.image.sprite = value.image;
			this.image.gameObject.SetActive(value.image == null);
		}

		// Token: 0x040015B5 RID: 5557
		[SerializeField]
		private FadeGroup placeHolder;

		// Token: 0x040015B6 RID: 5558
		[SerializeField]
		private FadeGroup content;

		// Token: 0x040015B7 RID: 5559
		[SerializeField]
		private TextMeshProUGUI textTitle;

		// Token: 0x040015B8 RID: 5560
		[SerializeField]
		private TextMeshProUGUI textContent;

		// Token: 0x040015B9 RID: 5561
		[SerializeField]
		private Image image;

		// Token: 0x040015BA RID: 5562
		private Note note;
	}
}
