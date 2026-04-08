using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrainingFlowManager : MonoBehaviour
{
    public enum TrainingScenario
    {
        PPE,
        FireExtinguisher,
        HazardQuiz,
        Complete
    }
    
    [Header("Current State")]
    public TrainingScenario currentScenario = TrainingScenario.PPE;
    private bool scenarioCompleted = false; // NEW: Prevents multiple completions
    
    [Header("Scenario GameObjects")]
    public GameObject ppeArea;
    public GameObject fireArea;
    public GameObject quizArea;
    
    [Header("UI")]
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI progressText;
    
    [Header("Managers")]
    public PPEManager ppeManager;
    public HazardQuiz hazardQuiz;
    
    private int totalScore = 0;
    private bool hasAddedPPEScore = false;
    private bool hasAddedFireScore = false;
    private bool hasAddedQuizScore = false;
    
    void Start()
{
    // Check if specific scenario was selected from menu
    int selectedScenario = PlayerPrefs.GetInt("StartScenario", 0);
    
    if (selectedScenario == 1)
    {
        // Start at PPE scenario
        Debug.Log("Starting directly at PPE scenario");
        StartScenario(TrainingScenario.PPE);
        PlayerPrefs.DeleteKey("StartScenario");
    }
    else if (selectedScenario == 2)
    {
        // Start at Fire scenario
        Debug.Log("Starting directly at Fire scenario");
        StartScenario(TrainingScenario.FireExtinguisher);
        PlayerPrefs.DeleteKey("StartScenario");
    }
    else if (selectedScenario == 3)
    {
        // Start at Quiz scenario
        Debug.Log("Starting directly at Quiz scenario");
        StartScenario(TrainingScenario.HazardQuiz);
        PlayerPrefs.DeleteKey("StartScenario");
    }
    else
    {
        // Start full training from beginning
        Debug.Log("Starting full training from PPE");
        StartScenario(TrainingScenario.PPE);
    }
}
    
    void Update()
    {
        // Only check if scenario is NOT already completed
        if (!scenarioCompleted)
        {
            CheckScenarioCompletion();
        }
    }
    
    void StartScenario(TrainingScenario scenario)
    {
        currentScenario = scenario;
        scenarioCompleted = false; // Reset flag for new scenario
        
        // Disable all areas
        if (ppeArea != null) ppeArea.SetActive(false);
        if (fireArea != null) fireArea.SetActive(false);
        if (quizArea != null) quizArea.SetActive(false);
        
        // Enable current scenario
        switch (scenario)
        {
            case TrainingScenario.PPE:
                if (ppeArea != null) ppeArea.SetActive(true);
                UpdateInstructions("Scenario 1/3: Put on all PPE equipment");
                UpdateProgress("Progress: PPE Safety");
                Debug.Log("=== PPE SCENARIO STARTED ===");
                break;
                
            case TrainingScenario.FireExtinguisher:
                if (fireArea != null) fireArea.SetActive(true);
                UpdateInstructions("Scenario 2/3: Extinguish the fire");
                UpdateProgress("Progress: Fire Safety");
                Debug.Log("=== FIRE EXTINGUISHER SCENARIO STARTED ===");
                break;
                
            case TrainingScenario.HazardQuiz:
                if (quizArea != null) quizArea.SetActive(true);
                UpdateInstructions("Scenario 3/3: Identify all hazardous chemicals");
                UpdateProgress("Progress: Hazard Recognition");
                Debug.Log("=== HAZARD QUIZ SCENARIO STARTED ===");
                break;
                
            case TrainingScenario.Complete:
                ShowCompletionScreen();
                break;
        }
    }
    
    void CheckScenarioCompletion()
    {
        switch (currentScenario)
        {
            case TrainingScenario.PPE:
                if (ppeManager != null && ppeManager.coatWorn && 
                    ppeManager.gogglesWorn && ppeManager.gloveWorn)
                {
                    CompleteCurrentScenario();
                }
                break;
                
            case TrainingScenario.FireExtinguisher:
                // Check if fire is extinguished
                FireController fire = FindObjectOfType<FireController>();
                if (fire != null && fire.isExtinguished)
                {
                    CompleteCurrentScenario();
                }
                break;
                
            case TrainingScenario.HazardQuiz:
                // Check if quiz is complete
                if (hazardQuiz != null && hazardQuiz.currentQuestionIndex >= hazardQuiz.questions.Length)
                {
                    CompleteCurrentScenario();
                }
                break;
        }
    }
    
    void CompleteCurrentScenario()
    {
        if (scenarioCompleted) return; // Already completed, don't run again
        
        scenarioCompleted = true; // Mark as completed
        
        Debug.Log($"✅ {currentScenario} scenario complete!");
        
        // Add score for this scenario (only once)
        AddScoreForScenario(currentScenario);
        
        // Show completion message
        string completionMessage = "";
        switch (currentScenario)
        {
            case TrainingScenario.PPE:
                completionMessage = "✓ PPE Complete! Moving to Fire Safety...";
                break;
            case TrainingScenario.FireExtinguisher:
                completionMessage = "✓ Fire Safety Complete! Moving to Hazard Quiz...";
                break;
            case TrainingScenario.HazardQuiz:
                completionMessage = "✓ Quiz Complete! Finishing training...";
                break;
        }
        
        UpdateInstructions(completionMessage);
        
        // Wait 3 seconds before moving to next scenario
        Invoke("NextScenario", 3f);
    }
    
    void AddScoreForScenario(TrainingScenario scenario)
    {
        int scoreToAdd = 0;
        
        switch (scenario)
        {
            case TrainingScenario.PPE:
                if (!hasAddedPPEScore)
                {
                    scoreToAdd = 40;
                    hasAddedPPEScore = true;
                }
                break;
                
            case TrainingScenario.FireExtinguisher:
                if (!hasAddedFireScore)
                {
                    scoreToAdd = 50;
                    hasAddedFireScore = true;
                }
                break;
                
            case TrainingScenario.HazardQuiz:
                if (!hasAddedQuizScore)
                {
                    scoreToAdd = 70;
                    hasAddedQuizScore = true;
                }
                break;
        }
        
        if (scoreToAdd > 0)
        {
            totalScore += scoreToAdd;
            Debug.Log($"Added {scoreToAdd} points. Total: {totalScore}");
        }
    }
    
    void NextScenario()
    {
        // Move to next scenario
        currentScenario++;
        
        if (currentScenario > TrainingScenario.Complete)
        {
            currentScenario = TrainingScenario.Complete;
        }
        
        Debug.Log($"Moving to next scenario: {currentScenario}");
        StartScenario(currentScenario);
    }
    
    void UpdateInstructions(string text)
    {
        if (instructionText != null)
        {
            instructionText.text = text;
        }
        Debug.Log("Instructions: " + text);
    }
    
    void UpdateProgress(string text)
    {
        if (progressText != null)
        {
            progressText.text = text;
        }
        Debug.Log("Progress: " + text);
    }
    
    void ShowCompletionScreen()
    {
        Debug.Log("🎉 === TRAINING COMPLETE === 🎉");
        
        UpdateInstructions("🎉 TRAINING COMPLETE! 🎉");
        
        string progressMessage = $"All scenarios finished!\nTotal Score: {totalScore}/160";
        UpdateProgress(progressMessage);
        
        Debug.Log($"Final Score: {totalScore}/160");
        
        // Disable all scenario areas
        if (ppeArea != null) ppeArea.SetActive(false);
        if (fireArea != null) fireArea.SetActive(false);
        if (quizArea != null) quizArea.SetActive(false);
        
        // Return to menu after delay
        Invoke("ReturnToMenu", 5f);
    }
    
    void ReturnToMenu()
    {
        Debug.Log("Returning to main menu...");
        SceneManager.LoadScene("MainMenu");
    }
}
