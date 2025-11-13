using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Sirenix.Utilities;
using Steamworks;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x02000271 RID: 625
	public class SteamWorkshopManager : MonoBehaviour
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x000499A8 File Offset: 0x00047BA8
		// (set) Token: 0x060013A0 RID: 5024 RVA: 0x000499AF File Offset: 0x00047BAF
		public static SteamWorkshopManager Instance { get; private set; }

		// Token: 0x060013A1 RID: 5025 RVA: 0x000499B7 File Offset: 0x00047BB7
		private void Awake()
		{
			SteamWorkshopManager.Instance = this;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x000499BF File Offset: 0x00047BBF
		private void OnEnable()
		{
			ModManager.Rescan();
			this.SendQueryDetailsRequest();
			ModManager.OnScan += this.OnScanMods;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x000499DD File Offset: 0x00047BDD
		private void OnDisable()
		{
			ModManager.OnScan -= this.OnScanMods;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x000499F0 File Offset: 0x00047BF0
		private void OnScanMods(List<ModInfo> list)
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			foreach (SteamUGCDetails_t steamUGCDetails_t in SteamWorkshopManager.ugcDetailsCache)
			{
				PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
				EItemState itemState = (EItemState)SteamUGC.GetItemState(nPublishedFileId);
				ulong num;
				string text;
				uint num2;
				if ((itemState | EItemState.k_EItemStateInstalled) == itemState && SteamUGC.GetItemInstallInfo(nPublishedFileId, out num, out text, 1024U, out num2))
				{
					ModInfo item;
					if (!ModManager.TryProcessModFolder(text, out item, true, nPublishedFileId.m_PublishedFileId))
					{
						Debug.LogError("Mod processing failed! \nPath:" + text);
					}
					else
					{
						list.Add(item);
					}
				}
			}
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00049A98 File Offset: 0x00047C98
		public void SendQueryDetailsRequest()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			if (this.CRSteamUGCQueryCompleted == null)
			{
				this.CRSteamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompleted));
			}
			HashSet<PublishedFileId_t> hashSet = new HashSet<PublishedFileId_t>();
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(array, numSubscribedItems);
			hashSet.AddRange(array);
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				if (modInfo.publishedFileId != 0UL)
				{
					hashSet.Add((PublishedFileId_t)modInfo.publishedFileId);
				}
			}
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(SteamUGC.CreateQueryUGCDetailsRequest(hashSet.ToArray<PublishedFileId_t>(), (uint)hashSet.Count));
			this.CRSteamUGCQueryCompleted.Set(hAPICall, null);
			new StringBuilder();
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00049B74 File Offset: 0x00047D74
		private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t completed, bool bIOFailure)
		{
			if (bIOFailure)
			{
				Debug.LogError("Steam UGC Query failed", base.gameObject);
				ModManager.Instance.ScanAndActivateMods();
				return;
			}
			UGCQueryHandle_t handle = completed.m_handle;
			uint unNumResultsReturned = completed.m_unNumResultsReturned;
			for (uint num = 0U; num < unNumResultsReturned; num += 1U)
			{
				SteamUGCDetails_t item;
				SteamUGC.GetQueryUGCResult(handle, num, out item);
				SteamWorkshopManager.ugcDetailsCache.Add(item);
			}
			SteamUGC.ReleaseQueryUGCRequest(handle);
			ModManager.Instance.ScanAndActivateMods();
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x00049BE0 File Offset: 0x00047DE0
		public UniTask<PublishedFileId_t> RequestNewWorkshopItemID()
		{
			SteamWorkshopManager.<RequestNewWorkshopItemID>d__14 <RequestNewWorkshopItemID>d__;
			<RequestNewWorkshopItemID>d__.<>t__builder = AsyncUniTaskMethodBuilder<PublishedFileId_t>.Create();
			<RequestNewWorkshopItemID>d__.<>4__this = this;
			<RequestNewWorkshopItemID>d__.<>1__state = -1;
			<RequestNewWorkshopItemID>d__.<>t__builder.Start<SteamWorkshopManager.<RequestNewWorkshopItemID>d__14>(ref <RequestNewWorkshopItemID>d__);
			return <RequestNewWorkshopItemID>d__.<>t__builder.Task;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00049C23 File Offset: 0x00047E23
		private void OnCreateItemResult(CreateItemResult_t result, bool bIOFailure)
		{
			Debug.Log("Creat Item Result Fired A");
			this.createItemResultFired = true;
			this.createItemResult = result;
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00049C3D File Offset: 0x00047E3D
		// (set) Token: 0x060013AA RID: 5034 RVA: 0x00049C44 File Offset: 0x00047E44
		public static ulong punBytesProcess { get; private set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x00049C4C File Offset: 0x00047E4C
		// (set) Token: 0x060013AC RID: 5036 RVA: 0x00049C53 File Offset: 0x00047E53
		public static ulong punBytesTotal { get; private set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x00049C5B File Offset: 0x00047E5B
		public static float UploadingProgress
		{
			get
			{
				return (float)(SteamWorkshopManager.punBytesProcess / SteamWorkshopManager.punBytesTotal);
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x00049C6D File Offset: 0x00047E6D
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x00049C75 File Offset: 0x00047E75
		public bool UploadSucceed { get; private set; }

		// Token: 0x060013B0 RID: 5040 RVA: 0x00049C80 File Offset: 0x00047E80
		public UniTask<bool> UploadWorkshopItem(string path, string changeNote = "Unknown")
		{
			SteamWorkshopManager.<UploadWorkshopItem>d__32 <UploadWorkshopItem>d__;
			<UploadWorkshopItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<UploadWorkshopItem>d__.<>4__this = this;
			<UploadWorkshopItem>d__.path = path;
			<UploadWorkshopItem>d__.changeNote = changeNote;
			<UploadWorkshopItem>d__.<>1__state = -1;
			<UploadWorkshopItem>d__.<>t__builder.Start<SteamWorkshopManager.<UploadWorkshopItem>d__32>(ref <UploadWorkshopItem>d__);
			return <UploadWorkshopItem>d__.<>t__builder.Task;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00049CD4 File Offset: 0x00047ED4
		public static bool IsOwner(ModInfo info)
		{
			if (!SteamManager.Initialized)
			{
				return false;
			}
			if (info.publishedFileId == 0UL)
			{
				return false;
			}
			foreach (SteamUGCDetails_t steamUGCDetails_t in SteamWorkshopManager.ugcDetailsCache)
			{
				if (steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId == info.publishedFileId)
				{
					return steamUGCDetails_t.m_ulSteamIDOwner == SteamUser.GetSteamID().m_SteamID;
				}
			}
			return false;
		}

		// Token: 0x04000E90 RID: 3728
		private CallResult<SteamUGCQueryCompleted_t> CRSteamUGCQueryCompleted;

		// Token: 0x04000E91 RID: 3729
		private CallResult<CreateItemResult_t> CRCreateItemResult;

		// Token: 0x04000E92 RID: 3730
		private UGCQueryHandle_t activeQueryHandle;

		// Token: 0x04000E93 RID: 3731
		private static List<SteamUGCDetails_t> ugcDetailsCache = new List<SteamUGCDetails_t>();

		// Token: 0x04000E94 RID: 3732
		private bool createItemResultFired;

		// Token: 0x04000E95 RID: 3733
		private CreateItemResult_t createItemResult;
	}
}
