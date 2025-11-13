using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001C3 RID: 451
public class MapMarkerPanelButton : MonoBehaviour
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000D88 RID: 3464 RVA: 0x000384F3 File Offset: 0x000366F3
	public Image Image
	{
		get
		{
			return this.image;
		}
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x000384FB File Offset: 0x000366FB
	public void Setup(UnityAction action, bool selected)
	{
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(action);
		this.selectionIndicator.gameObject.SetActive(selected);
	}

	// Token: 0x04000B8C RID: 2956
	[SerializeField]
	private GameObject selectionIndicator;

	// Token: 0x04000B8D RID: 2957
	[SerializeField]
	private Image image;

	// Token: 0x04000B8E RID: 2958
	[SerializeField]
	private Button button;
}
