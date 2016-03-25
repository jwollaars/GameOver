using UnityEngine;
using System.Collections;

public class Cage : MonoBehaviour
{
    private Animator m_Animator;

    private GameObject m_ChildPrefab;

    private GameObject m_Player;
    private Player m_PlayerScript;

    private GameObject m_PlaagGeest;
    private PlaagGeest m_PlaagScript;

    private GameObject m_Particles;
    private ParticleSystem m_PSystem;
    private bool m_Test = false;

    private bool m_IsReleased = false;

    private Renderer m_RenderScript;

    private Vector3 m_StartPos;

    private float m_Timer = 0.5f;
    private float m_ShakeTimer = 0.2f;

    [SerializeField]
    private bool m_CageState = true;
    public bool GetCageState
    {
        get { return m_CageState; }
    }

    [SerializeField]
    private bool m_Special = false;

    [SerializeField]
    private Material m_Color;
    [SerializeField]
    private Color m_Filled;
    [SerializeField]
    private Color m_Released;

    private GameManager m_GameManager;

    void Start()
    {
        m_ChildPrefab = GameObject.Find("ProtoChild");
        m_PlaagGeest = GameObject.Find("PlaagGeest");
        m_Player = GameObject.Find("Player");
        m_Particles = transform.GetChild(0).gameObject;
        m_PSystem = m_Particles.GetComponent<ParticleSystem>();
        m_RenderScript = GetComponent<Renderer>();
        m_Animator = GetComponent<Animator>();
        m_PlaagScript = m_PlaagGeest.GetComponent<PlaagGeest>();
        m_PlayerScript = m_Player.GetComponent<Player>();
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(DisableCanvas());

        m_RenderScript.material.color = m_Filled;
        m_StartPos = transform.position;
    }

    void Update()
    {
        if(m_CageState)
        {
            m_ShakeTimer -= Time.deltaTime;
            if (m_ShakeTimer <= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 5));
                transform.position = new Vector3(m_StartPos.x + Random.Range(-0.05f, 0.05f), m_StartPos.y + Random.Range(-0.05f, 0.05f), m_StartPos.z);
                m_ShakeTimer = 0.05f;
            }
        }

        if(m_IsReleased)
        {
            m_ShakeTimer = 5;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            m_Timer -= Time.deltaTime;
            if (m_Timer <= 0 && !m_Test)
            {
                m_PSystem.Stop();
            }
        }

        if(!m_CageState)
        {
            m_Animator.SetBool("Evil", true);
        }
    }

    //public void ActivateQuestion()
    //{
    //    if (m_QuestionSystem.activeSelf == false && m_IsReleased == false)
    //    {
    //        Time.timeScale = 0;
    //        m_QuestionSystem.SetActive(true);
    //        m_QuizScript.SetQuestions();
    //    }
    //}

    public void ReleaseChild()
    {
        if (m_IsReleased == false && !m_Special)
        {
            m_IsReleased = true;
            GameObject Clone = Instantiate(m_ChildPrefab, transform.position + new Vector3(0, 1), Quaternion.identity) as GameObject;
            Clone.name = "Child";
            Clone.SetActive(true);
            Clone.GetComponent<ChildBehaviour>().Start();
            Clone.GetComponent<ChildBehaviour>().Jump();
            m_Animator.SetBool("Released", true);
            m_Particles.SetActive(true);
            m_PlaagScript.AddCount(2);
            m_PlayerScript.FollowingChildren += 1;
            transform.position = m_StartPos;
        }

        if (m_IsReleased == false && m_Special)
        {
            m_IsReleased = true;
            GameObject Clone = Instantiate(m_ChildPrefab, transform.position + new Vector3(0, 1), Quaternion.identity) as GameObject;
            Clone.name = "Child";
            Clone.SetActive(true);
            Clone.GetComponent<ChildBehaviour>().Start();
            Clone.GetComponent<ChildBehaviour>().Special = true;
            m_PlaagScript.Target = Clone;
            m_Animator.SetBool("Released", true);
            m_Particles.SetActive(true);
            m_PlaagScript.AddCount(2);
            m_PlayerScript.FollowingChildren += 1;
            transform.position = m_StartPos;
        }
    }

    public void ReleaseDarkness()
    {
        if (m_IsReleased == false)
        {
            m_IsReleased = true;
            m_PlaagScript.AddCount(-2);
            m_Animator.SetBool("Released", true);
            m_Particles.SetActive(true);
            m_GameManager.PlaySoundOnce(3);
        }
    }

    private IEnumerator DisableCanvas()
    {
        yield return new WaitForSeconds(0.01f);
        m_ChildPrefab.SetActive(false);
    }
}