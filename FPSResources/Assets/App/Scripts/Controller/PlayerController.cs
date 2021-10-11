using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float speed = 5;
    public float rSpeed = 10.0f;
    public PlayerWeapon weapon;
    public Rigidbody rigid;

    UIGameController input;
    Vector3 euler;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance = this;
        input = UIGameController.Instance;
        euler = this.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null || GameManager.IsGameOver()) return;
        if (input.Movement != Vector2.zero)
            UpdateMovememt(input.Movement.x, input.Movement.y);
        if (input.Rotate != Vector2.zero)
            UpdateRotation(input.Rotate.x, input.Rotate.y);
        Shot(input.IsShot);
    }

    void UpdateMovememt(float v, float h)
    {
        var deltaTime = Time.deltaTime;
        var fwd = transform.forward;
        var right = transform.right;
        fwd.y = 0;
        right.y = 0;
        var position = transform.position;
        if (h != 0)
            position += fwd * (h * speed * deltaTime);
        if (v != 0)
            position += right * (v * speed * deltaTime);

        //this.transform.position = position;
        rigid.MovePosition(position); 
    }

    void UpdateRotation(float v, float h)
    {
        var deltaTime = Time.deltaTime;
        euler.x -= h * rSpeed * deltaTime;
        euler.y += v * rSpeed * deltaTime;
        euler.z = 0;
        transform.eulerAngles = euler;
    }

    void Shot(bool active)
    {
        weapon.IsFireButtonDown = active;
    }

    public static void EnablePlayer(bool active)
    {
        if (Instance == null) return;
        Instance.gameObject.SetActive(active);
    }
}
