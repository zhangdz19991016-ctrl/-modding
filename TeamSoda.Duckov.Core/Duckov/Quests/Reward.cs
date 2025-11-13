using System;
using Saves;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x0200033D RID: 829
	public abstract class Reward : MonoBehaviour, ISelfValidator, ISaveDataProvider
	{
		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06001C91 RID: 7313 RVA: 0x00067EEC File Offset: 0x000660EC
		// (remove) Token: 0x06001C92 RID: 7314 RVA: 0x00067F20 File Offset: 0x00066120
		public static event Action<Reward> OnRewardClaimed;

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00067F53 File Offset: 0x00066153
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x00067F5B File Offset: 0x0006615B
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

		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06001C95 RID: 7317 RVA: 0x00067F64 File Offset: 0x00066164
		// (remove) Token: 0x06001C96 RID: 7318 RVA: 0x00067F9C File Offset: 0x0006619C
		internal event Action onStatusChanged;

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x00067FD1 File Offset: 0x000661D1
		public bool Claimable
		{
			get
			{
				return this.master.Complete;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x00067FDE File Offset: 0x000661DE
		public virtual Sprite Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x00067FE1 File Offset: 0x000661E1
		public virtual string Description
		{
			get
			{
				return "未定义奖励描述";
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001C9A RID: 7322
		public abstract bool Claimed { get; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x00067FE8 File Offset: 0x000661E8
		public virtual bool Claiming { get; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00067FF0 File Offset: 0x000661F0
		public virtual bool AutoClaim
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x00067FF3 File Offset: 0x000661F3
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x00067FFB File Offset: 0x000661FB
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

		// Token: 0x06001C9F RID: 7327 RVA: 0x00068004 File Offset: 0x00066204
		public void Claim()
		{
			if (!this.Claimable || this.Claimed)
			{
				return;
			}
			this.OnClaim();
			this.Master.NotifyRewardClaimed(this);
			Action<Reward> onRewardClaimed = Reward.OnRewardClaimed;
			if (onRewardClaimed == null)
			{
				return;
			}
			onRewardClaimed(this);
		}

		// Token: 0x06001CA0 RID: 7328
		public abstract void OnClaim();

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0006803C File Offset: 0x0006623C
		public virtual void Validate(SelfValidationResult result)
		{
			if (this.master == null)
			{
				result.AddWarning("Reward需要master(Quest)。").WithFix("设为父物体中的Quest。", delegate()
				{
					this.master = base.GetComponent<Quest>();
					if (this.master == null)
					{
						this.master = base.GetComponentInParent<Quest>();
					}
				}, true);
			}
			if (this.master != null)
			{
				if (base.transform != this.master.transform && !base.transform.IsChildOf(this.master.transform))
				{
					result.AddError("Task需要存在于master子物体中。").WithFix("设为master子物体", delegate()
					{
						base.transform.SetParent(this.master.transform);
					}, true);
				}
				if (!this.master.rewards.Contains(this))
				{
					result.AddError("Master的Task列表中不包含本物体。").WithFix("将本物体添加至master的Task列表中", delegate()
					{
						this.master.rewards.Add(this);
					}, true);
				}
			}
		}

		// Token: 0x06001CA2 RID: 7330
		public abstract object GenerateSaveData();

		// Token: 0x06001CA3 RID: 7331
		public abstract void SetupSaveData(object data);

		// Token: 0x06001CA4 RID: 7332 RVA: 0x00068114 File Offset: 0x00066314
		private void Awake()
		{
			this.Master.onStatusChanged += this.OnMasterStatusChanged;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0006812D File Offset: 0x0006632D
		private void OnDestroy()
		{
			this.Master.onStatusChanged -= this.OnMasterStatusChanged;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x00068146 File Offset: 0x00066346
		public void OnMasterStatusChanged(Quest quest)
		{
			Action action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x00068158 File Offset: 0x00066358
		protected void ReportStatusChanged()
		{
			Action action = this.onStatusChanged;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0006816A File Offset: 0x0006636A
		public virtual void NotifyReload(Quest questInstance)
		{
		}

		// Token: 0x040013FC RID: 5116
		[SerializeField]
		private int id;

		// Token: 0x040013FD RID: 5117
		[SerializeField]
		private Quest master;
	}
}
