using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    // VISUAL
    [SerializeField]
    private Image sliderValueImage;
    [SerializeField]
    private GameObject victoryPanel;

    public float AccelerationMultiplier { get; private set; }
    private float maxAccelerationMultiplier = 4f;

    private float movementTimeAmount;

    // VISUAL
    [HideInInspector]
    public bool isGameFinished;

    private void Start()
    {
        AccelerationMultiplier = 1f;

        // VISUAL
        movementTimeAmount = 17.5f;
        sliderValueImage.fillAmount = 0;
    }
    private void Update()
    {
        // game acceleration
        if (AccelerationMultiplier <= maxAccelerationMultiplier)
            AccelerationMultiplier += Time.deltaTime / 15f;

        // VISUAL
        sliderValueImage.fillAmount += Time.deltaTime / movementTimeAmount;
        if(sliderValueImage.fillAmount >= .95f)
            HideSliderPanel();
        if(sliderValueImage.fillAmount >= .975f)
            Victory();
    }

    // VISUAL
    private void HideSliderPanel()
    {
        sliderValueImage.transform.parent.GetComponent<Animator>().SetTrigger("Disappearing");
    }
    private void Victory()
    {
        // show victory panel
        // *DEMO: other work will be done in animator*
        isGameFinished = true;
        victoryPanel.SetActive(true);
    }
}
