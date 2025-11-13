using System;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000250 RID: 592
	[RequireComponent(typeof(Perk))]
	public abstract class PerkBehaviour : MonoBehaviour
	{
		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x00047056 File Offset: 0x00045256
		protected Perk Master
		{
			get
			{
				return this.master;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x0004705E File Offset: 0x0004525E
		private bool Unlocked
		{
			get
			{
				return !(this.master == null) && this.master.Unlocked;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600129B RID: 4763 RVA: 0x0004707B File Offset: 0x0004527B
		public virtual string Description
		{
			get
			{
				return "";
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00047084 File Offset: 0x00045284
		private void Awake()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<Perk>();
			}
			this.master.onUnlockStateChanged += this.OnMasterUnlockStateChanged;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			if (LevelManager.LevelInited)
			{
				this.OnLevelInitialized();
			}
			this.OnAwake();
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000470E6 File Offset: 0x000452E6
		private void OnLevelInitialized()
		{
			this.NotifyUnlockStateChanged(this.Unlocked);
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x000470F4 File Offset: 0x000452F4
		private void OnDestroy()
		{
			this.OnOnDestroy();
			if (this.master == null)
			{
				return;
			}
			this.master.onUnlockStateChanged -= this.OnMasterUnlockStateChanged;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x00047133 File Offset: 0x00045333
		private void OnValidate()
		{
			if (this.master == null)
			{
				this.master = base.GetComponent<Perk>();
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0004714F File Offset: 0x0004534F
		private void OnMasterUnlockStateChanged(Perk perk, bool unlocked)
		{
			if (perk != this.master)
			{
				Debug.LogError("Perk对象不匹配");
			}
			this.NotifyUnlockStateChanged(unlocked);
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00047170 File Offset: 0x00045370
		private void NotifyUnlockStateChanged(bool unlocked)
		{
			this.OnUnlockStateChanged(unlocked);
			if (unlocked)
			{
				this.OnUnlocked();
				return;
			}
			this.OnLocked();
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00047189 File Offset: 0x00045389
		protected virtual void OnUnlockStateChanged(bool unlocked)
		{
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0004718B File Offset: 0x0004538B
		protected virtual void OnUnlocked()
		{
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0004718D File Offset: 0x0004538D
		protected virtual void OnLocked()
		{
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0004718F File Offset: 0x0004538F
		protected virtual void OnAwake()
		{
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00047191 File Offset: 0x00045391
		protected virtual void OnOnDestroy()
		{
		}

		// Token: 0x04000E38 RID: 3640
		private Perk master;
	}
}
