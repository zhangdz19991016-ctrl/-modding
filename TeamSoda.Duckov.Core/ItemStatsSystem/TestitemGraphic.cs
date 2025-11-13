using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemStatsSystem
{
	// Token: 0x0200022D RID: 557
	public class TestitemGraphic : MonoBehaviour
	{
		// Token: 0x06001142 RID: 4418 RVA: 0x000433C7 File Offset: 0x000415C7
		private void Start()
		{
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x000433CC File Offset: 0x000415CC
		private void Update()
		{
			if (Keyboard.current.gKey.wasPressedThisFrame)
			{
				if (this.instance)
				{
					UnityEngine.Object.Destroy(this.instance.gameObject);
				}
				DuckovItemAgent currentHoldItemAgent = CharacterMainControl.Main.CurrentHoldItemAgent;
				if (!currentHoldItemAgent)
				{
					return;
				}
				this.instance = ItemGraphicInfo.CreateAGraphic(currentHoldItemAgent.Item, base.transform);
			}
		}

		// Token: 0x04000D7E RID: 3454
		private ItemGraphicInfo instance;
	}
}
