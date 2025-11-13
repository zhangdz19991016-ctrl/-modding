using System;
using Duckov.Achievements;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Duckov
{
	// Token: 0x02000240 RID: 576
	public class SocialManager : MonoBehaviour
	{
		// Token: 0x060011F8 RID: 4600 RVA: 0x000458AA File Offset: 0x00043AAA
		private void Awake()
		{
			AchievementManager.OnAchievementUnlocked += this.UnlockAchievement;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000458BD File Offset: 0x00043ABD
		private void Start()
		{
			Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x000458D5 File Offset: 0x00043AD5
		private void ProcessAuthentication(bool success)
		{
			if (success)
			{
				this.initialized = true;
				Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000458F2 File Offset: 0x00043AF2
		private void ProcessLoadedAchievements(IAchievement[] loadedAchievements)
		{
			this._achievement_cache = loadedAchievements;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x000458FB File Offset: 0x00043AFB
		private void UnlockAchievement(string id)
		{
			if (this.initialized)
			{
				return;
			}
			Social.ReportProgress(id, 100.0, new Action<bool>(this.OnReportProgressResult));
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00045921 File Offset: 0x00043B21
		private void OnReportProgressResult(bool success)
		{
			Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
		}

		// Token: 0x04000DDC RID: 3548
		private bool initialized;

		// Token: 0x04000DDD RID: 3549
		private IAchievement[] _achievement_cache;
	}
}
