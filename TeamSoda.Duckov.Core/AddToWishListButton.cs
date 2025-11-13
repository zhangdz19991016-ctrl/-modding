using System;
using Duckov;
using SodaCraft.Localizations;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000165 RID: 357
public class AddToWishListButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000AEB RID: 2795 RVA: 0x0002F77F File Offset: 0x0002D97F
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			AddToWishListButton.ShowPage();
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0002F790 File Offset: 0x0002D990
	public static void ShowPage()
	{
		if (SteamManager.Initialized)
		{
			SteamFriends.ActivateGameOverlayToStore(new AppId_t(3167020U), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			return;
		}
		if (GameMetaData.Instance.Platform == Platform.Steam)
		{
			Application.OpenURL("https://store.steampowered.com/app/3167020/");
			return;
		}
		if (LocalizationManager.CurrentLanguage == SystemLanguage.ChineseSimplified)
		{
			Application.OpenURL("https://game.bilibili.com/duckov/");
			return;
		}
		Application.OpenURL("https://www.duckov.com");
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0002F7EB File Offset: 0x0002D9EB
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400097E RID: 2430
	private const string url = "https://store.steampowered.com/app/3167020/";

	// Token: 0x0400097F RID: 2431
	private const string CNUrl = "https://game.bilibili.com/duckov/";

	// Token: 0x04000980 RID: 2432
	private const string ENUrl = "https://www.duckov.com";

	// Token: 0x04000981 RID: 2433
	private const uint appid = 3167020U;
}
