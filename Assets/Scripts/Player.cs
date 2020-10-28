using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed = 0.3f;
	[SerializeField] private Vector2 _stopCoordinate = new Vector2(-6.2544f, 6.2544f);
	[SerializeField] private Bullet _bulletPrefab = null;
	[SerializeField] private AudioSource _shootSound = null;
	[SerializeField] private AudioSource _explosionSound = null;
	
	private Bullet _createdBullet;
	private Animator _animator;
	private bool _isControllerEnabled = true;
	private GameManager _gameManager;
	private InvadersManager _invadersManager;
	
	[NonSerialized] public UnityEvent PlayerDieEvent;
	
	private void Awake()
	{
		_animator = gameObject.GetComponent<Animator>();
		PlayerDieEvent = new UnityEvent();
		_gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
		PlayerDieEvent.AddListener(Die);
		PlayerDieEvent.AddListener(_gameManager.SubtractLives);
		_invadersManager = GameObject.FindWithTag("InvadersManager").GetComponent<InvadersManager>();
	}
	
    private void FixedUpdate()
    {
		Move();
		Shoot();
    }
	
	private void Move()
	{
		if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && _isControllerEnabled)
		{
			Vector3 newPosition = transform.position + new Vector3(Input.GetAxisRaw("Horizontal") * _speed, 0, 0);
			if(newPosition.x > _stopCoordinate.x && newPosition.x < _stopCoordinate.y)
				transform.position = newPosition;
		}
	}
	
	private void Shoot()
	{
		if(Input.GetButton("Fire1") && _createdBullet == null && _isControllerEnabled)
		{
			_createdBullet = Instantiate(_bulletPrefab, new Vector2(transform.position.x, transform.position.y + 1), transform.rotation);
			_shootSound.Play();
		}
	}
	
	public void Die()
	{
		StartCoroutine(DieAnimationCoroutine());
		_explosionSound.Play();
	}
	
	private IEnumerator DieAnimationCoroutine()
	{
		_animator.SetBool("isDead", true);
		_isControllerEnabled = false;
		_invadersManager.PlayerDiedPause = true;
		yield return new WaitForSeconds(5.0f);
		_animator.SetBool("isDead", false);
		yield return new WaitForSeconds(0.4f);
		transform.position = new Vector3(0, -4.47f, 0);
		_isControllerEnabled = true;
		_invadersManager.PlayerDiedPause = false;
	}
}