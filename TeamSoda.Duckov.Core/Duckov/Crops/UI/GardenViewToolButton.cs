using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Crops.UI
{
	// Token: 0x020002F7 RID: 759
	public class GardenViewToolButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060018C8 RID: 6344 RVA: 0x0005AF77 File Offset: 0x00059177
		public void OnPointerClick(PointerEventData eventData)
		{
			this.master.SetTool(this.tool);
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0005AF8A File Offset: 0x0005918A
		private void Awake()
		{
			this.master.onToolChanged += this.OnToolChanged;
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0005AFA3 File Offset: 0x000591A3
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0005AFAB File Offset: 0x000591AB
		private void Refresh()
		{
			this.indicator.SetActive(this.tool == this.master.Tool);
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0005AFCB File Offset: 0x000591CB
		private void OnToolChanged()
		{
			this.Refresh();
		}

		// Token: 0x04001209 RID: 4617
		[SerializeField]
		private GardenView master;

		// Token: 0x0400120A RID: 4618
		[SerializeField]
		private GardenView.ToolType tool;

		// Token: 0x0400120B RID: 4619
		[SerializeField]
		private GameObject indicator;
	}
}
