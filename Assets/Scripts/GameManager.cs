using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //게임 매니저가 필요한 변수 세팅
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public GameObject menuChick;

    public Button startButton;
    public Button ContinueButton;
    public bool hasPlaylog;
    
    public string[] errandLists;
    public GameObject[] ErrandListObjects;
    public Text[] errandTexts;
    public Image checkFrontAImg;
    public Image checkFrontBImg;
    public Image checkFrontCImg;
    public Image checkFrontDImg;

    public GameObject stampA;
    public GameObject stampB;
    public GameObject stampC;
    public GameObject stampD;
    public GameObject stampE;
    
    public float playTime;
    private float playTimeLimit = 10 * 60;
    public RectTransform bossHealthBar;

    public Image TrafficRedLightImage;
    public Image TrafficGreenLightImage;
    public Text TrafficTimeText;
    public int trafficCountownValue;

    public int stage;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreText;
    public Text scoreText;
    public Text stageText;
    public Text playTimeText;
    public Text playerCoinText;

    public int selectionCount = 4;
    private Color redColor = new Color(0.980f, 0.282f, 0.282f);
    private Color greenColor = new Color(0.086f, 0.784f, 0f);
    private Color grayColor = new Color(0.4f, 0.4f, 0.4f);
    private bool isGreen = false;
    

    void Awake()
    {
        maxScoreText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
        checkPlayLog();
        makeErrandList();
    }

    void Start()
    {
        InvokeRepeating("DecreaseCountdown", 1f, 1f);
    }

    void checkPlayLog()
    {
        if(hasPlaylog){
            Text startButtonText = startButton.GetComponentInChildren<Text>();
            startButtonText.text = "새 게임 시작";
            ContinueButton.gameObject.SetActive(true);
        }
        
    }

    int[] generateRandomNumbers(){
        int[] selectedIndices = {-1, -1, -1, -1};
        bool[] boolMap = new bool[18];

        // 배열을 false로 초기화
        for (int i = 0; i < boolMap.Length; i++)
        {
            boolMap[i] = false;
        }
        
        for(int i=0; i<selectedIndices.Length; i++){
            int randomNum = Random.Range(0, errandLists.Length);
            if(!boolMap[randomNum]){
                selectedIndices[i] = randomNum;
                boolMap[randomNum] = true;
            }
            else i--;
        }
        return selectedIndices;
    }

    void makeErrandList()
    {
        int[] selectedIndices = generateRandomNumbers();

        for(int i=0; i<ErrandListObjects.Length; i++){
            GameObject AErrandList = ErrandListObjects[i];
            Image[] childImages = AErrandList.GetComponentsInChildren<Image>();

            int randomIndex = selectedIndices[i];
            Debug.Log(randomIndex);
            for (int j = 0; j < childImages.Length; j++){
                childImages[j].gameObject.SetActive(false);
            }
            childImages[randomIndex].gameObject.SetActive(true);

            int buyQuantity = Random.Range(1, 4);
            if(randomIndex >= 14) buyQuantity = 1;

            errandTexts[i].text = errandLists[randomIndex] + " " + buyQuantity.ToString() + " 개";
        }
    }
    
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        menuChick.SetActive(false);
        player.gameObject.SetActive(true);
    }
    
    void Update()
    {
        playTime += Time.deltaTime;
    }

    void LateUpdate()
    {
        scoreText.text = string.Format("{0:n0}", player.score);
        playerCoinText.text = string.Format("{0:n0}", player.coin);

        int min = (int)(playTime / 60);
        int second = (int)(playTime % 60);
        playTimeText.text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        checkFrontAImg.color = new Color(1,1,1, player.iscompletedErrand[0] ? 1:0);
        checkFrontBImg.color = new Color(1,1,1, player.iscompletedErrand[1] ? 1:0); 
        checkFrontCImg.color = new Color(1,1,1, player.iscompletedErrand[2] ? 1:0); 
        checkFrontDImg.color = new Color(1,1,1, player.iscompletedErrand[3] ? 1:0);   

        bossHealthBar.localScale = new Vector3((playTimeLimit-playTime)/playTimeLimit,1,1);

        stampA.SetActive(player.iscompletedStamp[0]);
        stampB.SetActive(player.iscompletedStamp[1]);
        stampC.SetActive(player.iscompletedStamp[2]);
        stampD.SetActive(player.iscompletedStamp[3]);
        stampE.SetActive(player.iscompletedStamp[4]);
    }

    private void DecreaseCountdown()
    {
        // countdownValue 값을 1씩 감소시키고 텍스트에 반영
        trafficCountownValue--;

        if (trafficCountownValue <= 0)
        {
            if(isGreen) ChageRedCountdown();
            else ChageGreenCountdown();
        }

        if(trafficCountownValue != 0) TrafficTimeText.text = trafficCountownValue.ToString();

        
    }
    private void ChageRedCountdown()
    {
        TrafficRedLightImage.color = redColor;
        TrafficGreenLightImage.color = grayColor;
        TrafficTimeText.color = redColor;
        trafficCountownValue = 10;
        isGreen = false;
    }

    private void ChageGreenCountdown()
    {
        TrafficRedLightImage.color = grayColor;
        TrafficGreenLightImage.color = greenColor;
        TrafficTimeText.color = greenColor;
        trafficCountownValue = 20;
        isGreen = true;
    }

    private void StopCountdown()
    {
        CancelInvoke("DecreaseCountdown");
    }
}
