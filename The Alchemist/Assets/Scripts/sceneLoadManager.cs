using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoadManager : MonoBehaviour {

   public void loadTown()
    {
        SceneManager.LoadScene(1);
    }

    public void loadCave()
    {
        SceneManager.LoadScene(2);
    }
}
