using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F8 RID: 504
public class SplitDialogue : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0003B9FA File Offset: 0x00039BFA
	public static SplitDialogue Instance
	{
		get
		{
			if (GameplayUIManager.Instance == null)
			{
				return null;
			}
			return GameplayUIManager.Instance.SplitDialogue;
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0003BA15 File Offset: 0x00039C15
	private void OnEnable()
	{
		View.OnActiveViewChanged += this.OnActiveViewChanged;
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0003BA28 File Offset: 0x00039C28
	private void OnDisable()
	{
		View.OnActiveViewChanged -= this.OnActiveViewChanged;
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0003BA3B File Offset: 0x00039C3B
	private void OnActiveViewChanged()
	{
		this.Hide();
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0003BA43 File Offset: 0x00039C43
	private void Awake()
	{
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0003BA7D File Offset: 0x00039C7D
	private void OnSliderValueChanged(float value)
	{
		this.RefreshCountText();
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0003BA88 File Offset: 0x00039C88
	private void RefreshCountText()
	{
		this.countText.text = this.slider.value.ToString("0");
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0003BAB8 File Offset: 0x00039CB8
	private void OnConfirmButtonClicked()
	{
		if (this.status != SplitDialogue.Status.Normal)
		{
			return;
		}
		this.Confirm().Forget();
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0003BAD0 File Offset: 0x00039CD0
	private void Setup(Item target, Inventory destination = null, int destinationIndex = -1)
	{
		this.target = target;
		this.destination = destination;
		this.destinationIndex = destinationIndex;
		this.slider.minValue = 1f;
		this.slider.maxValue = (float)target.StackCount;
		this.slider.value = (float)(target.StackCount - 1) / 2f;
		this.RefreshCountText();
		this.SwitchStatus(SplitDialogue.Status.Normal);
		this.cachedInInventory = target.InInventory;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0003BB47 File Offset: 0x00039D47
	public void Cancel()
	{
		if (this.status != SplitDialogue.Status.Normal)
		{
			return;
		}
		this.SwitchStatus(SplitDialogue.Status.Canceled);
		this.Hide();
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0003BB60 File Offset: 0x00039D60
	private UniTask Confirm()
	{
		SplitDialogue.<Confirm>d__22 <Confirm>d__;
		<Confirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Confirm>d__.<>4__this = this;
		<Confirm>d__.<>1__state = -1;
		<Confirm>d__.<>t__builder.Start<SplitDialogue.<Confirm>d__22>(ref <Confirm>d__);
		return <Confirm>d__.<>t__builder.Task;
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0003BBA3 File Offset: 0x00039DA3
	private void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0003BBB0 File Offset: 0x00039DB0
	private UniTask DoSplit(int value)
	{
		SplitDialogue.<DoSplit>d__24 <DoSplit>d__;
		<DoSplit>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DoSplit>d__.<>4__this = this;
		<DoSplit>d__.value = value;
		<DoSplit>d__.<>1__state = -1;
		<DoSplit>d__.<>t__builder.Start<SplitDialogue.<DoSplit>d__24>(ref <DoSplit>d__);
		return <DoSplit>d__.<>t__builder.Task;
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0003BBFC File Offset: 0x00039DFC
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject == base.gameObject)
		{
			this.Cancel();
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0003BC2C File Offset: 0x00039E2C
	private void SwitchStatus(SplitDialogue.Status status)
	{
		this.status = status;
		this.normalIndicator.SetActive(status == SplitDialogue.Status.Normal);
		this.busyIndicator.SetActive(status == SplitDialogue.Status.Busy);
		this.completeIndicator.SetActive(status == SplitDialogue.Status.Complete);
		switch (status)
		{
		default:
			return;
		}
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0003BC87 File Offset: 0x00039E87
	public static void SetupAndShow(Item item)
	{
		if (SplitDialogue.Instance == null)
		{
			return;
		}
		SplitDialogue.Instance.Setup(item, null, -1);
		SplitDialogue.Instance.fadeGroup.Show();
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0003BCB3 File Offset: 0x00039EB3
	public static void SetupAndShow(Item item, Inventory destinationInventory, int destinationIndex)
	{
		if (SplitDialogue.Instance == null)
		{
			return;
		}
		SplitDialogue.Instance.Setup(item, destinationInventory, destinationIndex);
		SplitDialogue.Instance.fadeGroup.Show();
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0003BCE8 File Offset: 0x00039EE8
	[CompilerGenerated]
	private void <DoSplit>g__Send|24_0(Item item)
	{
		item.Detach();
		if (this.destination != null && this.destination.Capacity > this.destinationIndex && this.destination.GetItemAt(this.destinationIndex) == null)
		{
			this.destination.AddAt(item, this.destinationIndex);
			return;
		}
		ItemUtilities.SendToPlayerCharacterInventory(item, true);
	}

	// Token: 0x04000C47 RID: 3143
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C48 RID: 3144
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000C49 RID: 3145
	[SerializeField]
	private TextMeshProUGUI countText;

	// Token: 0x04000C4A RID: 3146
	[SerializeField]
	private GameObject normalIndicator;

	// Token: 0x04000C4B RID: 3147
	[SerializeField]
	private GameObject busyIndicator;

	// Token: 0x04000C4C RID: 3148
	[SerializeField]
	private GameObject completeIndicator;

	// Token: 0x04000C4D RID: 3149
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C4E RID: 3150
	private Item target;

	// Token: 0x04000C4F RID: 3151
	private Inventory destination;

	// Token: 0x04000C50 RID: 3152
	private int destinationIndex;

	// Token: 0x04000C51 RID: 3153
	private Inventory cachedInInventory;

	// Token: 0x04000C52 RID: 3154
	private SplitDialogue.Status status;

	// Token: 0x020004E7 RID: 1255
	private enum Status
	{
		// Token: 0x04001D5A RID: 7514
		Idle,
		// Token: 0x04001D5B RID: 7515
		Normal,
		// Token: 0x04001D5C RID: 7516
		Busy,
		// Token: 0x04001D5D RID: 7517
		Complete,
		// Token: 0x04001D5E RID: 7518
		Canceled
	}
}
