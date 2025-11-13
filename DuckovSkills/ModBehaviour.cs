using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Duckov.Buffs;
using Duckov.Endowment;
using Duckov.Modding;
using Duckov.UI;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DuckovSkills
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	public class ModBehaviour : ModBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002253 File Offset: 0x00000453
		private static string persistentConfigPath
		{
			get
			{
				return Path.Combine(Application.streamingAssetsPath, "DuckovSkillsConfig.txt");
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002264 File Offset: 0x00000464
		private TextMeshProUGUI SkillShowText
		{
			get
			{
				if (this.skillShowText == null)
				{
					this.skillShowText = Object.Instantiate<TextMeshProUGUI>(GameplayDataSettings.UIStyle.TemplateTextUGUI);
					this.skillShowText.name = "SkillShowText";
					this.skillShowText.alignment = 513;
					this.skillShowText.fontSize = 40f;
					this.skillShowText.enableWordWrapping = true;
				}
				return this.skillShowText;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022D8 File Offset: 0x000004D8
		private void SkillInit()
		{
			this.isInCoolDown = false;
			this.skillDuration = 0f;
			this.skillStartTime = -1f;
			this.currentEndowmentIndex = EndowmentIndex.None;
			this.surviverNextTickTime = -1f;
			Health.OnHurt -= this.OnHurt;
			this.buffManager = CharacterMainControl.Main.GetBuffManager();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002335 File Offset: 0x00000535
		private void Awake()
		{
			Debug.Log("DuckovSkills Loaded!!!");
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002341 File Offset: 0x00000541
		private void OnDestroy()
		{
			if (this.skillShowText != null)
			{
				Object.Destroy(this.skillShowText);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000235C File Offset: 0x0000055C
		private void OnEnable()
		{
			if (!ModBehaviour.translationsRegistered)
			{
				this.RegisterLocalizations();
				ModBehaviour.translationsRegistered = true;
			}
			this.activateSkillAction = new InputAction("ActivateSkill", InputActionType.Button, "<Keyboard>/alt", null, null, null);
			this.activateSkillAction.performed += this.OnActivateSkill;
			this.activateSkillAction.Enable();
			LevelManager.OnLevelInitialized += this.SkillInit;
			this.skillStartTime = -1f;
			this.activeSkillTemplate = this.CreateBuffTemplate("duckovskills.buff.active.name", "duckovskills.buff.active.desc", Color.red, 7000);
			this.cooldownSkillTemplate = this.CreateBuffTemplate("duckovskills.buff.cooldown.name", "duckovskills.buff.cooldown.desc", Color.gray, 7001);
			ModManager.OnModActivated += this.OnModActivated;
			if (ModConfigAPI.IsAvailable())
			{
				Debug.Log("DuckovSkills: ModConfig already available!");
				this.LoadConfigFromModConfig();
				this.SetupModConfig();
			}
			ManagedUIElement.onOpen += this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose += this.OnManagedUIBehaviorClose;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002464 File Offset: 0x00000664
		private void OnDisable()
		{
			this.RemoveLocalizations();
			ModBehaviour.translationsRegistered = false;
			if (this.activateSkillAction != null)
			{
				this.activateSkillAction.performed -= this.OnActivateSkill;
				this.activateSkillAction.Disable();
				this.activateSkillAction.Dispose();
				this.activateSkillAction = null;
			}
			LevelManager.OnLevelInitialized -= this.SkillInit;
			Health.OnHurt -= this.OnHurt;
			this.skillStartTime = -1f;
			if (this.activeSkillTemplate != null)
			{
				Object.Destroy(this.activeSkillTemplate.gameObject);
			}
			if (this.cooldownSkillTemplate != null)
			{
				Object.Destroy(this.cooldownSkillTemplate.gameObject);
			}
			this.activeSkillTemplate = null;
			this.cooldownSkillTemplate = null;
			this.buffManager = null;
			ModManager.OnModActivated -= this.OnModActivated;
			ModConfigAPI.SafeRemoveOnOptionsChangedDelegate(new Action<string>(this.OnModConfigOptionsChanged));
			ManagedUIElement.onOpen -= this.OnManagedUIBehaviorOpen;
			ManagedUIElement.onClose -= this.OnManagedUIBehaviorClose;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000257C File Offset: 0x0000077C
		private void Update()
		{
			if (this.skillStartTime != -1f)
			{
				if (Time.time >= this.skillStartTime + this.skillDuration + (float)this.config.skillCoolDown)
				{
					this.skillStartTime = -1f;
					this.isInCoolDown = false;
					return;
				}
				if (Time.time >= this.skillStartTime + this.skillDuration && !this.isInCoolDown)
				{
					this.isInCoolDown = true;
					this.OnRemoveSkill();
					if (this.buffManager != null && this.cooldownSkillTemplate != null)
					{
						FieldInfo field = typeof(Buff).GetField("totalLifeTime", BindingFlags.Instance | BindingFlags.NonPublic);
						if (field != null)
						{
							field.SetValue(this.cooldownSkillTemplate, this.config.skillCoolDown);
						}
						this.buffManager.AddBuff(this.cooldownSkillTemplate, CharacterMainControl.Main, 0);
					}
				}
				if (this.currentEndowmentIndex == EndowmentIndex.Surviver && this.surviverNextTickTime <= this.skillStartTime + this.skillDuration && Time.time >= this.surviverNextTickTime)
				{
					this.surviverNextTickTime = Time.time + 1f;
					this.SurviverSkill(CharacterMainControl.Main, 0f, true);
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026B0 File Offset: 0x000008B0
		private void OnManagedUIBehaviorOpen(ManagedUIElement obj)
		{
			Debug.Log("OnManagedUIBehaviorOpen: " + obj.name);
			PlayerStatsView playerStatsView = obj as PlayerStatsView;
			if (playerStatsView == null)
			{
				this.SkillShowText.gameObject.SetActive(false);
				return;
			}
			this.SkillShowText.gameObject.SetActive(true);
			RectTransform rectTransform = this.SkillShowText.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			Transform parent = playerStatsView.transform.parent;
			if (parent != null)
			{
				Debug.Log("DuckovSkills: playerStatsView.transform.parent");
				rectTransform.SetParent(parent, false);
			}
			else
			{
				rectTransform.SetParent(PlayerStatsView.Instance.transform);
			}
			rectTransform.anchorMin = new Vector2(0.6f, 0.65f);
			rectTransform.anchorMax = new Vector2(0.6f, 0.4f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(0f, 100f);
			rectTransform.sizeDelta = new Vector2(600f, 300f);
			rectTransform.localScale = Vector3.one;
			switch (EndowmentManager.CurrentIndex)
			{
			case EndowmentIndex.None:
				this.SkillShowText.text = string.Concat(new string[]
				{
					"<color=red>",
					"duckovskills.endowment.title".ToPlainText(),
					"</color>\n<color=white>",
					"duckovskills.endowment.none.desc".ToPlainText(),
					"</color>"
				});
				return;
			case EndowmentIndex.Surviver:
				this.SkillShowText.text = string.Concat(new string[]
				{
					"<color=red>",
					"duckovskills.endowment.title".ToPlainText(),
					"</color>\n<color=white>",
					"duckovskills.endowment.surviver.desc".ToPlainText(),
					"</color>"
				});
				return;
			case EndowmentIndex.Porter:
				this.SkillShowText.text = string.Concat(new string[]
				{
					"<color=red>",
					"duckovskills.endowment.title".ToPlainText(),
					"</color>\n<color=white>",
					"duckovskills.endowment.porter.desc".ToPlainText(),
					"</color>"
				});
				return;
			case EndowmentIndex.Berserker:
				this.SkillShowText.text = string.Concat(new string[]
				{
					"<color=red>",
					"duckovskills.endowment.title".ToPlainText(),
					"</color>\n<color=white>",
					"duckovskills.endowment.berserker.desc".ToPlainText(),
					"</color>"
				});
				return;
			case EndowmentIndex.Marksman:
				this.SkillShowText.text = string.Concat(new string[]
				{
					"<color=red>",
					"duckovskills.endowment.title".ToPlainText(),
					"</color>\n<color=white>",
					"duckovskills.endowment.marksman.desc".ToPlainText(),
					"</color>"
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002957 File Offset: 0x00000B57
		private void OnManagedUIBehaviorClose(ManagedUIElement obj)
		{
			Debug.Log("OnManagedUIBehaviorClose: " + obj.name);
			if (obj is PlayerStatsView)
			{
				this.SkillShowText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002987 File Offset: 0x00000B87
		private void SetSkillDuration(float duration = 30f)
		{
			this.skillDuration = duration;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002990 File Offset: 0x00000B90
		private bool IsSkillActive()
		{
			return this.skillStartTime != -1f && Time.time < this.skillStartTime + this.skillDuration;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000029B8 File Offset: 0x00000BB8
		private bool IsSkillOnCooldown()
		{
			return this.skillStartTime != -1f && Time.time >= this.skillStartTime + this.skillDuration && Time.time < this.skillStartTime + this.skillDuration + (float)this.config.skillCoolDown;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002A0C File Offset: 0x00000C0C
		private void OnActivateSkill(InputAction.CallbackContext context)
		{
			if (this.skillStartTime != -1f)
			{
				if (this.IsSkillActive())
				{
					CharacterMainControl.Main.PopText("duckovskills.poptext.active".ToPlainText(), -1f);
					return;
				}
				CharacterMainControl.Main.PopText("duckovskills.poptext.cooldown".ToPlainText(), -1f);
				return;
			}
			else
			{
				CharacterMainControl main = CharacterMainControl.Main;
				if (main == null)
				{
					return;
				}
				this.isInCoolDown = false;
				this.currentEndowmentIndex = EndowmentManager.CurrentIndex;
				switch (this.currentEndowmentIndex)
				{
				case EndowmentIndex.None:
					this.NoneSkill(main, 0f);
					break;
				case EndowmentIndex.Surviver:
					this.SurviverSkill(main, (float)this.config.skillDuration, false);
					break;
				case EndowmentIndex.Porter:
					this.PorterSkill(main, false, (float)this.config.skillDuration);
					break;
				case EndowmentIndex.Berserker:
					this.BerserkerSkill(main, false, (float)this.config.skillDuration);
					break;
				case EndowmentIndex.Marksman:
					this.MarksmanSkill(main, false, (float)this.config.skillDuration);
					break;
				default:
					main.PopText("duckovskills.poptext.nothing".ToPlainText(), -1f);
					break;
				}
				if (this.skillStartTime != -1f && this.buffManager != null && this.activeSkillTemplate != null)
				{
					FieldInfo field = typeof(Buff).GetField("totalLifeTime", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field != null)
					{
						field.SetValue(this.activeSkillTemplate, this.skillDuration);
					}
					this.buffManager.AddBuff(this.activeSkillTemplate, main, 0);
				}
				return;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002B90 File Offset: 0x00000D90
		private void OnRemoveSkill()
		{
			if (CharacterMainControl.Main == null)
			{
				return;
			}
			switch (this.currentEndowmentIndex)
			{
			case EndowmentIndex.None:
				break;
			case EndowmentIndex.Surviver:
				if (CharacterMainControl.Main)
				{
					CharacterMainControl.Main.PopText("duckovskills.poptext.powerlost".ToPlainText(), -1f);
					return;
				}
				break;
			case EndowmentIndex.Porter:
				this.PorterSkill(CharacterMainControl.Main, true, 30f);
				return;
			case EndowmentIndex.Berserker:
				this.BerserkerSkill(CharacterMainControl.Main, true, 30f);
				return;
			case EndowmentIndex.Marksman:
				this.MarksmanSkill(CharacterMainControl.Main, true, 20f);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002C28 File Offset: 0x00000E28
		private void NoneSkill(CharacterMainControl character, float continuetime = 0f)
		{
			if (character == null)
			{
				return;
			}
			if (this.config.defaultRemoveBleeding)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Bleeding, false);
			}
			character.Health.AddHealth(this.config.defaultAddHealth);
			this.SetSkillDuration(continuetime);
			this.skillStartTime = Time.time;
			character.PopText("duckovskills.poptext.bandage".ToPlainText(), -1f);
			character.Quack();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002C98 File Offset: 0x00000E98
		private void SurviverSkill(CharacterMainControl character, float continuetime = 30f, bool tickTime = false)
		{
			if (character == null)
			{
				return;
			}
			if (this.config.surviverImmuneBleeding)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Bleeding, false);
			}
			if (this.config.surviverImmunePoison)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Poison, false);
			}
			if (this.config.surviverImmuneElectric)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Electric, false);
			}
			if (this.config.surviverImmuneBurning)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Burning, false);
			}
			if (this.config.surviverImmuneNauseous)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Nauseous, false);
			}
			if (this.config.surviverImmuneStun)
			{
				character.RemoveBuffsByTag(Buff.BuffExclusiveTags.Stun, false);
			}
			float healthValue = Mathf.Max(character.Health.MaxHealth * this.config.surviverAddHealthTickPercent * 0.01f, 5f);
			character.Health.AddHealth(healthValue);
			if (!tickTime)
			{
				this.SetSkillDuration(continuetime);
				this.skillStartTime = Time.time;
				this.surviverNextTickTime = Time.time + 1f;
				character.PopText("duckovskills.poptext.nopain".ToPlainText(), -1f);
				character.Quack();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002DA4 File Offset: 0x00000FA4
		private void PorterSkill(CharacterMainControl character, bool remove = false, float continuetime = 30f)
		{
			if (character == null)
			{
				return;
			}
			if (!remove)
			{
				character.Health.AddHealth(10f);
				Item characterItem = character.CharacterItem;
				if (characterItem)
				{
					Stat stat = characterItem.GetStat("WalkSpeed".GetHashCode());
					if (stat != null)
					{
						stat.AddModifier(new Modifier(ModifierType.Add, this.config.porterAddWalkSpeed, this));
					}
					Stat stat2 = characterItem.GetStat("RunSpeed".GetHashCode());
					if (stat2 != null)
					{
						stat2.AddModifier(new Modifier(ModifierType.Add, this.config.porterAddRunSpeed, this));
					}
					Stat stat3 = characterItem.GetStat("MaxWeight".GetHashCode());
					if (stat3 != null)
					{
						stat3.AddModifier(new Modifier(ModifierType.Add, this.config.porterAddMaxWeight, this));
					}
				}
				this.SetSkillDuration(continuetime);
				this.skillStartTime = Time.time;
				character.PopText("duckovskills.poptext.adrenaline".ToPlainText(), -1f);
				character.Quack();
				return;
			}
			Item characterItem2 = character.CharacterItem;
			if (characterItem2)
			{
				Stat stat4 = characterItem2.GetStat("WalkSpeed".GetHashCode());
				if (stat4 != null)
				{
					stat4.RemoveAllModifiersFromSource(this);
				}
				Stat stat5 = characterItem2.GetStat("RunSpeed".GetHashCode());
				if (stat5 != null)
				{
					stat5.RemoveAllModifiersFromSource(this);
				}
				Stat stat6 = characterItem2.GetStat("MaxWeight".GetHashCode());
				if (stat6 != null)
				{
					stat6.RemoveAllModifiersFromSource(this);
				}
			}
			character.PopText("duckovskills.poptext.powerlost".ToPlainText(), -1f);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002F14 File Offset: 0x00001114
		private void BerserkerSkill(CharacterMainControl character, bool remove = false, float continuetime = 30f)
		{
			if (character == null)
			{
				return;
			}
			if (!remove)
			{
				Item characterItem = character.CharacterItem;
				if (characterItem)
				{
					Health.OnHurt += this.OnHurt;
					Stat stat = characterItem.GetStat("StaminaDrainRate".GetHashCode());
					if (stat != null)
					{
						stat.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddStaminaDrainRate, this));
					}
					Stat stat2 = characterItem.GetStat("BodyArmor".GetHashCode());
					if (stat2 != null)
					{
						stat2.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddBodyArmor, this));
					}
					Stat stat3 = characterItem.GetStat("HeadArmor".GetHashCode());
					if (stat3 != null)
					{
						stat3.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddHeadArmor, this));
					}
					Stat stat4 = characterItem.GetStat("GunDamageMultiplier".GetHashCode());
					if (stat4 != null)
					{
						stat4.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddGunDamageMultiplier, this));
					}
					Stat stat5 = characterItem.GetStat("MeleeDamageMultiplier".GetHashCode());
					if (stat5 != null)
					{
						stat5.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddMeleeDamageMultiplier, this));
					}
					Stat stat6 = characterItem.GetStat("MeleeCritRateGain".GetHashCode());
					if (stat6 != null)
					{
						stat6.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddMeleeCritRateGain, this));
					}
					Stat stat7 = characterItem.GetStat("MeleeCritDamageGain".GetHashCode());
					if (stat7 != null)
					{
						stat7.AddModifier(new Modifier(ModifierType.Add, this.config.berserkerAddMeleeCritDamageGain, this));
					}
					Stat stat8 = characterItem.GetStat("WalkSoundRange".GetHashCode());
					if (stat8 != null)
					{
						stat8.AddModifier(new Modifier(ModifierType.PercentageAdd, this.config.berserkerAddWalkSoundRange, this));
					}
					Stat stat9 = characterItem.GetStat("RunSoundRange".GetHashCode());
					if (stat9 != null)
					{
						stat9.AddModifier(new Modifier(ModifierType.PercentageAdd, this.config.berserkerAddRunSoundRange, this));
					}
				}
				this.SetSkillDuration(continuetime);
				this.skillStartTime = Time.time;
				character.PopText("duckovskills.poptext.berserker".ToPlainText(), -1f);
				character.Quack();
				return;
			}
			Item characterItem2 = CharacterMainControl.Main.CharacterItem;
			if (characterItem2)
			{
				Health.OnHurt -= this.OnHurt;
				Stat stat10 = characterItem2.GetStat("StaminaDrainRate".GetHashCode());
				if (stat10 != null)
				{
					stat10.RemoveAllModifiersFromSource(this);
				}
				Stat stat11 = characterItem2.GetStat("BodyArmor".GetHashCode());
				if (stat11 != null)
				{
					stat11.RemoveAllModifiersFromSource(this);
				}
				Stat stat12 = characterItem2.GetStat("HeadArmor".GetHashCode());
				if (stat12 != null)
				{
					stat12.RemoveAllModifiersFromSource(this);
				}
				Stat stat13 = characterItem2.GetStat("GunDamageMultiplier".GetHashCode());
				if (stat13 != null)
				{
					stat13.RemoveAllModifiersFromSource(this);
				}
				Stat stat14 = characterItem2.GetStat("MeleeDamageMultiplier".GetHashCode());
				if (stat14 != null)
				{
					stat14.RemoveAllModifiersFromSource(this);
				}
				Stat stat15 = characterItem2.GetStat("MeleeCritRateGain".GetHashCode());
				if (stat15 != null)
				{
					stat15.RemoveAllModifiersFromSource(this);
				}
				Stat stat16 = characterItem2.GetStat("MeleeCritDamageGain".GetHashCode());
				if (stat16 != null)
				{
					stat16.RemoveAllModifiersFromSource(this);
				}
				Stat stat17 = characterItem2.GetStat("WalkSoundRange".GetHashCode());
				if (stat17 != null)
				{
					stat17.RemoveAllModifiersFromSource(this);
				}
				Stat stat18 = characterItem2.GetStat("RunSoundRange".GetHashCode());
				if (stat18 != null)
				{
					stat18.RemoveAllModifiersFromSource(this);
				}
			}
			character.PopText("duckovskills.poptext.powerlost".ToPlainText(), -1f);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000325C File Offset: 0x0000145C
		private void MarksmanSkill(CharacterMainControl character, bool remove = false, float continuetime = 20f)
		{
			if (character == null)
			{
				return;
			}
			if (!remove)
			{
				Item characterItem = CharacterMainControl.Main.CharacterItem;
				if (characterItem)
				{
					Stat stat = characterItem.GetStat("GunDamageMultiplier".GetHashCode());
					if (stat != null)
					{
						stat.AddModifier(new Modifier(ModifierType.Add, this.config.marksmanAddGunDamageMultiplier, this));
					}
					Stat stat2 = characterItem.GetStat("GunCritRateGain".GetHashCode());
					if (stat2 != null)
					{
						stat2.AddModifier(new Modifier(ModifierType.Add, this.config.marksmanAddGunCritRateGain, this));
					}
					Stat stat3 = characterItem.GetStat("GunCritDamageGain".GetHashCode());
					if (stat3 != null)
					{
						stat3.AddModifier(new Modifier(ModifierType.Add, this.config.marksmanAddGunCritDamageGain, this));
					}
					Stat stat4 = characterItem.GetStat("GunDistanceMultiplier".GetHashCode());
					if (stat4 != null)
					{
						stat4.AddModifier(new Modifier(ModifierType.Add, this.config.marksmanAddGunDistanceMultiplier, this));
					}
					Stat stat5 = characterItem.GetStat("GunScatterMultiplier".GetHashCode());
					if (stat5 != null)
					{
						stat5.AddModifier(new Modifier(ModifierType.Add, this.config.marksmanAddGunScatterMultiplier, this));
					}
					Stat stat6 = characterItem.GetStat("RecoilControl".GetHashCode());
					if (stat6 != null)
					{
						stat6.AddModifier(new Modifier(ModifierType.Add, this.config.marksmanAddRecoilControl, this));
					}
				}
				this.SetSkillDuration(continuetime);
				this.skillStartTime = Time.time;
				character.PopText("duckovskills.poptext.marksman".ToPlainText(), -1f);
				character.Quack();
				return;
			}
			Item characterItem2 = CharacterMainControl.Main.CharacterItem;
			if (characterItem2)
			{
				Stat stat7 = characterItem2.GetStat("GunDamageMultiplier".GetHashCode());
				if (stat7 != null)
				{
					stat7.RemoveAllModifiersFromSource(this);
				}
				Stat stat8 = characterItem2.GetStat("GunCritRateGain".GetHashCode());
				if (stat8 != null)
				{
					stat8.RemoveAllModifiersFromSource(this);
				}
				Stat stat9 = characterItem2.GetStat("GunCritDamageGain".GetHashCode());
				if (stat9 != null)
				{
					stat9.RemoveAllModifiersFromSource(this);
				}
				Stat stat10 = characterItem2.GetStat("GunDistanceMultiplier".GetHashCode());
				if (stat10 != null)
				{
					stat10.RemoveAllModifiersFromSource(this);
				}
				Stat stat11 = characterItem2.GetStat("GunScatterMultiplier".GetHashCode());
				if (stat11 != null)
				{
					stat11.RemoveAllModifiersFromSource(this);
				}
				Stat stat12 = characterItem2.GetStat("RecoilControl".GetHashCode());
				if (stat12 != null)
				{
					stat12.RemoveAllModifiersFromSource(this);
				}
			}
			character.PopText("duckovskills.poptext.powerlost".ToPlainText(), -1f);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000034A4 File Offset: 0x000016A4
		private void OnHurt(Health health, DamageInfo damageInfo)
		{
			CharacterMainControl main = CharacterMainControl.Main;
			if (main == null)
			{
				return;
			}
			if (damageInfo.fromCharacter.IsMainCharacter)
			{
				main.Health.AddHealth(damageInfo.damageValue * 0.3f);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000034E8 File Offset: 0x000016E8
		private Buff CreateBuffTemplate(string nameKey, string descriptionKey, Color iconColor, int id)
		{
			GameObject gameObject = new GameObject("ModSkillBuff_" + nameKey);
			gameObject.SetActive(false);
			Object.DontDestroyOnLoad(gameObject);
			Buff buff = gameObject.AddComponent<Buff>();
			Type typeFromHandle = typeof(Buff);
			FieldInfo field = typeFromHandle.GetField("id", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field != null)
			{
				field.SetValue(buff, id);
			}
			FieldInfo field2 = typeFromHandle.GetField("displayName", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field2 != null)
			{
				field2.SetValue(buff, nameKey);
			}
			FieldInfo field3 = typeFromHandle.GetField("description", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field3 != null)
			{
				field3.SetValue(buff, descriptionKey);
			}
			FieldInfo field4 = typeFromHandle.GetField("icon", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field4 != null)
			{
				field4.SetValue(buff, this.CreateDummySprite(iconColor));
			}
			FieldInfo field5 = typeFromHandle.GetField("limitedLifeTime", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field5 != null)
			{
				field5.SetValue(buff, true);
			}
			return buff;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000035B8 File Offset: 0x000017B8
		private Sprite CreateDummySprite(Color color)
		{
			int num = 60;
			Texture2D texture2D = new Texture2D(num, num, TextureFormat.ARGB32, false);
			float num2 = (float)num / 2f;
			Vector2 b = new Vector2(num2, num2);
			Color[] array = new Color[num * num];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					float num3 = Vector2.Distance(new Vector2((float)j, (float)i), b);
					if (num3 <= num2 - 1f)
					{
						array[i * num + j] = color;
					}
					else if (num3 <= num2)
					{
						float a = 1f - (num3 - (num2 - 1f));
						array[i * num + j] = new Color(color.r, color.g, color.b, a);
					}
					else
					{
						array[i * num + j] = Color.clear;
					}
				}
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
			return Sprite.Create(texture2D, new Rect(0f, 0f, (float)num, (float)num), new Vector2(0.5f, 0.5f));
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000036D4 File Offset: 0x000018D4
		private void RegisterLocalizations()
		{
			if (LocalizationManager.CurrentLanguage == SystemLanguage.ChineseSimplified || LocalizationManager.CurrentLanguage == SystemLanguage.Chinese || LocalizationManager.CurrentLanguage == SystemLanguage.ChineseTraditional)
			{
				LocalizationManager.SetOverrideText("duckovskills.poptext.active", "技能持续中！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.cooldown", "技能冷却中！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.powerlost", "力量消失了！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.nopain", "无惧疼痛！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.adrenaline", "肾上腺素！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.berserker", "猛虎下山！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.marksman", "超能光束！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.bandage", "包扎！");
				LocalizationManager.SetOverrideText("duckovskills.poptext.nothing", "无事发生！");
				LocalizationManager.SetOverrideText("duckovskills.buff.active.name", "技能激活");
				LocalizationManager.SetOverrideText("duckovskills.buff.active.desc", "你的技能正在生效！");
				LocalizationManager.SetOverrideText("duckovskills.buff.cooldown.name", "技能冷却");
				LocalizationManager.SetOverrideText("duckovskills.buff.cooldown.desc", "等待技能再次可用。");
				LocalizationManager.SetOverrideText("duckovskills.endowment.title", "主动技能");
				LocalizationManager.SetOverrideText("duckovskills.endowment.none.desc", "包扎伤口，移除流血效果并恢复少量生命值");
				LocalizationManager.SetOverrideText("duckovskills.endowment.surviver.desc", "持续时间内免疫多种负面效果，并持续恢复生命值");
				LocalizationManager.SetOverrideText("duckovskills.endowment.porter.desc", "爆发肾上腺素，大幅提升移动速度和最大负重");
				LocalizationManager.SetOverrideText("duckovskills.endowment.berserker.desc", "进入狂暴状态，获得强大的近战增益和吸血效果");
				LocalizationManager.SetOverrideText("duckovskills.endowment.marksman.desc", "进入专注状态，大幅提升枪械的伤害、暴击和稳定性");
			}
			else
			{
				LocalizationManager.SetOverrideText("duckovskills.poptext.active", "Skill is active!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.cooldown", "Skill on cooldown!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.powerlost", "The power fades!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.nopain", "Fear no pain!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.adrenaline", "Adrenaline!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.berserker", "Rush Down!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.marksman", "Hyperbeam!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.bandage", "Bandage Up!");
				LocalizationManager.SetOverrideText("duckovskills.poptext.nothing", "Nothing happened!");
				LocalizationManager.SetOverrideText("duckovskills.buff.active.name", "Skill Active");
				LocalizationManager.SetOverrideText("duckovskills.buff.active.desc", "Your skill is in effect!");
				LocalizationManager.SetOverrideText("duckovskills.buff.cooldown.name", "Skill Cooldown");
				LocalizationManager.SetOverrideText("duckovskills.buff.cooldown.desc", "Waiting for the skill to be available again.");
				LocalizationManager.SetOverrideText("duckovskills.endowment.title", "Active Skill");
				LocalizationManager.SetOverrideText("duckovskills.endowment.none.desc", "Bandages wounds, removing bleeding and restoring a small amount of health.");
				LocalizationManager.SetOverrideText("duckovskills.endowment.surviver.desc", "Become immune to various negative effects and regenerate health over time.");
				LocalizationManager.SetOverrideText("duckovskills.endowment.porter.desc", "An adrenaline rush greatly increases movement speed and max weight capacity.");
				LocalizationManager.SetOverrideText("duckovskills.endowment.berserker.desc", "Enter a berserk rage, gaining powerful melee buffs and lifesteal.");
				LocalizationManager.SetOverrideText("duckovskills.endowment.marksman.desc", "Enter a state of focus, greatly enhancing firearm damage, criticals, and stability.");
			}
			Debug.Log("DuckovSkills: Localized texts registered.");
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003948 File Offset: 0x00001B48
		private void RemoveLocalizations()
		{
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.active");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.cooldown");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.powerlost");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.nopain");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.adrenaline");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.berserker");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.marksman");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.bandage");
			LocalizationManager.RemoveOverrideText("duckovskills.poptext.nothing");
			LocalizationManager.RemoveOverrideText("duckovskills.buff.active.name");
			LocalizationManager.RemoveOverrideText("duckovskills.buff.active.desc");
			LocalizationManager.RemoveOverrideText("duckovskills.buff.cooldown.name");
			LocalizationManager.RemoveOverrideText("duckovskills.buff.cooldown.desc");
			Debug.Log("DuckovSkills: Localized texts removed.");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000039EE File Offset: 0x00001BEE
		private void OnModActivated(ModInfo info, ModBehaviour behaviour)
		{
			if (info.name == ModConfigAPI.ModConfigName)
			{
				Debug.Log("DuckovSkills: ModConfig activated!");
				this.LoadConfigFromModConfig();
				this.SetupModConfig();
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00003A18 File Offset: 0x00001C18
		private void SetupModConfig()
		{
			if (!ModConfigAPI.IsAvailable())
			{
				return;
			}
			ModConfigAPI.SafeAddOnOptionsChangedDelegate(new Action<string>(this.OnModConfigOptionsChanged));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "marksmanAddRecoilControl", ModBehaviour.IS_CHINESE ? "枪手增加后坐力控制" : "Berserker Add RecoilControl", typeof(float), this.config.marksmanAddRecoilControl, new Vector2?(new Vector2(0.1f, 0.5f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "marksmanAddGunScatterMultiplier", ModBehaviour.IS_CHINESE ? "枪手增加散射倍率" : "Berserker Add GunScatterMultiplier", typeof(float), this.config.marksmanAddGunScatterMultiplier, new Vector2?(new Vector2(-0.6f, -0.1f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "marksmanAddGunDistanceMultiplier", ModBehaviour.IS_CHINESE ? "枪手增加射程倍率" : "Berserker Add GunDistanceMultiplier", typeof(float), this.config.marksmanAddGunDistanceMultiplier, new Vector2?(new Vector2(0.1f, 0.5f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "marksmanAddGunCritDamageGain", ModBehaviour.IS_CHINESE ? "枪手增加枪械暴击伤害（非加算）" : "Berserker Add marksmanAddGunCritDamageGain", typeof(float), this.config.marksmanAddGunCritDamageGain, new Vector2?(new Vector2(0.1f, 0.7f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "marksmanAddGunCritRateGain", ModBehaviour.IS_CHINESE ? "枪手增加枪械暴击率（非加算）" : "Berserker Add GunCritRateGain", typeof(float), this.config.marksmanAddGunCritRateGain, new Vector2?(new Vector2(0.1f, 0.5f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "marksmanAddGunDamageMultiplier", ModBehaviour.IS_CHINESE ? "枪手增加枪械伤害倍率" : "Berserker Add GunDamageMultiplier", typeof(float), this.config.marksmanAddGunDamageMultiplier, new Vector2?(new Vector2(0.1f, 0.5f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddRunSoundRange", ModBehaviour.IS_CHINESE ? "狂战士增加奔跑声音距离百分比" : "Berserker Add RunSoundRangePercent", typeof(float), this.config.berserkerAddRunSoundRange, new Vector2?(new Vector2(-0.8f, -0.1f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddWalkSoundRange", ModBehaviour.IS_CHINESE ? "狂战士增加行走声音距离百分比" : "Berserker Add WalkSoundRangePercent", typeof(float), this.config.berserkerAddWalkSoundRange, new Vector2?(new Vector2(-0.8f, -0.1f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddMeleeCritDamageGain", ModBehaviour.IS_CHINESE ? "狂战士增加近战暴击伤害（非加算）" : "Berserker Add MeleeCritDamageGain", typeof(float), this.config.berserkerAddMeleeCritDamageGain, new Vector2?(new Vector2(0.2f, 1f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddMeleeCritRateGain", ModBehaviour.IS_CHINESE ? "狂战士增加近战暴击率（非加算）" : "Berserker Add MeleeCritRateGain", typeof(float), this.config.berserkerAddMeleeCritRateGain, new Vector2?(new Vector2(0.8f, 2f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddMeleeDamageMultiplier", ModBehaviour.IS_CHINESE ? "狂战士增加近战伤害倍率" : "Berserker Add MeleeDamageMultiplier", typeof(float), this.config.berserkerAddMeleeDamageMultiplier, new Vector2?(new Vector2(0.25f, 0.5f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddGunDamageMultiplier", ModBehaviour.IS_CHINESE ? "狂战士增加枪械伤害倍率" : "Berserker Add GunDamageMultiplier", typeof(float), this.config.berserkerAddGunDamageMultiplier, new Vector2?(new Vector2(-0.9f, -0.7f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddHeadArmor", ModBehaviour.IS_CHINESE ? "狂战士增加头部护甲" : "Berserker Add HeadArmor", typeof(float), this.config.berserkerAddHeadArmor, new Vector2?(new Vector2(0f, 4f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddBodyArmor", ModBehaviour.IS_CHINESE ? "狂战士增加身体护甲" : "Berserker Add BodyArmor", typeof(float), this.config.berserkerAddBodyArmor, new Vector2?(new Vector2(0f, 4f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddStaminaDrainRate", ModBehaviour.IS_CHINESE ? "狂战士增加耐力消耗" : "Berserker Add StaminaDrainRate", typeof(float), this.config.berserkerAddStaminaDrainRate, new Vector2?(new Vector2(-2f, -1f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "berserkerAddLifeStealPercent", ModBehaviour.IS_CHINESE ? "狂战士增加吸血百分比" : "Berserker Add LifeStealPercent", typeof(float), this.config.berserkerAddLifeStealPercent, new Vector2?(new Vector2(15f, 40f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "porterAddMaxWeight", ModBehaviour.IS_CHINESE ? "搬运者增加最大负重" : "Porter Add MaxWeight", typeof(float), this.config.porterAddMaxWeight, new Vector2?(new Vector2(15f, 40f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "porterAddRunSpeed", ModBehaviour.IS_CHINESE ? "搬运者增加奔跑速度" : "Porter Add RunSpeed", typeof(float), this.config.porterAddRunSpeed, new Vector2?(new Vector2(2f, 6f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "porterAddWalkSpeed", ModBehaviour.IS_CHINESE ? "搬运者增加行走速度" : "Porter Add WalkSpeed", typeof(float), this.config.porterAddWalkSpeed, new Vector2?(new Vector2(2f, 6f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "surviverAddHealthTickPercent", ModBehaviour.IS_CHINESE ? "幸存者持续回复百分比" : "Survivor Continuously Recover Percent", typeof(float), this.config.surviverAddHealthTickPercent, new Vector2?(new Vector2(5f, 25f)));
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "surviverImmuneStun", ModBehaviour.IS_CHINESE ? "幸存者是否免疫眩晕" : "Surviver Immune to Stun", this.config.surviverImmuneStun);
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "surviverImmuneNauseous", ModBehaviour.IS_CHINESE ? "幸存者是否免疫恶心" : "Surviver Immune to Nauseous", this.config.surviverImmuneNauseous);
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "surviverImmuneBurning", ModBehaviour.IS_CHINESE ? "幸存者是否免疫燃烧" : "Surviver Immune to Burning", this.config.surviverImmuneBurning);
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "surviverImmuneElectric", ModBehaviour.IS_CHINESE ? "幸存者是否免疫电击" : "Surviver Immune to Electric", this.config.surviverImmuneElectric);
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "surviverImmunePoison", ModBehaviour.IS_CHINESE ? "幸存者是否免疫中毒" : "Surviver Immune to Poison", this.config.surviverImmunePoison);
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "surviverImmuneBleeding", ModBehaviour.IS_CHINESE ? "幸存者是否免疫流血" : "Surviver Immune to Bleeding", this.config.defaultRemoveBleeding);
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "defaultAddHealth", ModBehaviour.IS_CHINESE ? "默认角色回复生命值" : "Default Role Add Health", typeof(float), this.config.defaultAddHealth, new Vector2?(new Vector2(10f, 40f)));
			ModConfigAPI.SafeAddBoolDropdownList(ModBehaviour.MOD_NAME, "defaultRemoveBleeding", ModBehaviour.IS_CHINESE ? "默认角色是否清除流血" : "Default Role Removes Bleed", this.config.defaultRemoveBleeding);
			Debug.Log(string.Format("DisplayItemValue skillCoolDown InitUI: {0}", this.config.skillCoolDown));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "skillCoolDown", ModBehaviour.IS_CHINESE ? "技能冷却" : "Skill CD", typeof(int), this.config.skillCoolDown, new Vector2?(new Vector2(30f, 90f)));
			ModConfigAPI.SafeAddInputWithSlider(ModBehaviour.MOD_NAME, "skillDuration", ModBehaviour.IS_CHINESE ? "技能持续" : "Skill Duration", typeof(int), this.config.skillDuration, new Vector2?(new Vector2(10f, 60f)));
			this.SetupHotKeyConfig();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000042F0 File Offset: 0x000024F0
		private void SetupHotKeyConfig()
		{
			if (!ModConfigAPI.IsAvailable())
			{
				return;
			}
			SortedDictionary<string, object> sortedDictionary = new SortedDictionary<string, object>();
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "Alt 键 (默认)" : "Alt Key (Default)", "<Keyboard>/alt");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "Ctrl 键" : "Ctrl Key", "<Keyboard>/ctrl");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "Shift 键" : "Shift Key", "<Keyboard>/shift");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "空格键" : "Spacebar", "<Keyboard>/space");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "Tab 键" : "Tab Key", "<Keyboard>/tab");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "Caps Lock (大写锁定)" : "Caps Lock", "<Keyboard>/capsLock");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "波浪键 (`~`)" : "Backquote Key(`~`)", "<Keyboard>/backquote");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "鼠标中键" : "Middle Mouse Button", "<Mouse>/middleButton");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "鼠标侧键 (前进)" : "Mouse Forward Button", "<Mouse>/forwardButton");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "鼠标侧键 (后退)" : "Mouse Back Button", "<Mouse>/backButton");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 Q" : "Keyboard Q", "<Keyboard>/q");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 E" : "Keyboard E", "<Keyboard>/e");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 T" : "Keyboard T", "<Keyboard>/t");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 Y" : "Keyboard Y", "<Keyboard>/y");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 F" : "Keyboard F", "<Keyboard>/f");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 G" : "Keyboard G", "<Keyboard>/g");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 Z" : "Keyboard Z", "<Keyboard>/z");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 X" : "Keyboard X", "<Keyboard>/x");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 C" : "Keyboard C", "<Keyboard>/c");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 V" : "Keyboard V", "<Keyboard>/v");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "键盘 B" : "Keyboard B", "<Keyboard>/b");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "数字键 1" : "Num 1", "<Keyboard>/1");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "数字键 2" : "Num 2", "<Keyboard>/2");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "数字键 3" : "Num 3", "<Keyboard>/3");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "数字键 4" : "Num 4", "<Keyboard>/4");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "数字键 5" : "Num 5", "<Keyboard>/5");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "数字键 6" : "Num 6", "<Keyboard>/6");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "F1 键" : "F1", "<Keyboard>/f1");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "F2 键" : "F2", "<Keyboard>/f2");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "F3 键" : "F3", "<Keyboard>/f3");
			sortedDictionary.Add(ModBehaviour.IS_CHINESE ? "F4 键" : "F4", "<Keyboard>/f4");
			ModConfigAPI.SafeAddDropdownList(ModBehaviour.MOD_NAME, "hotKey", ModBehaviour.IS_CHINESE ? "技能快捷键" : "Skill Hotkey", sortedDictionary, typeof(string), this.config.hotKey);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000046E8 File Offset: 0x000028E8
		private void UpdateHotkey()
		{
			if (this.activateSkillAction != null)
			{
				if (this.activateSkillAction.bindings[0].effectivePath == this.config.hotKey)
				{
					Debug.Log("DuckovSkills no need update hotKey: '" + this.config.hotKey + "'");
					return;
				}
				this.activateSkillAction.performed -= this.OnActivateSkill;
				this.activateSkillAction.Disable();
				this.activateSkillAction.Dispose();
				this.activateSkillAction = null;
			}
			Debug.Log("DuckovSkills: Attempting to set hotKey to: '" + this.config.hotKey + "'");
			try
			{
				this.activateSkillAction = new InputAction("ActivateSkill", InputActionType.Button, this.config.hotKey, null, null, null);
			}
			catch (Exception ex)
			{
				Debug.LogError("DuckovSkills: Invalid hotKey format '" + this.config.hotKey + "'. Reverting to default '<Keyboard>/alt'. Error: " + ex.Message);
				this.activateSkillAction = new InputAction("ActivateSkill", InputActionType.Button, "<Keyboard>/alt", null, null, null);
			}
			this.activateSkillAction.performed += this.OnActivateSkill;
			this.activateSkillAction.Enable();
			Debug.Log("DuckovSkills: hotKey successfully bound to '" + this.activateSkillAction.bindings[0].effectivePath + "'");
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004868 File Offset: 0x00002A68
		private void OnModConfigOptionsChanged(string key)
		{
			if (!key.StartsWith(ModBehaviour.MOD_NAME + "_"))
			{
				return;
			}
			this.LoadConfigFromModConfig();
			this.SaveConfig(this.config);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00004894 File Offset: 0x00002A94
		private void LoadConfigFromModConfig()
		{
			this.config.hotKey = ModConfigAPI.SafeLoad<string>(ModBehaviour.MOD_NAME, "hotKey", this.config.hotKey);
			Debug.Log("DisplayItemValue HotKey: " + this.config.hotKey);
			this.UpdateHotkey();
			this.config.skillCoolDown = ModConfigAPI.SafeLoad<int>(ModBehaviour.MOD_NAME, "skillCoolDown", this.config.skillCoolDown);
			this.config.skillDuration = ModConfigAPI.SafeLoad<int>(ModBehaviour.MOD_NAME, "skillDuration", this.config.skillDuration);
			this.config.defaultRemoveBleeding = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "defaultRemoveBleeding", this.config.defaultRemoveBleeding);
			this.config.defaultAddHealth = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "defaultAddHealth", this.config.defaultAddHealth);
			this.config.surviverImmuneBleeding = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "surviverImmuneBleeding", this.config.surviverImmuneBleeding);
			this.config.surviverImmunePoison = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "surviverImmunePoison", this.config.surviverImmunePoison);
			this.config.surviverImmuneElectric = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "surviverImmuneElectric", this.config.surviverImmuneElectric);
			this.config.surviverImmuneBurning = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "surviverImmuneBurning", this.config.surviverImmuneBurning);
			this.config.surviverImmuneNauseous = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "surviverImmuneNauseous", this.config.surviverImmuneNauseous);
			this.config.surviverImmuneStun = ModConfigAPI.SafeLoad<bool>(ModBehaviour.MOD_NAME, "surviverImmuneStun", this.config.surviverImmuneStun);
			this.config.surviverAddHealthTickPercent = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "surviverAddHealthTickPercent", this.config.surviverAddHealthTickPercent);
			this.config.porterAddWalkSpeed = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "porterAddWalkSpeed", this.config.porterAddWalkSpeed);
			this.config.porterAddRunSpeed = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "porterAddRunSpeed", this.config.porterAddRunSpeed);
			this.config.porterAddMaxWeight = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "porterAddMaxWeight", this.config.porterAddMaxWeight);
			this.config.berserkerAddLifeStealPercent = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddLifeStealPercent", this.config.berserkerAddLifeStealPercent);
			this.config.berserkerAddStaminaDrainRate = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddStaminaDrainRate", this.config.berserkerAddStaminaDrainRate);
			this.config.berserkerAddBodyArmor = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddBodyArmor", this.config.berserkerAddBodyArmor);
			this.config.berserkerAddHeadArmor = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddHeadArmor", this.config.berserkerAddHeadArmor);
			this.config.berserkerAddGunDamageMultiplier = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddGunDamageMultiplier", this.config.berserkerAddGunDamageMultiplier);
			this.config.berserkerAddMeleeDamageMultiplier = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddMeleeDamageMultiplier", this.config.berserkerAddMeleeDamageMultiplier);
			this.config.berserkerAddMeleeCritRateGain = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddMeleeCritRateGain", this.config.berserkerAddMeleeCritRateGain);
			this.config.berserkerAddMeleeCritDamageGain = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddMeleeCritDamageGain", this.config.berserkerAddMeleeCritDamageGain);
			this.config.berserkerAddWalkSoundRange = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddWalkSoundRange", this.config.berserkerAddWalkSoundRange);
			this.config.berserkerAddRunSoundRange = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "berserkerAddRunSoundRange", this.config.berserkerAddRunSoundRange);
			this.config.marksmanAddGunDamageMultiplier = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "marksmanAddGunDamageMultiplier", this.config.marksmanAddGunDamageMultiplier);
			this.config.marksmanAddGunCritRateGain = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "marksmanAddGunCritRateGain", this.config.marksmanAddGunCritRateGain);
			this.config.marksmanAddGunCritDamageGain = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "marksmanAddGunCritDamageGain", this.config.marksmanAddGunCritDamageGain);
			this.config.marksmanAddGunDistanceMultiplier = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "marksmanAddGunDistanceMultiplier", this.config.marksmanAddGunDistanceMultiplier);
			this.config.marksmanAddGunScatterMultiplier = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "marksmanAddGunScatterMultiplier", this.config.marksmanAddGunScatterMultiplier);
			this.config.marksmanAddRecoilControl = ModConfigAPI.SafeLoad<float>(ModBehaviour.MOD_NAME, "marksmanAddRecoilControl", this.config.marksmanAddRecoilControl);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004D3C File Offset: 0x00002F3C
		private void SaveConfig(DisplaySkillValueConfig config)
		{
			try
			{
				string contents = JsonUtility.ToJson(config, true);
				File.WriteAllText(ModBehaviour.persistentConfigPath, contents);
				Debug.Log("DisplaySkill: Config saved");
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("DisplaySkill: Failed to save config: {0}", arg));
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004DB8 File Offset: 0x00002FB8
		// Note: this type is marked as 'beforefieldinit'.
		static ModBehaviour()
		{
			SystemLanguage[] array = new SystemLanguage[3];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.57FE32E3C06A9C8A856AA759083194872CC0627114DC7F3E6554D0D3DA0C9BB6).FieldHandle);
			ModBehaviour.CHINESE_LANGUAGES = array;
			ModBehaviour.IS_CHINESE = ModBehaviour.CHINESE_LANGUAGES.Contains(LocalizationManager.CurrentLanguage);
			ModBehaviour.MOD_NAME = (ModBehaviour.IS_CHINESE ? "角色专属技能" : "Character Skills");
			ModBehaviour.translationsRegistered = false;
		}

		// Token: 0x04000027 RID: 39
		private static SystemLanguage[] CHINESE_LANGUAGES;

		// Token: 0x04000028 RID: 40
		public static bool IS_CHINESE;

		// Token: 0x04000029 RID: 41
		private static string MOD_NAME;

		// Token: 0x0400002A RID: 42
		private DisplaySkillValueConfig config = new DisplaySkillValueConfig();

		// Token: 0x0400002B RID: 43
		private const float TICK_TIME = 1f;

		// Token: 0x0400002C RID: 44
		private bool isInCoolDown;

		// Token: 0x0400002D RID: 45
		private float skillStartTime = -1f;

		// Token: 0x0400002E RID: 46
		private float skillDuration;

		// Token: 0x0400002F RID: 47
		private float surviverNextTickTime = -1f;

		// Token: 0x04000030 RID: 48
		private EndowmentIndex currentEndowmentIndex;

		// Token: 0x04000031 RID: 49
		private InputAction activateSkillAction;

		// Token: 0x04000032 RID: 50
		private CharacterBuffManager buffManager;

		// Token: 0x04000033 RID: 51
		private Buff activeSkillTemplate;

		// Token: 0x04000034 RID: 52
		private Buff cooldownSkillTemplate;

		// Token: 0x04000035 RID: 53
		private static bool translationsRegistered;

		// Token: 0x04000036 RID: 54
		private TextMeshProUGUI skillShowText;
	}
}
