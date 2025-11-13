using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x0200006A RID: 106
public class CheatingManager : MonoBehaviour
{
	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x0600041B RID: 1051 RVA: 0x00012088 File Offset: 0x00010288
	public static CheatingManager Instance
	{
		get
		{
			return CheatingManager._instance;
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001208F File Offset: 0x0001028F
	private void Awake()
	{
		CheatingManager._instance = this;
		CheatMode.Activate();
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001209C File Offset: 0x0001029C
	private void Update()
	{
		if (!CheatMode.Active)
		{
			return;
		}
		if (!CharacterMainControl.Main)
		{
			return;
		}
		if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.equalsKey.wasPressedThisFrame)
		{
			this.ToggleInvincible();
		}
		if (Keyboard.current != null && Keyboard.current.numpadMultiplyKey.wasPressedThisFrame)
		{
			this.typing = !this.typing;
			if (this.typing)
			{
				this.typingID = 0;
				this.LogCurrentTypingID();
			}
			else
			{
				this.LockItem();
			}
		}
		this.UpdateTyping();
		if (Keyboard.current != null && this.typing && Keyboard.current.backspaceKey.wasPressedThisFrame && this.typingID > 0)
		{
			this.typingID /= 10;
			this.LogCurrentTypingID();
		}
		if (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed && Mouse.current.backButton.wasPressedThisFrame)
		{
			this.CheatMove();
		}
		if (Keyboard.current != null && Keyboard.current.leftAltKey.isPressed && Keyboard.current.sKey.wasPressedThisFrame)
		{
			SleepView.Instance.Open(null);
		}
		if (Keyboard.current != null && Keyboard.current.numpadPlusKey.wasPressedThisFrame)
		{
			if (this.typing)
			{
				this.LockItem();
				this.typing = false;
			}
			this.CreateItem(this.lockedItem, 1);
		}
		if (Keyboard.current != null && Keyboard.current.numpadMinusKey.wasPressedThisFrame)
		{
			int displayingItemID = ItemHoveringUI.DisplayingItemID;
			if (displayingItemID > 0)
			{
				this.SetTypedItem(displayingItemID);
				this.CreateItem(this.lockedItem, 1);
			}
		}
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00012248 File Offset: 0x00010448
	private void UpdateTyping()
	{
		if (Keyboard.current != null && Keyboard.current.numpad0Key.wasPressedThisFrame)
		{
			this.TypeOne(0);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad1Key.wasPressedThisFrame)
		{
			this.TypeOne(1);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad2Key.wasPressedThisFrame)
		{
			this.TypeOne(2);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad3Key.wasPressedThisFrame)
		{
			this.TypeOne(3);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad4Key.wasPressedThisFrame)
		{
			this.TypeOne(4);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad5Key.wasPressedThisFrame)
		{
			this.TypeOne(5);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad6Key.wasPressedThisFrame)
		{
			this.TypeOne(6);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad7Key.wasPressedThisFrame)
		{
			this.TypeOne(7);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad8Key.wasPressedThisFrame)
		{
			this.TypeOne(8);
			return;
		}
		if (Keyboard.current != null && Keyboard.current.numpad9Key.wasPressedThisFrame)
		{
			this.TypeOne(9);
		}
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00012398 File Offset: 0x00010598
	private void LogCurrentTypingID()
	{
		if (this.typingID <= 0)
		{
			CharacterMainControl.Main.PopText("_", 999f);
			return;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.typingID);
		if (metaData.id > 0)
		{
			CharacterMainControl.Main.PopText(string.Format(" {0}_  ({1})", this.typingID, metaData.DisplayName), 999f);
			return;
		}
		CharacterMainControl.Main.PopText(string.Format("{0}_", this.typingID), 999f);
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00012428 File Offset: 0x00010628
	private void TypeOne(int i)
	{
		this.typingID = this.typingID * 10 + i;
		this.LogCurrentTypingID();
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00012441 File Offset: 0x00010641
	private void SetTypedItem(int id)
	{
		this.typingID = id;
		this.LockItem();
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00012450 File Offset: 0x00010650
	private void LockItem()
	{
		this.typing = false;
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(this.typingID);
		if (metaData.id <= 0)
		{
			CharacterMainControl.Main.PopText("没有这个物品。", 999f);
			return;
		}
		this.lockedItem = this.typingID;
		CharacterMainControl.Main.PopText(metaData.DisplayName + " 已选定", 999f);
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000124BC File Offset: 0x000106BC
	public void CreateItem(int id, int quantity = 1)
	{
		if (ItemAssetsCollection.GetMetaData(id).id <= 0)
		{
			CharacterMainControl.Main.PopText("没有这个物品。", 999f);
			return;
		}
		this.CreateItemAsync(id, quantity).Forget();
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x000124FC File Offset: 0x000106FC
	private UniTaskVoid CreateItemAsync(int id, int quantity = 1)
	{
		CheatingManager.<CreateItemAsync>d__15 <CreateItemAsync>d__;
		<CreateItemAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateItemAsync>d__.id = id;
		<CreateItemAsync>d__.quantity = quantity;
		<CreateItemAsync>d__.<>1__state = -1;
		<CreateItemAsync>d__.<>t__builder.Start<CheatingManager.<CreateItemAsync>d__15>(ref <CreateItemAsync>d__);
		return <CreateItemAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00012547 File Offset: 0x00010747
	private void ToggleTypeing()
	{
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001254C File Offset: 0x0001074C
	public void ToggleInvincible()
	{
		this.isInvincible = !this.isInvincible;
		CharacterMainControl.Main.Health.SetInvincible(this.isInvincible);
		CharacterMainControl.Main.PopText(this.isInvincible ? "我无敌了" : "我不无敌了", -1f);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x000125A0 File Offset: 0x000107A0
	public void CheatMove()
	{
		Vector2 v = Mouse.current.position.ReadValue();
		Ray ray = LevelManager.Instance.GameCamera.renderCamera.ScreenPointToRay(v);
		LayerMask mask = GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, mask, QueryTriggerInteraction.Ignore))
		{
			CharacterMainControl.Main.SetPosition(raycastHit.point);
		}
	}

	// Token: 0x04000311 RID: 785
	private static CheatingManager _instance;

	// Token: 0x04000312 RID: 786
	private bool isInvincible;

	// Token: 0x04000313 RID: 787
	private bool typing;

	// Token: 0x04000314 RID: 788
	private int typingID;

	// Token: 0x04000315 RID: 789
	private int lockedItem;
}
