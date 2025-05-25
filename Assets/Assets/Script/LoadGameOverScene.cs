using UnityEngine;
using System.Collections; 
using UnityEngine.SceneManagement;


public class LoadGameOverScene : MonoBehaviour
{
    [Header("Configuración de Game Over")]
    public string gameOverSceneName = "Game Over";
    public float dieAnimationDuration = 3f;
    public float gameOverDuration = 2f;
    
    private static LoadGameOverScene instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void TriggerGameOver(string previousScene)
    {
        StartCoroutine(LoadGameOverSceneCoroutine(previousScene));
    }
    
    public IEnumerator LoadGameOverSceneCoroutine(string previousScene)
    {
        yield return new WaitForSeconds(dieAnimationDuration);

        SceneManager.LoadScene(gameOverSceneName);
        
        yield return new WaitForSeconds(gameOverDuration);
        
        Debug.Log("Recargando escena: " + previousScene);
        SceneManager.LoadScene(previousScene);
    }
}
