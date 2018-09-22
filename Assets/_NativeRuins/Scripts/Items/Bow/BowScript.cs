using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour {

    public Rigidbody m_Arrow;       // Prefab of the arrow.  
    public AudioSource m_ShootingAudio;     // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_BendingClip;         // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;            // Audio that plays when each shot is fired.

    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxBendTime = 0.75f;         // How long the shell can charge for before it is fired at max force.

    private GameObject judy;

    // Use this for initialization
    void Start() {
        judy = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        // ?
    }

    public void PlayZoomSound() {
        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_BendingClip;
        m_ShootingAudio.Play();
    }

    // -----------------------------------------------------------------------------
    // targetDirection : Direction toward the target (mouse position in the World)
    // -----------------------------------------------------------------------------
    public void FireArrow(Vector3 targetDirection) {
        // Fire the arrow
        GameObject fleche = GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D");
        fleche.SetActive(false);
        Rigidbody arrowInstance = Instantiate(m_Arrow, fleche.transform.position + targetDirection.normalized * 2, fleche.transform.rotation) as Rigidbody;
        arrowInstance.velocity = 150f * targetDirection.normalized;
        arrowInstance.GetComponent<ArrowSwitch>().enabled = true;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();


        // unbend the string
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D/BowRig_tex/Root/String").GetComponent<BowString>().Release();

        // ?
        InventoryManager.DrawArrow();

    }

    public IEnumerator DrawArrow() {
        bool hasArrowLeft = InventoryManager.hasArrowLeft();
        if (hasArrowLeft)
        {
            // SetActive the arrow
            GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigArmRight1/RigArmRight2/RigArmRight3/Arrow3D").SetActive(true);
        }
        yield return new WaitForSeconds(0.80f);

        // Make the string bend
        GameObject.Find("SportyGirl/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigArmLeft1/RigArmLeft2/RigArmLeft3/Bow3D/BowRig_tex/Root/String").GetComponent<BowString>().Stretch();

        yield return new WaitForSeconds(0.05f);
        judy.GetComponent<MovementControllerHuman>().SetIsReloading(false);
    }
}
