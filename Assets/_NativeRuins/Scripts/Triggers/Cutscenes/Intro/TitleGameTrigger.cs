using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleGameTrigger : Trigger
{
    /*
    public override void Fire()
    {
        
        Debug.Log(cutscenePhases.Length);
        Debug.Log((cutscenePhases[0] as Phase).Dialogue.sentences[0]);
        base.Activate();
    }*/

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Sprite[] spriteCredits;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Animator animator;

    public override void Fire() {
        // Setting up
        /*
        GameObject.Find("ThirdCutSceneCamera").GetComponent<Camera>().enabled = false;
        GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
        cameraCutScene = GameObject.Find("ForthCutSceneCamera").GetComponent<Camera>();
        GameObject.Find("ForthCutSceneCamera").GetComponent<AudioListener>().enabled = true;
        cameraCutScene.enabled = true;
        songs = GameObject.Find("Songs/Others");
        songs.SetActive(false);*/

        if (animator != null)
        {
            animator.SetTrigger("StartAnimation");
        }

        // Start the music
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Execute the desired action
        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp() {
        
        yield return new WaitForSeconds(10f);
        // Credit 1
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[0];
        yield return new WaitForSeconds(6f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Credit 2
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[1];
        yield return new WaitForSeconds(6f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Credit 3
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[2];
        yield return new WaitForSeconds(6f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Credit 4
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[3];
        yield return new WaitForSeconds(4f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Credit 5
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[4];
        yield return new WaitForSeconds(6f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Credit 6
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[5];
        yield return new WaitForSeconds(6f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Present
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[6];
        yield return new WaitForSeconds(5f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(3f);

        // Active the title
        image.color = new Color(255, 255, 225, 255);
        image.sprite = spriteCredits[7];
        yield return new WaitForSeconds(6f);
        image.color = new Color(255, 255, 225, 0);
        yield return new WaitForSeconds(2f);

        // End CutScene
        yield return new WaitForSeconds(3f);
        //StopCutScene();

        // Setting the game player
        /*
        GameObject.Find("Player").GetComponent<FormsController>().enabled = true;
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().FinIntro();
        GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>().enabled = true;

        // Set back songs of the game
        songs.SetActive(true);

        // Disable audio listener
        GetComponent<AudioListener>().enabled = false;

        sauvegarde.EnableUI();

        yield return new WaitForEndOfFrame();
        DialogueTrigger.TriggerDialogueInstructions(null);*/
    }
}
