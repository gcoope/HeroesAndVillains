using System;
using UnityEngine;
using UnityEngine.UI;
using smoothstudio.heroesandvillains.player.events;

public class PauseMenuVolumeSlider : MonoBehaviour {

	public enum Type {
		SFX,
		MUSIC
	}

	private Slider volSlider;
	public Type sliderType;
	public Text volSliderLabel;

	void Start() {
		volSlider = GetComponent<Slider>();
		if(volSlider != null) { 
			volSlider.onValueChanged.AddListener(OnSliderChange);

			if(sliderType == Type.MUSIC) {
				volSlider.value = SoundManager.instance.MusicVolume;
			} else {
				volSlider.value = SoundManager.instance.SFXVolume;
			}

			volSliderLabel.text = Mathf.Round(volSlider.value * 100) + "%";
		}

	}

	void OnSliderChange(float val) {
		volSliderLabel.text = Mathf.Round(val * 100) + "%";
		gameObject.DispatchGlobalEvent(MenuEvent.UpdateAudioVolumes);
		if(sliderType == Type.MUSIC) {
			SoundManager.instance.SetMusicVolume(val);
		} else {
			SoundManager.instance.SetSFXVolume(val);
		}
	}

}

