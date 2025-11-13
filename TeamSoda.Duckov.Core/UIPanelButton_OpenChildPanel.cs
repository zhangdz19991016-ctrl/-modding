using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000157 RID: 343
public class UIPanelButton_OpenChildPanel : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000A9A RID: 2714 RVA: 0x0002EBB3 File Offset: 0x0002CDB3
	private void Awake()
	{
		if (this.master == null)
		{
			this.master = base.GetComponentInParent<UIPanel>();
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0002EBCF File Offset: 0x0002CDCF
	public void OnPointerClick(PointerEventData eventData)
	{
		UIPanel uipanel = this.master;
		if (uipanel != null)
		{
			uipanel.OpenChild(this.target);
		}
		eventData.Use();
	}

	// Token: 0x0400094A RID: 2378
	[SerializeField]
	private UIPanel master;

	// Token: 0x0400094B RID: 2379
	[SerializeField]
	private UIPanel target;
}
