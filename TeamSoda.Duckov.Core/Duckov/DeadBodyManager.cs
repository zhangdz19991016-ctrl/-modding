using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules;
using Duckov.Scenes;
using ItemStatsSystem.Data;
using Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Duckov
{
	// Token: 0x02000242 RID: 578
	public class DeadBodyManager : MonoBehaviour
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00045A1B File Offset: 0x00043C1B
		// (set) Token: 0x06001209 RID: 4617 RVA: 0x00045A22 File Offset: 0x00043C22
		public static DeadBodyManager Instance { get; private set; }

		// Token: 0x0600120A RID: 4618 RVA: 0x00045A2A File Offset: 0x00043C2A
		private void AppendDeathInfo(DeadBodyManager.DeathInfo deathInfo)
		{
			while (this.deaths.Count >= GameRulesManager.Current.SaveDeadbodyCount)
			{
				this.deaths.RemoveAt(0);
			}
			this.deaths.Add(deathInfo);
			this.Save();
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00045A63 File Offset: 0x00043C63
		private static List<DeadBodyManager.DeathInfo> LoadDeathInfos()
		{
			return SavesSystem.Load<List<DeadBodyManager.DeathInfo>>("DeathList");
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00045A70 File Offset: 0x00043C70
		internal static void RecordDeath(CharacterMainControl mainCharacter)
		{
			if (DeadBodyManager.Instance == null)
			{
				Debug.LogError("DeadBodyManager Instance is null");
				return;
			}
			DeadBodyManager.DeathInfo deathInfo = new DeadBodyManager.DeathInfo();
			deathInfo.valid = true;
			deathInfo.raidID = RaidUtilities.CurrentRaid.ID;
			deathInfo.subSceneID = MultiSceneCore.ActiveSubSceneID;
			deathInfo.worldPosition = mainCharacter.transform.position;
			deathInfo.itemTreeData = ItemTreeData.FromItem(mainCharacter.CharacterItem);
			DeadBodyManager.Instance.AppendDeathInfo(deathInfo);
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00045AEC File Offset: 0x00043CEC
		private void Awake()
		{
			DeadBodyManager.Instance = this;
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
			this.deaths.Clear();
			List<DeadBodyManager.DeathInfo> list = DeadBodyManager.LoadDeathInfos();
			if (list != null)
			{
				this.deaths.AddRange(list);
			}
			SavesSystem.OnCollectSaveData += this.Save;
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00045B41 File Offset: 0x00043D41
		private void OnDestroy()
		{
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00045B65 File Offset: 0x00043D65
		private void Save()
		{
			SavesSystem.Save<List<DeadBodyManager.DeathInfo>>("DeathList", this.deaths);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00045B78 File Offset: 0x00043D78
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Spawning bodies";
			if (!LevelConfig.SpawnTomb)
			{
				return;
			}
			foreach (DeadBodyManager.DeathInfo info in this.deaths)
			{
				if (this.ShouldSpawnDeadBody(info))
				{
					this.SpawnDeadBody(info).Forget();
				}
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00045BEC File Offset: 0x00043DEC
		private UniTask SpawnDeadBody(DeadBodyManager.DeathInfo info)
		{
			DeadBodyManager.<SpawnDeadBody>d__13 <SpawnDeadBody>d__;
			<SpawnDeadBody>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SpawnDeadBody>d__.info = info;
			<SpawnDeadBody>d__.<>1__state = -1;
			<SpawnDeadBody>d__.<>t__builder.Start<DeadBodyManager.<SpawnDeadBody>d__13>(ref <SpawnDeadBody>d__);
			return <SpawnDeadBody>d__.<>t__builder.Task;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00045C2F File Offset: 0x00043E2F
		private static void NotifyDeadbodyTouched(DeadBodyManager.DeathInfo info)
		{
			if (DeadBodyManager.Instance == null)
			{
				return;
			}
			DeadBodyManager.Instance.OnDeadbodyTouched(info);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00045C4C File Offset: 0x00043E4C
		private void OnDeadbodyTouched(DeadBodyManager.DeathInfo info)
		{
			DeadBodyManager.DeathInfo deathInfo = this.deaths.Find((DeadBodyManager.DeathInfo e) => e.raidID == info.raidID);
			if (deathInfo == null)
			{
				return;
			}
			deathInfo.touched = true;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00045C8C File Offset: 0x00043E8C
		private bool ShouldSpawnDeadBody(DeadBodyManager.DeathInfo info)
		{
			return info != null && GameRulesManager.Current.SpawnDeadBody && LevelManager.Instance && LevelManager.Instance.IsRaidMap && info != null && info.valid && !info.touched && !(MultiSceneCore.ActiveSubSceneID != info.subSceneID);
		}

		// Token: 0x04000DE1 RID: 3553
		private List<DeadBodyManager.DeathInfo> deaths = new List<DeadBodyManager.DeathInfo>();

		// Token: 0x02000533 RID: 1331
		[Serializable]
		public class DeathInfo
		{
			// Token: 0x04001EA2 RID: 7842
			public bool valid;

			// Token: 0x04001EA3 RID: 7843
			public uint raidID;

			// Token: 0x04001EA4 RID: 7844
			public string subSceneID;

			// Token: 0x04001EA5 RID: 7845
			public Vector3 worldPosition;

			// Token: 0x04001EA6 RID: 7846
			public ItemTreeData itemTreeData;

			// Token: 0x04001EA7 RID: 7847
			public bool spawned;

			// Token: 0x04001EA8 RID: 7848
			public bool touched;
		}
	}
}
