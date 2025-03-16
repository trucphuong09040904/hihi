using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // Được gán từ Inspector hoặc tự động tìm

    private const string VolumeKey = "Volume"; // Định nghĩa khóa lưu trữ để tránh lỗi chính tả

    void Start()
    {
        // Tự động tìm Slider nếu chưa được gán trong Inspector
        if (volumeSlider == null)
        {
            volumeSlider = FindObjectOfType<Slider>();

            if (volumeSlider == null)
            {
                Debug.LogError("Không tìm thấy Slider âm lượng! Hãy gán nó vào AudioManager.");
                return; // Thoát khỏi hàm để tránh lỗi tiếp theo
            }
        }

        // Lấy giá trị âm lượng đã lưu hoặc đặt mặc định là 1
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Gán sự kiện thay đổi giá trị
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Cập nhật âm lượng khi Slider thay đổi
    private void SetVolume(float volume)
    {
        if (Mathf.Approximately(AudioListener.volume, volume)) return; // Tránh lưu liên tục nếu không thay đổi

        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save(); // Lưu lại để dùng lần sau
    }
}
