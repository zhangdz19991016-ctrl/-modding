using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000166 RID: 358
public class CopyTextOnClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0002F808 File Offset: 0x0002DA08
	[SerializeField]
	private string content
	{
		get
		{
			return Path.Combine(Application.persistentDataPath, "Saves");
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0002F819 File Offset: 0x0002DA19
	public void OnPointerClick(PointerEventData eventData)
	{
		GUIUtility.systemCopyBuffer = this.content;
	}
}
