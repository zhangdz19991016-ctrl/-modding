using System;
using Duckov.MiniGames;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class GamingConsoleView : View
{
	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000D74 RID: 3444 RVA: 0x000382EE File Offset: 0x000364EE
	public static GamingConsoleView Instance
	{
		get
		{
			return View.GetViewInstance<GamingConsoleView>();
		}
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x000382F8 File Offset: 0x000364F8
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.Setup(this.target);
		if (CharacterMainControl.Main)
		{
			this.characterInventory.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, null, false, null);
		}
		if (PetProxy.PetInventory)
		{
			this.petInventory.Setup(PetProxy.PetInventory, null, null, false, null);
		}
		if (PlayerStorage.Inventory)
		{
			this.storageInventory.Setup(PlayerStorage.Inventory, null, null, false, null);
		}
		this.RefreshConsole();
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x00038392 File Offset: 0x00036592
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x000383A8 File Offset: 0x000365A8
	private void SetTarget(GamingConsole target)
	{
		if (this.target != null)
		{
			this.target.onContentChanged -= this.OnTargetContentChanged;
		}
		if (target != null)
		{
			this.target = target;
			return;
		}
		this.target = UnityEngine.Object.FindObjectOfType<GamingConsole>();
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x000383F8 File Offset: 0x000365F8
	private void Setup(GamingConsole target)
	{
		this.SetTarget(target);
		if (this.target == null)
		{
			return;
		}
		this.target.onContentChanged += this.OnTargetContentChanged;
		this.consoleSlotDisplay.Setup(this.target.ConsoleSlot);
		this.monitorSlotDisplay.Setup(this.target.MonitorSlot);
		this.RefreshConsole();
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x00038464 File Offset: 0x00036664
	private void OnTargetContentChanged(GamingConsole console)
	{
		this.RefreshConsole();
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x0003846C File Offset: 0x0003666C
	private void RefreshConsole()
	{
		if (this.isBeingDestroyed)
		{
			return;
		}
		Slot consoleSlot = this.target.ConsoleSlot;
		if (consoleSlot == null)
		{
			return;
		}
		Item content = consoleSlot.Content;
		this.consoleSlotCollectionDisplay.gameObject.SetActive(content);
		if (content)
		{
			this.consoleSlotCollectionDisplay.Setup(content, false);
		}
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x000384C4 File Offset: 0x000366C4
	internal static void Show(GamingConsole console)
	{
		GamingConsoleView.Instance.target = console;
		GamingConsoleView.Instance.Open(null);
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000384DC File Offset: 0x000366DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.isBeingDestroyed = true;
	}

	// Token: 0x04000B83 RID: 2947
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B84 RID: 2948
	[SerializeField]
	private InventoryDisplay characterInventory;

	// Token: 0x04000B85 RID: 2949
	[SerializeField]
	private InventoryDisplay petInventory;

	// Token: 0x04000B86 RID: 2950
	[SerializeField]
	private InventoryDisplay storageInventory;

	// Token: 0x04000B87 RID: 2951
	[SerializeField]
	private SlotDisplay monitorSlotDisplay;

	// Token: 0x04000B88 RID: 2952
	[SerializeField]
	private SlotDisplay consoleSlotDisplay;

	// Token: 0x04000B89 RID: 2953
	[SerializeField]
	private ItemSlotCollectionDisplay consoleSlotCollectionDisplay;

	// Token: 0x04000B8A RID: 2954
	private GamingConsole target;

	// Token: 0x04000B8B RID: 2955
	private bool isBeingDestroyed;
}
