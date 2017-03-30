using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ContextManager : MonoBehaviour {
    public enum GameContext
    {
        Waiting,
        TransitionDToA,
        Diving,
        Ascent,
        TransitionAtoS,
        Shoot
    }

    public static ContextManager instance;
    public float transitionDurationDToA;
    public float transitionDurationAToS;
    public GameContext currentGameContext;
    public GameContext previousGameContext;

    public CameraController cameraController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Start()
    {
        InitGame();
        
    }

    public void SwitchContext(GameContext contextWanted)
    {
        if(contextWanted == currentGameContext)
        {
            return;
        }

        if(contextWanted == GameContext.TransitionAtoS)
        {
            StartCoroutine(TransitionAToS());
        }
        else if(contextWanted == GameContext.TransitionDToA)
        {
            StartCoroutine(TransitionDToA());
        }
        previousGameContext = currentGameContext;
        currentGameContext = contextWanted;
    }

    public void InitGame()
    {
        SwitchContext(GameContext.Waiting);
    }

    public bool CompareContext(GameContext contextToCompare)
    {
        return contextToCompare == currentGameContext;
    }

    IEnumerator TransitionAToS()
    {
        yield return new WaitForSeconds(transitionDurationAToS);
        SwitchContext(GameContext.Shoot);
    }

    IEnumerator TransitionDToA()
    {
        cameraController.TransitionDToA(transitionDurationDToA);
        yield return new WaitForSeconds(transitionDurationDToA);
        SwitchContext(GameContext.Ascent);
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}
