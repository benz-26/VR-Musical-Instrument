using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance;

    [System.Serializable]
    public class Question
    {
        public string questionString;
        public string procieString;
        public string vgaString;
        public string[] answerString;
        public int idCorrectAns;
        public Sprite questionSprite;
    }

    [Header("GENERAL SETTINGS")]
    [SerializeField] private bool useImageQuestion = true;
    [SerializeField] private bool useTimer = false;


    [Header("Question & Answer Bank")]
    public Question[] questionBank;
    [Header("Current Question (No Need to Change)")]
    public Question currentQuestion;

    [Header("Question ID")]
    [SerializeField] private int maxQuestion;
    [SerializeField] private int currentID;
    [SerializeField] private int randomID;
    [SerializeField] private int answerQuestion;
    [SerializeField] private int[] prevListID;

    [Header("Score Game")]
    [SerializeField] private int scoreGame;
    [SerializeField] private int maxScoreGame;
    [SerializeField] private TextMeshProUGUI gameScore;
    [SerializeField] private TextMeshProUGUI endGameScore;

    [Header("TIMER SETTINGS")]
    [SerializeField] private float timePerQuestion = 10f;
    private float currentTimer = 0f;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Quiz Audio")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;

    [Header("Panels Game")]
    [SerializeField] private GameObject loadingDivider;
    [SerializeField] private GameObject endPopUpPanel;

    [Header("Question Components")]
    [SerializeField] private GameObject textQuestion;
    [SerializeField] private Image imageQuestion;
    [SerializeField] private TextMeshProUGUI textProcie;
    [SerializeField] private TextMeshProUGUI textVGA;
    [SerializeField] private TextMeshProUGUI[] textAnswer;
    [SerializeField] private Button[] answerButtons;

    [Header("End Game Components")]
    [SerializeField] private TextMeshProUGUI endQoutes;
    [SerializeField] private int QuotesRanID;
    [SerializeField] private string[] endQoutesTextLow;
    [SerializeField] private string[] endQoutesTextMid;
    [SerializeField] private string[] endQoutesTextHigh;

    [Header("End Game Rank")]
    [SerializeField] private TextMeshProUGUI EndRankText;
    [SerializeField] private string[] EndRankString;

    private bool theEnd = false;

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        endPopUpPanel.SetActive(false);
        LoadQuestionBank();
        LoadQuestionText();
        gameScore.text = answerQuestion.ToString();

        if (useTimer)
        {
            currentTimer = timePerQuestion;
            timerText.text = currentTimer.ToString();
        }
    }

    public void Update()
    {
        gameScore.text = answerQuestion.ToString();
        endGameScore.text = scoreGame.ToString();


        if (useTimer && currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
            int remainingSeconds = Mathf.CeilToInt(currentTimer);
            timerText.text = remainingSeconds.ToString();
        }
        else if(useTimer && currentTimer <= 0f)
        {
            currentTimer = 0f;
            int remainingSeconds = Mathf.CeilToInt(currentTimer);
            timerText.text = remainingSeconds.ToString();
            endPopUpPanel.SetActive(true);

        }
    }

    public void LoadQuestionRandom()
    {
        //di acak dulu value currentID
        randomID = Random.Range(0, questionBank.Length);

        bool flag = false;

        for (int i = 0; i < prevListID.Length; i++)
        {
            if (randomID == prevListID[i])
            {
                flag = true;
                Debug.Log("same number = " + randomID);
                break;
            }
        }

        if (flag == false)
        {
            prevListID[answerQuestion] = randomID;
            currentID = randomID;
        }
        else
        {
            LoadQuestionRandom();
        }
    }

    public void LoadQuestionBank()
    {
        LoadQuestionRandom();
        currentQuestion = questionBank[currentID];
    }


    public void LoadQuestionText()
    {
        textQuestion.GetComponent<TextMeshProUGUI>().text = currentQuestion.questionString;
        /*        textProcie.GetComponent<TextMeshProUGUI>().text = currentQuestion.procieString;
                textVGA.GetComponent<TextMeshProUGUI>().text = currentQuestion.vgaString;*/

        if (useImageQuestion)
        {
            imageQuestion.gameObject.SetActive(true);
            imageQuestion.sprite = currentQuestion.questionSprite;
        }
        else
        {
            imageQuestion.gameObject.SetActive(false);
        }

        for (int i = 0; i < textAnswer.Length; i++)
        {
            textAnswer[i].text = currentQuestion.answerString[i];

            // Remove existing listeners and add a new one
            answerButtons[i].onClick.RemoveAllListeners();
            int answerId = i;
            answerButtons[i].onClick.AddListener(() => CompareAnswer(answerId));
        }
    }

    public void CompareAnswer(int id)
    {
        if (useTimer)
        {
            currentTimer = timePerQuestion;
        }


        if (currentQuestion.idCorrectAns == id)
        {
            Debug.Log("benar");
            audioSource.PlayOneShot(audioClip);
            loadingDivider.SetActive(true);
            StartCoroutine(QuestionChecker());
            AddScore();
        }
        else if (currentQuestion.idCorrectAns != id)
        {
            Debug.Log("Salah");
            audioSource.PlayOneShot(audioClip);
            loadingDivider.SetActive(true);
            StartCoroutine(QuestionChecker());
        }

        if (answerQuestion != maxQuestion - 1)
        {
            answerQuestion++;
            LoadQuestionBank();
            LoadQuestionText();
        }
        else
        {
            theEnd = true;
            StartCoroutine(QuestionChecker());

            if (scoreGame <= 40 && scoreGame >= 0)
            {
                RandomQoutes(endQoutesTextLow);
                EndRankText.text = EndRankString[0];
            }
            else if (scoreGame <= 80 && scoreGame > 40)
            {
                RandomQoutes(endQoutesTextMid);
                EndRankText.text = EndRankString[1];
            }
            else if (scoreGame <= 100 && scoreGame > 80)
            {
                RandomQoutes(endQoutesTextHigh);
                EndRankText.text = EndRankString[2];
            }
        }
    }

    private void RandomQoutes(string[] qoutesTextArray)
    {
        QuotesRanID = Random.Range(0, qoutesTextArray.Length);
        endQoutes.GetComponent<TextMeshProUGUI>().text = qoutesTextArray[QuotesRanID];
    }

    public void PanelPopUpClose()
    {
        endPopUpPanel.SetActive(false);
        SceneManager.LoadScene("Gameplay");
        audioSource.PlayOneShot(audioClip);
/*        MainLoader.Loading = true;*/
    }

    public void AddScore()
    {
        if (scoreGame >= maxScoreGame)
        {
            scoreGame = maxScoreGame;
        }
        else
        {
            scoreGame+=1;
        }


    }

    IEnumerator QuestionChecker()
    {
        yield return new WaitForSeconds(0.5f);
        loadingDivider.SetActive(false);

        if (theEnd == true)
        {
            endPopUpPanel.SetActive(true);
        }
    }



}

