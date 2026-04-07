using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] int maxHP = 3;
    [SerializeField] AspectRatioFitter lives;
    [SerializeField] GameObject deadText;
    [SerializeField] Sprite spriteNormal;
    [SerializeField] Sprite spriteHit;
    [SerializeField] float hitDuration = 0.5f;
    
    bool alive = true;

    public DodgerAttributes attributes;

    Rigidbody2D body;
    SpriteRenderer sr;
    private bool fireballing;

    readonly List<Action> onDeathEvent = new();

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        SetSprite(spriteNormal);

        //if (attributes == null) attributes = new(maxHP);
        if (attributes.GetMaxHP() != maxHP) attributes = new(maxHP);

        lives.transform.parent.GetComponent<AspectRatioFitter>().aspectRatio = maxHP;
        UpdateHealthbar();
    }

    void Update()
    {
        if (!alive) return;

        //Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();
        //Physics2D.gravity = new Vector2(acceleration.x, acceleration.y);

        if (InputManager.IsPressing(out Vector2 movePoint))
        {
            float xDiff = movePoint.x - transform.position.x;
            if (Mathf.Abs(xDiff) < 0.1f) body.linearVelocityX = 0;
            else if (xDiff > 0) body.linearVelocityX = moveSpeed;
            else if (xDiff < 0) body.linearVelocityX = -moveSpeed;
        }
        else
        {
            body.linearVelocityX = 0;
        }

        if (InputManager.swipeUp) Fireball();

        float viewportPosX = Camera.main.WorldToViewportPoint(transform.position).x;
        if (viewportPosX > 1 && body.linearVelocityX > 0) body.linearVelocityX = 0;
        if (viewportPosX < 0 && body.linearVelocityX < 0) body.linearVelocityX = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && alive)
        {
            var xDiff = collision.transform.position.x - transform.position.x;
            collision.rigidbody.linearVelocity = new Vector2(xDiff * 10, 4);
            Destroy(collision.collider);

            if (!fireballing && attributes.TakeDamage(1))
            {
                transform.Rotate(new Vector3(0, 0, 90));
                alive = false;
                foreach (Action action in onDeathEvent) { action(); }
            }
            else
            {
                //Handheld.Vibrate();

                //int time = 1f - 1f * (attributes.GetHP() / attributes.GetMaxHP());
                Vibes.Vibrate(100);

                SetSprite(spriteHit);
                StartCoroutine(SetSpriteDelay(spriteNormal, hitDuration));
            }

            UpdateHealthbar();
        }
    }

    public void SubscribeDeathEvent(Action action)
    {
        onDeathEvent.Add(action);
    }

    public void Fireball()
    {
        if (fireballing) return;

        if (alive)
        {
            body.linearVelocityY = 8;
            body.mass *= 20;

            fireballing = true;
            transform.GetChild(0).gameObject.SetActive(fireballing);
            StartCoroutine(Chill(1.0f));
        }
    }

    IEnumerator Chill(float delay)
    {
        body.mass /= 20;

        if (delay > 0) yield return new WaitForSeconds(delay);
        fireballing = false;
        transform.GetChild(0).gameObject.SetActive(fireballing);
    }

    private void UpdateHealthbar()
    {
        int hp = attributes.GetHP();
        if (hp == 0) deadText.SetActive(true);
        lives.aspectRatio = hp;
    }

    void SetSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }

    static float RemapRange(float value, float min, float max, float newMin, float newMax)
    {
        float t = Mathf.InverseLerp(value, min, max);
        return Mathf.Lerp(newMin, newMax, t);
    }

    IEnumerator SetSpriteDelay(Sprite sprite, float delay)
    {
        if (delay > 0) yield return new WaitForSeconds(delay);
        SetSprite(sprite);
    }

    public PlayerData GetSaveData()
    {
        PlayerData data = new PlayerData();
        data.attributes = attributes;
        data.alive = alive;

        data.position = GetComponent<Rigidbody2D>().position;
        return data;
    }

    public void LoadSaveData(PlayerData data)
    {
        attributes = data.attributes;
        alive = data.alive;

        GetComponent<Rigidbody2D>().position = data.position;
        UpdateHealthbar();
    }
}

[Serializable]
public struct PlayerData
{
    public DodgerAttributes attributes;
    public bool alive;

    public Vector3 position;
}

//    File.WriteAllText(k_ProjectSettingsPath, JsonUtility.ToJson(this));