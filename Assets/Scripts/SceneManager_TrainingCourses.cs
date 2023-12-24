using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_TrainingCourses : MonoBehaviour
{
    public static SceneManager_TrainingCourses Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(Input.GetKey(KeyCode.LeftControl))
    //    {
    //        if(Input.GetKeyDown(KeyCode.Keypad1))
    //        {
    //            SceneManager.LoadScene(0);
    //            return;
    //        }
    //        if(Input.GetKeyDown(KeyCode.Keypad2))
    //        {
    //            SceneManager.LoadScene(1);
    //            return;
    //        }
    //        if(Input.GetKeyDown(KeyCode.Keypad3))
    //        {
    //            SceneManager.LoadScene(2);
    //            return;
    //        }

    //    }
    //}

    public void LoadSceneByNumber(int number)
    {
        SceneManager.LoadScene(number);
    }

    public void QuitGame()
    {
        //ON QUIT GAME?
        Application.Quit();
    }
}
