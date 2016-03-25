using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private int m_KidsSaved;
    public int GetKidsSaved
    {
        get { return m_KidsSaved; }
    }
    private float m_PlaagGeestRecordHeight = 0;

    private GameObject m_Player;
    private GameObject m_PlaagGeest;

    private Player m_PlayerScript;
    private PlaagGeest m_PlaagGeestScript;

    private bool m_GameOver = false;

    private GameObject[] m_Cages;
    private int m_KidsToSave;

    [SerializeField]
    private AudioClip[] m_Audios;
    private AudioSource m_AudioSource;

    [SerializeField]
    private Canvas m_ScoreScreen;

    private string m_Message;
    public string Message
    {
        get { return m_Message; }
        set { m_Message = value; }
    }

    [SerializeField]
    private GameObject m_StartScreen;
    [SerializeField]
    private GameObject m_MainCanvas;

    [SerializeField]
    private GameObject m_TutText;

    [SerializeField]
    private Text m_KTSMessage;

    [SerializeField]
    private Text m_HeadMessage;
    [SerializeField]
    private Text m_Kids;
    [SerializeField]
    private Text m_GeestHeight;

    void Start()
    {
        m_Player = GameObject.Find("Player");
        m_PlaagGeest = GameObject.Find("PlaagGeest");
        m_Cages = GameObject.FindGameObjectsWithTag("Cage");

        m_AudioSource = Camera.main.GetComponent<AudioSource>();

        m_PlayerScript = m_Player.GetComponent<Player>();
        m_PlaagGeestScript = m_PlaagGeest.GetComponent<PlaagGeest>();

        PlaySoundOnce(2);

        Time.timeScale = 0;
        m_MainCanvas.SetActive(false);
        
        for (int i = 0; i < m_Cages.Length; i++)
        {
            if(m_Cages[i].GetComponent<Cage>().GetCageState)
            {
                m_KidsToSave++;
            }
        }
    }

    void Update()
    {
        if(m_PlaagGeestScript.GetCurrentHeight > m_PlaagGeestRecordHeight)
        {
            m_PlaagGeestRecordHeight = m_PlaagGeestScript.GetCurrentHeight;
        }

        m_KidsSaved = m_PlayerScript.FollowingChildren;

        m_KTSMessage.text = m_KidsSaved + "/" + m_KidsToSave;

        if(Input.GetKeyDown(KeyCode.R))
        {
            m_GameOver = true;
        }

        if(m_GameOver)
        {
            ScoreScreen("Test");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_StartScreen.SetActive(false);
            m_MainCanvas.SetActive(true);
            Time.timeScale = 1;
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(m_TutText.activeSelf)
            {
                m_TutText.SetActive(false);
            }
            else
            {
                m_TutText.SetActive(true);
            }
        }
    }

    public void ScoreScreen(string message)
    {
        if(!Camera.main.GetComponent<SmoothCamera2D>().OnCheck)
        {
            Camera.main.GetComponent<SmoothCamera2D>().OnCheck = true;
        }

        if(m_PlaagGeestScript.m_GameRunning)
        {
            m_PlaagGeestScript.m_GameRunning = false;
        }

        m_HeadMessage.text = message;
        m_Kids.text = "Aantal kinderen gered: " + m_KidsSaved;
        m_GeestHeight.text = "Record hoogte van de geest: " + Mathf.RoundToInt(m_PlaagGeestRecordHeight);
        transform.position = Camera.main.transform.position + new Vector3(0, 0, 2);
        Time.timeScale = 0;
        //Vector3.Lerp(Camera.main.transform.position, transform.position, 0.5f);
    }

    public void PlaySoundOnce(int i)
    {
        m_AudioSource.Stop();
        m_AudioSource.PlayOneShot(m_Audios[i]);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        Application.LoadLevel(0);
        Time.timeScale = 1;
    }
}