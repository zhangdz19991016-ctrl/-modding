using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.Modding.UI
{
	// Token: 0x02000274 RID: 628
	public class ModPathButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x060013D1 RID: 5073 RVA: 0x0004A357 File Offset: 0x00048557
		private void OnEnable()
		{
			this.pathText.text = ModManager.DefaultModFolderPath;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004A369 File Offset: 0x00048569
		public void OnPointerClick(PointerEventData eventData)
		{
			GUIUtility.systemCopyBuffer = ModManager.DefaultModFolderPath;
		}

		// Token: 0x04000EB7 RID: 3767
		[SerializeField]
		private TextMeshProUGUI pathText;
	}
}
