using System;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000B9 RID: 185
public class BulletCountHUD : MonoBehaviour
{
	// Token: 0x0600060A RID: 1546 RVA: 0x0001B0B6 File Offset: 0x000192B6
	private void Awake()
	{
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0001B0B8 File Offset: 0x000192B8
	public void Update()
	{
		if (!this.characterMainControl)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl)
			{
				this.characterMainControl.OnHoldAgentChanged += this.OnHoldAgentChanged;
				this.characterMainControl.CharacterItem.Inventory.onContentChanged += this.OnInventoryChanged;
				if (this.characterMainControl.CurrentHoldItemAgent != null)
				{
					this.OnHoldAgentChanged(this.characterMainControl.CurrentHoldItemAgent);
				}
				this.ChangeTotalCount();
				this.capacityText.text = this.totalCount.ToString("D2");
			}
		}
		if (this.gunAgnet == null)
		{
			this.canvasGroup.alpha = 0f;
			return;
		}
		bool flag = false;
		this.canvasGroup.alpha = 1f;
		int num = this.gunAgnet.BulletCount;
		if (this.bulletCount != num)
		{
			this.bulletCount = num;
			this.bulletCountText.text = num.ToString("D2");
			flag = true;
		}
		if (flag)
		{
			UnityEvent onValueChangeEvent = this.OnValueChangeEvent;
			if (onValueChangeEvent != null)
			{
				onValueChangeEvent.Invoke();
			}
			if (this.bulletCount <= 0 && (this.totalCount <= 0 || !this.capacityText.gameObject.activeInHierarchy))
			{
				this.background.color = this.emptyBackgroundColor;
				return;
			}
			this.background.color = this.normalBackgroundColor;
		}
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x0001B230 File Offset: 0x00019430
	private void OnInventoryChanged(Inventory inventory, int index)
	{
		this.ChangeTotalCount();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001B238 File Offset: 0x00019438
	private void ChangeTotalCount()
	{
		int num = 0;
		if (this.gunAgnet)
		{
			num = this.gunAgnet.GetBulletCountInInventory();
		}
		if (this.totalCount != num)
		{
			this.totalCount = num;
			this.capacityText.text = this.totalCount.ToString("D2");
		}
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0001B28C File Offset: 0x0001948C
	private void OnDestroy()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.OnHoldAgentChanged -= this.OnHoldAgentChanged;
			this.characterMainControl.CharacterItem.Inventory.onContentChanged -= this.OnInventoryChanged;
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0001B2DE File Offset: 0x000194DE
	private void OnHoldAgentChanged(DuckovItemAgent newAgent)
	{
		if (newAgent == null)
		{
			this.gunAgnet = null;
		}
		this.gunAgnet = (newAgent as ItemAgent_Gun);
		this.ChangeTotalCount();
	}

	// Token: 0x04000595 RID: 1429
	private ItemAgent_Gun gunAgent;

	// Token: 0x04000596 RID: 1430
	private CharacterMainControl characterMainControl;

	// Token: 0x04000597 RID: 1431
	private ItemAgent_Gun gunAgnet;

	// Token: 0x04000598 RID: 1432
	public CanvasGroup canvasGroup;

	// Token: 0x04000599 RID: 1433
	public TextMeshProUGUI bulletCountText;

	// Token: 0x0400059A RID: 1434
	public TextMeshProUGUI capacityText;

	// Token: 0x0400059B RID: 1435
	public ProceduralImage background;

	// Token: 0x0400059C RID: 1436
	public Color normalBackgroundColor;

	// Token: 0x0400059D RID: 1437
	public Color emptyBackgroundColor;

	// Token: 0x0400059E RID: 1438
	private int bulletCount = -1;

	// Token: 0x0400059F RID: 1439
	private int totalCount = -1;

	// Token: 0x040005A0 RID: 1440
	public UnityEvent OnValueChangeEvent;
}
