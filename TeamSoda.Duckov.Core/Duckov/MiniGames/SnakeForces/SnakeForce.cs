using System;
using System.Collections.Generic;
using DG.Tweening;
using Duckov.Utilities;
using Saves;
using TMPro;
using UnityEngine;

namespace Duckov.MiniGames.SnakeForces
{
	// Token: 0x0200028E RID: 654
	public class SnakeForce : MiniGameBehaviour
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x0004EA94 File Offset: 0x0004CC94
		public List<SnakeForce.Part> Snake
		{
			get
			{
				return this.snake;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x0004EA9C File Offset: 0x0004CC9C
		public List<Vector2Int> Foods
		{
			get
			{
				return this.foods;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0004EAA4 File Offset: 0x0004CCA4
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x0004EAAC File Offset: 0x0004CCAC
		public int Score
		{
			get
			{
				return this._score;
			}
			private set
			{
				this._score = value;
				Action<SnakeForce> onScoreChanged = this.OnScoreChanged;
				if (onScoreChanged == null)
				{
					return;
				}
				onScoreChanged(this);
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0004EAC6 File Offset: 0x0004CCC6
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x0004EAD2 File Offset: 0x0004CCD2
		public static int HighScore
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/Snake/HighScore");
			}
			private set
			{
				SavesSystem.Save<int>("MiniGame/Snake/HighScore", value);
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0004EADF File Offset: 0x0004CCDF
		public SnakeForce.Part Head
		{
			get
			{
				if (this.snake.Count <= 0)
				{
					return null;
				}
				return this.snake[0];
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0004EAFD File Offset: 0x0004CCFD
		public SnakeForce.Part Tail
		{
			get
			{
				if (this.snake.Count <= 0)
				{
					return null;
				}
				List<SnakeForce.Part> list = this.snake;
				return list[list.Count - 1];
			}
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x0600152B RID: 5419 RVA: 0x0004EB24 File Offset: 0x0004CD24
		// (remove) Token: 0x0600152C RID: 5420 RVA: 0x0004EB5C File Offset: 0x0004CD5C
		public event Action<SnakeForce.Part> OnAddPart;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x0600152D RID: 5421 RVA: 0x0004EB94 File Offset: 0x0004CD94
		// (remove) Token: 0x0600152E RID: 5422 RVA: 0x0004EBCC File Offset: 0x0004CDCC
		public event Action<SnakeForce.Part> OnRemovePart;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x0600152F RID: 5423 RVA: 0x0004EC04 File Offset: 0x0004CE04
		// (remove) Token: 0x06001530 RID: 5424 RVA: 0x0004EC3C File Offset: 0x0004CE3C
		public event Action<SnakeForce> OnAfterTick;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06001531 RID: 5425 RVA: 0x0004EC74 File Offset: 0x0004CE74
		// (remove) Token: 0x06001532 RID: 5426 RVA: 0x0004ECAC File Offset: 0x0004CEAC
		public event Action<SnakeForce> OnScoreChanged;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06001533 RID: 5427 RVA: 0x0004ECE4 File Offset: 0x0004CEE4
		// (remove) Token: 0x06001534 RID: 5428 RVA: 0x0004ED1C File Offset: 0x0004CF1C
		public event Action<SnakeForce> OnGameStart;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06001535 RID: 5429 RVA: 0x0004ED54 File Offset: 0x0004CF54
		// (remove) Token: 0x06001536 RID: 5430 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		public event Action<SnakeForce> OnGameOver;

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06001537 RID: 5431 RVA: 0x0004EDC4 File Offset: 0x0004CFC4
		// (remove) Token: 0x06001538 RID: 5432 RVA: 0x0004EDFC File Offset: 0x0004CFFC
		public event Action<SnakeForce, Vector2Int> OnFoodEaten;

		// Token: 0x06001539 RID: 5433 RVA: 0x0004EE31 File Offset: 0x0004D031
		protected override void Start()
		{
			base.Start();
			this.titleScreen.SetActive(true);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x0004EE48 File Offset: 0x0004D048
		private void Restart()
		{
			this.Clear();
			this.gameOverScreen.SetActive(false);
			for (int i = this.borderXMin; i <= this.borderXMax; i++)
			{
				for (int j = this.borderYMin; j <= this.borderYMax; j++)
				{
					this.allCoords.Add(new Vector2Int(i, j));
				}
			}
			this.AddPart(new Vector2Int((this.borderXMax + this.borderXMin) / 2, (this.borderYMax + this.borderYMin) / 2), Vector2Int.up);
			this.Grow();
			this.Grow();
			this.AddFood(3);
			this.PunchCamera();
			this.playing = true;
			this.RefreshScoreText();
			this.highScoreText.text = string.Format("{0}", SnakeForce.HighScore);
			Action<SnakeForce> onGameStart = this.OnGameStart;
			if (onGameStart == null)
			{
				return;
			}
			onGameStart(this);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0004EF2C File Offset: 0x0004D12C
		private void AddFood(int count = 3)
		{
			List<Vector2Int> list = new List<Vector2Int>(this.allCoords);
			foreach (SnakeForce.Part part in this.snake)
			{
				list.Remove(part.coord);
			}
			if (list.Count <= 0)
			{
				this.Win();
				return;
			}
			foreach (Vector2Int item in list.GetRandomSubSet(count))
			{
				this.foods.Add(item);
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0004EFD4 File Offset: 0x0004D1D4
		private void GameOver()
		{
			Action<SnakeForce> onGameOver = this.OnGameOver;
			if (onGameOver != null)
			{
				onGameOver(this);
			}
			bool active = this.Score > SnakeForce.HighScore;
			if (this.Score > SnakeForce.HighScore)
			{
				SnakeForce.HighScore = this.Score;
			}
			this.highScoreIndicator.SetActive(active);
			this.winIndicator.SetActive(this.won);
			this.scoreTextGameOver.text = string.Format("{0}", this.Score);
			this.gameOverScreen.SetActive(true);
			this.PunchCamera();
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0004F068 File Offset: 0x0004D268
		private void Win()
		{
			this.won = true;
			this.GameOver();
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0004F078 File Offset: 0x0004D278
		protected override void OnUpdate(float deltaTime)
		{
			Vector2 axis = base.Game.GetAxis(0);
			if (axis.sqrMagnitude > 0.1f)
			{
				Vector2Int rhs = default(Vector2Int);
				if (axis.x > 0f)
				{
					rhs = Vector2Int.right;
				}
				else if (axis.x < 0f)
				{
					rhs = Vector2Int.left;
				}
				else if (axis.y > 0f)
				{
					rhs = Vector2Int.up;
				}
				else if (axis.y < 0f)
				{
					rhs = Vector2Int.down;
				}
				if (this.lastFrameAxis != rhs)
				{
					this.axisInput = true;
				}
				this.lastFrameAxis = rhs;
			}
			else
			{
				this.lastFrameAxis = Vector2Int.zero;
			}
			if (this.freezeCountDown > 0.0)
			{
				this.freezeCountDown -= (double)Time.unscaledDeltaTime;
				return;
			}
			if (this.dead || this.won || !this.playing)
			{
				if (base.Game.GetButtonDown(MiniGame.Button.Start))
				{
					this.Restart();
				}
				return;
			}
			this.RefreshScoreText();
			bool flag = base.Game.GetButton(MiniGame.Button.B) || base.Game.GetButton(MiniGame.Button.A);
			this.tickETA -= deltaTime * (flag ? 10f : 1f);
			float time = (this.playTick < (ulong)((long)this.maxSpeedTick)) ? (this.playTick / (float)this.maxSpeedTick) : 1f;
			float num = Mathf.Lerp(this.tickIntervalFrom, this.tickIntervalTo, this.speedCurve.Evaluate(time));
			if (this.tickETA <= 0f || this.axisInput)
			{
				this.Tick();
				this.tickETA = num;
				this.axisInput = false;
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0004F22B File Offset: 0x0004D42B
		private void RefreshScoreText()
		{
			this.scoreText.text = string.Format("{0}", this.Score);
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0004F24D File Offset: 0x0004D44D
		private void Tick()
		{
			this.playTick += 1UL;
			if (this.Head == null)
			{
				return;
			}
			this.HandleMovement();
			this.DetectDeath();
			this.HandleEatAndGrow();
			Action<SnakeForce> onAfterTick = this.OnAfterTick;
			if (onAfterTick == null)
			{
				return;
			}
			onAfterTick(this);
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0004F28C File Offset: 0x0004D48C
		private void HandleMovement()
		{
			Vector2Int vector2Int = this.lastFrameAxis;
			if ((!(vector2Int == -this.Head.direction) || this.snake.Count <= 1) && vector2Int != Vector2Int.zero)
			{
				this.Head.direction = vector2Int;
			}
			for (int i = this.snake.Count - 1; i >= 0; i--)
			{
				SnakeForce.Part part = this.snake[i];
				Vector2Int coord = (i > 0) ? this.snake[i - 1].coord : (part.coord + part.direction);
				if (i > 0)
				{
					part.direction = this.snake[i - 1].direction;
				}
				if (coord.x > this.borderXMax)
				{
					coord.x = this.borderXMin;
				}
				if (coord.y > this.borderYMax)
				{
					coord.y = this.borderYMin;
				}
				if (coord.x < this.borderXMin)
				{
					coord.x = this.borderXMax;
				}
				if (coord.y < this.borderYMin)
				{
					coord.y = this.borderYMax;
				}
				part.MoveTo(coord);
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0004F3CC File Offset: 0x0004D5CC
		private void HandleEatAndGrow()
		{
			Vector2Int coord = this.Head.coord;
			if (this.foods.Remove(coord))
			{
				this.Grow();
				int score = this.Score;
				this.Score = score + 1;
				int num = 3 + Mathf.FloorToInt(Mathf.Log((float)this.Score, 2f));
				int count = Mathf.Max(0, num - this.foods.Count);
				this.AddFood(count);
				Action<SnakeForce, Vector2Int> onFoodEaten = this.OnFoodEaten;
				if (onFoodEaten != null)
				{
					onFoodEaten(this, coord);
				}
				this.PunchCamera();
			}
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0004F458 File Offset: 0x0004D658
		private void DetectDeath()
		{
			Vector2Int coord = this.Head.coord;
			for (int i = 1; i < this.snake.Count; i++)
			{
				if (this.snake[i].coord == coord)
				{
					this.dead = true;
					this.GameOver();
					return;
				}
			}
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0004F4B0 File Offset: 0x0004D6B0
		private SnakeForce.Part Grow()
		{
			if (this.snake.Count == 0)
			{
				Debug.LogError("Cannot grow the snake! It haven't been created yet.");
				return null;
			}
			SnakeForce.Part tail = this.Tail;
			Vector2Int coord = tail.coord - tail.direction;
			return this.AddPart(coord, tail.direction);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0004F4FC File Offset: 0x0004D6FC
		private SnakeForce.Part AddPart(Vector2Int coord, Vector2Int direction)
		{
			SnakeForce.Part part = new SnakeForce.Part(this, coord, direction);
			this.snake.Add(part);
			Action<SnakeForce.Part> onAddPart = this.OnAddPart;
			if (onAddPart != null)
			{
				onAddPart(part);
			}
			return part;
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0004F531 File Offset: 0x0004D731
		private bool RemovePart(SnakeForce.Part part)
		{
			if (!this.snake.Remove(part))
			{
				return false;
			}
			Action<SnakeForce.Part> onRemovePart = this.OnRemovePart;
			if (onRemovePart != null)
			{
				onRemovePart(part);
			}
			return true;
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0004F558 File Offset: 0x0004D758
		private void Clear()
		{
			this.titleScreen.SetActive(false);
			this.won = false;
			this.dead = false;
			this.Score = 0;
			this.playTick = 0UL;
			this.allCoords.Clear();
			this.foods.Clear();
			for (int i = this.snake.Count - 1; i >= 0; i--)
			{
				SnakeForce.Part part = this.snake[i];
				if (part == null)
				{
					this.snake.RemoveAt(i);
				}
				else
				{
					this.RemovePart(part);
				}
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0004F5E4 File Offset: 0x0004D7E4
		private void PunchCamera()
		{
			this.freezeCountDown = 0.10000000149011612;
			this.cameraParent.DOKill(true);
			this.cameraParent.DOShakePosition(0.4f, 1f, 10, 90f, false, true);
			this.cameraParent.DOShakeRotation(0.4f, Vector3.forward, 10, 90f, true);
		}

		// Token: 0x04000F78 RID: 3960
		[SerializeField]
		private GameObject gameOverScreen;

		// Token: 0x04000F79 RID: 3961
		[SerializeField]
		private GameObject titleScreen;

		// Token: 0x04000F7A RID: 3962
		[SerializeField]
		private GameObject winIndicator;

		// Token: 0x04000F7B RID: 3963
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04000F7C RID: 3964
		[SerializeField]
		private TextMeshProUGUI highScoreText;

		// Token: 0x04000F7D RID: 3965
		[SerializeField]
		private GameObject highScoreIndicator;

		// Token: 0x04000F7E RID: 3966
		[SerializeField]
		private TextMeshProUGUI scoreTextGameOver;

		// Token: 0x04000F7F RID: 3967
		[SerializeField]
		private Transform cameraParent;

		// Token: 0x04000F80 RID: 3968
		[SerializeField]
		private float tickIntervalFrom = 0.5f;

		// Token: 0x04000F81 RID: 3969
		[SerializeField]
		private float tickIntervalTo = 0.01f;

		// Token: 0x04000F82 RID: 3970
		[SerializeField]
		private int maxSpeedTick = 4096;

		// Token: 0x04000F83 RID: 3971
		[SerializeField]
		private AnimationCurve speedCurve;

		// Token: 0x04000F84 RID: 3972
		[SerializeField]
		private int borderXMin = -10;

		// Token: 0x04000F85 RID: 3973
		[SerializeField]
		private int borderXMax = 10;

		// Token: 0x04000F86 RID: 3974
		[SerializeField]
		private int borderYMin = -10;

		// Token: 0x04000F87 RID: 3975
		[SerializeField]
		private int borderYMax = 10;

		// Token: 0x04000F88 RID: 3976
		private bool playing;

		// Token: 0x04000F89 RID: 3977
		private bool dead;

		// Token: 0x04000F8A RID: 3978
		private bool won;

		// Token: 0x04000F8B RID: 3979
		private List<SnakeForce.Part> snake = new List<SnakeForce.Part>();

		// Token: 0x04000F8C RID: 3980
		private List<Vector2Int> foods = new List<Vector2Int>();

		// Token: 0x04000F8D RID: 3981
		private int _score;

		// Token: 0x04000F8E RID: 3982
		public const string HighScoreKey = "MiniGame/Snake/HighScore";

		// Token: 0x04000F96 RID: 3990
		private float tickETA;

		// Token: 0x04000F97 RID: 3991
		private List<Vector2Int> allCoords = new List<Vector2Int>();

		// Token: 0x04000F98 RID: 3992
		private ulong playTick;

		// Token: 0x04000F99 RID: 3993
		private Vector2Int lastFrameAxis;

		// Token: 0x04000F9A RID: 3994
		private double freezeCountDown;

		// Token: 0x04000F9B RID: 3995
		private bool axisInput;

		// Token: 0x02000567 RID: 1383
		public class Part
		{
			// Token: 0x0600288B RID: 10379 RVA: 0x000955C2 File Offset: 0x000937C2
			public Part(SnakeForce master, Vector2Int coord, Vector2Int direction)
			{
				this.Master = master;
				this.coord = coord;
				this.direction = direction;
			}

			// Token: 0x17000772 RID: 1906
			// (get) Token: 0x0600288C RID: 10380 RVA: 0x000955DF File Offset: 0x000937DF
			public bool IsHead
			{
				get
				{
					return this == this.Master.Head;
				}
			}

			// Token: 0x17000773 RID: 1907
			// (get) Token: 0x0600288D RID: 10381 RVA: 0x000955EF File Offset: 0x000937EF
			public bool IsTail
			{
				get
				{
					return this == this.Master.Tail;
				}
			}

			// Token: 0x0600288E RID: 10382 RVA: 0x000955FF File Offset: 0x000937FF
			internal void MoveTo(Vector2Int coord)
			{
				this.coord = coord;
				Action<SnakeForce.Part> onMove = this.OnMove;
				if (onMove == null)
				{
					return;
				}
				onMove(this);
			}

			// Token: 0x140000FD RID: 253
			// (add) Token: 0x0600288F RID: 10383 RVA: 0x0009561C File Offset: 0x0009381C
			// (remove) Token: 0x06002890 RID: 10384 RVA: 0x00095654 File Offset: 0x00093854
			public event Action<SnakeForce.Part> OnMove;

			// Token: 0x04001F60 RID: 8032
			public Vector2Int coord;

			// Token: 0x04001F61 RID: 8033
			public Vector2Int direction;

			// Token: 0x04001F62 RID: 8034
			public readonly SnakeForce Master;
		}
	}
}
