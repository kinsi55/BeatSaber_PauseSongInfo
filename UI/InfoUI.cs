using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using PauseSongInfo.HarmonyPatches;
using PauseSongInfo.Util;
using TMPro;
using UnityEngine;

namespace PauseSongInfo.UI {
	class InfoUIInfo {
		[UIComponent("icon")] readonly ImageView icon = null;
		[UIComponent("label")] readonly TextMeshProUGUI label = null;

		string _text;

		public string text {
			set {
				_text = value;

				if(label != null)
					label.text = value;
			}
		}

		string iconTouse;

		public InfoUIInfo(string icon) {
			iconTouse = icon;
		}


		Sprite iconBackup;
		[UIAction("#post-parse")]
		void Parsed() {
			if(iconBackup == null) {
				// This is not cached (yet) https://github.com/monkeymanboy/BeatSaberMarkupLanguage/pull/125
				icon.SetImage(iconTouse);
				iconBackup = icon.sprite;
			} else {
				icon.sprite = iconBackup;
			}

			label.text = _text;
		}
	}

	class InfoUI {
		InfoUI() { }

		static readonly InfoUI instance = new InfoUI();

		static List<InfoUIInfo> infos = new List<InfoUIInfo>() {
			new InfoUIInfo("#NPSIcon"),
			new InfoUIInfo("#GameNoteIcon"),
			new InfoUIInfo("#ObstacleIcon"),
			new InfoUIInfo("#BombIcon"),

			new InfoUIInfo("#ClockIcon"),
			new InfoUIInfo("#MetronomeIcon"),
			new InfoUIInfo("#FastNotesIcon"),
			new InfoUIInfo("#OstAndExtrasIcon")
		};

		public static void Create(GameObject gameObject) {
			gameObject.transform.localScale *= Config.Instance.MenuScale;

			var songLength = HookLeveldata.difficultyBeatmap.level.beatmapLevelData.audioClip.length;

#if pre_1_20
			infos[0].text = (HookLeveldata.difficultyBeatmap.beatmapData.cuttableNotesCount / songLength).ToString("0.#");
			infos[1].text = HookLeveldata.difficultyBeatmap.beatmapData.cuttableNotesCount.ToString();
			infos[2].text = HookLeveldata.difficultyBeatmap.beatmapData.obstaclesCount.ToString();
			infos[3].text = HookLeveldata.difficultyBeatmap.beatmapData.bombsCount.ToString();
#else
			for(var i = 0; i < 3; i++)
				infos[i].text = "...";

			HookLeveldata.difficultyBeatmap.GetBeatmapDataBasicInfoAsync().ContinueWith(x => {
				if(x.Result == null || x.IsFaulted) {
					for(var i = 0; i < 3; i++)
						infos[i].text = "?";

					return;
				}

				infos[0].text = (x.Result.cuttableNotesCount / songLength).ToString("0.#");
				infos[1].text = x.Result.cuttableNotesCount.ToString();
				infos[2].text = x.Result.obstaclesCount.ToString();
				infos[3].text = x.Result.bombsCount.ToString();
			}, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
#endif

			infos[4].text = TimeSpan.FromSeconds(songLength).ToString(@"mm\:ss");
			infos[5].text = ((int)HookLeveldata.difficultyBeatmap.level.beatsPerMinute).ToString();
			infos[6].text = HookLeveldata.difficultyBeatmap.noteJumpMovementSpeed.ToString("0.#");

			if(!SongDetailsUtil.isAvailable) {
				infos[7].text = "N/A";
			} else {
				GetMapKey();
			}

			BSMLParser.instance.Parse(
				Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"PauseSongInfo.UI.info.bsml"),
				gameObject, instance
			);
		}

		static void GetMapKey() {
			infos[7].text = "...";

			void SetTextMainthread(string text) => IPA.Utilities.Async.UnityMainThreadTaskScheduler.Factory.StartNew(() => infos[7].text = text);

			Task.Run(async () => {
				if(!SongDetailsUtil.finishedInitAttempt) {
					await SongDetailsUtil.TryGet();
				}

				if(SongDetailsUtil.songDetails == null) {
					SetTextMainthread("?");
					return;
				}

				var mh = BeatmapsUtil.GetHashOfPreview(HookLeveldata.difficultyBeatmap.level);

				if(mh == null ||
					!SongDetailsUtil.songDetails.instance.songs.FindByHash(mh, out var song)
				) {
					SetTextMainthread("?");
				} else {
					SetTextMainthread(song.key.ToUpper());
				}
			});
		}
	}
}
