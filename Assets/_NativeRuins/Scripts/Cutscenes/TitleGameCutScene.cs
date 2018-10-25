using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGameCutScene : CutScene
{

    new void Awake()
    {
        base.Awake();

        CreateDialogues();
    }

    #region Dialogue init
    private void CreateDialogues()
    {
        // Create all the phases
        /*
        cutscenePhases = new Phase[]
        {
            new Phase(new Dialogue() {
                name = "Judy",
                sentences = new string[] {
                    ".........           ",
                    "  ... aaah..  aaah.... ma tête...",
                }
            }),
            new Phase(new Dialogue() {
                name = "Judy",
                sentences = new string[] {
                    ".........",
                    "Ou suis-je... ?",
                    "Que m'est-il arrivé... ?",
                }
            }),
            new Phase(new Dialogue() {
                name = "Judy",
                sentences = new string[] {
                    ".........",
                    " Aie... J'ai mal partout...",
                }
            }),
            new Phase(new Dialogue() {
                name = "Judy",
                sentences = new string[] {
                    "Je suis sur une île ?! Mais comment c'est possible ? Je ne me souviens de rien..",
                    "Elle ne m'a pas l'air très habitée..",
                }
            }),
            new Phase(new Dialogue() {
                name = "Judy",
                sentences = new string[] {
                    "Quel-est ce cauchemard !",
                    "Comment vais-je partir d'ici ? ... Mmmmmhhhh ...",
                }
            }),
            new Phase(new Dialogue() {
                name = "Judy",
                sentences = new string[] {
                    "Je sais ! Et si je construisais un radeau !",
                    "Bon ne nous emballons pas trop...  Commençons par explorer cette plage !",
                }
            })
        };*/
        //Debug.Log(cutscenePhases.Length);
    }
    #endregion
    /*
    public override void Activate()
    {
        
        Debug.Log(cutscenePhases.Length);
        Debug.Log((cutscenePhases[0] as Phase).Dialogue.sentences[0]);
        base.Activate();
    }*/

    /*
    public AnimationClip cutScene;
    public AudioClip cutSceneMusic;
    private GameObject songs;
    [SerializeField]
    private Sauvegarde sauvegarde;

    protected override void ActivateSwitch() {
        // Setting up
        GameObject.Find("ThirdCutSceneCamera").GetComponent<Camera>().enabled = false;
        GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
        cameraCutScene = GameObject.Find("ForthCutSceneCamera").GetComponent<Camera>();
        GameObject.Find("ForthCutSceneCamera").GetComponent<AudioListener>().enabled = true;
        cameraCutScene.enabled = true;
        songs = GameObject.Find("Songs/Others");
        songs.SetActive(false);

        // Execute the desired action
        StartCoroutine("StartStandUp");
    }

    // Update is called once per frame
    IEnumerator StartStandUp() {
        cameraCutScene.GetComponent<Animator>().Play("Title");
        if (cutSceneMusic != null) {
            cameraCutScene.GetComponent<AudioSource>().clip = cutSceneMusic;
            cameraCutScene.GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(10f);
        // Credit 1
        GameObject.Find("Affichages/Introduction/Credit1").SetActive(true);
        yield return new WaitForSeconds(6f);
        GameObject.Find("Affichages/Introduction/Credit1").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Credit 2
        GameObject.Find("Affichages/Introduction/Credit2").SetActive(true);
        yield return new WaitForSeconds(6f);
        GameObject.Find("Affichages/Introduction/Credit2").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Credit 3
        GameObject.Find("Affichages/Introduction/Credit3").SetActive(true);
        yield return new WaitForSeconds(6f);
        GameObject.Find("Affichages/Introduction/Credit3").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Credit 4
        GameObject.Find("Affichages/Introduction/Credit4").SetActive(true);
        yield return new WaitForSeconds(4f);
        GameObject.Find("Affichages/Introduction/Credit4").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Credit 5
        GameObject.Find("Affichages/Introduction/Credit5").SetActive(true);
        yield return new WaitForSeconds(6f);
        GameObject.Find("Affichages/Introduction/Credit5").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Credit 6
        GameObject.Find("Affichages/Introduction/Credit6").SetActive(true);
        yield return new WaitForSeconds(6f);
        GameObject.Find("Affichages/Introduction/Credit6").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Present
        GameObject.Find("Affichages/Introduction/Present").SetActive(true);
        yield return new WaitForSeconds(5f);
        GameObject.Find("Affichages/Introduction/Present").SetActive(false);
        yield return new WaitForSeconds(3f);

        // Active the title
        GameObject.Find("Affichages/Introduction/Titre").SetActive(true);
        yield return new WaitForSeconds(6f);
        GameObject.Find("Affichages/Introduction/Titre").SetActive(false);
        yield return new WaitForSeconds(2f);

        // End CutScene
        yield return new WaitForSeconds(3f);
        StopCutScene();

        // Setting the game player
        GameObject.Find("Player").GetComponent<FormsController>().enabled = true;
        GameObject.FindWithTag("Player").GetComponent<ActionsNew>().FinIntro();
        GameObject.FindWithTag("MainCamera").GetComponent<AudioListener>().enabled = true;

        // Set back songs of the game
        songs.SetActive(true);

        // Disable audio listener
        GetComponent<AudioListener>().enabled = false;

        sauvegarde.EnableUI();

        yield return new WaitForEndOfFrame();
        DialogueTrigger.TriggerDialogueInstructions(null);
    }*/
}
