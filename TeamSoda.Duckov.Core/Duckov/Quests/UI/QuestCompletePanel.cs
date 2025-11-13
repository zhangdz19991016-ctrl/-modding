using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000349 RID: 841
	public class QuestCompletePanel : MonoBehaviour
	{
		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x00068F3C File Offset: 0x0006713C
		private PrefabPool<RewardEntry> RewardEntryPool
		{
			get
			{
				if (this._rewardEntryPool == null)
				{
					this._rewardEntryPool = new PrefabPool<RewardEntry>(this.rewardEntryTemplate, this.rewardEntryTemplate.transform.parent, null, null, null, true, 10, 10000, null);
					this.rewardEntryTemplate.gameObject.SetActive(false);
				}
				return this._rewardEntryPool;
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x00068F95 File Offset: 0x00067195
		public Quest Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x00068F9D File Offset: 0x0006719D
		private void Awake()
		{
			this.skipButton.onClick.AddListener(new UnityAction(this.Skip));
			this.takeAllButton.onClick.AddListener(new UnityAction(this.TakeAll));
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00068FD8 File Offset: 0x000671D8
		private void TakeAll()
		{
			if (this.target == null)
			{
				return;
			}
			foreach (Reward reward in this.target.rewards)
			{
				if (!reward.Claimed)
				{
					reward.Claim();
				}
			}
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x00069048 File Offset: 0x00067248
		public void Skip()
		{
			this.skipClicked = true;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00069054 File Offset: 0x00067254
		public UniTask Show(Quest quest)
		{
			QuestCompletePanel.<Show>d__14 <Show>d__;
			<Show>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Show>d__.<>4__this = this;
			<Show>d__.quest = quest;
			<Show>d__.<>1__state = -1;
			<Show>d__.<>t__builder.Start<QuestCompletePanel.<Show>d__14>(ref <Show>d__);
			return <Show>d__.<>t__builder.Task;
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000690A0 File Offset: 0x000672A0
		private UniTask WaitForEndOfInteraction()
		{
			QuestCompletePanel.<WaitForEndOfInteraction>d__16 <WaitForEndOfInteraction>d__;
			<WaitForEndOfInteraction>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<WaitForEndOfInteraction>d__.<>4__this = this;
			<WaitForEndOfInteraction>d__.<>1__state = -1;
			<WaitForEndOfInteraction>d__.<>t__builder.Start<QuestCompletePanel.<WaitForEndOfInteraction>d__16>(ref <WaitForEndOfInteraction>d__);
			return <WaitForEndOfInteraction>d__.<>t__builder.Task;
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000690E4 File Offset: 0x000672E4
		private void SetupContent(Quest quest)
		{
			this.target = quest;
			if (quest == null)
			{
				return;
			}
			this.questNameText.text = quest.DisplayName;
			this.RewardEntryPool.ReleaseAll();
			foreach (Reward reward in quest.rewards)
			{
				RewardEntry rewardEntry = this.RewardEntryPool.Get(this.rewardEntryTemplate.transform.parent);
				rewardEntry.Setup(reward);
				rewardEntry.transform.SetAsLastSibling();
			}
		}

		// Token: 0x0400142A RID: 5162
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x0400142B RID: 5163
		[SerializeField]
		private TextMeshProUGUI questNameText;

		// Token: 0x0400142C RID: 5164
		[SerializeField]
		private RewardEntry rewardEntryTemplate;

		// Token: 0x0400142D RID: 5165
		[SerializeField]
		private Button skipButton;

		// Token: 0x0400142E RID: 5166
		[SerializeField]
		private Button takeAllButton;

		// Token: 0x0400142F RID: 5167
		private PrefabPool<RewardEntry> _rewardEntryPool;

		// Token: 0x04001430 RID: 5168
		private Quest target;

		// Token: 0x04001431 RID: 5169
		private bool skipClicked;
	}
}
