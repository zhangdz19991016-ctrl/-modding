using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using Duckov.Utilities;
using Saves;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002DF RID: 735
	public class BubblePopper : MiniGameBehaviour
	{
		// Token: 0x0600175B RID: 5979 RVA: 0x00055A38 File Offset: 0x00053C38
		public void NextPallette()
		{
			this.palletteIndex++;
			if (this.palletteIndex >= this.pallettes.Count)
			{
				this.palletteIndex = 0;
			}
			if (this.palletteIndex >= this.pallettes.Count)
			{
				return;
			}
			BubblePopper.Pallette pallette = this.pallettes[this.palletteIndex];
			this.SetPallette(pallette.colors);
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x00055A9F File Offset: 0x00053C9F
		public int AvaliableColorCount
		{
			get
			{
				return this.colorPallette.Length;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x00055AA9 File Offset: 0x00053CA9
		public BubblePopperLayout Layout
		{
			get
			{
				return this.layout;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00055AB1 File Offset: 0x00053CB1
		public float BubbleRadius
		{
			get
			{
				if (this.bubbleTemplate == null)
				{
					return 8f;
				}
				return this.bubbleTemplate.Radius;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x00055AD2 File Offset: 0x00053CD2
		public Bubble BubbleTemplate
		{
			get
			{
				return this.bubbleTemplate;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x00055ADC File Offset: 0x00053CDC
		private PrefabPool<Bubble> BubblePool
		{
			get
			{
				if (this._bubblePool == null)
				{
					this._bubblePool = new PrefabPool<Bubble>(this.bubbleTemplate, null, new Action<Bubble>(this.OnGetBubble), null, null, true, 10, 10000, null);
				}
				return this._bubblePool;
			}
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00055B20 File Offset: 0x00053D20
		private void OnGetBubble(Bubble bubble)
		{
			bubble.Rest();
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x00055B28 File Offset: 0x00053D28
		// (set) Token: 0x06001763 RID: 5987 RVA: 0x00055B30 File Offset: 0x00053D30
		public BubblePopper.Status status { get; private set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00055B39 File Offset: 0x00053D39
		// (set) Token: 0x06001765 RID: 5989 RVA: 0x00055B41 File Offset: 0x00053D41
		public int FloorStepETA { get; private set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x00055B4A File Offset: 0x00053D4A
		// (set) Token: 0x06001767 RID: 5991 RVA: 0x00055B52 File Offset: 0x00053D52
		public int Score
		{
			get
			{
				return this._score;
			}
			private set
			{
				this._score = value;
				this.RefreshScoreText();
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x00055B61 File Offset: 0x00053D61
		// (set) Token: 0x06001769 RID: 5993 RVA: 0x00055B6D File Offset: 0x00053D6D
		public static int HighScore
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/BubblePopper/HighScore");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/BubblePopper/HighScore", value);
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x00055B7A File Offset: 0x00053D7A
		// (set) Token: 0x0600176B RID: 5995 RVA: 0x00055B86 File Offset: 0x00053D86
		public static int HighLevel
		{
			get
			{
				return SavesSystem.Load<int>("MiniGame/BubblePopper/HighLevel");
			}
			set
			{
				SavesSystem.Save<int>("MiniGame/BubblePopper/HighLevel", value);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x00055B93 File Offset: 0x00053D93
		// (set) Token: 0x0600176D RID: 5997 RVA: 0x00055B9B File Offset: 0x00053D9B
		public bool Busy { get; private set; }

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x0600176E RID: 5998 RVA: 0x00055BA4 File Offset: 0x00053DA4
		// (remove) Token: 0x0600176F RID: 5999 RVA: 0x00055BD8 File Offset: 0x00053DD8
		public static event Action<int> OnLevelClear;

		// Token: 0x06001770 RID: 6000 RVA: 0x00055C0B File Offset: 0x00053E0B
		protected override void Start()
		{
			base.Start();
			this.RefreshScoreText();
			this.RefreshLevelText();
			this.HideEndScreen();
			this.ShowStartScreen();
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00055C2C File Offset: 0x00053E2C
		private void RefreshScoreText()
		{
			this.scoreText.text = string.Format("{0}", this.Score);
			this.highScoreText.text = string.Format("{0}", BubblePopper.HighScore);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00055C78 File Offset: 0x00053E78
		private void RefreshLevelText()
		{
			this.levelText.text = string.Format("{0}", this.levelIndex);
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00055C9A File Offset: 0x00053E9A
		protected override void OnUpdate(float deltaTime)
		{
			this.UpdateStatus(deltaTime);
			this.HandleInput(deltaTime);
			this.UpdateAimingLine();
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00055CB0 File Offset: 0x00053EB0
		private void ShowStartScreen()
		{
			this.startScreen.SetActive(true);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00055CBE File Offset: 0x00053EBE
		private void HideStartScreen()
		{
			this.startScreen.SetActive(false);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00055CCC File Offset: 0x00053ECC
		private void ShowEndScreen()
		{
			this.endScreen.SetActive(true);
			this.endScreenLevelText.text = string.Format("LEVEL {0}", this.levelIndex);
			this.endScreenScoreText.text = string.Format("{0}", this.Score);
			this.failIndicator.SetActive(this.fail);
			this.clearIndicator.SetActive(this.clear);
			this.newRecordIndicator.SetActive(this.isHighScore);
			this.allLevelsClearIndicator.SetActive(this.allLevelsClear);
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00055D69 File Offset: 0x00053F69
		private void HideEndScreen()
		{
			this.endScreen.SetActive(false);
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00055D78 File Offset: 0x00053F78
		private void NewGame()
		{
			this.playing = true;
			this.levelIndex = 0;
			this.Score = 0;
			this.isHighScore = false;
			this.HideStartScreen();
			this.HideEndScreen();
			int[] levelData = this.LoadLevelData(this.levelIndex);
			this.StartNewLevel(levelData);
			this.RefreshLevelText();
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00055DC8 File Offset: 0x00053FC8
		private void NextLevel()
		{
			this.levelIndex++;
			this.HideStartScreen();
			this.HideEndScreen();
			int[] levelData = this.LoadLevelData(this.levelIndex);
			this.StartNewLevel(levelData);
			this.RefreshLevelText();
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00055E09 File Offset: 0x00054009
		private int[] LoadLevelData(int levelIndex)
		{
			return this.levelDataProvider.GetData(levelIndex);
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00055E18 File Offset: 0x00054018
		private Vector2Int LevelDataIndexToCoord(int index)
		{
			int num = this.layout.XCoordBorder.y - this.layout.XCoordBorder.x + 1;
			int num2 = index / num;
			return new Vector2Int(index % num, -num2);
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00055E58 File Offset: 0x00054058
		private void StartNewLevel(int[] levelData)
		{
			this.clear = false;
			this.fail = false;
			this.FloorStepETA = this.floorStepAfterShots;
			this.BubblePool.ReleaseAll();
			this.attachedBubbles.Clear();
			this.ResetFloor();
			for (int i = 0; i < levelData.Length; i++)
			{
				int num = levelData[i];
				if (num >= 0)
				{
					Vector2Int coord = this.LevelDataIndexToCoord(i);
					Bubble bubble = this.BubblePool.Get(null);
					bubble.Setup(this, num);
					this.Set(bubble, coord);
				}
			}
			this.PushRandomColor();
			this.PushRandomColor();
			this.SetStatus(BubblePopper.Status.Loaded);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00055EE9 File Offset: 0x000540E9
		private void ResetFloor()
		{
			this.floorYCoord = this.initialFloorYCoord;
			this.RefreshLayoutPosition();
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00055EFD File Offset: 0x000540FD
		private void StepFloor()
		{
			this.floorYCoord++;
			this.BeginMovingCeiling();
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00055F14 File Offset: 0x00054114
		private void RefreshLayoutPosition()
		{
			Vector3 localPosition = this.layout.transform.localPosition;
			localPosition.y = (float)(-(float)(this.floorYCoord - this.initialFloorYCoord)) * this.BubbleRadius * BubblePopperLayout.YOffsetFactor;
			this.layout.transform.localPosition = localPosition;
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00055F68 File Offset: 0x00054168
		private void UpdateStatus(float deltaTime)
		{
			switch (this.status)
			{
			case BubblePopper.Status.Idle:
			case BubblePopper.Status.GameOver:
				if (base.Game.GetButtonDown(MiniGame.Button.Start))
				{
					if (!this.playing || this.fail || this.allLevelsClear)
					{
						this.NewGame();
						return;
					}
					this.NextLevel();
					return;
				}
				break;
			case BubblePopper.Status.Loaded:
				break;
			case BubblePopper.Status.Launched:
				this.UpdateLaunched(deltaTime);
				return;
			case BubblePopper.Status.Settled:
				this.UpdateSettled(deltaTime);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00055FDA File Offset: 0x000541DA
		private void BeginMovingCeiling()
		{
			this.movingCeiling = true;
			this.moveCeilingT = 0f;
			this.originalCeilingPos = this.layout.transform.localPosition;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0005600C File Offset: 0x0005420C
		private void UpdateMoveCeiling(float deltaTime)
		{
			this.moveCeilingT += deltaTime;
			if (this.moveCeilingT >= this.moveCeilingTime)
			{
				this.movingCeiling = false;
				this.RefreshLayoutPosition();
				return;
			}
			Vector3 vector = this.layout.transform.localPosition;
			Vector2 b = new Vector2(vector.x, (float)(-(float)(this.floorYCoord - this.initialFloorYCoord)) * this.BubbleRadius * BubblePopperLayout.YOffsetFactor);
			float t = this.moveCeilingCurve.Evaluate(this.moveCeilingT / this.moveCeilingTime);
			vector = Vector2.LerpUnclamped(this.originalCeilingPos, b, t);
			this.layout.transform.localPosition = vector;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000560BA File Offset: 0x000542BA
		private void UpdateSettled(float deltaTime)
		{
			if (this.movingCeiling)
			{
				this.UpdateMoveCeiling(deltaTime);
				return;
			}
			if (this.CheckGameOver())
			{
				this.SetStatus(BubblePopper.Status.GameOver);
				return;
			}
			this.SetStatus(BubblePopper.Status.Loaded);
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x000560E4 File Offset: 0x000542E4
		private void HandleFloorStep()
		{
			int floorStepETA = this.FloorStepETA;
			this.FloorStepETA = floorStepETA - 1;
			if (this.FloorStepETA <= 0)
			{
				this.StepFloor();
				this.FloorStepETA = this.floorStepAfterShots;
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0005611C File Offset: 0x0005431C
		private bool CheckGameOver()
		{
			if (this.attachedBubbles.Count == 0)
			{
				this.clear = true;
				this.allLevelsClear = (this.levelIndex >= this.levelDataProvider.TotalLevels);
				if (this.clear)
				{
					if (this.levelIndex > BubblePopper.HighLevel)
					{
						BubblePopper.HighLevel = this.levelIndex;
					}
					Action<int> onLevelClear = BubblePopper.OnLevelClear;
					if (onLevelClear != null)
					{
						onLevelClear(this.levelIndex);
					}
				}
				return true;
			}
			if (this.attachedBubbles.Keys.Any((Vector2Int e) => e.y <= this.floorYCoord))
			{
				this.fail = true;
				return true;
			}
			return false;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x000561BC File Offset: 0x000543BC
		private void SetStatus(BubblePopper.Status newStatus)
		{
			this.OnExitStatus(this.status);
			this.status = newStatus;
			switch (this.status)
			{
			case BubblePopper.Status.Idle:
			case BubblePopper.Status.Loaded:
			case BubblePopper.Status.Launched:
				break;
			case BubblePopper.Status.Settled:
				this.PushRandomColor();
				this.HandleFloorStep();
				return;
			case BubblePopper.Status.GameOver:
				if (this.Score > BubblePopper.HighScore)
				{
					BubblePopper.HighScore = this.Score;
					this.isHighScore = true;
				}
				this.ShowGameOverScreen();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00056230 File Offset: 0x00054430
		private void ShowGameOverScreen()
		{
			this.ShowEndScreen();
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00056238 File Offset: 0x00054438
		private void OnExitStatus(BubblePopper.Status status)
		{
			switch (status)
			{
			default:
				return;
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00056250 File Offset: 0x00054450
		private void Set(Bubble bubble, Vector2Int coord)
		{
			this.attachedBubbles[coord] = bubble;
			bubble.NotifyAttached(coord);
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00056268 File Offset: 0x00054468
		private void Attach(Bubble bubble, Vector2Int coord)
		{
			Bubble bubble2;
			if (this.attachedBubbles.TryGetValue(coord, out bubble2))
			{
				Debug.LogError("Target coord is occupied!");
				return;
			}
			this.Set(bubble, coord);
			List<Vector2Int> continousCoords = this.GetContinousCoords(coord);
			if (continousCoords.Count >= 3)
			{
				HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
				int num = 0;
				foreach (Vector2Int vector2Int in continousCoords)
				{
					hashSet.AddRange(this.layout.GetAllNeighbourCoords(vector2Int, false));
					this.Explode(vector2Int, coord);
					num++;
				}
				this.PunchCamera();
				HashSet<Vector2Int> looseCoords = this.GetLooseCoords(hashSet);
				foreach (Vector2Int coord2 in looseCoords)
				{
					this.Detach(coord2);
				}
				this.CalculateAndAddScore(looseCoords, continousCoords);
			}
			this.Shockwave(coord, this.shockwaveStrength).Forget();
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0005637C File Offset: 0x0005457C
		private void CalculateAndAddScore(HashSet<Vector2Int> detached, List<Vector2Int> exploded)
		{
			float count = (float)exploded.Count;
			int count2 = detached.Count;
			int num = Mathf.FloorToInt(Mathf.Pow(count, 2f)) * (1 + count2);
			this.Score += num;
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000563BC File Offset: 0x000545BC
		private void Explode(Vector2Int coord, Vector2Int origin)
		{
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(coord, out bubble))
			{
				return;
			}
			this.attachedBubbles.Remove(coord);
			if (bubble == null)
			{
				return;
			}
			bubble.NotifyExplode(origin);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000563F8 File Offset: 0x000545F8
		private List<Vector2Int> GetContinousCoords(Vector2Int root)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(root, out bubble))
			{
				return list;
			}
			if (bubble == null)
			{
				return list;
			}
			int colorIndex = bubble.ColorIndex;
			BubblePopper.<>c__DisplayClass121_0 CS$<>8__locals1;
			CS$<>8__locals1.visitedCoords = new HashSet<Vector2Int>();
			CS$<>8__locals1.coords = new Stack<Vector2Int>();
			BubblePopper.<GetContinousCoords>g__Push|121_0(root, ref CS$<>8__locals1);
			while (CS$<>8__locals1.coords.Count > 0)
			{
				Vector2Int vector2Int = CS$<>8__locals1.coords.Pop();
				Bubble bubble2;
				if (this.attachedBubbles.TryGetValue(vector2Int, out bubble2) && !(bubble2 == null) && bubble2.ColorIndex == colorIndex)
				{
					list.Add(vector2Int);
					foreach (Vector2Int vector2Int2 in this.layout.GetAllNeighbourCoords(vector2Int, false))
					{
						if (!CS$<>8__locals1.visitedCoords.Contains(vector2Int2))
						{
							BubblePopper.<GetContinousCoords>g__Push|121_0(vector2Int2, ref CS$<>8__locals1);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000564E8 File Offset: 0x000546E8
		private HashSet<Vector2Int> GetLooseCoords(HashSet<Vector2Int> roots)
		{
			BubblePopper.<>c__DisplayClass122_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pendingRoots = roots.ToList<Vector2Int>();
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			while (CS$<>8__locals1.pendingRoots.Count > 0)
			{
				Vector2Int root = this.<GetLooseCoords>g__PopRoot|122_0(ref CS$<>8__locals1);
				List<Vector2Int> range;
				if (this.<GetLooseCoords>g__CheckConnectedLoose|122_1(root, out range, ref CS$<>8__locals1))
				{
					hashSet.AddRange(range);
				}
			}
			return hashSet;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00056540 File Offset: 0x00054740
		private void Detach(Vector2Int coord)
		{
			Bubble bubble;
			if (!this.attachedBubbles.TryGetValue(coord, out bubble))
			{
				return;
			}
			this.attachedBubbles.Remove(coord);
			bubble.NotifyDetached();
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00056574 File Offset: 0x00054774
		private void UpdateAimingLine()
		{
			this.aimingLine.gameObject.SetActive(this.status == BubblePopper.Status.Loaded);
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint(this.cannon.position);
			Vector3 vector2 = worldToLocalMatrix.MultiplyVector(this.cannon.up);
			Vector3 v = vector2 * this.aimingDistance;
			BubblePopper.CastResult castResult = this.SlideCast(vector, v);
			vector.z = 0f;
			this.aimlinePoints[0] = vector;
			this.aimlinePoints[1] = castResult.endPosition;
			if (castResult.touchWall)
			{
				float d = Mathf.Max(this.aimingDistance - (castResult.endPosition - vector).magnitude, 0f);
				Vector2 a = vector2;
				a.x *= -1f;
				this.aimlinePoints[2] = castResult.endPosition + a * d;
			}
			else
			{
				this.aimlinePoints[2] = castResult.endPosition;
			}
			this.aimingLine.SetPositions(this.aimlinePoints);
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000566C3 File Offset: 0x000548C3
		private void UpdateLaunched(float deltaTime)
		{
			if (this.activeBubble == null || this.activeBubble.status != Bubble.Status.Moving)
			{
				this.activeBubble = null;
				this.SetStatus(BubblePopper.Status.Settled);
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000566F0 File Offset: 0x000548F0
		private void HandleInput(float deltaTime)
		{
			float x = base.Game.GetAxis(0).x;
			this.cannonAngle = Mathf.Clamp(this.cannonAngle - x * this.cannonRotateSpeed * deltaTime, this.cannonAngleRange.x, this.cannonAngleRange.y);
			this.cannon.rotation = Quaternion.Euler(0f, 0f, this.cannonAngle);
			this.duckAnimator.SetInteger("MovementDirection", (x > 0.01f) ? 1 : ((x < -0.01f) ? -1 : 0));
			this.gear.Rotate(0f, 0f, x * this.cannonRotateSpeed * deltaTime);
			if (base.Game.GetButtonDown(MiniGame.Button.A))
			{
				this.LaunchBubble();
			}
			if (base.Game.GetButtonDown(MiniGame.Button.B))
			{
				this.NextPallette();
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000567D0 File Offset: 0x000549D0
		public void MoveBubble(Bubble bubble, float deltaTime)
		{
			if (bubble == null)
			{
				return;
			}
			Vector2 moveDirection = bubble.MoveDirection;
			float d = deltaTime * this.bubbleMoveSpeed;
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Matrix4x4 localToWorldMatrix = this.layout.transform.localToWorldMatrix;
			Vector2 normalized = moveDirection.normalized;
			Vector2 origin = worldToLocalMatrix.MultiplyPoint(bubble.transform.position);
			Vector2 delta = worldToLocalMatrix.MultiplyVector(moveDirection.normalized) * d;
			BubblePopper.CastResult castResult = this.SlideCast(origin, delta);
			bubble.transform.position = localToWorldMatrix.MultiplyPoint(castResult.endPosition);
			if (!castResult.Collide)
			{
				return;
			}
			if (castResult.touchWall && (float)castResult.touchWallDirection * normalized.x > 0f)
			{
				moveDirection.x *= -1f;
				bubble.MoveDirection = moveDirection;
			}
			if (castResult.touchingBubble || castResult.touchCeiling)
			{
				this.Attach(bubble, castResult.endCoord);
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000568EC File Offset: 0x00054AEC
		private Bubble LaunchBubble(Vector2 origin, Vector2 direction, int colorIndex)
		{
			Bubble bubble = this.BubblePool.Get(null);
			bubble.transform.position = this.layout.transform.localToWorldMatrix.MultiplyPoint(origin);
			bubble.MoveDirection = direction;
			bubble.Setup(this, colorIndex);
			bubble.Launch(direction);
			return bubble;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00056944 File Offset: 0x00054B44
		private void LaunchBubble()
		{
			if (this.status != BubblePopper.Status.Loaded)
			{
				return;
			}
			this.activeBubble = this.LaunchBubble(this.layout.transform.worldToLocalMatrix.MultiplyPoint(this.cannon.transform.position), this.layout.transform.worldToLocalMatrix.MultiplyVector(this.cannon.transform.up), this.loadedColor);
			this.loadedColor = -1;
			this.RefreshColorIndicators();
			this.SetStatus(BubblePopper.Status.Launched);
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000569DC File Offset: 0x00054BDC
		private void PunchLoadedIndicator()
		{
			this.loadedColorIndicator.transform.DOKill(true);
			this.loadedColorIndicator.transform.localPosition = Vector2.left * 15f;
			this.loadedColorIndicator.transform.DOLocalMove(Vector3.zero, 0.1f, true);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x00056A3C File Offset: 0x00054C3C
		private void PunchWaitingIndicator()
		{
			this.waitingColorIndicator.transform.localPosition = Vector2.zero;
			this.waitingColorIndicator.transform.DOKill(true);
			this.waitingColorIndicator.transform.DOPunchPosition(Vector3.down * 5f, 0.5f, 10, 1f, true);
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00056AA4 File Offset: 0x00054CA4
		private void PushRandomColor()
		{
			this.loadedColor = this.waitingColor;
			this.waitingColor = UnityEngine.Random.Range(0, this.AvaliableColorCount);
			if (this.attachedBubbles.Count <= 0)
			{
				this.waitingColor = UnityEngine.Random.Range(0, this.AvaliableColorCount);
			}
			List<int> list = (from e in this.attachedBubbles.Values
			group e by e.ColorIndex into g
			select g.Key).ToList<int>();
			this.waitingColor = list.GetRandom<int>();
			this.RefreshColorIndicators();
			this.PunchLoadedIndicator();
			this.PunchWaitingIndicator();
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00056B66 File Offset: 0x00054D66
		private void RefreshColorIndicators()
		{
			this.loadedColorIndicator.color = this.GetDisplayColor(this.loadedColor);
			this.waitingColorIndicator.color = this.GetDisplayColor(this.waitingColor);
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00056B96 File Offset: 0x00054D96
		private bool IsCoordOccupied(Vector2Int coord, out Bubble touchingBubble, out bool ceiling)
		{
			ceiling = false;
			if (this.attachedBubbles.TryGetValue(coord, out touchingBubble))
			{
				return true;
			}
			if (coord.y > this.ceilingYCoord)
			{
				ceiling = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00056BC4 File Offset: 0x00054DC4
		public BubblePopper.CastResult SlideCast(Vector2 origin, Vector2 delta)
		{
			float magnitude = delta.magnitude;
			Vector2 normalized = delta.normalized;
			float bubbleRadius = this.BubbleRadius;
			BubblePopper.CastResult castResult = default(BubblePopper.CastResult);
			castResult.origin = origin;
			castResult.castDirection = normalized;
			castResult.castDistance = magnitude;
			Vector2 vector = origin + delta;
			float d = 1f;
			float num = this.layout.XPositionBorder.x + bubbleRadius;
			float num2 = this.layout.XPositionBorder.y - bubbleRadius;
			if (origin.x < num || origin.x > num2)
			{
				Vector2 vector2 = origin;
				vector2.x = Mathf.Clamp(vector2.x, num + 0.001f, num2 - 0.001f);
				castResult.endPosition = vector2;
				castResult.clipWall = true;
				castResult.collide = true;
			}
			else
			{
				if (vector.x < num)
				{
					castResult.touchWall = true;
					d = Mathf.Abs(origin.x - num) / Mathf.Abs(delta.x);
					castResult.touchWallDirection = -1;
				}
				else if (vector.x > num2)
				{
					castResult.touchWall = true;
					d = Mathf.Abs(num2 - origin.x) / Mathf.Abs(delta.x);
					castResult.touchWallDirection = 1;
				}
				delta *= d;
				magnitude = delta.magnitude;
				castResult.endPosition = origin + delta;
				List<Vector2Int> allPassingCoords = this.layout.GetAllPassingCoords(origin, normalized, delta.magnitude);
				float num3 = magnitude;
				foreach (Vector2Int vector2Int in allPassingCoords)
				{
					Bubble touchingBubble;
					bool touchCeiling;
					Vector2 vector3;
					if (this.IsCoordOccupied(vector2Int, out touchingBubble, out touchCeiling) && this.BubbleCast(this.layout.CoordToLocalPosition(vector2Int), origin, normalized, magnitude, out vector3))
					{
						float magnitude2 = (vector3 - origin).magnitude;
						if (magnitude2 < num3)
						{
							castResult.collide = true;
							castResult.touchingBubble = touchingBubble;
							castResult.touchBubbleCoord = vector2Int;
							castResult.endPosition = vector3;
							castResult.touchCeiling = touchCeiling;
							num3 = magnitude2;
							castResult.touchWall = false;
						}
					}
				}
			}
			castResult.endCoord = this.layout.LocalPositionToCoord(castResult.endPosition);
			return castResult;
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00056E14 File Offset: 0x00055014
		private bool BubbleCast(Vector2 pos, Vector2 origin, Vector2 direction, float distance, out Vector2 hitCircleCenter)
		{
			float bubbleRadius = this.BubbleRadius;
			hitCircleCenter = origin;
			Vector2 vector = pos - origin;
			float sqrMagnitude = vector.sqrMagnitude;
			float magnitude = vector.magnitude;
			if (magnitude > distance + 2f * bubbleRadius)
			{
				return false;
			}
			if (magnitude <= bubbleRadius * 2f)
			{
				hitCircleCenter = pos - 2f * vector.normalized * bubbleRadius;
				return true;
			}
			if (Vector2.Dot(vector, direction) < 0f)
			{
				return false;
			}
			float f = 0.017453292f * Vector2.Angle(vector, direction);
			float num = vector.magnitude * Mathf.Sin(f);
			if (num > 2f * bubbleRadius)
			{
				return false;
			}
			float num2 = num * num;
			float num3 = bubbleRadius * bubbleRadius * 2f * 2f;
			float num4 = Mathf.Sqrt(sqrMagnitude - num2) - Mathf.Sqrt(num3 - num2);
			if (num4 > distance)
			{
				return false;
			}
			hitCircleCenter = origin + direction * num4;
			return true;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00056F10 File Offset: 0x00055110
		private void OnDrawGizmos()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			float bubbleRadius = this.BubbleRadius;
			Matrix4x4 worldToLocalMatrix = this.layout.transform.worldToLocalMatrix;
			Vector3 vector = worldToLocalMatrix.MultiplyPoint(this.cannon.position);
			Vector3 a = worldToLocalMatrix.MultiplyVector(this.cannon.up);
			BubblePopper.CastResult castResult = this.SlideCast(vector, a * this.distance);
			Gizmos.matrix = this.layout.transform.localToWorldMatrix;
			Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
			for (int i = this.layout.XCoordBorder.x; i <= this.layout.XCoordBorder.y; i++)
			{
				for (int j = this.floorYCoord; j <= this.ceilingYCoord; j++)
				{
					new Vector2Int(i, j);
					this.layout.GizmosDrawCoord(new Vector2Int(i, j), 0.25f);
				}
			}
			Gizmos.color = (castResult.Collide ? Color.red : Color.green);
			Gizmos.DrawWireSphere(vector, bubbleRadius);
			Gizmos.DrawWireSphere(castResult.endPosition, bubbleRadius);
			Gizmos.DrawLine(vector, castResult.endPosition);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(this.layout.CoordToLocalPosition(castResult.endCoord), bubbleRadius * 0.8f);
			if (castResult.collide)
			{
				Gizmos.color = Color.white;
				Gizmos.DrawWireSphere(this.layout.CoordToLocalPosition(castResult.touchBubbleCoord), bubbleRadius * 0.5f);
			}
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000570C9 File Offset: 0x000552C9
		internal void Release(Bubble bubble)
		{
			this.BubblePool.Release(bubble);
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000570D7 File Offset: 0x000552D7
		internal Color GetDisplayColor(int colorIndex)
		{
			if (colorIndex < 0)
			{
				return Color.clear;
			}
			if (colorIndex >= this.colorPallette.Length)
			{
				return Color.white;
			}
			return this.colorPallette[colorIndex];
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00057100 File Offset: 0x00055300
		public void SetPallette(Color[] colors)
		{
			this.colorPallette = new Color[colors.Length];
			colors.CopyTo(this.colorPallette, 0);
			foreach (Bubble bubble in this.BubblePool.ActiveEntries)
			{
				bubble.RefreshColor();
			}
			this.RefreshColorIndicators();
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00057170 File Offset: 0x00055370
		private UniTask Shockwave(Vector2Int origin, float amplitude)
		{
			BubblePopper.<Shockwave>d__145 <Shockwave>d__;
			<Shockwave>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<Shockwave>d__.<>4__this = this;
			<Shockwave>d__.origin = origin;
			<Shockwave>d__.amplitude = amplitude;
			<Shockwave>d__.<>1__state = -1;
			<Shockwave>d__.<>t__builder.Start<BubblePopper.<Shockwave>d__145>(ref <Shockwave>d__);
			return <Shockwave>d__.<>t__builder.Task;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000571C4 File Offset: 0x000553C4
		private void PunchCamera()
		{
			this.cameraParent.DOKill(true);
			this.cameraParent.DOShakePosition(0.4f, 1f, 10, 90f, false, true);
			this.cameraParent.DOShakeRotation(0.4f, Vector3.forward, 10, 90f, true);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000572BC File Offset: 0x000554BC
		[CompilerGenerated]
		internal static void <GetContinousCoords>g__Push|121_0(Vector2Int coord, ref BubblePopper.<>c__DisplayClass121_0 A_1)
		{
			A_1.coords.Push(coord);
			A_1.visitedCoords.Add(coord);
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000572D7 File Offset: 0x000554D7
		[CompilerGenerated]
		private Vector2Int <GetLooseCoords>g__PopRoot|122_0(ref BubblePopper.<>c__DisplayClass122_0 A_1)
		{
			Vector2Int result = A_1.pendingRoots[0];
			A_1.pendingRoots.RemoveAt(0);
			return result;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000572F4 File Offset: 0x000554F4
		[CompilerGenerated]
		private bool <GetLooseCoords>g__CheckConnectedLoose|122_1(Vector2Int root, out List<Vector2Int> connected, ref BubblePopper.<>c__DisplayClass122_0 A_3)
		{
			connected = new List<Vector2Int>();
			bool result = true;
			Stack<Vector2Int> stack = new Stack<Vector2Int>();
			HashSet<Vector2Int> hashSet = new HashSet<Vector2Int>();
			stack.Push(root);
			hashSet.Add(root);
			while (stack.Count > 0)
			{
				Vector2Int vector2Int = stack.Pop();
				A_3.pendingRoots.Remove(vector2Int);
				if (this.attachedBubbles.ContainsKey(vector2Int))
				{
					if (vector2Int.y >= this.ceilingYCoord)
					{
						result = false;
					}
					connected.Add(vector2Int);
					foreach (Vector2Int item in this.layout.GetAllNeighbourCoords(vector2Int, false))
					{
						if (!hashSet.Contains(item))
						{
							stack.Push(item);
							hashSet.Add(item);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001109 RID: 4361
		[SerializeField]
		private Bubble bubbleTemplate;

		// Token: 0x0400110A RID: 4362
		[SerializeField]
		private BubblePopperLayout layout;

		// Token: 0x0400110B RID: 4363
		[SerializeField]
		private Image waitingColorIndicator;

		// Token: 0x0400110C RID: 4364
		[SerializeField]
		private Image loadedColorIndicator;

		// Token: 0x0400110D RID: 4365
		[SerializeField]
		private Transform cannon;

		// Token: 0x0400110E RID: 4366
		[SerializeField]
		private LineRenderer aimingLine;

		// Token: 0x0400110F RID: 4367
		[SerializeField]
		private Transform cameraParent;

		// Token: 0x04001110 RID: 4368
		[SerializeField]
		private Animator duckAnimator;

		// Token: 0x04001111 RID: 4369
		[SerializeField]
		private Transform gear;

		// Token: 0x04001112 RID: 4370
		[SerializeField]
		private TextMeshProUGUI scoreText;

		// Token: 0x04001113 RID: 4371
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x04001114 RID: 4372
		[SerializeField]
		private TextMeshProUGUI highScoreText;

		// Token: 0x04001115 RID: 4373
		[SerializeField]
		private GameObject startScreen;

		// Token: 0x04001116 RID: 4374
		[SerializeField]
		private GameObject endScreen;

		// Token: 0x04001117 RID: 4375
		[SerializeField]
		private GameObject failIndicator;

		// Token: 0x04001118 RID: 4376
		[SerializeField]
		private GameObject clearIndicator;

		// Token: 0x04001119 RID: 4377
		[SerializeField]
		private GameObject newRecordIndicator;

		// Token: 0x0400111A RID: 4378
		[SerializeField]
		private GameObject allLevelsClearIndicator;

		// Token: 0x0400111B RID: 4379
		[SerializeField]
		private TextMeshProUGUI endScreenLevelText;

		// Token: 0x0400111C RID: 4380
		[SerializeField]
		private TextMeshProUGUI endScreenScoreText;

		// Token: 0x0400111D RID: 4381
		[SerializeField]
		private BubblePopperLevelDataProvider levelDataProvider;

		// Token: 0x0400111E RID: 4382
		[SerializeField]
		private Color[] colorPallette;

		// Token: 0x0400111F RID: 4383
		private int palletteIndex;

		// Token: 0x04001120 RID: 4384
		[SerializeField]
		public List<BubblePopper.Pallette> pallettes;

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private float aimingDistance = 100f;

		// Token: 0x04001122 RID: 4386
		[SerializeField]
		private Vector2 cannonAngleRange = new Vector2(-45f, 45f);

		// Token: 0x04001123 RID: 4387
		[SerializeField]
		private float cannonRotateSpeed = 20f;

		// Token: 0x04001124 RID: 4388
		[SerializeField]
		private int ceilingYCoord;

		// Token: 0x04001125 RID: 4389
		[SerializeField]
		private int initialFloorYCoord = -18;

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private int floorStepAfterShots = 4;

		// Token: 0x04001127 RID: 4391
		[SerializeField]
		private float bubbleMoveSpeed = 100f;

		// Token: 0x04001128 RID: 4392
		private float shockwaveStrength = 2f;

		// Token: 0x04001129 RID: 4393
		[SerializeField]
		private float moveCeilingTime = 1f;

		// Token: 0x0400112A RID: 4394
		[SerializeField]
		private AnimationCurve moveCeilingCurve;

		// Token: 0x0400112B RID: 4395
		private PrefabPool<Bubble> _bubblePool;

		// Token: 0x0400112C RID: 4396
		private Dictionary<Vector2Int, Bubble> attachedBubbles = new Dictionary<Vector2Int, Bubble>();

		// Token: 0x0400112D RID: 4397
		private float cannonAngle;

		// Token: 0x0400112E RID: 4398
		private int waitingColor;

		// Token: 0x0400112F RID: 4399
		private int loadedColor;

		// Token: 0x04001130 RID: 4400
		private Bubble activeBubble;

		// Token: 0x04001132 RID: 4402
		private bool clear;

		// Token: 0x04001133 RID: 4403
		private bool fail;

		// Token: 0x04001134 RID: 4404
		private bool allLevelsClear;

		// Token: 0x04001135 RID: 4405
		private bool playing;

		// Token: 0x04001136 RID: 4406
		[SerializeField]
		private int floorYCoord;

		// Token: 0x04001138 RID: 4408
		private int levelIndex;

		// Token: 0x04001139 RID: 4409
		private int _score;

		// Token: 0x0400113A RID: 4410
		private bool isHighScore;

		// Token: 0x0400113B RID: 4411
		private const string HighScoreSaveKey = "MiniGame/BubblePopper/HighScore";

		// Token: 0x0400113C RID: 4412
		private const string HighLevelSaveKey = "MiniGame/BubblePopper/HighLevel";

		// Token: 0x0400113E RID: 4414
		private const int CriticalCount = 3;

		// Token: 0x04001140 RID: 4416
		private bool movingCeiling;

		// Token: 0x04001141 RID: 4417
		private float moveCeilingT;

		// Token: 0x04001142 RID: 4418
		private Vector2 originalCeilingPos;

		// Token: 0x04001143 RID: 4419
		private Vector3[] aimlinePoints = new Vector3[3];

		// Token: 0x04001144 RID: 4420
		[SerializeField]
		private bool drawGizmos = true;

		// Token: 0x04001145 RID: 4421
		[SerializeField]
		private float distance;

		// Token: 0x0200057D RID: 1405
		[Serializable]
		public struct Pallette
		{
			// Token: 0x04001FC5 RID: 8133
			public Color[] colors;
		}

		// Token: 0x0200057E RID: 1406
		public enum Status
		{
			// Token: 0x04001FC7 RID: 8135
			Idle,
			// Token: 0x04001FC8 RID: 8136
			Loaded,
			// Token: 0x04001FC9 RID: 8137
			Launched,
			// Token: 0x04001FCA RID: 8138
			Settled,
			// Token: 0x04001FCB RID: 8139
			GameOver
		}

		// Token: 0x0200057F RID: 1407
		public struct CastResult
		{
			// Token: 0x17000776 RID: 1910
			// (get) Token: 0x060028C1 RID: 10433 RVA: 0x00096B96 File Offset: 0x00094D96
			public bool Collide
			{
				get
				{
					return this.collide || this.clipWall || this.touchWall || this.touchingBubble;
				}
			}

			// Token: 0x04001FCC RID: 8140
			public Vector2 origin;

			// Token: 0x04001FCD RID: 8141
			public Vector2 castDirection;

			// Token: 0x04001FCE RID: 8142
			public float castDistance;

			// Token: 0x04001FCF RID: 8143
			public bool clipWall;

			// Token: 0x04001FD0 RID: 8144
			public bool touchWall;

			// Token: 0x04001FD1 RID: 8145
			public int touchWallDirection;

			// Token: 0x04001FD2 RID: 8146
			public bool collide;

			// Token: 0x04001FD3 RID: 8147
			public Bubble touchingBubble;

			// Token: 0x04001FD4 RID: 8148
			public Vector2Int touchBubbleCoord;

			// Token: 0x04001FD5 RID: 8149
			public bool touchCeiling;

			// Token: 0x04001FD6 RID: 8150
			public Vector2 endPosition;

			// Token: 0x04001FD7 RID: 8151
			public Vector2Int endCoord;
		}
	}
}
