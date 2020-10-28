using UnityEngine;
using UnityEngine.Events;
using System;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float _moveSpeed = 0.2f;
	
	private Rigidbody2D _rigidbody;
	private GameManager _gameManager;
	
	private UnityEventInt EnemyDieEvent;
	
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
		_gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
		EnemyDieEvent = new UnityEventInt();
		EnemyDieEvent.AddListener(_gameManager.UpdateScore);
    }
	
    private void FixedUpdate()
    {
        Move();
    }
	
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
			Player player = collider.gameObject.GetComponent<Player>();
			player.PlayerDieEvent.Invoke();
		}
	    else if(collider.gameObject.tag == "Invader")
				{
					Invader invader = collider.gameObject.GetComponent<Invader>();
					EnemyDieEvent.Invoke(invader.Points);
					Destroy(collider.gameObject);
				}
		else if(collider.gameObject.tag == "BulletDestroyCollider")
		{
			Destroy(gameObject);
		}
		else
		{
			BunkerSprite bs = collider.gameObject.GetComponent<BunkerSprite>();
			bs.ChangeSprite();
		}
		Destroy(gameObject);
	}

	private void Move()
	{
		_rigidbody.MovePosition(new Vector2(_rigidbody.position.x, transform.position.y + _moveSpeed));
	}
}
