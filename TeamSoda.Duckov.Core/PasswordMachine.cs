using System;
using System.Collections.Generic;
using Duckov;
using Duckov.Quests;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class PasswordMachine : MonoBehaviour
{
	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x00019C64 File Offset: 0x00017E64
	public int maxNum
	{
		get
		{
			return this.rightCode.Count;
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00019C71 File Offset: 0x00017E71
	private void Start()
	{
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00019C73 File Offset: 0x00017E73
	private void Update()
	{
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00019C78 File Offset: 0x00017E78
	private bool CheckConditions()
	{
		if (this.conditions.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < this.conditions.Count; i++)
		{
			if (!(this.conditions[i] == null) && !this.conditions[i].Evaluate())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00019CD4 File Offset: 0x00017ED4
	public void InputNum(int num)
	{
		if (this.nums.Count < this.maxNum)
		{
			this.nums.Add(num);
		}
		AudioManager.Post(string.Format("SFX/Special/Phone/phone_{0}", num), base.gameObject);
		AudioManager.Post("SFX/Special/Phone/phone_key_dial", base.gameObject);
		this.dialogueBubbleProxy.Pop(this.CurrentNums(), this.popSpeed);
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00019D44 File Offset: 0x00017F44
	public void DeleteNum()
	{
		if (this.nums.Count > 0)
		{
			this.nums.RemoveAt(this.nums.Count - 1);
			AudioManager.Post("SFX/Special/Phone/phone_hash", base.gameObject);
			AudioManager.Post("SFX/Special/Phone/phone_key_dial", base.gameObject);
		}
		this.dialogueBubbleProxy.Pop(this.CurrentNums(), this.popSpeed);
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00019DB0 File Offset: 0x00017FB0
	public void Confirm()
	{
		if (this.rightCode.Count != this.nums.Count)
		{
			this.nums.Clear();
			this.dialogueBubbleProxy.Pop(this.wrongPassWorldKey.ToPlainText(), this.popSpeed);
			AudioManager.Post("SFX/Special/Phone/phone_busy", base.gameObject);
			return;
		}
		for (int i = 0; i < this.rightCode.Count; i++)
		{
			if (this.rightCode[i] != this.nums[i])
			{
				this.nums.Clear();
				this.dialogueBubbleProxy.Pop(this.wrongPassWorldKey.ToPlainText(), this.popSpeed);
				AudioManager.Post("SFX/Special/Phone/phone_busy", base.gameObject);
				return;
			}
		}
		if (!this.CheckConditions())
		{
			this.nums.Clear();
			this.dialogueBubbleProxy.Pop(this.wrongTimeKey.ToPlainText(), this.popSpeed);
			AudioManager.Post("SFX/Special/Phone/phone_busy", base.gameObject);
			return;
		}
		this.nums.Clear();
		this.dialogueBubbleProxy.Pop(this.rightKey.ToPlainText(), this.popSpeed);
		this.activeObject.SetActive(true);
		this.interactBoxCollider.enabled = false;
		AudioManager.Post("SFX/Special/Phone/phone_ringing", base.gameObject);
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00019F08 File Offset: 0x00018108
	private string CurrentNums()
	{
		string text = "";
		for (int i = 0; i < this.nums.Count; i++)
		{
			text += this.nums[i].ToString();
		}
		for (int j = 0; j < this.maxNum - this.nums.Count; j++)
		{
			text += "_";
		}
		return text;
	}

	// Token: 0x0400053D RID: 1341
	public List<Condition> conditions;

	// Token: 0x0400053E RID: 1342
	[Tooltip("从下到上")]
	public List<int> rightCode;

	// Token: 0x0400053F RID: 1343
	private List<int> nums = new List<int>();

	// Token: 0x04000540 RID: 1344
	[LocalizationKey("Default")]
	[SerializeField]
	private string wrongTimeKey = "Passworld_WrongTime";

	// Token: 0x04000541 RID: 1345
	[LocalizationKey("Default")]
	[SerializeField]
	private string wrongPassWorldKey = "Passworld_WrongNumber";

	// Token: 0x04000542 RID: 1346
	[LocalizationKey("Default")]
	[SerializeField]
	private string rightKey = "Passworld_Right";

	// Token: 0x04000543 RID: 1347
	[SerializeField]
	private DialogueBubbleProxy dialogueBubbleProxy;

	// Token: 0x04000544 RID: 1348
	[SerializeField]
	private GameObject activeObject;

	// Token: 0x04000545 RID: 1349
	[SerializeField]
	private BoxCollider interactBoxCollider;

	// Token: 0x04000546 RID: 1350
	private float popSpeed = 30f;
}
