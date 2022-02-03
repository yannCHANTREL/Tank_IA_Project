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
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;               // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public GameObject m_TankAIPrefab;           // Reference to the prefab the AI will control.
        
        public int m_TankAmountPerTeam; 

        public List<ListVariable> m_TankVariableList;// Reference to all tank variables SO
        public TeamList m_TeamList;                 // Reference Tank List
        
        public Transform[] m_TeamsSpawn; 
        
        public float m_MaxGameTimeInSeconds; 
        
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private Team m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.

        public CaptureData m_CaptureData;
        
        public Text m_Text;
        private Coroutine m_Coroutine;

        private bool m_IsGameOver;

        public GameOptions m_GameOptions;
        public UIGameManager m_UIGameManager;

        public TeamBehaviorTreeListVariable m_TeamBehaviorTrees;

        private int m_IndexAlgoUsed;
        
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
            m_TeamList.SetTeamSpawn(m_TeamsSpawn);

            ApplyOptionSettings();
                
            ResetTankValues();
            SpawnAllTanks();
            SetCameraTargets();
            
            // Once the tanks have been created and the camera is using them as targets, start the game.
            StartCoroutine (GameLoop ());
        }

        private void ApplyOptionSettings()
        {
            SetTeamType();
            m_TankAmountPerTeam = m_GameOptions.m_NbPlayer;
            m_IndexAlgoUsed = (int) m_GameOptions.m_SearchAlgo;
        } 

        private void SetTeamType()
        {
            switch (m_GameOptions.m_Mode)
            {
                case GameOptions.Mode.PlayerVSAI:
                    m_TeamList.SetOtherTeamsAsAI();
                    break;
                case GameOptions.Mode.AIVSAI:
                    m_TeamList.SetAllTeamAsAI();
                    break;
            }
        }

        private void SpawnAllTanks()
        {
            // For all the team ...
            for (int i = 0; i < m_TeamList.GetNumberTeam(); i++)
            {
                // Is this team an AI
                bool isAITeam = m_TeamList.IsAI(i);
                GameObject tankPrefab = isAITeam ? m_TankAIPrefab : m_TankPrefab;
                
                Transform spawn = m_TeamsSpawn[i % m_TeamsSpawn.Length];
                Color teamColor = m_TeamList.GetColorTeam(i);
                
                AddTeamValue();
                if (isAITeam) { m_TeamBehaviorTrees.m_Value[i] = m_TeamList.m_Teams[i].m_BehaviorTree; }
                
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
                    if (tankIndexManager)
                    {
                        tankIndexManager.m_TankIndex = index;
                        tankIndexManager.m_TeamIndex = i;
                    }

                    if (isAITeam)
                    {
                        NavigationManager navigationManager = tankManager.m_Instance.GetComponent<NavigationManager>();
                        if (navigationManager)
                        {
                            navigationManager.ChooseAAlgorithmMode(m_IndexAlgoUsed);
                        }
                    }

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
            foreach (ListVariable lv in m_TankVariableList) { lv.IncrementTankSize(); }
        }
        private void AddTeamValue()
        {
            foreach (ListVariable lv in m_TankVariableList) { lv.IncrementTeamSize(); }
        }
        private void SetCameraTargets()
        {
            m_CameraControl.m_Targets = m_TeamList.GetAllTanksTransform();
        }

        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop ()
        {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine (GameStarting ());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (GamePlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (RoundEnding());
        }

        private IEnumerator GameStarting ()
        {
            m_TeamList.ResetCaptureScore();
            m_CaptureData.UpdateScoreText();
            
            // As soon as the game starts reset the tanks and make sure they can't move.
            ResetAllTanks ();
            DisableTankControl ();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraControl.SetStartPositionAndSize ();
            m_MessageText.text = string.Empty;
            
            m_IsGameOver = false;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }

        private IEnumerator GamePlaying ()
        {
            // As soon as the game begins playing let the players control the tanks.
            EnableTankControl ();

            m_Coroutine = StartCoroutine(GameTimeCoroutine());

            while (!m_IsGameOver)
            {
                // ... return on the next frame.
                yield return null;
            }

            m_CaptureData.m_IsGameFinished = true;
            StopGameTimeCoroutine();
        }

        private IEnumerator RoundEnding ()
        {
            // Stop tanks from moving.
            DisableTankControl ();

            m_GameWinner = GetTeamMaxScore();

            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage ();
            m_MessageText.text = message;
            
            m_UIGameManager.DisplayEndMenu();

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }
        
        private Team GetTeamMaxScore()
        {
            return m_TeamList.GetTeamMaxScore();
        }
        
        private string EndMessage()
        {
            string message = "NOBODY WON !";

            if (m_GameWinner != null)
                message = m_GameWinner.GetColoredTeamText() + " WINS THE GAME!";
            
            return message;
        }

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

        private void StopGameTimeCoroutine()
        {
            StopCoroutine(m_Coroutine);
            m_Text.gameObject.SetActive(false);
        }

        private IEnumerator GameTimeCoroutine()
        {
            m_Text.gameObject.SetActive(true);
            float counter = m_MaxGameTimeInSeconds;
            float timeToDisplay = 0;
            while (counter > -0.5f)
            {
                counter -= Time.deltaTime;
                timeToDisplay = counter + 0.5f;
                m_Text.text = timeToDisplay.ToString("N0");

                yield return null;
            }

            m_IsGameOver = true;
            m_Text.gameObject.SetActive(false);
        }
    }
}
