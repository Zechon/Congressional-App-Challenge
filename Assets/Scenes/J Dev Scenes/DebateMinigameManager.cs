using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebateMinigameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;

    [Header("Settings")]
    public List<DebateQuestion> questions;
    public float timePerQuestion = 6f;

    private int currentIndex = 0;
    private int correctAnswers = 0;
    private float timer;

    private bool questionActive = false;

    void Start()
    {
        StartDebate();
    }

    public void StartDebate()
    {
        currentIndex = 0;
        correctAnswers = 0;
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            EndDebate();
            return;
        }

        DebateQuestion q = questions[currentIndex];
        questionText.text = q.questionText;
        timer = timePerQuestion;
        questionActive = true;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < q.answers.Length)
            {
                int index = i;
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswer(index));
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (!questionActive) return;

        timer -= Time.deltaTime;
        timerText.text = $"Time: {timer:F1}";

        if (timer <= 0)
        {
            questionActive = false;
            NextQuestion(false);
        }
    }

    void OnAnswer(int index)
    {
        questionActive = false;
        bool correct = (index == questions[currentIndex].correctAnswerIndex);

        if (correct) correctAnswers++;
        NextQuestion(correct);
    }

    void NextQuestion(bool correct)
    {
        currentIndex++;
        Invoke(nameof(DisplayQuestion), 0.8f);
    }

    void EndDebate()
    {
        float ratio = (float)correctAnswers / questions.Count;
        float multiplier = Mathf.Lerp(0.8f, 1.5f, ratio); // adjust this curve if needed

        resultText.text = $"Debate Complete! Performance: {(ratio * 100f):F0}% ? Multiplier: {multiplier:F2}x";

        CampaignManager.Instance.ApplyDebateMultiplier(multiplier);
    }
}
