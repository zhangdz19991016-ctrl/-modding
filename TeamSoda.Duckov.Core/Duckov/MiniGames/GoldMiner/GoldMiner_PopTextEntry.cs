using System;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A8 RID: 680
	public class GoldMiner_PopTextEntry : MonoBehaviour
	{
		// Token: 0x06001645 RID: 5701 RVA: 0x00052AB4 File Offset: 0x00050CB4
		public void Setup(Vector3 pos, string text, Action<GoldMiner_PopTextEntry> releaseAction)
		{
			this.initialized = true;
			this.tmp.text = text;
			this.life = 0f;
			base.transform.position = pos;
			this.releaseAction = releaseAction;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00052AE8 File Offset: 0x00050CE8
		private void Update()
		{
			if (!this.initialized)
			{
				return;
			}
			this.life += Time.deltaTime;
			base.transform.position += Vector3.up * this.moveSpeed * Time.deltaTime;
			if (this.life >= this.lifeTime)
			{
				this.Release();
			}
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x00052B54 File Offset: 0x00050D54
		private void Release()
		{
			if (this.releaseAction != null)
			{
				this.releaseAction(this);
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001072 RID: 4210
		public TextMeshProUGUI tmp;

		// Token: 0x04001073 RID: 4211
		public float lifeTime;

		// Token: 0x04001074 RID: 4212
		public float moveSpeed = 1f;

		// Token: 0x04001075 RID: 4213
		private bool initialized;

		// Token: 0x04001076 RID: 4214
		private float life;

		// Token: 0x04001077 RID: 4215
		private Action<GoldMiner_PopTextEntry> releaseAction;
	}
}
