using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class Actions : MonoBehaviour {

	private Animator animator;

	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;

	void Awake () {
		animator = GetComponent<Animator> ();
        PlayerPrefs.DeleteAll();
    }

	public void Stay () {
		animator.SetBool("Aiming", false);
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
		}

	public void Walk () {
		animator.SetBool("Aiming", false);
		animator.SetFloat ("Speed", 16.5f);
		animator.Play ("Walk");
	}

	public void Run () {
		animator.SetBool("Aiming", false);
		animator.SetFloat ("Speed", 44f);
		animator.Play ("Run");
	}

	public void Attack () {
		animator.SetBool ("Squat", false);
		Aiming ();
		animator.Play ("Attack");
	}

	public void Death () {
		//animator.SetBool ("Squat", false);
        Stay();
		//if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Death"))
		//	animator.Play("Idle", 0);
		//else
			animator.Play ("Death");
	}

	public void Damage () {
		animator.SetBool ("Squat", false);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Death")) return;
		int id = Random.Range(0, countOfDamageAnimations);
		if (countOfDamageAnimations > 1)
			while (id == lastDamageAnimation)
				id = Random.Range(0, countOfDamageAnimations);
		lastDamageAnimation = id;
		animator.SetInteger ("DamageID", id);
		animator.Play ("Damage");
	}

	public void Jump () {
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		animator.Play ("Jump");
	}

	public void Aiming () {
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
		animator.SetBool("Aiming", true);
		animator.SetBool("EquipWeapon", false);
	}

	public void Sitting () {
		animator.SetBool ("Squat", true);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		animator.Play ("Sneak");
	}

	public void Wary () {
		animator.SetBool ("Squat", true);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		animator.Play ("Wary");
	}


	public void CrouchingRun () {
		animator.SetBool ("Squat", true);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		animator.Play ("CrouchingRun");
	}

	public void EquipWeapon () {
		animator.SetBool ("Squat", false);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", true);
		animator.Play ("EquipArme");
	}

	public void DisarmWeapon () {
		animator.SetBool ("Squat", false);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		animator.Play ("DisarmArme");
	}

    // Make transition for the following animation :
    // celebrate (excited)
    public void Celebrate()
    {
    }

    // Make transition for the following animation :
    // walking forward with the bow equipped
    public void WalkFowardBow() { 
    }

    // Make transition for the following animation :
    // walking backward with the bow equipped
    public void WalkBackwardBow() {
    }

    // Make transition for the following animation :
    // walking to the left with the bow equipped
    public void WalkLeftBow() {
    }

    // Make transition for the following animation :
    // walking to the right with the bow equipped
    public void WalkRightBow() {
    }

    // Make transition for the following animation :
    // (setting) save the game
    public void SitDown() {
        // TODO LATER
    }

    // Make transition for the following animation :
    // leaving save the game
    public void StandUp()
    {
        // TODO LATER
    }
}
