using System;
using Duckov.BlackMarkets.UI;
using Duckov.Crops;
using Duckov.Crops.UI;
using Duckov.Endowment.UI;
using Duckov.MasterKeys.UI;
using Duckov.MiniGames;
using Duckov.MiniMaps.UI;
using Duckov.Quests.UI;
using Duckov.UI;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class ViewsProxy : MonoBehaviour
{
	// Token: 0x06000971 RID: 2417 RVA: 0x00029FF1 File Offset: 0x000281F1
	public void ShowInventoryView()
	{
		if (LevelManager.Instance.IsBaseLevel && PlayerStorage.Instance)
		{
			PlayerStorage.Instance.InteractableLootBox.InteractWithMainCharacter();
			return;
		}
		InventoryView.Show();
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0002A020 File Offset: 0x00028220
	public void ShowQuestView()
	{
		QuestView.Show();
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0002A027 File Offset: 0x00028227
	public void ShowMapView()
	{
		MiniMapView.Show();
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0002A02E File Offset: 0x0002822E
	public void ShowKeyView()
	{
		MasterKeysView.Show();
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0002A035 File Offset: 0x00028235
	public void ShowPlayerStats()
	{
		PlayerStatsView.Instance.Open(null);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0002A042 File Offset: 0x00028242
	public void ShowEndowmentView()
	{
		EndowmentSelectionPanel.Show();
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0002A049 File Offset: 0x00028249
	public void ShowMapSelectionView()
	{
		MapSelectionView.Instance.Open(null);
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0002A056 File Offset: 0x00028256
	public void ShowRepairView()
	{
		ItemRepairView.Instance.Open(null);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002A063 File Offset: 0x00028263
	public void ShowFormulasIndexView()
	{
		FormulasIndexView.Show();
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0002A06A File Offset: 0x0002826A
	public void ShowBitcoinView()
	{
		BitcoinMinerView.Show();
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0002A071 File Offset: 0x00028271
	public void ShowStorageDock()
	{
		StorageDock.Show();
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0002A078 File Offset: 0x00028278
	public void ShowBlackMarket_Demands()
	{
		BlackMarketView.Show(BlackMarketView.Mode.Demand);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0002A080 File Offset: 0x00028280
	public void ShowBlackMarket_Supplies()
	{
		BlackMarketView.Show(BlackMarketView.Mode.Supply);
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0002A088 File Offset: 0x00028288
	public void ShowSleepView()
	{
		SleepView.Show();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0002A08F File Offset: 0x0002828F
	public void ShowATMView()
	{
		ATMView.Show();
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0002A096 File Offset: 0x00028296
	public void ShowDecomposeView()
	{
		ItemDecomposeView.Show();
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x0002A09D File Offset: 0x0002829D
	public void ShowGardenView(Garden garnden)
	{
		GardenView.Show(garnden);
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0002A0A5 File Offset: 0x000282A5
	public void ShowGamingConsoleView(GamingConsole console)
	{
		GamingConsoleView.Show(console);
	}
}
