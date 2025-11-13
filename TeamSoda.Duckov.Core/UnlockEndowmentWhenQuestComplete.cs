using System;
using Duckov.Endowment;
using Duckov.Quests;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class UnlockEndowmentWhenQuestComplete : MonoBehaviour
{
	// Token: 0x060009A9 RID: 2473 RVA: 0x0002A5B8 File Offset: 0x000287B8
	private void Awake()
	{
		if (this.quest == null)
		{
			this.quest = base.GetComponent<Quest>();
		}
		if (this.quest != null)
		{
			this.quest.onCompleted += this.OnQuestCompleted;
		}
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0002A604 File Offset: 0x00028804
	private void Start()
	{
		if (this.quest.Complete && !EndowmentManager.GetEndowmentUnlocked(this.endowmentToUnlock))
		{
			EndowmentManager.UnlockEndowment(this.endowmentToUnlock);
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0002A62C File Offset: 0x0002882C
	private void OnDestroy()
	{
		if (this.quest != null)
		{
			this.quest.onCompleted -= this.OnQuestCompleted;
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0002A653 File Offset: 0x00028853
	private void OnQuestCompleted(Quest quest)
	{
		if (!EndowmentManager.GetEndowmentUnlocked(this.endowmentToUnlock))
		{
			EndowmentManager.UnlockEndowment(this.endowmentToUnlock);
		}
	}

	// Token: 0x04000891 RID: 2193
	[SerializeField]
	private Quest quest;

	// Token: 0x04000892 RID: 2194
	[SerializeField]
	private EndowmentIndex endowmentToUnlock;
}
