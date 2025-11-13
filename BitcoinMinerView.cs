using System;
using Duckov.Bitcoins;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001A3 RID: 419
public class BitcoinMinerView : View
{
	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00034A40 File Offset: 0x00032C40
	public static BitcoinMinerView Instance
	{
		get
		{
			return View.GetViewInstance<BitcoinMinerView>();
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00034A47 File Offset: 0x00032C47
	// (set) Token: 0x06000C69 RID: 3177 RVA: 0x00034A4E File Offset: 0x00032C4E
	[LocalizationKey("Default")]
	private string ActiveCommentKey
	{
		get
		{
			return "UI_BitcoinMiner_Active";
		}
		set
		{
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00034A50 File Offset: 0x00032C50
	// (set) Token: 0x06000C6B RID: 3179 RVA: 0x00034A57 File Offset: 0x00032C57
	[LocalizationKey("Default")]
	private string StoppedCommentKey
	{
		get
		{
			return "UI_BitcoinMiner_Stopped";
		}
		set
		{
		}
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00034A5C File Offset: 0x00032C5C
	protected override void Awake()
	{
		base.Awake();
		this.minerInventoryDisplay.onDisplayDoubleClicked += this.OnMinerInventoryEntryDoubleClicked;
		this.inventoryDisplay.onDisplayDoubleClicked += this.OnPlayerItemsDoubleClicked;
		this.storageDisplay.onDisplayDoubleClicked += this.OnPlayerItemsDoubleClicked;
		this.minerSlotsDisplay.onElementDoubleClicked += this.OnMinerSlotEntryDoubleClicked;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00034ACC File Offset: 0x00032CCC
	private void OnMinerSlotEntryDoubleClicked(ItemSlotCollectionDisplay display1, SlotDisplay slotDisplay)
	{
		Slot target = slotDisplay.Target;
		if (target == null)
		{
			return;
		}
		Item content = target.Content;
		if (content == null)
		{
			return;
		}
		ItemUtilities.SendToPlayer(content, false, PlayerStorage.Instance != null);
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00034B08 File Offset: 0x00032D08
	private void OnPlayerItemsDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
	{
		Item content = entry.Content;
		if (content == null)
		{
			return;
		}
		Item item = BitcoinMiner.Instance.Item;
		if (item == null)
		{
			return;
		}
		item.TryPlug(content, true, content.InInventory, 0);
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00034B4C File Offset: 0x00032D4C
	private void OnMinerInventoryEntryDoubleClicked(InventoryDisplay display, InventoryEntry entry, PointerEventData data)
	{
		Item content = entry.Content;
		if (content == null)
		{
			return;
		}
		if (data.button == PointerEventData.InputButton.Left)
		{
			ItemUtilities.SendToPlayer(content, false, true);
		}
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00034B7A File Offset: 0x00032D7A
	public static void Show()
	{
		if (BitcoinMinerView.Instance == null)
		{
			return;
		}
		if (BitcoinMiner.Instance == null)
		{
			return;
		}
		BitcoinMinerView.Instance.Open(null);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00034BA4 File Offset: 0x00032DA4
	protected override void OnOpen()
	{
		base.OnOpen();
		CharacterMainControl main = CharacterMainControl.Main;
		if (!(main == null))
		{
			Item characterItem = main.CharacterItem;
			if (!(characterItem == null))
			{
				BitcoinMiner instance = BitcoinMiner.Instance;
				if (!instance.Loading)
				{
					Item item = instance.Item;
					if (!(item == null))
					{
						this.inventoryDisplay.Setup(characterItem.Inventory, null, null, false, null);
						if (PlayerStorage.Inventory != null)
						{
							this.storageDisplay.gameObject.SetActive(true);
							this.storageDisplay.Setup(PlayerStorage.Inventory, null, null, false, null);
						}
						else
						{
							this.storageDisplay.gameObject.SetActive(false);
						}
						this.minerSlotsDisplay.Setup(item, false);
						this.minerInventoryDisplay.Setup(item.Inventory, null, null, false, null);
						this.fadeGroup.Show();
						return;
					}
				}
			}
		}
		Debug.Log("Failed");
		base.Close();
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00034C98 File Offset: 0x00032E98
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x00034CAB File Offset: 0x00032EAB
	private void FixedUpdate()
	{
		this.RefreshStatus();
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x00034CB4 File Offset: 0x00032EB4
	private void RefreshStatus()
	{
		if (BitcoinMiner.Instance.WorkPerSecond > 0.0)
		{
			TimeSpan remainingTime = BitcoinMiner.Instance.RemainingTime;
			TimeSpan timePerCoin = BitcoinMiner.Instance.TimePerCoin;
			this.remainingTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)remainingTime.TotalHours), remainingTime.Minutes, remainingTime.Seconds);
			this.timeEachCoinText.text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)timePerCoin.TotalHours), timePerCoin.Minutes, timePerCoin.Seconds);
			this.performanceText.text = string.Format("{0:0.#}", BitcoinMiner.Instance.Performance);
			this.commentText.text = this.ActiveCommentKey.ToPlainText();
		}
		else
		{
			this.remainingTimeText.text = "--:--:--";
			this.timeEachCoinText.text = "--:--:--";
			this.commentText.text = this.StoppedCommentKey.ToPlainText();
			this.performanceText.text = string.Format("{0:0.#}", BitcoinMiner.Instance.Performance);
		}
		this.fill.fillAmount = BitcoinMiner.Instance.NormalizedProgress;
	}

	// Token: 0x04000AC7 RID: 2759
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000AC8 RID: 2760
	[SerializeField]
	private InventoryDisplay inventoryDisplay;

	// Token: 0x04000AC9 RID: 2761
	[SerializeField]
	private InventoryDisplay storageDisplay;

	// Token: 0x04000ACA RID: 2762
	[SerializeField]
	private ItemSlotCollectionDisplay minerSlotsDisplay;

	// Token: 0x04000ACB RID: 2763
	[SerializeField]
	private InventoryDisplay minerInventoryDisplay;

	// Token: 0x04000ACC RID: 2764
	[SerializeField]
	private TextMeshProUGUI commentText;

	// Token: 0x04000ACD RID: 2765
	[SerializeField]
	private TextMeshProUGUI remainingTimeText;

	// Token: 0x04000ACE RID: 2766
	[SerializeField]
	private TextMeshProUGUI timeEachCoinText;

	// Token: 0x04000ACF RID: 2767
	[SerializeField]
	private TextMeshProUGUI performanceText;

	// Token: 0x04000AD0 RID: 2768
	[SerializeField]
	private Image fill;
}
