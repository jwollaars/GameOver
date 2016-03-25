using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuizSystem : MonoBehaviour
{
    private GameObject m_Canvas;
    private GameObject m_Player;
    private Player m_PlayerScript;

    private List<string> m_Questions;
    private List<string> m_Answers;
    private List<int> m_Numbers;

    private Vector3[] m_ButtonPositions;

    [SerializeField]
    private Text[] m_Texts;
    [SerializeField]
    private Button[] m_Buttons;

    void Start()
    {
        m_Canvas = GameObject.Find("Canvas");
        m_Player = GameObject.Find("Player");
        m_PlayerScript = m_Player.GetComponent<Player>();

        m_Questions = new List<string>();
        m_Answers = new List<string>();
        m_Numbers = new List<int>();
        m_ButtonPositions = new Vector3[3];

        m_ButtonPositions[0] = new Vector3(-290f, 130f, 0f);
        m_ButtonPositions[1] = new Vector3(0f, 130f, 0f);
        m_ButtonPositions[2] = new Vector3(290f, 130f, 0f);

        m_Questions.Add("Wat is pesten?");
        m_Questions.Add("Wat zijn de oorzaken van pesten?");
        m_Questions.Add("Wat zijn de gevolgen van pesten?");
        m_Questions.Add("Hoeveel kinderen worden gepest?");
        m_Questions.Add("Wat is het verschil tussen plagen en pesten?");

        m_Answers.Add("Iemand vocaal of fysiek pijn doen");
        m_Answers.Add("Jaloezie, onzekerheid en verschil");
        m_Answers.Add("Pijn in je buik/hoofd, slecht slapen en angst");
        m_Answers.Add("1 op de 6 kinderen");
        m_Answers.Add("Plagen kan je samen lachen en pesten doe je iemand pijn");
        m_Answers.Add("Het kind loopt heel onzeker rond");
        m_Answers.Add("Niks");
    }

    public void SetQuestions()
    {
        int questionAnswer = Random.Range(0, 3);
        m_Texts[0].text = m_Questions[questionAnswer];
        m_Texts[1].text = m_Answers[questionAnswer];
        m_Questions.RemoveAt(questionAnswer);
        m_Answers.RemoveAt(questionAnswer);

        m_Texts[2].text = m_Answers[Random.Range(0, m_Answers.Count)];
        m_Texts[3].text = m_Answers[Random.Range(0, m_Answers.Count)];

        m_Numbers.Add(0);
        m_Numbers.Add(1);
        m_Numbers.Add(2);

        int number = Random.Range(0, m_Numbers.Count);

        m_Buttons[0].GetComponent<RectTransform>().localPosition = m_ButtonPositions[m_Numbers[number]];
        m_Numbers.RemoveAt(number);
        number = Random.Range(0, m_Numbers.Count);
        m_Buttons[1].GetComponent<RectTransform>().localPosition = m_ButtonPositions[m_Numbers[number]];
        m_Numbers.RemoveAt(number);
        number = Random.Range(0, m_Numbers.Count);
        m_Buttons[2].GetComponent<RectTransform>().localPosition = m_ButtonPositions[m_Numbers[number]];
        m_Numbers.RemoveAt(number);
    }

    public void RightAnswer()
    {
        m_PlayerScript.GetLastCage.GetComponent<Cage>().ReleaseChild();
        Time.timeScale = 1;
        m_Canvas.SetActive(false);
    }

    public void WrongAnswer()
    {

    }
}