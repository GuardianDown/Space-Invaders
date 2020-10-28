using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InvadersManager : MonoBehaviour
{
	[SerializeField] private Invader _lowInvaderPrefab = null;
	[SerializeField] private Invader _mediumInvaderPrefab = null;
	[SerializeField] private Invader _highInvaderPrefab = null;
	[SerializeField] private Bullet _invaderBullet = null;
	[SerializeField] private Vector2 _invaderStartCoordinate = new Vector2(-6.0f, 3.0f);
	[SerializeField] private Vector2 _spaceBetweenInvaders = new Vector2(0.9f, 0.9f);
	[SerializeField] private float _pause = 1.0f;
	[SerializeField] private float _pauseChange = 0.05f;
	[SerializeField] private int _movesInOneRow = 20;
	[SerializeField] private Vector2 _moveDistance = new Vector2(0.15f, -0.2f);
	[SerializeField] private int _totalRows = 10;
	[SerializeField] private Vector2 _pauseShooting = new Vector2(5.0f, 20.0f);
	[SerializeField] private GameObject _ufoPrefab = null;
	[SerializeField] private Vector2 _pauseUfoSpawn = new Vector2(30.0f, 90.0f);
	[SerializeField] private Vector2Int _matrixSize = new Vector2Int(11, 5);
	
	private Invader[,] _invadersMatrix = null;
	private int[] _invaderInColumn = null;
	private int _totalRowsCounter;
	private int _movesInOneRowCounter;
	private int _invadersDiedCounter;
	private GameManager _gameManager;
	
	public bool PlayerDiedPause {get; set;}
	
	private UnityEvent AllEnemiesDieEvent;
	private UnityEvent EnemiesReachTargetEvent;
	
	private void Awake()
	{
		_invadersMatrix = new Invader[_matrixSize.x, _matrixSize.y];
		_invaderInColumn = new int[_matrixSize.x];
		EnemiesSpawn();
		_gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
		AllEnemiesDieEvent = new UnityEvent();
		EnemiesReachTargetEvent = new UnityEvent();
		AllEnemiesDieEvent.AddListener(_gameManager.ShowYouWinScene);
		EnemiesReachTargetEvent.AddListener(_gameManager.ShowGameOverScene);
	}
	
	private void Start()
    {
        StartCoroutine(EnemiesMoving());
		StartCoroutine(UfoSpawn());
		StartShooting();
    }
	
	private void Update()
	{
		CheckDestroy();
	}
	
	private void EnemiesSpawn()
	{
		for(int i = 0; i < _matrixSize.x; i++)
		{
			_invaderInColumn[i] = _matrixSize.y - 1;
			for(int j = 0; j < 1; j++)
			{
				_invadersMatrix[i, j] = Instantiate(_highInvaderPrefab, 
													new Vector2(_invaderStartCoordinate.x + i * _spaceBetweenInvaders.x, _invaderStartCoordinate.y - j * _spaceBetweenInvaders.y), 
													_highInvaderPrefab.transform.rotation,
													transform);
			}
			for(int j = 1; j < _matrixSize.y / 2 + 1; j++)
			{
				_invadersMatrix[i, j] = Instantiate(_mediumInvaderPrefab, 
													new Vector2(_invaderStartCoordinate.x + i * _spaceBetweenInvaders.x, _invaderStartCoordinate.y - j * _spaceBetweenInvaders.y), 
													_mediumInvaderPrefab.transform.rotation,
													transform);
			}
			for(int j = _matrixSize.y / 2 + 1; j < _matrixSize.y; j++)
			{
				_invadersMatrix[i, j] = Instantiate(_lowInvaderPrefab, 
													new Vector2(_invaderStartCoordinate.x + i * _spaceBetweenInvaders.x, _invaderStartCoordinate.y - j * _spaceBetweenInvaders.y), 
													_lowInvaderPrefab.transform.rotation,
													transform);
			}
		}
	}
	
	private void StartShooting()
	{
		for(int i = 0; i < _matrixSize.x; i++)
		{
			StartCoroutine(EnemiesShooting(i));
		}
	}
	
	private void CheckDestroy()
	{
		for(int i = 0; i < _matrixSize.x; i++)
		{
			if(_invaderInColumn[i] > -1 && _invadersMatrix[i, _invaderInColumn[i]] == null)
			{
				_invaderInColumn[i]--;
				_invadersDiedCounter++;
				if(_invadersDiedCounter == 55)
					AllEnemiesDieEvent.Invoke();
			}
		}
	}
	
	private IEnumerator EnemiesMoving()
	{
		while(_totalRowsCounter < _totalRows)
		{
			if(!PlayerDiedPause)
			{
				if(_movesInOneRowCounter == _movesInOneRow)
				{
					_movesInOneRowCounter = 0;
					_moveDistance.x = -1 * _moveDistance.x;
					_totalRowsCounter++;
					transform.position += new Vector3(0, _moveDistance.y, 0);
					_pause -= _pauseChange;
				}
				else
				{
					transform.position += new Vector3(_moveDistance.x, 0, 0);
					_movesInOneRowCounter++;
					foreach(Invader invader in _invadersMatrix)
						if(invader != null)
							invader.ChangeSprite();
				}
				
				yield return new WaitForSeconds(_pause);
			}
			else
			{
				yield return new WaitForSeconds(5.0f);
				PlayerDiedPause = false;
			}
		}
		EnemiesReachTargetEvent.Invoke();
	}
	
	private IEnumerator EnemiesShooting(int column)
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(_pauseShooting.x, _pauseShooting.y));
		while(_invaderInColumn[column] > -1)
		{
			if(!PlayerDiedPause)
			{
				Instantiate(_invaderBullet, new Vector2(_invadersMatrix[column, _invaderInColumn[column]].transform.position.x, 
							_invadersMatrix[column, _invaderInColumn[column]].transform.position.y - 1),
				_invadersMatrix[column, _invaderInColumn[column]].transform.rotation);
				yield return new WaitForSeconds(UnityEngine.Random.Range(_pauseShooting.x, _pauseShooting.y));
			}
			else
				yield return new WaitForSeconds(5.0f);
		}
	}
	
	private IEnumerator UfoSpawn()
	{
		while(true)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(_pauseUfoSpawn.x, _pauseUfoSpawn.y));
			if(PlayerDiedPause)
				yield return new WaitForSeconds(5.0f);
			Instantiate(_ufoPrefab, new Vector2(_ufoPrefab.transform.position.x, _ufoPrefab.transform.position.y), _ufoPrefab.transform.rotation);
		}
	}
}
