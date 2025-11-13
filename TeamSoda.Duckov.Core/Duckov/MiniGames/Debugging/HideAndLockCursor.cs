using System;
using UnityEngine;

namespace Duckov.MiniGames.Debugging
{
	// Token: 0x020002D2 RID: 722
	public class HideAndLockCursor : MonoBehaviour
	{
		// Token: 0x060016F9 RID: 5881 RVA: 0x0005491F File Offset: 0x00052B1F
		private void OnEnable()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x0005492D File Offset: 0x00052B2D
		private void OnDisable()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0005493B File Offset: 0x00052B3B
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				base.enabled = false;
			}
		}
	}
}
