using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class ActionsNew : MonoBehaviour {

	private Animator animator;

	const int countOfDamageAnimations = 3;
	int lastDamageAnimation = -1;
    private int MovementLayer;
    private int FightLayer;
    private int DamageLayer;

	void Awake () {
		animator = GetComponent<Animator> ();
        MovementLayer = animator.GetLayerIndex("Movement Layer");
        FightLayer = animator.GetLayerIndex("Fight Layer");
        DamageLayer = animator.GetLayerIndex("Damage Layer");
    }

	public void Stay () {
		//animator.SetBool("Aiming", false);
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0f);
        //animator.Play("StandMovement", MovementLayer);
    }

	public void Walk () {
		animator.SetBool("Aiming", false);
        animator.SetBool("Squat", false);
        animator.SetFloat ("Speed", 16.5f);
		//animator.Play ("StandMovement", MovementLayer);
	}

	public void Run () {
		animator.SetBool("Aiming", false);
        animator.SetBool("Squat", false);
        animator.SetFloat ("Speed", 44f);
		//animator.Play ("StandMovement", MovementLayer);
	}

	public void Attack () {
		animator.SetBool ("Squat", false);
		Aiming ();
		animator.Play ("Attack");
	}

	public void Death () {
		//animator.SetBool ("Squat", false);
        Stay();
        animator.SetTrigger("Death");
		//if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Death"))
		//	animator.Play("Idle", 0);
		//else
		//animator.Play ("Death", MovementLayer);
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
		animator.Play ("Damage", DamageLayer);
	}

	public void Jump () {
		animator.SetBool ("Squat", false);
		animator.SetFloat ("Speed", 0.0f);
		//animator.SetBool("Aiming", false);
		//animator.SetBool("EquipWeapon", false);
        //animator.SetTrigger("Jump");
        animator.Play ("JumpMecanics", MovementLayer);
	}

    // Aim with the bow
	public void Aiming () {
		animator.SetBool("Aiming", true);
        animator.Play("BowAimIdle", FightLayer);
    }

    // Release aim with the bow
	public void ReleaseAiming () {
		animator.SetBool("Aiming", false);
        animator.Play("Null", FightLayer);
        //animator.Play("BowAimIdle", 1);
    }

    // Reload the bow
    public void MoveWithBow(float x, float y) {
        animator.SetBool("Aiming", true);
        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);
        animator.SetFloat("Speed", 22f);
        animator.Play("BowMovement", MovementLayer);
    }

    // Move with the bow equipped
    public void Reloading() {
        animator.SetBool("Aiming", true);
        animator.SetTrigger("Reloading");
        animator.Play("BowDrawArrow", FightLayer);
    }

    public void HitWithTorch () {
        animator.SetFloat("Speed_f", 0f);
        animator.SetTrigger("Hit");
    }

	public void Sitting () {
		animator.SetBool ("Squat", true);
        animator.SetFloat("Speed", 16.5f);
        //animator.SetBool("Aiming", false);
        //animator.SetBool("EquipWeapon", false);
        animator.Play("CrouchMovement", MovementLayer);
	}

	public void Wary () {
		animator.SetBool ("Squat", true);
        animator.SetFloat("Speed", 0.0f);
        //animator.SetBool("Aiming", false);
        animator.Play("CrouchMovement", MovementLayer);
	}


	public void CrouchingRun () {
		animator.SetBool ("Squat", true);
        animator.SetFloat("Speed", 44f);
        //animator.SetBool("Aiming", false);
        //animator.SetBool("EquipWeapon", false);
		animator.Play("CrouchMovement", MovementLayer);
	}

	public void EquipWeapon () {
		//animator.SetBool ("Squat", false);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", true);
		animator.Play("EquipArme", FightLayer);
	}

	public void DisarmWeapon () {
		//animator.SetBool ("Squat", false);
		animator.SetBool("Aiming", false);
		animator.SetBool("EquipWeapon", false);
		animator.Play ("DisarmArme", FightLayer);
	}

    // Celebrate (excited)
    public void Celebrate() {
        // TODO LATER
    }

    // Sitting (save the game)
    public void SitDown() {
        // TODO LATER
    }

    // Standing up (leaving save the game)
    public void StandUp() {
        // TODO LATER
    }
}
