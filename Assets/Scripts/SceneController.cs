using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // NO Awake() function!
    // NO DontDestroyOnLoad!
    
    public void LoadLaboratoryScene()
    {
        Debug.Log("Loading Laboratory Scene");
        SceneManager.LoadScene("LaboratoryScene");
    }
    
    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadScenario1()
    {
        Debug.Log("Loading Scenario 1 (PPE Training)");
        PlayerPrefs.SetInt("StartScenario", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LaboratoryScene");
    }
    
    public void LoadScenario2()
    {
        Debug.Log("Loading Scenario 2 (Fire Extinguisher)");
        PlayerPrefs.SetInt("StartScenario", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LaboratoryScene");
    }
    
    public void LoadScenario3()
    {
        Debug.Log("Loading Scenario 3 (Hazard Quiz)");
        PlayerPrefs.SetInt("StartScenario", 3);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LaboratoryScene");
    }
    
    public void LoadFullTraining()
    {
        Debug.Log("Loading Full Training");
        PlayerPrefs.SetInt("StartScenario", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LaboratoryScene");
    }
    
    public void QuitApplication()
    {
        Debug.Log("Quitting Application");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}