using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003AB RID: 939
	public class HealthBar : MonoBehaviour, IPoolable
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x0007589F File Offset: 0x00073A9F
		// (set) Token: 0x060021A0 RID: 8608 RVA: 0x000758A7 File Offset: 0x00073AA7
		public Health target { get; private set; }

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000758B0 File Offset: 0x00073AB0
		private PrefabPool<HealthBar_DamageBar> DamageBarPool
		{
			get
			{
				if (this._damageBarPool == null)
				{
					this._damageBarPool = new PrefabPool<HealthBar_DamageBar>(this.damageBarTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._damageBarPool;
			}
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000758E9 File Offset: 0x00073AE9
		public void NotifyPooled()
		{
			this.pooled = true;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000758F2 File Offset: 0x00073AF2
		public void NotifyReleased()
		{
			this.UnregisterEvents();
			this.target = null;
			this.pooled = false;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00075908 File Offset: 0x00073B08
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x0007591B File Offset: 0x00073B1B
		private void OnDestroy()
		{
			this.UnregisterEvents();
			Image image = this.followFill;
			if (image != null)
			{
				image.DOKill(false);
			}
			Image image2 = this.hurtBlink;
			if (image2 == null)
			{
				return;
			}
			image2.DOKill(false);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00075948 File Offset: 0x00073B48
		private void LateUpdate()
		{
			if (this.target == null || !this.target.isActiveAndEnabled || this.target.Hidden)
			{
				this.Release();
				return;
			}
			this.UpdatePosition();
			bool flag = this.CheckInFrame();
			if (flag && !this.fadeGroup.IsShown)
			{
				this.fadeGroup.SkipShow();
				return;
			}
			if (!flag && this.fadeGroup.IsShown)
			{
				this.fadeGroup.SkipHide();
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000759C8 File Offset: 0x00073BC8
		private bool CheckInFrame()
		{
			if (this.target == null)
			{
				return false;
			}
			Camera main = Camera.main;
			return Vector3.Dot(this.target.transform.position - main.transform.position, main.transform.forward) > 0f;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00075A25 File Offset: 0x00073C25
		private void UpdateFrame()
		{
			if (this.CheckInFrame())
			{
				this.lastTimeInFrame = Time.unscaledTime;
			}
			if (Time.unscaledTime - this.lastTimeInFrame > this.releaseAfterOutOfFrame)
			{
				this.Release();
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00075A54 File Offset: 0x00073C54
		private void UpdatePosition()
		{
			Vector3 position = this.target.transform.position + this.displayOffset;
			Vector3 position2 = Camera.main.WorldToScreenPoint(position);
			position2.y += this.screenYOffset * (float)Screen.height;
			base.transform.position = position2;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00075AB0 File Offset: 0x00073CB0
		public void Setup(Health target, DamageInfo? damage = null, Action releaseAction = null)
		{
			this.releaseAction = releaseAction;
			this.UnregisterEvents();
			if (target == null)
			{
				this.Release();
				return;
			}
			if (target.IsDead)
			{
				this.Release();
				return;
			}
			this.background.SetActive(true);
			this.deathIndicator.SetActive(false);
			this.fill.gameObject.SetActive(true);
			this.followFill.gameObject.SetActive(true);
			this.target = target;
			this.RefreshOffset();
			this.RegisterEvents();
			this.Refresh();
			this.lastTimeInFrame = Time.unscaledTime;
			this.damageBarTemplate.gameObject.SetActive(false);
			if (damage != null)
			{
				this.OnTargetHurt(damage.Value);
			}
			this.UpdatePosition();
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00075B74 File Offset: 0x00073D74
		public void RefreshOffset()
		{
			if (!this.target)
			{
				return;
			}
			this.displayOffset = Vector3.up * 1.5f;
			CharacterMainControl characterMainControl = this.target.TryGetCharacter();
			if (characterMainControl && characterMainControl.characterModel)
			{
				Transform helmatSocket = characterMainControl.characterModel.HelmatSocket;
				if (helmatSocket)
				{
					this.displayOffset = Vector3.up * (Vector3.Distance(characterMainControl.transform.position, helmatSocket.position) + 0.5f);
				}
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x00075C08 File Offset: 0x00073E08
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.RefreshCharacterIcon();
			this.target.OnMaxHealthChange.AddListener(new UnityAction<Health>(this.OnTargetMaxHealthChange));
			this.target.OnHealthChange.AddListener(new UnityAction<Health>(this.OnTargetHealthChange));
			this.target.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnTargetHurt));
			this.target.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnTargetDead));
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00075C9C File Offset: 0x00073E9C
		private void RefreshCharacterIcon()
		{
			if (!this.target)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			CharacterMainControl characterMainControl = this.target.TryGetCharacter();
			if (!characterMainControl)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			CharacterRandomPreset characterPreset = characterMainControl.characterPreset;
			if (!characterPreset)
			{
				this.levelIcon.gameObject.SetActive(false);
				this.nameText.gameObject.SetActive(false);
				return;
			}
			Sprite characterIcon = characterPreset.GetCharacterIcon();
			if (!characterIcon)
			{
				this.levelIcon.gameObject.SetActive(false);
			}
			else
			{
				this.levelIcon.sprite = characterIcon;
				this.levelIcon.gameObject.SetActive(true);
			}
			if (!characterPreset.showName)
			{
				this.nameText.gameObject.SetActive(false);
				return;
			}
			this.nameText.text = characterPreset.DisplayName;
			this.nameText.gameObject.SetActive(true);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00075DC0 File Offset: 0x00073FC0
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.OnMaxHealthChange.RemoveListener(new UnityAction<Health>(this.OnTargetMaxHealthChange));
			this.target.OnHealthChange.RemoveListener(new UnityAction<Health>(this.OnTargetHealthChange));
			this.target.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTargetHurt));
			this.target.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnTargetDead));
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x00075E4C File Offset: 0x0007404C
		private void OnTargetMaxHealthChange(Health obj)
		{
			this.Refresh();
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00075E54 File Offset: 0x00074054
		private void OnTargetHealthChange(Health obj)
		{
			this.Refresh();
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00075E5C File Offset: 0x0007405C
		private void OnTargetHurt(DamageInfo damage)
		{
			Color blinkEndColor = this.blinkColor;
			blinkEndColor.a = 0f;
			if (this.hurtBlink != null)
			{
				this.hurtBlink.DOColor(this.blinkColor, this.blinkDuration).From<TweenerCore<Color, Color, ColorOptions>>().OnKill(delegate
				{
					if (this.hurtBlink != null)
					{
						this.hurtBlink.color = blinkEndColor;
					}
				});
			}
			UnityEvent unityEvent = this.onHurt;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this.ShowDamageBar(damage.finalDamage);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00075EEC File Offset: 0x000740EC
		private void OnTargetDead(DamageInfo damage)
		{
			this.UnregisterEvents();
			UnityEvent unityEvent = this.onDead;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			if (!damage.toDamageReceiver || !damage.toDamageReceiver.health)
			{
				return;
			}
			this.DeathTask(damage.toDamageReceiver.health).Forget();
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00075F48 File Offset: 0x00074148
		internal void Release()
		{
			if (!this.pooled)
			{
				return;
			}
			if (this.target != null && this.target.IsMainCharacterHealth && !this.target.IsDead && this.target.gameObject.activeInHierarchy)
			{
				return;
			}
			this.UnregisterEvents();
			this.target != null;
			this.target = null;
			Action action = this.releaseAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00075FC4 File Offset: 0x000741C4
		private void Refresh()
		{
			float currentHealth = this.target.CurrentHealth;
			float maxHealth = this.target.MaxHealth;
			float num = 0f;
			if (maxHealth > 0f)
			{
				num = currentHealth / maxHealth;
			}
			this.fill.fillAmount = num;
			this.fill.color = this.colorOverAmount.Evaluate(num);
			if (this.followFill != null)
			{
				this.followFill.DOKill(false);
				this.followFill.DOFillAmount(num, this.followFillDuration);
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00076050 File Offset: 0x00074250
		private void ShowDamageBar(float damageAmount)
		{
			float num = Mathf.Clamp01(damageAmount / this.target.MaxHealth);
			float num2 = Mathf.Clamp01(this.target.CurrentHealth / this.target.MaxHealth);
			float width = this.fill.rectTransform.rect.width;
			float damageBarWidth = width * num;
			float damageBarPostion = width * num2;
			HealthBar_DamageBar damageBar = this.DamageBarPool.Get(null);
			damageBar.Animate(damageBarPostion, damageBarWidth, delegate
			{
				this.DamageBarPool.Release(damageBar);
			}).Forget();
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000760F0 File Offset: 0x000742F0
		private UniTask DeathTask(Health health)
		{
			HealthBar.<DeathTask>d__52 <DeathTask>d__;
			<DeathTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DeathTask>d__.<>4__this = this;
			<DeathTask>d__.health = health;
			<DeathTask>d__.<>1__state = -1;
			<DeathTask>d__.<>t__builder.Start<HealthBar.<DeathTask>d__52>(ref <DeathTask>d__);
			return <DeathTask>d__.<>t__builder.Task;
		}

		// Token: 0x040016C3 RID: 5827
		private RectTransform rectTransform;

		// Token: 0x040016C4 RID: 5828
		[SerializeField]
		private GameObject background;

		// Token: 0x040016C5 RID: 5829
		[SerializeField]
		private Image fill;

		// Token: 0x040016C6 RID: 5830
		[SerializeField]
		private Image followFill;

		// Token: 0x040016C7 RID: 5831
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040016C8 RID: 5832
		[SerializeField]
		private GameObject deathIndicator;

		// Token: 0x040016C9 RID: 5833
		[SerializeField]
		private PunchReceiver deathIndicatorPunchReceiver;

		// Token: 0x040016CA RID: 5834
		[SerializeField]
		private Image hurtBlink;

		// Token: 0x040016CB RID: 5835
		[SerializeField]
		private HealthBar_DamageBar damageBarTemplate;

		// Token: 0x040016CC RID: 5836
		[SerializeField]
		private Gradient colorOverAmount = new Gradient();

		// Token: 0x040016CD RID: 5837
		[SerializeField]
		private float followFillDuration = 0.5f;

		// Token: 0x040016CE RID: 5838
		[SerializeField]
		private float blinkDuration = 0.1f;

		// Token: 0x040016CF RID: 5839
		[SerializeField]
		private Color blinkColor = Color.white;

		// Token: 0x040016D0 RID: 5840
		private Vector3 displayOffset = Vector3.zero;

		// Token: 0x040016D1 RID: 5841
		[SerializeField]
		private float releaseAfterOutOfFrame = 1f;

		// Token: 0x040016D2 RID: 5842
		[SerializeField]
		private float disappearDelay = 0.2f;

		// Token: 0x040016D3 RID: 5843
		[SerializeField]
		private Image levelIcon;

		// Token: 0x040016D4 RID: 5844
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x040016D5 RID: 5845
		[SerializeField]
		private UnityEvent onHurt;

		// Token: 0x040016D6 RID: 5846
		[SerializeField]
		private UnityEvent onDead;

		// Token: 0x040016D8 RID: 5848
		private Action releaseAction;

		// Token: 0x040016D9 RID: 5849
		private float lastTimeInFrame = float.MinValue;

		// Token: 0x040016DA RID: 5850
		private float screenYOffset = 0.02f;

		// Token: 0x040016DB RID: 5851
		private PrefabPool<HealthBar_DamageBar> _damageBarPool;

		// Token: 0x040016DC RID: 5852
		private bool pooled;

		// Token: 0x040016DD RID: 5853
		private Vector3[] cornersBuffer = new Vector3[4];
	}
}
