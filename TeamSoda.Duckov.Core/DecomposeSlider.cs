using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000203 RID: 515
public class DecomposeSlider : MonoBehaviour
{
	// Token: 0x1400006E RID: 110
	// (add) Token: 0x06000F29 RID: 3881 RVA: 0x0003C884 File Offset: 0x0003AA84
	// (remove) Token: 0x06000F2A RID: 3882 RVA: 0x0003C8BC File Offset: 0x0003AABC
	public event Action<float> OnValueChangedEvent;

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0003C8F1 File Offset: 0x0003AAF1
	// (set) Token: 0x06000F2C RID: 3884 RVA: 0x0003C903 File Offset: 0x0003AB03
	public int Value
	{
		get
		{
			return Mathf.RoundToInt(this.slider.value);
		}
		set
		{
			this.slider.value = (float)value;
			this.valueText.text = value.ToString();
		}
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0003C924 File Offset: 0x0003AB24
	private void Awake()
	{
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x0003C942 File Offset: 0x0003AB42
	private void OnDestroy()
	{
		this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnValueChanged));
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x0003C960 File Offset: 0x0003AB60
	private void OnValueChanged(float value)
	{
		this.OnValueChangedEvent(value);
		this.valueText.text = value.ToString();
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0003C980 File Offset: 0x0003AB80
	public void SetMinMax(int min, int max)
	{
		this.slider.minValue = (float)min;
		this.slider.maxValue = (float)max;
		this.minText.text = min.ToString();
		this.maxText.text = max.ToString();
	}

	// Token: 0x04000C7C RID: 3196
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C7D RID: 3197
	public TextMeshProUGUI minText;

	// Token: 0x04000C7E RID: 3198
	public TextMeshProUGUI maxText;

	// Token: 0x04000C7F RID: 3199
	public TextMeshProUGUI valueText;
}
