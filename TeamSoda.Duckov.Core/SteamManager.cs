using System;
using System.Text;
using AOT;
using Duckov;
using Duckov.Achievements;
using Steamworks;
using UnityEngine;

// Token: 0x02000136 RID: 310
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0002B673 File Offset: 0x00029873
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				Debug.Log("Creating steam manager");
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0002B6A1 File Offset: 0x000298A1
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0002B6AD File Offset: 0x000298AD
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0002B6B5 File Offset: 0x000298B5
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0002B6C4 File Offset: 0x000298C4
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary((AppId_t)3167020U))
			{
				Debug.Log("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
		AchievementManager.OnAchievementUnlocked += this.OnAchievementUnlocked;
		AchievementManager.OnAchievementDataLoaded += this.OnAchievementDataLoaded;
		RichPresenceManager.OnInstanceChanged = (Action<RichPresenceManager>)Delegate.Combine(RichPresenceManager.OnInstanceChanged, new Action<RichPresenceManager>(this.OnRichPresenceChanged));
		PlatformInfo.GetIDFunc = new Func<string>(SteamManager.GetID);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0002B808 File Offset: 0x00029A08
	private static string GetID()
	{
		if (SteamManager.s_instance == null)
		{
			return null;
		}
		if (!SteamManager.Initialized)
		{
			return null;
		}
		return SteamUser.GetSteamID().ToString();
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0002B840 File Offset: 0x00029A40
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
		AchievementManager.OnAchievementUnlocked -= this.OnAchievementUnlocked;
		AchievementManager.OnAchievementDataLoaded -= this.OnAchievementDataLoaded;
		RichPresenceManager.OnInstanceChanged = (Action<RichPresenceManager>)Delegate.Remove(RichPresenceManager.OnInstanceChanged, new Action<RichPresenceManager>(this.OnRichPresenceChanged));
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0002B8B4 File Offset: 0x00029AB4
	private void OnRichPresenceChanged(RichPresenceManager manager)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (manager == null)
		{
			return;
		}
		string steamDisplay = manager.GetSteamDisplay();
		if (!SteamFriends.SetRichPresence("steam_display", steamDisplay))
		{
			Debug.LogError("Failed setting rich presence: level = " + steamDisplay);
		}
		if (!SteamFriends.SetRichPresence("level", manager.levelDisplayNameRaw))
		{
			Debug.LogError("Failed setting rich presence: level = " + manager.levelDisplayNameRaw);
		}
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0002B920 File Offset: 0x00029B20
	private void OnAchievementDataLoaded(AchievementManager manager)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (manager == null)
		{
			return;
		}
		bool flag = false;
		foreach (string text in manager.UnlockedAchievements)
		{
			bool flag2;
			if (SteamUserStats.GetAchievement(text, out flag2) && !flag2)
			{
				SteamUserStats.SetAchievement(text);
				flag = true;
			}
		}
		if (flag)
		{
			SteamUserStats.StoreStats();
		}
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0002B9A0 File Offset: 0x00029BA0
	private void OnAchievementUnlocked(string achievementKey)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		SteamUserStats.SetAchievement(achievementKey);
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0002B9B8 File Offset: 0x00029BB8
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0002BA06 File Offset: 0x00029C06
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x040008E0 RID: 2272
	public const bool SteamEnabled = true;

	// Token: 0x040008E1 RID: 2273
	public const int AppID_Int = 3167020;

	// Token: 0x040008E2 RID: 2274
	protected static bool s_EverInitialized;

	// Token: 0x040008E3 RID: 2275
	protected static SteamManager s_instance;

	// Token: 0x040008E4 RID: 2276
	protected bool m_bInitialized;

	// Token: 0x040008E5 RID: 2277
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
