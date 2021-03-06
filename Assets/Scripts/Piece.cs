﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int damage;
    public int armor;
    public int health;
    public int healthMax;

    public int moveRange;
    public int attackRange;
    public bool hasMoved;
    public bool hasAttacked;

    public int playerNumber;

    private GameManager GameManager;
    private UIManager UIManager;
    private AudioManager AudioManager;

    public bool attackable;

    public GameObject deadParticle;

    public GameObject cell;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UIManager = GameObject.Find("UICanvas").GetComponent<UIManager>();
        AudioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        SetStandCell(true);
        health = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        Debug.Log(" this is a " + tag + " player:" + playerNumber);
        
        if (playerNumber == GameManager.mainPlayer)
        {

            AudioManager.audioSelect.Play();
            if (!hasMoved)
            {
                GameManager.selected = gameObject;
                GameManager.ShowMoveRange();
            }
            else if (!hasAttacked)
            {
                GameManager.selected = gameObject;
                GameManager.ShowAttackRange();
            }
        }
        else if (attackable)
        {
            AudioManager.audioAttack.Play();
            ApplyDamage(GameManager.selected.GetComponent<Piece>().damage);
            GameManager.selected.GetComponent<Piece>().hasAttacked = true;
            GameManager.CloseAllRanges();
            attackable = false;
        }
        else if (playerNumber == GameManager.nextPlayer)
        {
            AudioManager.audioClick.Play();
        }
        
        UIManager.ShowInfo(gameObject.GetComponent<Piece>());
        
    }
    //public void Attack()
    //{

    //}
    public void ApplyDamage(int _damage)
    {
        health -= (_damage - armor);
        UIManager.ShowInfo(gameObject.GetComponent<Piece>());
        Debug.Log(" apply damage:"+_damage+" health = "+health);
        if (health <= 0)
        {
            SetStandCell(false);
            Instantiate(deadParticle, transform.position, Quaternion.identity);
            GameManager.PieceDestroy(gameObject);
        }

    }
    public void Move(float _x, float _y)
    {
        
        SetStandCell(false);
        transform.position = new Vector3(_x, _y, transform.position.z);
        GameManager.CloseAllRanges();
        SetStandCell(true);
        hasMoved = true;
        if (!hasAttacked)
        {
            GameManager.ShowAttackRange();
        }
    }

    public void SetStandCell(bool _stand)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
        foreach (var hit in hits)
        {
            //Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Cell"))
            {
                if (_stand == true)
                {
                    hit.collider.GetComponent<Cell>().playerNumber = playerNumber;
                    cell = hit.collider.gameObject;
                }
                else
                {
                    hit.collider.GetComponent<Cell>().playerNumber = 0;
                }
                
            }
        }
    }   

    public void ReSet()
    {
        hasMoved = false;
        hasAttacked = false;
        attackable = false;
    }
}
