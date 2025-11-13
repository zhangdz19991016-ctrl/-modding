using System;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x0200028D RID: 653
	public class SnakeDisplay : MiniGameBehaviour
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0004E718 File Offset: 0x0004C918
		private PrefabPool<SnakePartDisplay> PartPool
		{
			get
			{
				if (this._partPool == null)
				{
					this._partPool = new PrefabPool<SnakePartDisplay>(this.partDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._partPool;
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0004E754 File Offset: 0x0004C954
		private PrefabPool<Transform> FoodPool
		{
			get
			{
				if (this._foodPool == null)
				{
					this._foodPool = new PrefabPool<Transform>(this.foodDisplayTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._foodPool;
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0004E790 File Offset: 0x0004C990
		private void Awake()
		{
			this.master.OnAddPart += this.OnAddPart;
			this.master.OnGameStart += this.OnGameStart;
			this.master.OnRemovePart += this.OnRemovePart;
			this.master.OnAfterTick += this.OnAfterTick;
			this.master.OnFoodEaten += this.OnFoodEaten;
			this.partDisplayTemplate.gameObject.SetActive(false);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0004E821 File Offset: 0x0004CA21
		protected override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);
			this.HandlePunchColor();
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0004E830 File Offset: 0x0004CA30
		private void HandlePunchColor()
		{
			if (!this.punchingColor)
			{
				return;
			}
			if (this.punchColorIndex >= this.master.Snake.Count)
			{
				this.punchingColor = false;
				return;
			}
			SnakePartDisplay snakePartDisplay = this.PartPool.ActiveEntries.First((SnakePartDisplay e) => e.Target == this.master.Snake[this.punchColorIndex]);
			if (snakePartDisplay)
			{
				snakePartDisplay.PunchColor(Color.HSVToRGB((float)this.punchColorIndex % 12f / 12f, 1f, 1f));
			}
			this.punchColorIndex++;
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x0004E8C1 File Offset: 0x0004CAC1
		private void OnGameStart(SnakeForce force)
		{
			this.RefreshFood();
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x0004E8CC File Offset: 0x0004CACC
		private void OnFoodEaten(SnakeForce force, Vector2Int coord)
		{
			FXPool.Play(this.eatFXPrefab, this.GetWorldPosition(coord), Quaternion.LookRotation((Vector3Int)this.master.Head.direction, Vector3.forward));
			foreach (SnakePartDisplay snakePartDisplay in this.PartPool.ActiveEntries)
			{
				snakePartDisplay.Punch();
			}
			this.StartPunchingColor();
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0004E958 File Offset: 0x0004CB58
		private void StartPunchingColor()
		{
			this.punchingColor = true;
			this.punchColorIndex = 0;
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x0004E968 File Offset: 0x0004CB68
		private void OnAfterTick(SnakeForce force)
		{
			this.RefreshFood();
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0004E970 File Offset: 0x0004CB70
		private void RefreshFood()
		{
			this.FoodPool.ReleaseAll();
			foreach (Vector2Int coord in this.master.Foods)
			{
				this.FoodPool.Get(null).localPosition = this.GetPosition(coord);
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0004E9E4 File Offset: 0x0004CBE4
		private void OnRemovePart(SnakeForce.Part part)
		{
			this.PartPool.ReleaseAll((SnakePartDisplay e) => e.Target == part);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0004EA16 File Offset: 0x0004CC16
		private void OnAddPart(SnakeForce.Part part)
		{
			this.PartPool.Get(null).Setup(this, part);
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0004EA2B File Offset: 0x0004CC2B
		internal Vector3 GetPosition(Vector2Int coord)
		{
			return coord * this.gridSize;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0004EA44 File Offset: 0x0004CC44
		internal Vector3 GetWorldPosition(Vector2Int coord)
		{
			Vector3 position = this.GetPosition(coord);
			return base.transform.TransformPoint(position);
		}

		// Token: 0x04000F6E RID: 3950
		[SerializeField]
		private SnakeForce master;

		// Token: 0x04000F6F RID: 3951
		[SerializeField]
		private SnakePartDisplay partDisplayTemplate;

		// Token: 0x04000F70 RID: 3952
		[SerializeField]
		private Transform foodDisplayTemplate;

		// Token: 0x04000F71 RID: 3953
		[SerializeField]
		private Transform exitDisplayTemplte;

		// Token: 0x04000F72 RID: 3954
		[SerializeField]
		private ParticleSystem eatFXPrefab;

		// Token: 0x04000F73 RID: 3955
		[SerializeField]
		private int gridSize = 8;

		// Token: 0x04000F74 RID: 3956
		private PrefabPool<SnakePartDisplay> _partPool;

		// Token: 0x04000F75 RID: 3957
		private PrefabPool<Transform> _foodPool;

		// Token: 0x04000F76 RID: 3958
		private bool punchingColor;

		// Token: 0x04000F77 RID: 3959
		private int punchColorIndex;
	}
}
