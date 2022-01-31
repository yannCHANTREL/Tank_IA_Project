using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public GameObject m_TankAIPrefab;           // Reference to the prefab the AI will control.
        
        public int m_TankAmountPerTeam; 

        public List<ListVariable> m_TankVariableList;// Reference to all tank variables SO
        public VirtualGrid m_ClassGrid;             // Reference Grid
        public TeamList m_TeamList;                 // Reference Tank List
        
        public Transform[] m_TeamsSpawn; 
        
        public float m_MaxRoundTimeInSeconds; 
        
        private int m_RoundNumber;                  // Which round the game is currently on.
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private Team m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private Team m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

        public CaptureData m_CaptureData;
        
        public int m_RoundScoreForWin;
        
        public Text m_Text;
        private Coroutine m_Coroutine;

        private bool m_IsRoundOver;

        public GameOptions m_GameOptions;
        
        const float k_MaxDepenetrationVelocity = float.PositiveInfinity;

        private void Start()
        {
            // This line fixes a change to the physics engine.
            Physics.defaultMaxDepenetrationVelocity = k_MaxDepenetrationVelocity;
            
            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            m_TeamList.EmptyTeamList();
            m_TeamList.GiveTeamNumber();
            m_TeamList.ResetAllScore();

            SetTeamType();
            
            ResetTankValues();
            SpawnAllTanks();
            SetCameraTargets();
            
            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine (GameLoop ());
        }

        private void SetTeamType()
        {
            if (m_GameOptions.m_Mode == GameOptions.Mode.PVP)
            {
                m_TeamList.SetAllTeamAsPlayer();
            }
            else
            {
                m_TeamList.SetOtherTeamsAsAI();
            }
        }

        private void SpawnAllTanks()
        {
            // For all the team ...
            for (int i = 0; i < m_TeamList.GetNumberTeam(); i++)
            {
                // Is this team an AI
                GameObject tankPrefab = m_TeamList.IsAI(i) ? m_TankAIPrefab : m_TankPrefab;
                
                Transform spawn = m_TeamsSpawn[i % m_TeamsSpawn.Length];
                Color teamColor = m_TeamList.GetColorTeam(i);
                
                // ... For the number of player per team ...
                for (int j = 0; j < m_TankAmountPerTeam; j++)
                {
                    // ... create them, set their player number and references needed for control.
                    int index = m_TankAmountPerTeam * i + j;

                    AddTankValue();

                    TankManager tankManager = new TankManager(index + 1, teamColor , spawn)
                    {
                        m_Instance = Instantiate(tankPrefab, spawn.position, spawn.rotation)
                    };
                    
                    m_TeamList.AddTankToTeam(tankManager, i);
                    
                    TankIndexManager tankIndexManager = tankManager.m_Instance.GetComponent<TankIndexManager>();
                    if (tankIndexManager) tankIndexManager.m_TankIndex = index;
                    
                    tankManager.Setup();
                }
            }
        }
        private void ResetTankValues()
        {
            foreach (ListVariable lv in m_TankVariableList) { lv.Reset(); }
        }

        private void AddTankValue()
        {
            foreach (ListVariable lv in m_TankVariableList) { lv.IncrementSize(); }
        }
        private void SetCameraTargets()
        {
            m_CameraControl.m_Targets = m_TeamList.GetAllTanksTransform();
        }

        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop ()
        {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundStarting ());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (RoundEnding());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
            if (m_GameWinner != null)
            {
                // If there is a game winner, restart the level.
                SceneManager.LoadScene (0);
            }
            else
            {
                // If there isn't a winner yet, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine (GameLoop ());
            }
        }

        private IEnumerator RoundStarting ()
        {
            m_TeamList.ResetCaptureScore();
            m_CaptureData.UpdateScoreText();
            
            // As soon as the round starts reset the tanks and make sure they can't move.
            ResetAllTanks ();
            DisableTankControl ();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraControl.SetStartPositionAndSize ();

            // Increment the round number and display text showing the players what round it is.
            m_RoundNumber++;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            m_CaptureData.m_IsRoundStarting = true;
            m_IsRoundOver = false;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }

        private IEnumerator RoundPlaying ()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl ();

            m_Coroutine = StartCoroutine(RoundTimeCoroutine());

            // Clear the text from the screen.
            m_MessageText.text = string.Empty;

            // While there is not one tank left...
            while (!OneTeamObtainedCaptureScore() && !m_IsRoundOver)
            {
                // ... return on the next frame.
                yield return null;
            }

            m_CaptureData.m_IsRoundFinished = true;
            StopRoundTimeCoroutine();
        }

        private IEnumerator RoundEnding ()
        {
            // Stop tanks from moving.
            DisableTankControl ();

            // Clear the winner from the previous round.
            m_RoundWinner = null;

            m_RoundWinner = m_IsRoundOver ? GetTeamRoundMaxScore() : GetRoundWinner();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null)
                m_RoundWinner.m_RoundScore += 1;

            // Now the winner's score has been incremented, see if someone has won the game.
            m_GameWinner = GetGameWinner();

            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage ();
            m_MessageText.text = message;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }

        private bool OneTeamObtainedCaptureScore()
        {
            return m_TeamList.OneTeamObtainedCaptureScore(m_RoundScoreForWin);
        }

        private Team GetRoundWinner()
        {
            return m_TeamList.GetTeamCaptureWinner(m_RoundScoreForWin);
        }

        private Team GetGameWinner()
        {
            return m_TeamList.GetGameWinner(m_NumRoundsToWin);
        }

        private Team GetTeamRoundMaxScore()
        {
            return m_TeamList.GetTeamRoundMaxScore();
        }
        
        // Returns a string message to display at the end of each round.
        private string EndMessage()
        {
            // By default when a round ends there are no winners so the default end message is a draw.
            string message = "DRAW!";

            if (m_RoundWinner != null)
                message = m_RoundWinner.GetColoredTeamText() +
                          " WINS THE ROUND!";
            
            // Add some line breaks after the initial message.
            message += "\n\n\n\n";
            
            message += m_TeamList.GetScores(); 

            if (m_GameWinner != null)
                message = m_GameWinner.GetColoredTeamText() + " WINS THE GAME!";
            
            return message;
        }

        // This function is used to turn all the tanks back on and reset their positions and properties.
        private void ResetAllTanks()
        {
            m_TeamList.ResetAllTank();
        }

        private void EnableTankControl()
        {
            m_TeamList.EnableAllTankControl();
        }

        private void DisableTankControl()
        {
            m_TeamList.DisableAllTankControl();
        }

        private void StopRoundTimeCoroutine()
        {
            StopCoroutine(m_Coroutine);
            m_Text.gameObject.SetActive(false);
        }

        private IEnumerator RoundTimeCoroutine()
        {
            m_Text.gameObject.SetActive(true);
            float counter = m_MaxRoundTimeInSeconds;
            float timeToDisplay = 0;
            while (counter > -0.5f)
            {
                counter -= Time.deltaTime;
                timeToDisplay = counter + 0.5f;
                m_Text.text = timeToDisplay.ToString("N0");

                yield return null;
            }

            m_IsRoundOver = true;
            m_Text.gameObject.SetActive(false);
        }
    }
}
