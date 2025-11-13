using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019F RID: 415
public class DigitInputPanel : MonoBehaviour
{
	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06000C55 RID: 3157 RVA: 0x00034734 File Offset: 0x00032934
	// (remove) Token: 0x06000C56 RID: 3158 RVA: 0x0003476C File Offset: 0x0003296C
	public event Action<string> onInputFieldValueChanged;

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000C57 RID: 3159 RVA: 0x000347A4 File Offset: 0x000329A4
	public long Value
	{
		get
		{
			string text = this.inputField.text;
			if (string.IsNullOrEmpty(text))
			{
				return 0L;
			}
			long result;
			if (!long.TryParse(text, out result))
			{
				return 0L;
			}
			return result;
		}
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x000347D8 File Offset: 0x000329D8
	private void Awake()
	{
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnInputFieldValueChanged));
		for (int i = 0; i < this.numKeys.Length; i++)
		{
			int v = i;
			this.numKeys[i].onClick.AddListener(delegate()
			{
				this.OnNumKeyClicked((long)v);
			});
		}
		this.clearButton.onClick.AddListener(new UnityAction(this.OnClearButtonClicked));
		this.backspaceButton.onClick.AddListener(new UnityAction(this.OnBackspaceButtonClicked));
		this.maximumButton.onClick.AddListener(new UnityAction(this.Max));
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0003489C File Offset: 0x00032A9C
	private void OnBackspaceButtonClicked()
	{
		if (string.IsNullOrEmpty(this.inputField.text))
		{
			return;
		}
		this.inputField.text = this.inputField.text.Substring(0, this.inputField.text.Length - 1);
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x000348EA File Offset: 0x00032AEA
	private void OnClearButtonClicked()
	{
		this.inputField.text = string.Empty;
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x000348FC File Offset: 0x00032AFC
	private void OnNumKeyClicked(long v)
	{
		this.inputField.text = string.Format("{0}{1}", this.inputField.text, v);
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00034924 File Offset: 0x00032B24
	private void OnInputFieldValueChanged(string value)
	{
		long num;
		if (long.TryParse(value, out num) && num == 0L)
		{
			this.inputField.SetTextWithoutNotify(string.Empty);
		}
		Action<string> action = this.onInputFieldValueChanged;
		if (action == null)
		{
			return;
		}
		action(value);
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0003495F File Offset: 0x00032B5F
	public void Setup(long value, Func<long> maxFunc = null)
	{
		this.maxFunction = maxFunc;
		this.inputField.text = string.Format("{0}", value);
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00034984 File Offset: 0x00032B84
	public void Max()
	{
		if (this.maxFunction == null)
		{
			return;
		}
		long num = this.maxFunction();
		this.inputField.text = string.Format("{0}", num);
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x000349C1 File Offset: 0x00032BC1
	internal void Clear()
	{
		this.inputField.text = string.Empty;
	}

	// Token: 0x04000ABD RID: 2749
	[SerializeField]
	private TMP_InputField inputField;

	// Token: 0x04000ABE RID: 2750
	[SerializeField]
	private Button clearButton;

	// Token: 0x04000ABF RID: 2751
	[SerializeField]
	private Button backspaceButton;

	// Token: 0x04000AC0 RID: 2752
	[SerializeField]
	private Button maximumButton;

	// Token: 0x04000AC1 RID: 2753
	[SerializeField]
	private Button[] numKeys;

	// Token: 0x04000AC2 RID: 2754
	public Func<long> maxFunction;
}
