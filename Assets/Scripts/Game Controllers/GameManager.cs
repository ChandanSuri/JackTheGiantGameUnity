using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [HideInInspector]
    public bool gameStartedFromMainMenu, gameRestartedAfterPlayerDied;

    [HideInInspector]
    public int score, coinScore, lifeScore;

    void Awake () {
        MakeSingleton();
	}

    void Start()
    {
        InitializeVariables();
    }
    void OnLevelWasLoaded()//whenevr a level is loaded, level = window
    {
        if(Application.loadedLevelName == "Gameplay")
        {
            if (gameRestartedAfterPlayerDied)
            {
                GameplayController.instance.SetScore(score);
                GameplayController.instance.SetLifeScore(lifeScore);
                GameplayController.instance.SetCoinScore(coinScore);

                PlayerScore.scoreCount = score;
                PlayerScore.coinCount = coinScore;
                PlayerScore.lifeCount = lifeScore;

            }else if (gameStartedFromMainMenu)
            {
                PlayerScore.scoreCount = 0;
                PlayerScore.coinCount = 0;
                PlayerScore.lifeCount = 2;

                GameplayController.instance.SetScore(0);
                GameplayController.instance.SetLifeScore(2);
                GameplayController.instance.SetCoinScore(0);
            }
        }
    }

    void InitializeVariables()
    {
        if(!PlayerPrefs.HasKey("Game Initialized"))
        {
            GamePreferences.SetEasyDifficultyState(0);
            GamePreferences.SetEasyDifficultyHighscore(0);
            GamePreferences.SetEasyDifficultyCoinScore(0);

            GamePreferences.SetMediumDifficultyState(1);//initial difficulty
            GamePreferences.SetMediumDifficultyHighscore(0);
            GamePreferences.SetMediumDifficultyCoinScore(0);

            GamePreferences.SetHardDifficultyState(0);
            GamePreferences.SetHardDifficultyHighscore(0);
            GamePreferences.SetHardDifficultyCoinScore(0);

            GamePreferences.SetMusicState(0);

            PlayerPrefs.SetInt("Game Initialized", 123);//saving the key for future use
        }
    }
	//We make here a singleton so that we can have the Game Manager with us in all the windows
    //If we create an instance, then that's not possible.
    // A singleton does not allow us to have multiple copies of the same GameObject (singleton pattern)
    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
    }

    public void CheckGameStatus(int score, int coinScore, int lifeScore)
    {
        if (lifeScore < 0)
        {
            if (GamePreferences.GetEasyDifficultyState() == 1)
            {
                int highScore = GamePreferences.GetEasyDifficultyHighscore();
                int coinHighScore = GamePreferences.GetEasyDifficultyCoinScore();
                if (highScore < score)
                {
                    GamePreferences.SetEasyDifficultyHighscore(score);
                }
                if (coinHighScore < coinScore)
                {
                    GamePreferences.SetEasyDifficultyCoinScore(coinScore);
                }
            }

            if (GamePreferences.GetMediumDifficultyState() == 1)
            {
                int highScore = GamePreferences.GetMediumDifficultyHighscore();
                int coinHighScore = GamePreferences.GetMediumDifficultyCoinScore();
                if (highScore < score)
                {
                    GamePreferences.SetMediumDifficultyHighscore(score);
                }
                if (coinHighScore < coinScore)
                {
                    GamePreferences.SetMediumDifficultyCoinScore(coinScore);
                }
            }

            if (GamePreferences.GetHardDifficultyState() == 1)
            {
                int highScore = GamePreferences.GetHardDifficultyHighscore();
                int coinHighScore = GamePreferences.GetHardDifficultyCoinScore();
                if (highScore < score)
                {
                    GamePreferences.SetHardDifficultyHighscore(score);
                }
                if (coinHighScore < coinScore)
                {
                    GamePreferences.SetHardDifficultyCoinScore(coinScore);
                }
            }

            gameStartedFromMainMenu = false;
            gameRestartedAfterPlayerDied = false;

            GameplayController.instance.GameOverShowPanel(score, coinScore);
        }else
        {
            this.score = score;
            this.coinScore = coinScore;
            this.lifeScore = lifeScore;

            gameStartedFromMainMenu = false;
            gameRestartedAfterPlayerDied = true;
            GameplayController.instance.PlayerDiedRestartTheGame();
        }
    }
}//Game Manager
