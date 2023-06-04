using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// combat control properties
	public float walkSpeed = 10f;
	public float runSpeed = 16f;
	public float turnSpeed = 2f;
	public float upDownRange = 30.0f; // how far the player can look vertically
	public int jumpHeight = 5;

	// health bar control:
	public RectTransform healthBar;
	public RectTransform xpBar;
	public Text levelText;
	public GameObject gameOverPane;
	public GameObject canvas;
	public GameObject sword;

	float maxWidth;
	float currentHealth;
	float currentXP;
	int currentLevel;

	bool stunned = false;
	bool gameOver = false;
	bool jumping = false;
	bool isBlocking = false;
	bool isAttacking = false;

	// components:
	GameObject weapon;
	CharacterController controller;
	Animator anim;

	// movement related properties:
	Vector3 movement;
	float y = 0; // mouse y input (needs to be stored for camera control)

	void Awake() {
		controller = GetComponent<CharacterController> ();
		weapon = GameObject.FindGameObjectWithTag ("PlayerSword");
		anim = weapon.GetComponent<Animator> ();
		GameObject.FindGameObjectWithTag ("GameOverPanel").SetActive (false);
		// Screen.lockCursor = true; // to hide the cursor!
	}

	void Start() {
		currentHealth = (float) Statistics.GetValue("Player", "health");
		currentXP = 0;
		maxWidth = healthBar.rect.width;
		currentLevel = 0;
		LevelUp ();
	}

	void Update () {
		if (gameOver) {
			transform.position = new Vector3 (
				transform.position.x, 
				transform.position.y * 0.98f, 
				transform.position.z);
			GameObject torch = GameObject.FindGameObjectWithTag ("Torch");
			if (torch != null) {
				GameObject.Destroy (torch);
				GameObject.Destroy (GameObject.FindGameObjectWithTag ("PlayerSword"));
				gameOverPane.SetActive (true);
			}
		} else {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				canvas.GetComponent<OptionsMenu>().Toggle();
			} else if (!canvas.GetComponent<OptionsMenu>().IsShown ()) {
				// Rotation:
				float x = Input.GetAxis ("Mouse X") * turnSpeed;
				transform.Rotate (0f, x, 0f);

				y -= Input.GetAxis ("Mouse Y") * turnSpeed;
				y = Mathf.Clamp (y, -upDownRange, upDownRange);
				Quaternion rotation = Quaternion.Euler (y, 0, 0);
				Camera.main.transform.localRotation = rotation;

				UpdateMovement ();
				UpdateCombat ();
			}
		}
	}

	void UpdateMovement() {
		float moveSpeed = 0;

		if (Input.GetKey (KeyCode.Space) && !jumping) {
			jumping = true;
		} else if (Input.GetKey (KeyCode.LeftShift) && !isBlocking && !jumping) {
			moveSpeed = runSpeed;
		} else {
			//controller.Move (new Vector3(0, jumpHeight * Time.deltaTime, 0));
			moveSpeed = walkSpeed;
		}
		jumping = false;			
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");
		movement.Set (h, 0f, v);
		movement = transform.rotation * movement * moveSpeed;
		controller.SimpleMove (movement);
	}
	
	void UpdateCombat() {
		if (Input.GetButton ("Fire1")) {
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("SwingSword")) {
				if (!isAttacking) {
					anim.SetTrigger ("SwingSword");
					sword.GetComponent<AudioSource>().Play();
					isAttacking = true;
				}
			}
		} else if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("SwingSword")) {
			isAttacking = false;
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Stunned")) 
				stunned = false;

			if (Input.GetButton ("Fire2") && !stunned) {
				anim.SetBool ("Block", true);
				isBlocking = true;
			} else {
				anim.SetBool ("Block", false);
				isBlocking = false;
			}
		} 
	}

	void GameOver() {
		gameOver = true;
	}

	public bool IsAttacking() {
		return isAttacking;
	}

	public void SetAttacking(bool isAttacking) {
		this.isAttacking = isAttacking;
	}

	void UpdateHealthBar() {
		float maxHealth = (float) Statistics.GetValue("Player", "health");
		float scale = currentHealth / maxHealth;
		float newWidth = Mathf.Abs(scale * maxWidth);
		healthBar.sizeDelta = new Vector2 (newWidth, healthBar.rect.height);
	}

	void UpdateXPBar() {
		float maxXP = (float) Statistics.GetValue("Player", "xpNeeded");
		float scale = currentXP / maxXP;
		float newWidth = Mathf.Abs(scale * maxWidth);
		xpBar.sizeDelta = new Vector2 (newWidth, xpBar.rect.height);
	}

	void LevelUp() {
		currentLevel++;
		currentXP = 0;
		float maxXP = (float) Statistics.GetValue("Player", "xpNeeded");
		levelText.text = string.Format("Level: {0} ({1}/{2})", currentLevel, currentXP, maxXP);

		if (currentLevel > 1) {
			// increase stats
			Statistics.PlayerLevelUp (currentLevel);

			// put health back to full
			currentHealth = (float)Statistics.GetValue ("Player", "health");
			UpdateHealthBar ();
		}
	}

	public void PickUp(string pickUp) {
		switch (pickUp) {
		case "WeaponPickUp":
			//Debug.Log("Item Picked Up!"); // should go to UI
			//playerController.EquipWeapon();
			break;
		case "XPPickUp" :
			currentXP += (float) Statistics.GetValue("XPPickUp", "value");
			float maxXP = (float) Statistics.GetValue("Player", "xpNeeded");
			if (currentXP > maxXP) {
				LevelUp();
			} else {
				levelText.text = string.Format("Level: {0} ({1}/{2})", currentLevel, currentXP, maxXP);
			}
			UpdateXPBar();
			break;
		case "HealthPickUp" :
			currentHealth += (float) Statistics.GetValue("HealthPickUp", "value");
			float maxHealth = (float) Statistics.GetValue("Player", "health");
			if (currentHealth > maxHealth)
				currentHealth = maxHealth;
			UpdateHealthBar();
			break;
		}
	}

	public bool IsBlocking() {
		return isBlocking;
	}

	
	// whether successful or not
	public bool Hit(string source) {
		if (isBlocking && !stunned) {
			anim.SetBool ("Block", false);
			stunned = true;
			anim.SetTrigger ("Stunned");
			isBlocking = false;
			return false;
		}
		int damage = (int) Statistics.CalculateDamage (source, "Player");
		Debug.Log (damage);
		currentHealth -= damage;
		if (currentHealth <= 0) {
			currentHealth = 0;
			GameOver();
		} 
		UpdateHealthBar ();
		return true;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "SkeletonSword") {
			SkeletonCombat otherCombat = other.GetComponentInParent<SkeletonCombat> ();
			if (otherCombat.IsCombatEnabled () && otherCombat.IsAttacking ()) {
				bool successful = Hit ("Skeleton");
				if (!successful) {
					otherCombat.PlayAnimation ("Knockback");
				}
			}
		} else if (other.tag == "GateKeeperSword") {
			Debug.Log (other.tag);
			GateKeeperCombat otherCombat = other.GetComponentInParent<GateKeeperCombat> ();
			if (otherCombat.IsCombatEnabled () && otherCombat.IsAttacking ()) {
				bool successful = Hit ("GateKeeper");
				if (!successful) {
					otherCombat.PlayAnimation ("Knockback");
				}
			}
		}
	}
}
