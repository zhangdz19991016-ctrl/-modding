using System;
using System.Collections.Generic;
using Duckov.Utilities;
using ParadoxNotion;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class AIMainBrain : MonoBehaviour
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x00025F5C File Offset: 0x0002415C
	private static CharacterMainControl mainCharacter
	{
		get
		{
			if (AIMainBrain._mc == null)
			{
				AIMainBrain._mc = CharacterMainControl.Main;
			}
			return AIMainBrain._mc;
		}
	}

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x0600087A RID: 2170 RVA: 0x00025F7C File Offset: 0x0002417C
	// (remove) Token: 0x0600087B RID: 2171 RVA: 0x00025FB0 File Offset: 0x000241B0
	public static event Action<AISound> OnSoundSpawned;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x0600087C RID: 2172 RVA: 0x00025FE4 File Offset: 0x000241E4
	// (remove) Token: 0x0600087D RID: 2173 RVA: 0x00026018 File Offset: 0x00024218
	public static event Action<AISound> OnPlayerHearSound;

	// Token: 0x0600087E RID: 2174 RVA: 0x0002604B File Offset: 0x0002424B
	public static void MakeSound(AISound sound)
	{
		Action<AISound> onSoundSpawned = AIMainBrain.OnSoundSpawned;
		if (onSoundSpawned != null)
		{
			onSoundSpawned(sound);
		}
		AIMainBrain.FilterPlayerHearSound(sound);
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00026064 File Offset: 0x00024264
	private static void FilterPlayerHearSound(AISound sound)
	{
		if (!AIMainBrain.mainCharacter)
		{
			return;
		}
		if (!Team.IsEnemy(Teams.player, sound.fromTeam))
		{
			return;
		}
		if (sound.fromCharacter && sound.fromCharacter.characterModel && !sound.fromCharacter.characterModel.Hidden && !GameCamera.Instance.IsOffScreen(sound.pos))
		{
			return;
		}
		float num = Vector3.Distance(sound.pos, AIMainBrain.mainCharacter.transform.position);
		if (AIMainBrain.mainCharacter.SoundVisable < 0.2f)
		{
			return;
		}
		float hearingAbility = AIMainBrain.mainCharacter.HearingAbility;
		if (num > sound.radius * hearingAbility)
		{
			return;
		}
		Action<AISound> onPlayerHearSound = AIMainBrain.OnPlayerHearSound;
		if (onPlayerHearSound == null)
		{
			return;
		}
		onPlayerHearSound(sound);
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00026125 File Offset: 0x00024325
	public void Awake()
	{
		this.searchTasks = new Queue<AIMainBrain.SearchTaskContext>();
		this.checkObsticleTasks = new Queue<AIMainBrain.CheckObsticleTaskContext>();
		this.fowBlockLayer = LayerMask.NameToLayer("FowBlock");
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00026150 File Offset: 0x00024350
	private void Start()
	{
		this.dmgReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
		this.interactLayers = 1 << LayerMask.NameToLayer("Interactable");
		this.obsticleLayers = GameplayDataSettings.Layers.fowBlockLayers;
		this.obsticleLayersWithThermal = GameplayDataSettings.Layers.fowBlockLayersWithThermal;
		this.cols = new Collider[15];
		this.ObsHits = new RaycastHit[15];
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x000261C4 File Offset: 0x000243C4
	private void Update()
	{
		int num = 0;
		while (num < this.maxSeachCount && this.searchTasks.Count > 0)
		{
			this.DoSearch(this.searchTasks.Dequeue());
			num++;
		}
		int num2 = 0;
		while (num2 < this.maxCheckObsticleCount && this.checkObsticleTasks.Count > 0)
		{
			this.DoCheckObsticle(this.checkObsticleTasks.Dequeue());
			num2++;
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00026234 File Offset: 0x00024434
	private void DoSearch(AIMainBrain.SearchTaskContext context)
	{
		int num = Physics.OverlapSphereNonAlloc(context.searchCenter, context.searchDistance, this.cols, (context.searchPickupID > 0) ? (this.dmgReceiverLayers | this.interactLayers) : this.dmgReceiverLayers, QueryTriggerInteraction.Collide);
		if (num <= 0)
		{
			context.onSearchFinishedCallback(null, null);
			return;
		}
		float num2 = 9999f;
		DamageReceiver arg = null;
		float num3 = 9999f;
		InteractablePickup arg2 = null;
		float num4 = 1.5f;
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.cols[i];
			if (Mathf.Abs(context.searchCenter.y - collider.transform.position.y) <= 4f)
			{
				float num5 = Vector3.Distance(context.searchCenter, collider.transform.position);
				if (Vector3.Angle(context.searchDirection.normalized, (collider.transform.position - context.searchCenter).normalized) <= context.searchAngle * 0.5f || num5 <= num4)
				{
					this.dmgReceiverTemp = null;
					float num6 = 1f;
					if (collider.gameObject.IsInLayerMask(this.dmgReceiverLayers))
					{
						this.dmgReceiverTemp = collider.GetComponent<DamageReceiver>();
						if (this.dmgReceiverTemp != null && this.dmgReceiverTemp.health)
						{
							CharacterMainControl characterMainControl = this.dmgReceiverTemp.health.TryGetCharacter();
							if (characterMainControl)
							{
								num6 = characterMainControl.VisableDistanceFactor;
							}
						}
					}
					if (num5 <= context.searchDistance * num6 && (num5 < num2 || num5 < num3) && (!context.checkObsticle || num5 <= num4 || !this.CheckObsticle(context.searchCenter, collider.transform.position + Vector3.up * 1.5f, context.thermalOn, context.ignoreFowBlockLayer)))
					{
						if (this.dmgReceiverTemp)
						{
							if (!(this.dmgReceiverTemp.health == null) && Team.IsEnemy(context.selfTeam, this.dmgReceiverTemp.Team))
							{
								num2 = num5;
								arg = this.dmgReceiverTemp;
							}
						}
						else if (context.searchPickupID > 0)
						{
							InteractablePickup component = collider.GetComponent<InteractablePickup>();
							if (component && component.ItemAgent && component.ItemAgent.Item && component.ItemAgent.Item.TypeID == context.searchPickupID)
							{
								num3 = num5;
								arg2 = component;
							}
						}
					}
				}
			}
		}
		context.onSearchFinishedCallback(arg, arg2);
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x000264EC File Offset: 0x000246EC
	public void AddSearchTask(Vector3 center, Vector3 dir, float searchAngle, float searchDistance, Teams selfTeam, bool checkObsticle, bool thermalOn, bool ignoreFowBlockLayer, int searchPickupID, Action<DamageReceiver, InteractablePickup> callback)
	{
		AIMainBrain.SearchTaskContext item = new AIMainBrain.SearchTaskContext(center, dir, searchAngle, searchDistance, selfTeam, checkObsticle, thermalOn, ignoreFowBlockLayer, searchPickupID, callback);
		this.searchTasks.Enqueue(item);
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00026520 File Offset: 0x00024720
	private void DoCheckObsticle(AIMainBrain.CheckObsticleTaskContext context)
	{
		bool obj = this.CheckObsticle(context.start, context.end, context.thermalOn, context.ignoreFowBlockLayer);
		context.onCheckFinishCallback(obj);
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00026558 File Offset: 0x00024758
	public void AddCheckObsticleTask(Vector3 start, Vector3 end, bool thermalOn, bool ignoreFowBlockLayer, Action<bool> callback)
	{
		AIMainBrain.CheckObsticleTaskContext item = new AIMainBrain.CheckObsticleTaskContext(start, end, thermalOn, ignoreFowBlockLayer, callback);
		this.checkObsticleTasks.Enqueue(item);
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00026580 File Offset: 0x00024780
	private bool CheckObsticle(Vector3 startPoint, Vector3 endPoint, bool thermalOn, bool ignoreFowBlockLayer)
	{
		Ray ray = new Ray(startPoint, (endPoint - startPoint).normalized);
		LayerMask mask = thermalOn ? this.obsticleLayersWithThermal : this.obsticleLayers;
		if (ignoreFowBlockLayer)
		{
			mask &= ~(1 << this.fowBlockLayer);
		}
		return Physics.RaycastNonAlloc(ray, this.ObsHits, (endPoint - startPoint).magnitude, mask) > 0;
	}

	// Token: 0x040007B7 RID: 1975
	private Queue<AIMainBrain.SearchTaskContext> searchTasks;

	// Token: 0x040007B8 RID: 1976
	private Queue<AIMainBrain.CheckObsticleTaskContext> checkObsticleTasks;

	// Token: 0x040007B9 RID: 1977
	private LayerMask dmgReceiverLayers;

	// Token: 0x040007BA RID: 1978
	private LayerMask interactLayers;

	// Token: 0x040007BB RID: 1979
	private LayerMask obsticleLayers;

	// Token: 0x040007BC RID: 1980
	private LayerMask obsticleLayersWithThermal;

	// Token: 0x040007BD RID: 1981
	private Collider[] cols;

	// Token: 0x040007BE RID: 1982
	private RaycastHit[] ObsHits;

	// Token: 0x040007BF RID: 1983
	public int maxSeachCount;

	// Token: 0x040007C0 RID: 1984
	public int maxCheckObsticleCount;

	// Token: 0x040007C1 RID: 1985
	private static CharacterMainControl _mc;

	// Token: 0x040007C4 RID: 1988
	private int fowBlockLayer;

	// Token: 0x040007C5 RID: 1989
	private DamageReceiver dmgReceiverTemp;

	// Token: 0x0200048C RID: 1164
	public struct SearchTaskContext
	{
		// Token: 0x060026F5 RID: 9973 RVA: 0x0008A5F8 File Offset: 0x000887F8
		public SearchTaskContext(Vector3 center, Vector3 dir, float searchAngle, float searchDistance, Teams selfTeam, bool checkObsticle, bool thermalOn, bool ignoreFowBlockLayer, int searchPickupID, Action<DamageReceiver, InteractablePickup> callback)
		{
			this.searchCenter = center;
			this.searchDirection = dir;
			this.searchAngle = searchAngle;
			this.searchDistance = searchDistance;
			this.selfTeam = selfTeam;
			this.thermalOn = thermalOn;
			this.checkObsticle = checkObsticle;
			this.searchPickupID = searchPickupID;
			this.onSearchFinishedCallback = callback;
			this.ignoreFowBlockLayer = ignoreFowBlockLayer;
		}

		// Token: 0x04001BCE RID: 7118
		public Vector3 searchCenter;

		// Token: 0x04001BCF RID: 7119
		public Vector3 searchDirection;

		// Token: 0x04001BD0 RID: 7120
		public float searchAngle;

		// Token: 0x04001BD1 RID: 7121
		public float searchDistance;

		// Token: 0x04001BD2 RID: 7122
		public Teams selfTeam;

		// Token: 0x04001BD3 RID: 7123
		public bool checkObsticle;

		// Token: 0x04001BD4 RID: 7124
		public bool thermalOn;

		// Token: 0x04001BD5 RID: 7125
		public bool ignoreFowBlockLayer;

		// Token: 0x04001BD6 RID: 7126
		public int searchPickupID;

		// Token: 0x04001BD7 RID: 7127
		public Action<DamageReceiver, InteractablePickup> onSearchFinishedCallback;
	}

	// Token: 0x0200048D RID: 1165
	public struct CheckObsticleTaskContext
	{
		// Token: 0x060026F6 RID: 9974 RVA: 0x0008A652 File Offset: 0x00088852
		public CheckObsticleTaskContext(Vector3 start, Vector3 end, bool thermalOn, bool ignoreFowBlockLayer, Action<bool> onCheckFinishCallback)
		{
			this.start = start;
			this.end = end;
			this.thermalOn = thermalOn;
			this.onCheckFinishCallback = onCheckFinishCallback;
			this.ignoreFowBlockLayer = ignoreFowBlockLayer;
		}

		// Token: 0x04001BD8 RID: 7128
		public Vector3 start;

		// Token: 0x04001BD9 RID: 7129
		public Vector3 end;

		// Token: 0x04001BDA RID: 7130
		public bool thermalOn;

		// Token: 0x04001BDB RID: 7131
		public bool ignoreFowBlockLayer;

		// Token: 0x04001BDC RID: 7132
		public Action<bool> onCheckFinishCallback;
	}
}
