using System;
using Duckov.Rules;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.CustomerService
{
	// Token: 0x020003FD RID: 1021
	public class QuestionairButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06002505 RID: 9477 RVA: 0x00080BC8 File Offset: 0x0007EDC8
		public string GenerateQuestionair()
		{
			SystemLanguage currentLanguage = LocalizationManager.CurrentLanguage;
			string address;
			if (currentLanguage != SystemLanguage.Japanese)
			{
				if (currentLanguage == SystemLanguage.ChineseSimplified)
				{
					address = this.addressCN;
				}
				else
				{
					address = this.addressEN;
				}
			}
			else
			{
				address = this.addressJP;
			}
			int currentSlot = SavesSystem.CurrentSlot;
			string id = string.Format("{0}_{1}", PlatformInfo.Platform, PlatformInfo.GetID());
			string time = string.Format("{0:0}", GameClock.GetRealTimePlayedOfSaveSlot(currentSlot).TotalMinutes);
			string level = string.Format("{0}", EXPManager.Level);
			RuleIndex ruleIndexOfSaveSlot = GameRulesManager.GetRuleIndexOfSaveSlot(currentSlot);
			int num = 0;
			if (ruleIndexOfSaveSlot <= RuleIndex.Easy)
			{
				if (ruleIndexOfSaveSlot != RuleIndex.Standard)
				{
					if (ruleIndexOfSaveSlot != RuleIndex.Custom)
					{
						if (ruleIndexOfSaveSlot == RuleIndex.Easy)
						{
							num = 2;
						}
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 3;
				}
			}
			else if (ruleIndexOfSaveSlot != RuleIndex.ExtraEasy)
			{
				if (ruleIndexOfSaveSlot != RuleIndex.Hard)
				{
					if (ruleIndexOfSaveSlot == RuleIndex.ExtraHard)
					{
						num = 5;
					}
				}
				else
				{
					num = 4;
				}
			}
			else
			{
				num = 1;
			}
			string difficulty = string.Format("{0}", num);
			return this.format.Format(new
			{
				address,
				id,
				time,
				level,
				difficulty
			});
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x00080CD2 File Offset: 0x0007EED2
		public void OnPointerClick(PointerEventData eventData)
		{
			Application.OpenURL(this.GenerateQuestionair());
		}

		// Token: 0x0400192D RID: 6445
		private string addressCN = "rsmTLx1";

		// Token: 0x0400192E RID: 6446
		private string addressJP = "mHE3yAa";

		// Token: 0x0400192F RID: 6447
		private string addressEN = "YdoJpod";

		// Token: 0x04001930 RID: 6448
		private string format = "https://usersurvey.biligame.com/vm/{address}.aspx?sojumpparm={id}|{difficulty}|{time}|{level}";
	}
}
