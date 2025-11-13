using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.GoldMiner
{
	// Token: 0x020002A7 RID: 679
	public class GoldMiner_PopText : MiniGameBehaviour
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x00052A44 File Offset: 0x00050C44
		private PrefabPool<GoldMiner_PopTextEntry> TextPool
		{
			get
			{
				if (this._textPool == null)
				{
					this._textPool = new PrefabPool<GoldMiner_PopTextEntry>(this.textTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._textPool;
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x00052A7D File Offset: 0x00050C7D
		public void Pop(string content, Vector3 position)
		{
			this.TextPool.Get(null).Setup(position, content, new Action<GoldMiner_PopTextEntry>(this.ReleaseEntry));
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00052A9E File Offset: 0x00050C9E
		private void ReleaseEntry(GoldMiner_PopTextEntry entry)
		{
			this.TextPool.Release(entry);
		}

		// Token: 0x04001070 RID: 4208
		[SerializeField]
		private GoldMiner_PopTextEntry textTemplate;

		// Token: 0x04001071 RID: 4209
		private PrefabPool<GoldMiner_PopTextEntry> _textPool;
	}
}
