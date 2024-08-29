using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;

public class UITime : TruongSingleton<UITime>
{
    [SerializeField] private TextMeshProUGUI timerText; // Kéo và thả Text component từ UI vào đây trong Inspector  
    [SerializeField] private Slider timeSlider;
    [SerializeField] private float maxTime = 30f;
    private float countdownTime; // Thời gian đếm ngược  

    private void Start()
    {
        // Đặt giá trị Slider tối đa  
        maxTime = 30f;
        countdownTime = maxTime;
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;

        // Bắt đầu đếm ngược  
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            // Lấy số giây nguyên  
            int secondsWhole = Mathf.FloorToInt(countdownTime);

            // Tính toán giây lẻ  
            float fractionalSeconds = (countdownTime - secondsWhole) * 100; // Chuyển đổi phần lẻ thành phần trăm  

            // Cập nhật thời gian hiển thị theo định dạng "Giây: Số lẻ giây"  
            timerText.text = $"{secondsWhole:D2}:{fractionalSeconds:F0}";

            // Cập nhật giá trị Slider  
            timeSlider.value = this.maxTime - countdownTime;

            countdownTime -= Time.deltaTime; // Giảm thời gian theo frame  
            yield return null; // Chờ đến frame tiếp theo  
        }

        // Đảm bảo thời gian là 0 khi kết thúc  
        timerText.text = "00:00";
        // timeSlider.value = 0; // Đặt giá trị Slider về 0  
    }

    [Button]
    public void AddTime()
    {
        this.countdownTime = Mathf.Min(this.countdownTime + 2, this.maxTime);
    }
}