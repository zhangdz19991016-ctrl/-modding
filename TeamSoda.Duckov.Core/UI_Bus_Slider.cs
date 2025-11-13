using System;
using Duckov;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000200 RID: 512
public class UI_Bus_Slider : MonoBehaviour
{
	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0003C658 File Offset: 0x0003A858
	private AudioManager.Bus BusRef
	{
		get
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (this.busRef == null)
			{
				this.busRef = AudioManager.GetBus(this.busName);
				if (this.busRef == null)
				{
					Debug.LogError("Bus not found:" + this.busName);
				}
			}
			return this.busRef;
		}
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0003C6AC File Offset: 0x0003A8AC
	private void Initialize()
	{
		if (this.BusRef == null)
		{
			return;
		}
		this.slider.SetValueWithoutNotify(this.BusRef.Volume);
		this.volumeNumberText.text = (this.BusRef.Volume * 100f).ToString("0");
		this.initialized = true;
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x0003C708 File Offset: 0x0003A908
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0003C726 File Offset: 0x0003A926
	private void Start()
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0003C736 File Offset: 0x0003A936
	private void OnEnable()
	{
		this.Initialize();
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0003C740 File Offset: 0x0003A940
	private void OnValueChanged(float value)
	{
		if (this.BusRef == null)
		{
			return;
		}
		this.BusRef.Volume = value;
		this.BusRef.Mute = (value == 0f);
		this.volumeNumberText.text = (this.BusRef.Volume * 100f).ToString("0");
	}

	// Token: 0x04000C75 RID: 3189
	private AudioManager.Bus busRef;

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	private string busName;

	// Token: 0x04000C77 RID: 3191
	[SerializeField]
	private TextMeshProUGUI volumeNumberText;

	// Token: 0x04000C78 RID: 3192
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C79 RID: 3193
	private bool initialized;
}
