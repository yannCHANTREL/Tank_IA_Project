using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_LaunchForce = 20f;           // The force given to the shell.


        private string m_FireButton;                // The input axis that is used for launching shells.


        private void OnEnable()
        {

        }


        private void Start ()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;
        }


        private void Update ()
        {
            // Otherwise, if the fire button has just started being pressed...
            if (Input.GetButtonDown (m_FireButton))
            {
                // ... reset the fired flag and reset the launch force.
                Fire();
            }
        }


        private void Fire ()
        {
            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; 

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();
        }
    }
}