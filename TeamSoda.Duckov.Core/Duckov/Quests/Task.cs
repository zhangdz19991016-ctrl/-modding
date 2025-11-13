using System;
using Saves;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033F RID: 831
	[Serializable]
	public abstract class Task : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x000682E3 File Offset: 0x000664E3
		// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x000682EB File Offset: 0x000664EB
		public Quest Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001CBA RID: 7354 RVA: 0x000682F4 File Offset: 0x000664F4
		// (set) Token: 0x06001CBB RID: 7355 RVA: 0x000682FC File Offset: 0x000664FC
		public int ID
		{
			get
			{
				return this.id;
			}
			internal set
			{
				this.id = value;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001CBC RID: 7356 RVA: 0x00068305 File Offset: 0x00066505
		public virtual string Description
		{
			get
			{
				return "未定义Task描述。";
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x0006830C File Offset: 0x0006650C
		public virtual string[] ExtraDescriptsions
		{
			get
			{
				return new string[0];
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x00068314 File Offset: 0x00066514
		public virtual Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x140000D1 RID: 209
		// (add) Token: 0x06001CBF RID: 7359 RVA: 0x00068318 File Offset: 0x00066518
		// (remove) Token: 0x06001CC0 RID: 7360 RVA: 0x00068350 File Offset: 0x00066550
		public event Action<Task> onStatusChanged;

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00068385 File Offset: 0x00066585
		public virtual bool Interactable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x00068388 File Offset: 0x00066588
		public virtual bool PossibleValidInteraction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x0006838B File Offset: 0x0006658B
		public virtual bool NeedInspection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x0006838E File Offset: 0x0006658E
		public virtual string InteractText
		{
			get
			{
				return "交互";
			}
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x00068395 File Offset: 0x00066595
		public virtual void Interact()
		{
			Debug.LogWarning(string.Format("{0}可能未定义交互行为", base.GetType()));
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000683AC File Offset: 0x000665AC
		public bool IsFinished()
		{
			return this.forceFinish || this.CheckFinished();
		}

		// Token: 0x06001CC7 RID: 7367
		protected abstract bool CheckFinished();

		// Token: 0x06001CC8 RID: 7368
		public abstract object GenerateSaveData();

		// Token: 0x06001CC9 RID: 7369
		public abstract void SetupSaveData(object data);

		// Token: 0x06001CCA RID: 7370 RVA: 0x000683BE File Offset: 0x000665BE
		protected void ReportStatusChanged()
		{
			Action<Task> action = this.onStatusChanged;
			if (action != null)
			{
				action(this);
			}
			if (this.IsFinished())
			{
				Quest quest = this.Master;
				if (quest == null)
				{
					return;
				}
				quest.NotifyTaskFinished(this);
			}
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000683EB File Offset: 0x000665EB
		internal void Init()
		{
			if (this.IsFinished())
			{
				base.enabled = false;
				return;
			}
			this.OnInit();
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00068403 File Offset: 0x00066603
		protected virtual void OnInit()
		{
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00068405 File Offset: 0x00066605
		internal void ForceFinish()
		{
			this.forceFinish = true;
			Action<Task> action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x04001405 RID: 5125
		[SerializeField]
		private Quest master;

		// Token: 0x04001406 RID: 5126
		[SerializeField]
		private int id;

		// Token: 0x04001408 RID: 5128
		[SerializeField]
		private bool forceFinish;
	}
}
