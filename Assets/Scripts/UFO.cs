using UnityEngine;

public class UFO : Invader
{
	[SerializeField] private float _speed = 0.1f;
	[SerializeField] private float _destroyCoordinate = 7.0f;
	
	private InvadersManager _invadersManager;
	
	private void Awake()
	{
		_invadersManager = GameObject.FindWithTag("InvadersManager").GetComponent<InvadersManager>();
	}
	
	private void FixedUpdate()
	{
		Move();
		DestroyUfo();
	}
	
	private void Move()
	{
		if(!_invadersManager.PlayerDiedPause)
			transform.position += new Vector3(_speed, 0, 0);
	}
	
	private void DestroyUfo()
	{
		if(transform.position.x > _destroyCoordinate)
			Destroy(gameObject);
	}
}
