using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Saves;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000167 RID: 359
public class DeleteSaveDataButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0002F82E File Offset: 0x0002DA2E
	private float TimeSinceStartedHolding
	{
		get
		{
			return Time.unscaledTime - this.timeWhenStartedHolding;
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0002F83C File Offset: 0x0002DA3C
	private float T
	{
		get
		{
			if (this.totalTime <= 0f)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.TimeSinceStartedHolding / this.totalTime);
		}
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0002F863 File Offset: 0x0002DA63
	public void OnPointerDown(PointerEventData eventData)
	{
		this.holding = true;
		this.timeWhenStartedHolding = Time.unscaledTime;
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0002F877 File Offset: 0x0002DA77
	public void OnPointerUp(PointerEventData eventData)
	{
		this.holding = false;
		this.timeWhenStartedHolding = float.MaxValue;
		this.RefreshProgressBar();
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0002F891 File Offset: 0x0002DA91
	private void Start()
	{
		this.barFill.fillAmount = 0f;
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x0002F8A3 File Offset: 0x0002DAA3
	private void Update()
	{
		if (this.holding)
		{
			this.RefreshProgressBar();
			if (this.T >= 1f)
			{
				this.Execute();
			}
		}
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0002F8C6 File Offset: 0x0002DAC6
	private void Execute()
	{
		this.holding = false;
		this.DeleteCurrentSaveData();
		this.RefreshProgressBar();
		this.NotifySaveDeleted().Forget();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0002F8E8 File Offset: 0x0002DAE8
	private UniTask NotifySaveDeleted()
	{
		DeleteSaveDataButton.<NotifySaveDeleted>d__14 <NotifySaveDeleted>d__;
		<NotifySaveDeleted>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<NotifySaveDeleted>d__.<>4__this = this;
		<NotifySaveDeleted>d__.<>1__state = -1;
		<NotifySaveDeleted>d__.<>t__builder.Start<DeleteSaveDataButton.<NotifySaveDeleted>d__14>(ref <NotifySaveDeleted>d__);
		return <NotifySaveDeleted>d__.<>t__builder.Task;
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0002F92B File Offset: 0x0002DB2B
	private void DeleteCurrentSaveData()
	{
		SavesSystem.DeleteCurrentSave();
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0002F932 File Offset: 0x0002DB32
	private void RefreshProgressBar()
	{
		this.barFill.fillAmount = this.T;
	}

	// Token: 0x04000982 RID: 2434
	[SerializeField]
	private float totalTime = 3f;

	// Token: 0x04000983 RID: 2435
	[SerializeField]
	private Image barFill;

	// Token: 0x04000984 RID: 2436
	[SerializeField]
	private FadeGroup saveDeletedNotifierFadeGroup;

	// Token: 0x04000985 RID: 2437
	private float timeWhenStartedHolding = float.MaxValue;

	// Token: 0x04000986 RID: 2438
	private bool holding;
}
