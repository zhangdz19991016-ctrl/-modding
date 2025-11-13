using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000070 RID: 112
public class Grenade : MonoBehaviour
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x000129A9 File Offset: 0x00010BA9
	private bool needCustomFx
	{
		get
		{
			return this.fxType == ExplosionFxTypes.custom;
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000129B4 File Offset: 0x00010BB4
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.collide)
		{
			this.collide = true;
		}
		Vector3 velocity = this.rb.velocity;
		velocity.x *= 0.5f;
		velocity.z *= 0.5f;
		this.rb.velocity = velocity;
		this.rb.angularVelocity = this.rb.angularVelocity * 0.3f;
		if (this.makeSoundCount > 0 && Time.time - this.makeSoundTimeMarker > 0.3f)
		{
			this.makeSoundCount--;
			this.makeSoundTimeMarker = Time.time;
			AISound sound = default(AISound);
			sound.fromObject = base.gameObject;
			sound.pos = base.transform.position;
			if (this.damageInfo.fromCharacter)
			{
				sound.fromTeam = this.damageInfo.fromCharacter.Team;
			}
			else
			{
				sound.fromTeam = Teams.all;
			}
			sound.soundType = SoundTypes.unknowNoise;
			if (this.isDangerForAi)
			{
				sound.soundType = SoundTypes.grenadeDropSound;
			}
			sound.radius = 20f;
			AIMainBrain.MakeSound(sound);
			if (this.hasCollideSound && this.collideSound != "")
			{
				AudioManager.Post(this.collideSound, base.gameObject);
			}
		}
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x00012B14 File Offset: 0x00010D14
	public void BindAgent(ItemAgent _agent)
	{
		this.bindAgent = true;
		this.bindedAgent = _agent;
		this.bindedAgent.transform.SetParent(base.transform, false);
		this.bindedAgent.transform.localPosition = Vector3.zero;
		this.bindedAgent.gameObject.SetActive(false);
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00012B6C File Offset: 0x00010D6C
	private void Update()
	{
		this.lifeTimer += Time.deltaTime;
		if (!this.delayFromCollide || this.collide)
		{
			this.delayTimer += Time.deltaTime;
		}
		if (!this.bindAgent)
		{
			if (!this.exploded && this.delayTimer > this.delayTime)
			{
				this.exploded = true;
				if (!this.isLandmine)
				{
					this.Explode();
					return;
				}
				this.ActiveLandmine().Forget();
			}
			return;
		}
		if (this.bindedAgent == null)
		{
			Debug.Log("bind  null destroied");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.lifeTimer > 0.5f && !this.bindedAgent.gameObject.activeInHierarchy)
		{
			this.bindedAgent.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00012C44 File Offset: 0x00010E44
	private void Explode()
	{
		if (this.createExplosion)
		{
			this.damageInfo.isExplosion = true;
			LevelManager.Instance.ExplosionManager.CreateExplosion(base.transform.position, this.damageRange, this.damageInfo, this.fxType, this.explosionShakeStrength, this.canHurtSelf);
		}
		if (this.createExplosion && this.needCustomFx && this.fx != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.fx, base.transform.position, Quaternion.identity);
		}
		if (this.createOnExlode)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.createOnExlode, base.transform.position, Quaternion.identity);
		}
		UnityEvent unityEvent = this.onExplodeEvent;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.rb != null)
		{
			this.rb.constraints = (RigidbodyConstraints)10;
		}
		if (this.destroyDelay <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.destroyDelay < 999f)
		{
			this.DestroyOverTime().Forget();
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00012D60 File Offset: 0x00010F60
	private UniTask DestroyOverTime()
	{
		Grenade.<DestroyOverTime>d__37 <DestroyOverTime>d__;
		<DestroyOverTime>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DestroyOverTime>d__.<>4__this = this;
		<DestroyOverTime>d__.<>1__state = -1;
		<DestroyOverTime>d__.<>t__builder.Start<Grenade.<DestroyOverTime>d__37>(ref <DestroyOverTime>d__);
		return <DestroyOverTime>d__.<>t__builder.Task;
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00012DA4 File Offset: 0x00010FA4
	private UniTask ActiveLandmine()
	{
		Grenade.<ActiveLandmine>d__38 <ActiveLandmine>d__;
		<ActiveLandmine>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<ActiveLandmine>d__.<>4__this = this;
		<ActiveLandmine>d__.<>1__state = -1;
		<ActiveLandmine>d__.<>t__builder.Start<Grenade.<ActiveLandmine>d__38>(ref <ActiveLandmine>d__);
		return <ActiveLandmine>d__.<>t__builder.Task;
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00012DE7 File Offset: 0x00010FE7
	private void OnLinemineTriggerd()
	{
		if (this.landmineTriggerd)
		{
			return;
		}
		this.landmineTriggerd = true;
		this.Explode();
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00012DFF File Offset: 0x00010FFF
	public void SetWeaponIdInfo(int typeId)
	{
		this.damageInfo.fromWeaponItemID = typeId;
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00012E10 File Offset: 0x00011010
	public void Launch(Vector3 startPoint, Vector3 velocity, CharacterMainControl fromCharacter, bool canHurtSelf)
	{
		this.canHurtSelf = canHurtSelf;
		this.groundLayer = LayerMask.NameToLayer("Ground");
		this.rb.position = startPoint;
		base.transform.position = startPoint;
		this.rb.velocity = velocity;
		Vector3 angularVelocity = (UnityEngine.Random.insideUnitSphere + Vector3.one) * 7f;
		angularVelocity.y = 0f;
		this.rb.angularVelocity = angularVelocity;
		if (fromCharacter != null)
		{
			Collider component = fromCharacter.GetComponent<Collider>();
			Collider component2 = base.GetComponent<Collider>();
			this.selfTeam = fromCharacter.Team;
			this.IgnoreCollisionForSeconds(component, component2, 0.5f).Forget();
		}
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00012EC0 File Offset: 0x000110C0
	private UniTask IgnoreCollisionForSeconds(Collider col1, Collider col2, float ignoreTime)
	{
		Grenade.<IgnoreCollisionForSeconds>d__42 <IgnoreCollisionForSeconds>d__;
		<IgnoreCollisionForSeconds>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<IgnoreCollisionForSeconds>d__.col1 = col1;
		<IgnoreCollisionForSeconds>d__.col2 = col2;
		<IgnoreCollisionForSeconds>d__.ignoreTime = ignoreTime;
		<IgnoreCollisionForSeconds>d__.<>1__state = -1;
		<IgnoreCollisionForSeconds>d__.<>t__builder.Start<Grenade.<IgnoreCollisionForSeconds>d__42>(ref <IgnoreCollisionForSeconds>d__);
		return <IgnoreCollisionForSeconds>d__.<>t__builder.Task;
	}

	// Token: 0x0400033D RID: 829
	public bool hasCollideSound;

	// Token: 0x0400033E RID: 830
	public string collideSound;

	// Token: 0x0400033F RID: 831
	public int makeSoundCount = 3;

	// Token: 0x04000340 RID: 832
	private float makeSoundTimeMarker = -1f;

	// Token: 0x04000341 RID: 833
	public float damageRange;

	// Token: 0x04000342 RID: 834
	public bool isDangerForAi = true;

	// Token: 0x04000343 RID: 835
	public bool isLandmine;

	// Token: 0x04000344 RID: 836
	public float landmineTriggerRange;

	// Token: 0x04000345 RID: 837
	private bool landmineActived;

	// Token: 0x04000346 RID: 838
	private bool landmineTriggerd;

	// Token: 0x04000347 RID: 839
	public ExplosionFxTypes fxType;

	// Token: 0x04000348 RID: 840
	public GameObject fx;

	// Token: 0x04000349 RID: 841
	public Animator animator;

	// Token: 0x0400034A RID: 842
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x0400034B RID: 843
	private int groundLayer;

	// Token: 0x0400034C RID: 844
	public bool delayFromCollide;

	// Token: 0x0400034D RID: 845
	public float delayTime = 1f;

	// Token: 0x0400034E RID: 846
	public bool createExplosion = true;

	// Token: 0x0400034F RID: 847
	public float explosionShakeStrength = 1f;

	// Token: 0x04000350 RID: 848
	public DamageInfo damageInfo;

	// Token: 0x04000351 RID: 849
	private bool bindAgent;

	// Token: 0x04000352 RID: 850
	private ItemAgent bindedAgent;

	// Token: 0x04000353 RID: 851
	private float lifeTimer;

	// Token: 0x04000354 RID: 852
	private float delayTimer;

	// Token: 0x04000355 RID: 853
	private Teams selfTeam;

	// Token: 0x04000356 RID: 854
	public GameObject createOnExlode;

	// Token: 0x04000357 RID: 855
	public float destroyDelay;

	// Token: 0x04000358 RID: 856
	public UnityEvent onExplodeEvent;

	// Token: 0x04000359 RID: 857
	private bool exploded;

	// Token: 0x0400035A RID: 858
	private bool canHurtSelf;

	// Token: 0x0400035B RID: 859
	private bool collide;
}
