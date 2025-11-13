using System;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x02000288 RID: 648
	public class MiniGameBehaviour : MonoBehaviour
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0004DA04 File Offset: 0x0004BC04
		public MiniGame Game
		{
			get
			{
				return this.game;
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0004DA0C File Offset: 0x0004BC0C
		public void SetGame(MiniGame game = null)
		{
			if (game == null)
			{
				this.game = base.GetComponentInParent<MiniGame>();
				return;
			}
			this.game = game;
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x0004DA2B File Offset: 0x0004BC2B
		private void OnUpdateLogic(MiniGame game, float deltaTime)
		{
			if (this == null)
			{
				return;
			}
			if (!base.enabled)
			{
				return;
			}
			if (game == null)
			{
				return;
			}
			if (game != this.game)
			{
				return;
			}
			this.OnUpdate(deltaTime);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0004DA60 File Offset: 0x0004BC60
		protected virtual void OnEnable()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Combine(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0004DA82 File Offset: 0x0004BC82
		protected virtual void OnDisable()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Remove(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x0004DAA4 File Offset: 0x0004BCA4
		private void OnDestroy()
		{
			MiniGame.onUpdateLogic = (Action<MiniGame, float>)Delegate.Remove(MiniGame.onUpdateLogic, new Action<MiniGame, float>(this.OnUpdateLogic));
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x0004DAC6 File Offset: 0x0004BCC6
		protected virtual void Start()
		{
			if (this.game == null)
			{
				this.SetGame(null);
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0004DADD File Offset: 0x0004BCDD
		protected virtual void OnUpdate(float deltaTime)
		{
		}

		// Token: 0x04000F3E RID: 3902
		[SerializeField]
		private MiniGame game;
	}
}
