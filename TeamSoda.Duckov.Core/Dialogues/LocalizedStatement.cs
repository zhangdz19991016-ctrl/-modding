using System;
using NodeCanvas.DialogueTrees;
using SodaCraft.Localizations;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x0200021F RID: 543
	[Serializable]
	public class LocalizedStatement : IStatement
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x00040377 File Offset: 0x0003E577
		public string text
		{
			get
			{
				return this.textKey.ToPlainText();
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x00040384 File Offset: 0x0003E584
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x0004038C File Offset: 0x0003E58C
		public string textKey
		{
			get
			{
				return this._textKey;
			}
			set
			{
				this._textKey = value;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x00040395 File Offset: 0x0003E595
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x0004039D File Offset: 0x0003E59D
		public AudioClip audio
		{
			get
			{
				return this._audio;
			}
			set
			{
				this._audio = value;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x000403A6 File Offset: 0x0003E5A6
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x000403AE File Offset: 0x0003E5AE
		public string meta
		{
			get
			{
				return this._meta;
			}
			set
			{
				this._meta = value;
			}
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x000403B7 File Offset: 0x0003E5B7
		public LocalizedStatement()
		{
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x000403D5 File Offset: 0x0003E5D5
		public LocalizedStatement(string textKey)
		{
			this._textKey = textKey;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000403FA File Offset: 0x0003E5FA
		public LocalizedStatement(string textKey, AudioClip audio)
		{
			this._textKey = textKey;
			this.audio = audio;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00040426 File Offset: 0x0003E626
		public LocalizedStatement(string textKey, AudioClip audio, string meta)
		{
			this._textKey = textKey;
			this.audio = audio;
			this.meta = meta;
		}

		// Token: 0x04000D25 RID: 3365
		[SerializeField]
		private string _textKey = string.Empty;

		// Token: 0x04000D26 RID: 3366
		[SerializeField]
		private AudioClip _audio;

		// Token: 0x04000D27 RID: 3367
		[SerializeField]
		private string _meta = string.Empty;
	}
}
