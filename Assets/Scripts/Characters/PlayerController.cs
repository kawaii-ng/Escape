using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * player controller to control player behaviour
 */

public class PlayerController : MonoBehaviour
{

    public enum GhostType { 
    
        Blue,
        Yellow,
        Green
    
    }

    public enum PlayerState
    {

        Idle,
        Walk,
        Decoding,
        Using,
        UsingSkill

    }

    [Header ("Player Basic Info")]
    public GhostType ghostType = GhostType.Blue;
    public float speed = 3; // moving speed
    public float decodeSpeed = 1; 
    public float skillCooldown = 10f;
    public float MaxPlayerHealth = 100f;
    
    [Header ("Control Setting")]
    public CharacterController controller;
    public Transform cam;
    public CollisionChecking collisionChecking;
    public float turnSmoothTime = 0.1f;

    private MyPlayerInput playerInput; // new input system
    private VignetteController vignetteController; // limit player sight
    
    // UI component
    private ProgressBar progressBar; 
    private ProgressBar healthBar;
    private CanvasGroup hurtImage; // for hurt effect
    private CanvasGroup skillButton;

    // player component
    private AudioSource[] sounds;
    private Animator anim;

    // player status
    private PlayerState state;
    private float playerHealth;
    private float turnSmoothVelocity;
    private bool isScary;

    void Awake()
    {

        // initialize 
        playerInput = new MyPlayerInput();
        vignetteController = GameObject.FindWithTag("VignetteController").gameObject.GetComponent<VignetteController>();

        // UI
        progressBar = GameObject.FindWithTag("ProgressBar").gameObject.GetComponent<ProgressBar>();
        progressBar.HideProgressBar();
        healthBar = GameObject.FindWithTag("HealthBar").gameObject.GetComponent<ProgressBar>();
        hurtImage = GameObject.FindWithTag("HurtImage").gameObject.GetComponent<CanvasGroup>();
        skillButton = GameObject.FindWithTag("SkillButton").gameObject.GetComponent<CanvasGroup>();

        // Sound and animator
        sounds = GetComponents<AudioSource>();
        anim = GetComponent<Animator>();

        // status
        state = PlayerState.Idle;
        playerHealth = MaxPlayerHealth;
        
        isScary = false;

        // initialize the health bar
        healthBar.SetCurrentProgress(playerHealth, MaxPlayerHealth);
        healthBar.ShowProgressBar();

    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    // for getting the camera
    public void SetUpCam(Camera cam) {
        this.cam = cam.transform;
    }

    void Update() {

        Move();

        if (playerInput.PlayerMain.Skill1.triggered)
            StartCoroutine(TriggerObject());

        if (playerInput.PlayerMain.Skill2.triggered && state != PlayerState.UsingSkill)
            StartCoroutine(UseSkill());

        if (collisionChecking.GetIsEnter()) { 
            if (!isScary)
                StartHeartBeat();
        }
        else 
            StopHeartBeat();

    }

    void OnTriggerEnter(Collider other) {

        // get the light 
        if (other.gameObject.tag == "LightBall")
        {
            vignetteController.ResetIntensity();
            other.gameObject.SetActive(false);
        }
    
    }

    // if first person camera, the head will be removed. otherwise, the head will be existed
    public void ShowHead() {

        transform.GetChild(2).gameObject.SetActive(!CamSwitchController.isFirstPersonCam);

    }

    /**
     * Player Movement
     */

    void Move()
    {

        Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(movementInput.x, 0f, movementInput.y);

        if (movementInput != Vector2.zero)
        {
            float targetAngle = 0; 
            anim.SetBool("isWalking", true);

            // character direction
            if (!CamSwitchController.isFirstPersonCam)
            {
                // first person camera mode
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }
            else { 
                // third person camera mode
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + transform.eulerAngles.y;
            
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }
        else
            anim.SetBool("isWalking", false);

        // fixed the height of the player
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

    }

    /**
     * if enemy approach, player will have a huge heart beat sound
     */

    public void StartHeartBeat()
    {
        isScary = true;
        sounds[0].Play();
    }

    public void StopHeartBeat()
    {
        isScary = false;
        sounds[0].Stop();
    }

    /**
     * play the specific animation
     */

    public void PlayAnimation(string animation)
    {

        anim.SetTrigger("trigger" + animation);

    }

    /**
     * change state to using for triggering object
     */

    IEnumerator TriggerObject()
    {

        state = PlayerState.Using;
        yield return new WaitForSeconds(0.5f);
        state = PlayerState.Idle;
        yield return null;

    }

    /**
     * using skills
     */

    IEnumerator UseSkill() {

        state = PlayerState.UsingSkill;

        skillButton.alpha = 0.5f;

        switch (ghostType)
        {
            case GhostType.Green: // restore health skill
                ChangeHealth(20f, false);
                break;
            case GhostType.Yellow: // increase the lighting skill
                vignetteController.ResetIntensity();
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(skillCooldown);
        state = PlayerState.Idle;
        skillButton.alpha = 1f;
        yield return null;

    }

    /**
     * show and hide progress
     */

    public void ShowProgressDisplay(float progress, float maxProgress)
    {
        progressBar.SetCurrentProgress(progress, maxProgress);
        progressBar.ShowProgressBar();
    }

    public void HideProgressDisplay()
    {
        progressBar.HideProgressBar();
    }

    /**
     * changing health and show damage effect 
     */

    public void ChangeHealth(float changeValue, bool isDamage = true)
    {

        playerHealth += changeValue * (isDamage ? -1f : 1f);
        healthBar.SetCurrentProgress(playerHealth, MaxPlayerHealth);

        // show damage effect
        if (isDamage)
        {
            PlayAnimation("Hurt");
            StartCoroutine(HurtEffect());
            Vibrator.Vibrate(1000);
        }

        // check whether < 0 or > Max Health
        if (playerHealth >= MaxPlayerHealth)
            playerHealth = MaxPlayerHealth;

        if (playerHealth <= 0f)
        {
            playerHealth = 0f;
            GameController.isDead = true; // if player health reaches 0, it will dead
        }

    }

    /**
     * hurt effect: fade in and fade out of the white image and play sound effect
     */

    IEnumerator HurtEffect()
    {

        sounds[1].Play();
        hurtImage.alpha = 0f;
        hurtImage.alpha = 1f;
        yield return new WaitForSeconds(0.1f);
        hurtImage.alpha = 0f;
        yield return new WaitForSeconds(0.1f);
        yield return null;

    }

    /**
     * help check whether it is full health
     */

    public bool IsFullHealth()
    {
        return playerHealth == MaxPlayerHealth ? true : false;
    }


    public float GetHealth() 
    {
        return playerHealth;
    }
    
    /**
     * Set state of the player
     */

    public void SetState(PlayerState state)
    {
        this.state = state;
    }

    /**
     * Get state of the player
     */

    public PlayerState GetState()
    {
        return state;
    }

    /**
     * get decode speed
     */

    public float GetDecodeSpeed()
    {
        return decodeSpeed;
    }

}
