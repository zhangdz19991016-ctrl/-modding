using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Duckov.Options;
using Duckov.Scenes;
using Duckov.UI;
using FMOD.Studio;
using FMODUnity;
using ItemStatsSystem;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Duckov
{
	// Token: 0x0200022E RID: 558
	public class AudioManager : MonoBehaviour
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x0004343A File Offset: 0x0004163A
		public static AudioManager Instance
		{
			get
			{
				return GameManager.AudioManager;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00043444 File Offset: 0x00041644
		public static bool IsStingerPlaying
		{
			get
			{
				if (AudioManager.Instance == null)
				{
					return false;
				}
				if (AudioManager.Instance.stingerSource == null)
				{
					return false;
				}
				return AudioManager.Instance.stingerSource.events.Any((EventInstance e) => e.isValid());
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x000434A7 File Offset: 0x000416A7
		private IEnumerable<AudioManager.Bus> AllBueses()
		{
			yield return this.masterBus;
			yield return this.sfxBus;
			yield return this.musicBus;
			yield break;
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000434B7 File Offset: 0x000416B7
		private Transform listener
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x000434BF File Offset: 0x000416BF
		private static Transform SoundSourceParent
		{
			get
			{
				if (AudioManager._soundSourceParent == null)
				{
					GameObject gameObject = new GameObject("Sound Sources");
					AudioManager._soundSourceParent = gameObject.transform;
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				return AudioManager._soundSourceParent;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x000434F0 File Offset: 0x000416F0
		private static ObjectPool<GameObject> SoundSourcePool
		{
			get
			{
				if (AudioManager._soundSourcePool == null)
				{
					AudioManager._soundSourcePool = new ObjectPool<GameObject>(delegate()
					{
						GameObject gameObject = new GameObject("SoundSource");
						gameObject.transform.SetParent(AudioManager.SoundSourceParent);
						return gameObject;
					}, delegate(GameObject e)
					{
						e.SetActive(true);
					}, delegate(GameObject e)
					{
						e.SetActive(false);
					}, null, true, 10, 10000);
				}
				return AudioManager._soundSourcePool;
			}
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0004357C File Offset: 0x0004177C
		public static EventInstance? Post(string eventName, GameObject gameObject)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			if (gameObject == null)
			{
				Debug.LogError(string.Format("Posting event but gameObject is null: {0}", gameObject));
			}
			if (!gameObject.activeSelf)
			{
				Debug.LogError(string.Format("Posting event but gameObject is not active: {0}", gameObject));
			}
			return AudioManager.Instance.MPost(eventName, gameObject);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000435D8 File Offset: 0x000417D8
		public static EventInstance? PostCustomSFX(string filePath, GameObject gameObject = null, bool loop = false)
		{
			if (AudioManager.Instance == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(filePath))
			{
				return null;
			}
			if (gameObject != null && !gameObject.activeSelf)
			{
				Debug.LogError(string.Format("Posting event but gameObject is not active: {0}", gameObject));
			}
			return AudioManager.Instance.MPostCustomSFX(filePath, gameObject, loop);
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0004363C File Offset: 0x0004183C
		public static EventInstance? Post(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			return AudioManager.Instance.MPost(eventName, null);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x00043668 File Offset: 0x00041868
		public static EventInstance? Post(string eventName, Vector3 position)
		{
			if (string.IsNullOrEmpty(eventName))
			{
				return null;
			}
			return AudioManager.Instance.MPost(eventName, position);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00043693 File Offset: 0x00041893
		internal static EventInstance? PostQuak(string soundKey, AudioManager.VoiceType voiceType, GameObject gameObject)
		{
			AudioObject orCreate = AudioObject.GetOrCreate(gameObject);
			orCreate.VoiceType = voiceType;
			return orCreate.PostQuak(soundKey);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x000436A8 File Offset: 0x000418A8
		public static void PostHitMarker(bool crit)
		{
			AudioManager.Post(crit ? "SFX/Combat/Marker/hitmarker_head" : "SFX/Combat/Marker/hitmarker");
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x000436BF File Offset: 0x000418BF
		public static void PostKillMarker(bool crit = false)
		{
			AudioManager.Post(crit ? "SFX/Combat/Marker/killmarker_head" : "SFX/Combat/Marker/killmarker");
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x000436D8 File Offset: 0x000418D8
		private void Awake()
		{
			CharacterSoundMaker.OnFootStepSound = (Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>)Delegate.Combine(CharacterSoundMaker.OnFootStepSound, new Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>(this.OnFootStepSound));
			Projectile.OnBulletFlyByCharacter = (Action<Vector3>)Delegate.Combine(Projectile.OnBulletFlyByCharacter, new Action<Vector3>(this.OnBulletFlyby));
			MultiSceneCore.OnSubSceneLoaded += this.OnSubSceneLoaded;
			ItemUIUtilities.OnPutItem += this.OnPutItem;
			Health.OnDead += this.OnHealthDead;
			LevelManager.OnLevelInitialized += this.OnLevelInitialized;
			SceneLoader.onStartedLoadingScene += this.OnStartedLoadingScene;
			OptionsManager.OnOptionsChanged += this.OnOptionsChanged;
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.LoadOptions();
			}
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000437C8 File Offset: 0x000419C8
		private void OnDestroy()
		{
			CharacterSoundMaker.OnFootStepSound = (Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>)Delegate.Remove(CharacterSoundMaker.OnFootStepSound, new Action<Vector3, CharacterSoundMaker.FootStepTypes, CharacterMainControl>(this.OnFootStepSound));
			Projectile.OnBulletFlyByCharacter = (Action<Vector3>)Delegate.Remove(Projectile.OnBulletFlyByCharacter, new Action<Vector3>(this.OnBulletFlyby));
			MultiSceneCore.OnSubSceneLoaded -= this.OnSubSceneLoaded;
			ItemUIUtilities.OnPutItem -= this.OnPutItem;
			Health.OnDead -= this.OnHealthDead;
			LevelManager.OnLevelInitialized -= this.OnLevelInitialized;
			SceneLoader.onStartedLoadingScene -= this.OnStartedLoadingScene;
			OptionsManager.OnOptionsChanged -= this.OnOptionsChanged;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x0004387C File Offset: 0x00041A7C
		private void OnOptionsChanged(string key)
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.NotifyOptionsChanged(key);
			}
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x000438C8 File Offset: 0x00041AC8
		public static AudioManager.Bus GetBus(string name)
		{
			if (AudioManager.Instance == null)
			{
				return null;
			}
			foreach (AudioManager.Bus bus in AudioManager.Instance.AllBueses())
			{
				if (bus.Name == name)
				{
					return bus;
				}
			}
			return null;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00043938 File Offset: 0x00041B38
		private void OnStartedLoadingScene(SceneLoadingContext context)
		{
			if (this.ambientSource)
			{
				this.ambientSource.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00043953 File Offset: 0x00041B53
		private void OnLevelInitialized()
		{
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00043955 File Offset: 0x00041B55
		private void Start()
		{
			this.UpdateBuses();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0004395D File Offset: 0x00041B5D
		private void OnHealthDead(Health health, DamageInfo info)
		{
			if (health.TryGetCharacter() == CharacterMainControl.Main)
			{
				AudioManager.StopBGM();
				AudioManager.Post("Music/Stinger/stg_death");
			}
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00043981 File Offset: 0x00041B81
		private void OnPutItem(Item item, bool pickup = false)
		{
			AudioManager.PlayPutItemSFX(item, pickup);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0004398A File Offset: 0x00041B8A
		public static void PlayPutItemSFX(Item item, bool pickup = false)
		{
			if (item == null)
			{
				return;
			}
			if (!LevelManager.LevelInited)
			{
				return;
			}
			AudioManager.Post((pickup ? "SFX/Item/pickup_{soundkey}" : "SFX/Item/put_{soundkey}").Format(new
			{
				soundkey = item.SoundKey.ToLower()
			}));
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x000439C8 File Offset: 0x00041BC8
		private void OnSubSceneLoaded(MultiSceneCore core, Scene scene)
		{
			LevelManager.LevelInitializingComment = "Opening ears";
			SubSceneEntry subSceneInfo = core.GetSubSceneInfo(scene);
			if (subSceneInfo == null)
			{
				return;
			}
			if (this.ambientSource)
			{
				LevelManager.LevelInitializingComment = "Hearing Ambient";
				this.ambientSource.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
				this.ambientSource.Post("Amb/amb_{soundkey}".Format(new
				{
					soundkey = subSceneInfo.AmbientSound.ToLower()
				}), true);
			}
			LevelManager.LevelInitializingComment = "Hearing Buses";
			this.ApplyBuses();
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x00043A45 File Offset: 0x00041C45
		public static bool PlayingBGM
		{
			get
			{
				return AudioManager.playingBGM;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00043A4C File Offset: 0x00041C4C
		private static bool LogEvent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00043A50 File Offset: 0x00041C50
		public static bool TryCreateEventInstance(string eventPath, out EventInstance eventInstance)
		{
			eventInstance = default(EventInstance);
			string text = "event:/" + eventPath;
			try
			{
				eventInstance = RuntimeManager.CreateInstance(text);
				return true;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				if (AudioManager.LogEvent)
				{
					Debug.LogError("[AudioEvent][Failed] " + text);
				}
			}
			return false;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00043AB4 File Offset: 0x00041CB4
		public static EventInstance? PlayCustomBGM(string filePath, bool loop = true)
		{
			AudioManager.StopBGM();
			if (AudioManager.Instance == null)
			{
				return null;
			}
			AudioManager.playingBGM = true;
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return null;
			}
			if (!File.Exists(filePath))
			{
				Debug.Log("[Audio] [Custom BGM] File don't exist: " + filePath);
			}
			string eventPath = loop ? "Music/custom_loop" : "Music/custom";
			return AudioManager.Instance.bgmSource.PostFile(eventPath, filePath, true);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00043B30 File Offset: 0x00041D30
		public static EventInstance? PlayBGM(string name)
		{
			AudioManager.StopBGM();
			if (AudioManager.Instance == null)
			{
				return null;
			}
			AudioManager.playingBGM = true;
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			string eventName = "Music/Loop/{soundkey}".Format(new
			{
				soundkey = name
			});
			return AudioManager.Instance.bgmSource.Post(eventName, true);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00043B93 File Offset: 0x00041D93
		public static void StopBGM()
		{
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.Instance.bgmSource.StopAll(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00043BB4 File Offset: 0x00041DB4
		public static void PlayStringer(string key)
		{
			string eventName = "Music/Stinger/{key}".Format(new
			{
				key
			});
			AudioManager.Instance.stingerSource.Post(eventName, true);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00043BE4 File Offset: 0x00041DE4
		private void OnBulletFlyby(Vector3 vector)
		{
			AudioManager.Post("SFX/Combat/Bullet/flyby", vector);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00043BF2 File Offset: 0x00041DF2
		public static void SetState(string stateGroup, string state)
		{
			AudioManager.globalStates[stateGroup] = state;
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00043C00 File Offset: 0x00041E00
		public static string GetState(string stateGroup)
		{
			string result;
			if (AudioManager.globalStates.TryGetValue(stateGroup, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00043C1F File Offset: 0x00041E1F
		private void Update()
		{
			this.UpdateListener();
			this.UpdateBuses();
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00043C30 File Offset: 0x00041E30
		private void UpdateListener()
		{
			if (LevelManager.Instance == null)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					this.listener.transform.position = main.transform.position;
					this.listener.transform.rotation = main.transform.rotation;
				}
				return;
			}
			GameCamera gameCamera = LevelManager.Instance.GameCamera;
			if (gameCamera != null)
			{
				if (CharacterMainControl.Main != null)
				{
					this.listener.transform.position = CharacterMainControl.Main.transform.position + Vector3.up * 2f;
				}
				else
				{
					this.listener.transform.position = gameCamera.renderCamera.transform.position;
				}
				this.listener.transform.rotation = gameCamera.renderCamera.transform.rotation;
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00043D2C File Offset: 0x00041F2C
		private void UpdateBuses()
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				if (bus.Dirty)
				{
					bus.Apply();
				}
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00043D80 File Offset: 0x00041F80
		private void ApplyBuses()
		{
			foreach (AudioManager.Bus bus in this.AllBueses())
			{
				bus.Apply();
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00043DCC File Offset: 0x00041FCC
		private void OnFootStepSound(Vector3 position, CharacterSoundMaker.FootStepTypes type, CharacterMainControl character)
		{
			if (character == null)
			{
				return;
			}
			GameObject gameObject = character.gameObject;
			string value = "floor";
			this.MSetParameter(gameObject, "terrain", value);
			if (character.FootStepMaterialType != AudioManager.FootStepMaterialType.noSound)
			{
				string charaType = character.FootStepMaterialType.ToString();
				string strengthType = "light";
				switch (type)
				{
				case CharacterSoundMaker.FootStepTypes.walkLight:
				case CharacterSoundMaker.FootStepTypes.runLight:
					strengthType = "light";
					break;
				case CharacterSoundMaker.FootStepTypes.walkHeavy:
				case CharacterSoundMaker.FootStepTypes.runHeavy:
					strengthType = "heavy";
					break;
				}
				AudioManager.Post("Char/Footstep/footstep_{charaType}_{strengthType}".Format(new
				{
					charaType,
					strengthType
				}), character.gameObject);
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x00043E65 File Offset: 0x00042065
		public static bool Initialized
		{
			get
			{
				return RuntimeManager.IsInitialized;
			}
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00043E6C File Offset: 0x0004206C
		private void MSetParameter(GameObject gameObject, string parameterName, string value)
		{
			if (gameObject == null)
			{
				Debug.LogError("Game Object must exist");
				return;
			}
			AudioObject.GetOrCreate(gameObject).SetParameterByNameWithLabel(parameterName, value);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00043E90 File Offset: 0x00042090
		private EventInstance? MPost(string eventName, GameObject gameObject = null)
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(eventName))
			{
				return null;
			}
			if (gameObject == null)
			{
				gameObject = AudioManager.Instance.gameObject;
			}
			else if (!gameObject.activeInHierarchy)
			{
				Debug.LogWarning("Posting event on inactive object, canceled");
				return null;
			}
			return AudioObject.GetOrCreate(gameObject).Post(eventName ?? "", true);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00043F0C File Offset: 0x0004210C
		private EventInstance? MPostCustomSFX(string filePath, GameObject gameObject = null, bool loop = false)
		{
			if (!AudioManager.Initialized)
			{
				return null;
			}
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return null;
			}
			if (gameObject == null)
			{
				gameObject = AudioManager.Instance.gameObject;
			}
			else if (!gameObject.activeInHierarchy)
			{
				Debug.LogWarning("Posting event on inactive object, canceled");
				return null;
			}
			return AudioObject.GetOrCreate(gameObject).PostCustomSFX(filePath, true, loop);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00043F80 File Offset: 0x00042180
		private EventInstance? MPost(string eventName, Vector3 position)
		{
			AudioManager.SoundSourcePool.Get().transform.position = position;
			EventInstance value;
			if (!AudioManager.TryCreateEventInstance(eventName ?? "", out value))
			{
				return null;
			}
			value.set3DAttributes(position.To3DAttributes());
			value.start();
			value.release();
			return new EventInstance?(value);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00043FE3 File Offset: 0x000421E3
		public static void StopAll(GameObject gameObject, FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
		{
			AudioObject.GetOrCreate(gameObject).StopAll(mode);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00043FF4 File Offset: 0x000421F4
		internal void MSetRTPC(string key, float value, GameObject gameObject = null)
		{
			if (gameObject == null)
			{
				RuntimeManager.StudioSystem.setParameterByName("parameter:/" + key, value, false);
				if (AudioManager.LogEvent)
				{
					Debug.Log(string.Format("[AudioEvent][Parameter][Global] {0} = {1}", key, value));
					return;
				}
			}
			else
			{
				AudioObject.GetOrCreate(gameObject).SetParameterByName("parameter:/" + key, value);
				if (AudioManager.LogEvent)
				{
					Debug.Log(string.Format("[AudioEvent][Parameter][GameObject] {0} = {1}", key, value), gameObject);
				}
			}
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00044078 File Offset: 0x00042278
		internal static void SetRTPC(string key, float value, GameObject gameObject = null)
		{
			if (AudioManager.Instance == null)
			{
				return;
			}
			AudioManager.Instance.MSetRTPC(key, value, gameObject);
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00044095 File Offset: 0x00042295
		public static void SetVoiceType(GameObject gameObject, AudioManager.VoiceType voiceType)
		{
			if (gameObject == null)
			{
				return;
			}
			AudioObject.GetOrCreate(gameObject).VoiceType = voiceType;
		}

		// Token: 0x04000D7F RID: 3455
		[SerializeField]
		private AudioObject ambientSource;

		// Token: 0x04000D80 RID: 3456
		[SerializeField]
		private AudioObject bgmSource;

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		private AudioObject stingerSource;

		// Token: 0x04000D82 RID: 3458
		[SerializeField]
		private AudioManager.Bus masterBus = new AudioManager.Bus("Master");

		// Token: 0x04000D83 RID: 3459
		[SerializeField]
		private AudioManager.Bus sfxBus = new AudioManager.Bus("Master/SFX");

		// Token: 0x04000D84 RID: 3460
		[SerializeField]
		private AudioManager.Bus musicBus = new AudioManager.Bus("Master/Music");

		// Token: 0x04000D85 RID: 3461
		private static Transform _soundSourceParent;

		// Token: 0x04000D86 RID: 3462
		private static ObjectPool<GameObject> _soundSourcePool;

		// Token: 0x04000D87 RID: 3463
		private const string path_hitmarker_norm = "SFX/Combat/Marker/hitmarker";

		// Token: 0x04000D88 RID: 3464
		private const string path_hitmarker_crit = "SFX/Combat/Marker/hitmarker_head";

		// Token: 0x04000D89 RID: 3465
		private const string path_killmarker_norm = "SFX/Combat/Marker/killmarker";

		// Token: 0x04000D8A RID: 3466
		private const string path_killmarker_crit = "SFX/Combat/Marker/killmarker_head";

		// Token: 0x04000D8B RID: 3467
		private const string path_music_death = "Music/Stinger/stg_death";

		// Token: 0x04000D8C RID: 3468
		private const string path_bullet_flyby = "SFX/Combat/Bullet/flyby";

		// Token: 0x04000D8D RID: 3469
		private const string path_pickup_item_fmt_soundkey = "SFX/Item/pickup_{soundkey}";

		// Token: 0x04000D8E RID: 3470
		private const string path_put_item_fmt_soundkey = "SFX/Item/put_{soundkey}";

		// Token: 0x04000D8F RID: 3471
		private const string path_ambient_fmt_soundkey = "Amb/amb_{soundkey}";

		// Token: 0x04000D90 RID: 3472
		private const string path_music_loop_fmt_soundkey = "Music/Loop/{soundkey}";

		// Token: 0x04000D91 RID: 3473
		private const string path_footstep_fmt_soundkey = "Char/Footstep/footstep_{charaType}_{strengthType}";

		// Token: 0x04000D92 RID: 3474
		public const string path_reload_fmt_soundkey = "SFX/Combat/Gun/Reload/{soundkey}";

		// Token: 0x04000D93 RID: 3475
		public const string path_shoot_fmt_gunkey = "SFX/Combat/Gun/Shoot/{soundkey}";

		// Token: 0x04000D94 RID: 3476
		public const string path_task_finished = "UI/mission_small";

		// Token: 0x04000D95 RID: 3477
		public const string path_building_built = "UI/building_up";

		// Token: 0x04000D96 RID: 3478
		public const string path_gun_unload = "SFX/Combat/Gun/unload";

		// Token: 0x04000D97 RID: 3479
		public const string path_stinger_fmt_key = "Music/Stinger/{key}";

		// Token: 0x04000D98 RID: 3480
		private static bool playingBGM;

		// Token: 0x04000D99 RID: 3481
		private static EventInstance bgmEvent;

		// Token: 0x04000D9A RID: 3482
		private static Dictionary<string, string> globalStates = new Dictionary<string, string>();

		// Token: 0x04000D9B RID: 3483
		private static Dictionary<int, AudioManager.VoiceType> gameObjectVoiceTypes = new Dictionary<int, AudioManager.VoiceType>();

		// Token: 0x0200052B RID: 1323
		[Serializable]
		public class Bus
		{
			// Token: 0x1700075C RID: 1884
			// (get) Token: 0x060027F8 RID: 10232 RVA: 0x00092920 File Offset: 0x00090B20
			public string Name
			{
				get
				{
					return this.volumeRTPC;
				}
			}

			// Token: 0x1700075D RID: 1885
			// (get) Token: 0x060027F9 RID: 10233 RVA: 0x00092928 File Offset: 0x00090B28
			// (set) Token: 0x060027FA RID: 10234 RVA: 0x00092930 File Offset: 0x00090B30
			public float Volume
			{
				get
				{
					return this.volume;
				}
				set
				{
					this.volume = value;
					this.Apply();
				}
			}

			// Token: 0x1700075E RID: 1886
			// (get) Token: 0x060027FB RID: 10235 RVA: 0x0009293F File Offset: 0x00090B3F
			// (set) Token: 0x060027FC RID: 10236 RVA: 0x00092947 File Offset: 0x00090B47
			public bool Mute
			{
				get
				{
					return this.mute;
				}
				set
				{
					this.mute = value;
					this.Apply();
				}
			}

			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x060027FD RID: 10237 RVA: 0x00092956 File Offset: 0x00090B56
			public bool Dirty
			{
				get
				{
					return this.appliedVolume != this.Volume;
				}
			}

			// Token: 0x060027FE RID: 10238 RVA: 0x0009296C File Offset: 0x00090B6C
			public void Apply()
			{
				try
				{
					FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + this.volumeRTPC);
					bus.setVolume(this.Volume);
					bus.setMute(this.Mute);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				this.appliedVolume = this.Volume;
				OptionsManager.Save<float>(this.SaveKey, this.volume);
			}

			// Token: 0x17000760 RID: 1888
			// (get) Token: 0x060027FF RID: 10239 RVA: 0x000929E4 File Offset: 0x00090BE4
			private string SaveKey
			{
				get
				{
					return "Audio/" + this.volumeRTPC;
				}
			}

			// Token: 0x06002800 RID: 10240 RVA: 0x000929F6 File Offset: 0x00090BF6
			public Bus(string rtpc)
			{
				this.volumeRTPC = rtpc;
			}

			// Token: 0x06002801 RID: 10241 RVA: 0x00092A26 File Offset: 0x00090C26
			internal void LoadOptions()
			{
				this.volume = OptionsManager.Load<float>(this.SaveKey, 1f);
			}

			// Token: 0x06002802 RID: 10242 RVA: 0x00092A3E File Offset: 0x00090C3E
			internal void NotifyOptionsChanged(string key)
			{
				if (key == this.SaveKey)
				{
					this.LoadOptions();
				}
			}

			// Token: 0x04001E83 RID: 7811
			[SerializeField]
			private string volumeRTPC = "Master";

			// Token: 0x04001E84 RID: 7812
			[HideInInspector]
			[SerializeField]
			private float volume = 1f;

			// Token: 0x04001E85 RID: 7813
			[HideInInspector]
			[SerializeField]
			private bool mute;

			// Token: 0x04001E86 RID: 7814
			private float appliedVolume = float.MinValue;
		}

		// Token: 0x0200052C RID: 1324
		public enum FootStepMaterialType
		{
			// Token: 0x04001E88 RID: 7816
			organic,
			// Token: 0x04001E89 RID: 7817
			mech,
			// Token: 0x04001E8A RID: 7818
			danger,
			// Token: 0x04001E8B RID: 7819
			noSound
		}

		// Token: 0x0200052D RID: 1325
		public enum VoiceType
		{
			// Token: 0x04001E8D RID: 7821
			Duck,
			// Token: 0x04001E8E RID: 7822
			Robot,
			// Token: 0x04001E8F RID: 7823
			Wolf,
			// Token: 0x04001E90 RID: 7824
			Chicken,
			// Token: 0x04001E91 RID: 7825
			Crow,
			// Token: 0x04001E92 RID: 7826
			Eagle
		}
	}
}
