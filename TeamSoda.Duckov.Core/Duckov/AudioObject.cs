using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200022F RID: 559
	public class AudioObject : MonoBehaviour
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x000440FB File Offset: 0x000422FB
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x00044103 File Offset: 0x00042303
		public AudioManager.VoiceType VoiceType
		{
			get
			{
				return this.voiceType;
			}
			set
			{
				this.voiceType = value;
			}
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0004410C File Offset: 0x0004230C
		internal static AudioObject GetOrCreate(GameObject from)
		{
			AudioObject component = from.GetComponent<AudioObject>();
			if (component != null)
			{
				return component;
			}
			return from.AddComponent<AudioObject>();
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00044134 File Offset: 0x00042334
		public EventInstance? PostQuak(string soundKey)
		{
			string eventName = "Char/Voice/vo_" + this.voiceType.ToString().ToLower() + "_" + soundKey;
			return this.Post(eventName, true);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00044170 File Offset: 0x00042370
		public EventInstance? Post(string eventName, bool doRelease = true)
		{
			EventInstance eventInstance;
			if (!AudioManager.TryCreateEventInstance(eventName ?? "", out eventInstance))
			{
				return null;
			}
			eventInstance.setCallback(new EVENT_CALLBACK(AudioObject.EventCallback), (EVENT_CALLBACK_TYPE)4294967295U);
			this.events.Add(eventInstance);
			eventInstance.set3DAttributes(base.gameObject.transform.position.To3DAttributes());
			this.ApplyParameters(eventInstance);
			eventInstance.start();
			if (doRelease)
			{
				eventInstance.release();
			}
			return new EventInstance?(eventInstance);
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000441F8 File Offset: 0x000423F8
		public EventInstance? PostCustomSFX(string filePath, bool doRelease = true, bool loop = false)
		{
			string eventPath = loop ? "SFX/custom_loop" : "SFX/custom";
			return this.PostFile(eventPath, filePath, doRelease);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00044220 File Offset: 0x00042420
		public EventInstance? PostFile(string eventPath, string filePath, bool doRelease = true)
		{
			if (!File.Exists(filePath))
			{
				UnityEngine.Debug.Log("[Audio] File don't exist: " + filePath);
			}
			EventInstance eventInstance;
			if (!AudioManager.TryCreateEventInstance(eventPath, out eventInstance))
			{
				return null;
			}
			this.events.Add(eventInstance);
			GCHandle value = GCHandle.Alloc(filePath);
			eventInstance.setUserData(GCHandle.ToIntPtr(value));
			eventInstance.setCallback(new EVENT_CALLBACK(AudioObject.CustomSFXCallback), (EVENT_CALLBACK_TYPE)4294967295U);
			eventInstance.start();
			if (doRelease)
			{
				eventInstance.release();
			}
			return new EventInstance?(eventInstance);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x000442A8 File Offset: 0x000424A8
		private static RESULT CustomSFXCallback(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
		{
			EventInstance eventInstance = new EventInstance(_event);
			IntPtr value;
			eventInstance.getUserData(out value);
			GCHandle gchandle = GCHandle.FromIntPtr(value);
			string name = gchandle.Target as string;
			if (type != EVENT_CALLBACK_TYPE.DESTROYED)
			{
				if (type != EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND)
				{
					if (type == EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND)
					{
						PROGRAMMER_SOUND_PROPERTIES programmer_SOUND_PROPERTIES = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(PROGRAMMER_SOUND_PROPERTIES));
						Sound sound = new Sound(programmer_SOUND_PROPERTIES.sound);
						sound.release();
					}
				}
				else
				{
					MODE mode = MODE.LOOP_NORMAL | MODE.CREATECOMPRESSEDSAMPLE | MODE.NONBLOCKING;
					PROGRAMMER_SOUND_PROPERTIES structure = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(PROGRAMMER_SOUND_PROPERTIES));
					Sound sound2;
					if (RuntimeManager.CoreSystem.createSound(name, mode, out sound2) == RESULT.OK)
					{
						structure.sound = sound2.handle;
						structure.subsoundIndex = -1;
						Marshal.StructureToPtr<PROGRAMMER_SOUND_PROPERTIES>(structure, parameters, false);
					}
				}
			}
			else
			{
				gchandle.Free();
			}
			return RESULT.OK;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00044384 File Offset: 0x00042584
		public void Stop(string eventName, FMOD.Studio.STOP_MODE mode)
		{
			foreach (EventInstance eventInstance in this.events)
			{
				EventDescription eventDescription;
				string str;
				if (eventInstance.getDescription(out eventDescription) == RESULT.OK && eventDescription.getPath(out str) == RESULT.OK && !("event:/" + str != eventName))
				{
					eventInstance.stop(mode);
					break;
				}
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00044404 File Offset: 0x00042604
		private static RESULT EventCallback(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
		{
			if (type <= EVENT_CALLBACK_TYPE.PLUGIN_DESTROYED)
			{
				if (type <= EVENT_CALLBACK_TYPE.STOPPED)
				{
					if (type <= EVENT_CALLBACK_TYPE.STARTED)
					{
						switch (type)
						{
						case EVENT_CALLBACK_TYPE.CREATED:
						case EVENT_CALLBACK_TYPE.DESTROYED:
						case EVENT_CALLBACK_TYPE.CREATED | EVENT_CALLBACK_TYPE.DESTROYED:
						case EVENT_CALLBACK_TYPE.STARTING:
							break;
						default:
							if (type != EVENT_CALLBACK_TYPE.STARTED)
							{
							}
							break;
						}
					}
					else if (type != EVENT_CALLBACK_TYPE.RESTARTED && type != EVENT_CALLBACK_TYPE.STOPPED)
					{
					}
				}
				else if (type <= EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND)
				{
					if (type != EVENT_CALLBACK_TYPE.START_FAILED && type != EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND)
					{
					}
				}
				else if (type != EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND && type != EVENT_CALLBACK_TYPE.PLUGIN_CREATED && type != EVENT_CALLBACK_TYPE.PLUGIN_DESTROYED)
				{
				}
			}
			else if (type <= EVENT_CALLBACK_TYPE.SOUND_STOPPED)
			{
				if (type <= EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
				{
					if (type != EVENT_CALLBACK_TYPE.TIMELINE_MARKER && type != EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
					{
					}
				}
				else if (type != EVENT_CALLBACK_TYPE.SOUND_PLAYED && type != EVENT_CALLBACK_TYPE.SOUND_STOPPED)
				{
				}
			}
			else if (type <= EVENT_CALLBACK_TYPE.VIRTUAL_TO_REAL)
			{
				if (type != EVENT_CALLBACK_TYPE.REAL_TO_VIRTUAL && type != EVENT_CALLBACK_TYPE.VIRTUAL_TO_REAL)
				{
				}
			}
			else if (type == EVENT_CALLBACK_TYPE.START_EVENT_COMMAND || type != EVENT_CALLBACK_TYPE.NESTED_TIMELINE_BEAT)
			{
			}
			return RESULT.OK;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000444F4 File Offset: 0x000426F4
		private void FixedUpdate()
		{
			if (this == null)
			{
				return;
			}
			if (base.transform == null)
			{
				return;
			}
			if (this.events == null)
			{
				return;
			}
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.set3DAttributes(base.transform.position.To3DAttributes());
				}
			}
			if (this.needCleanup)
			{
				this.events.RemoveAll((EventInstance e) => !e.isValid());
				this.needCleanup = false;
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000445C8 File Offset: 0x000427C8
		internal void SetParameterByName(string parameter, float value)
		{
			this.parameters[parameter] = value;
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.setParameterByName(parameter, value, false);
				}
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00044640 File Offset: 0x00042840
		internal void SetParameterByNameWithLabel(string parameter, string label)
		{
			this.strParameters[parameter] = label;
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.setParameterByNameWithLabel(parameter, label, false);
				}
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000446B8 File Offset: 0x000428B8
		private void ApplyParameters(EventInstance eventInstance)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.parameters)
			{
				eventInstance.setParameterByName(keyValuePair.Key, keyValuePair.Value, false);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in this.strParameters)
			{
				eventInstance.setParameterByNameWithLabel(keyValuePair2.Key, keyValuePair2.Value, false);
			}
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00044770 File Offset: 0x00042970
		internal void StopAll(FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
		{
			foreach (EventInstance eventInstance in this.events)
			{
				if (!eventInstance.isValid())
				{
					this.needCleanup = true;
				}
				else
				{
					eventInstance.stop(mode);
				}
			}
		}

		// Token: 0x04000D9C RID: 3484
		private Dictionary<string, float> parameters = new Dictionary<string, float>();

		// Token: 0x04000D9D RID: 3485
		private Dictionary<string, string> strParameters = new Dictionary<string, string>();

		// Token: 0x04000D9E RID: 3486
		private AudioManager.VoiceType voiceType;

		// Token: 0x04000D9F RID: 3487
		public List<EventInstance> events = new List<EventInstance>();

		// Token: 0x04000DA0 RID: 3488
		private bool needCleanup;
	}
}
