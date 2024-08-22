using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : PopUp
{
    [SerializeField] private Slider volumeSlider;

    protected override void Start()
    {
        base.Start();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        volumeSlider.value = SoundManager.Instance.GetVolume();
    }

    private void OnVolumeChanged(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }

    public void ClosePopup()
    {
        HideScreen();
    }
}