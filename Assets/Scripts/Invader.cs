using UnityEngine;
using System.Collections;
using System;

public abstract class Invader : MonoBehaviour
{	
	[SerializeField] protected Sprite _state1 = null;
	[SerializeField] protected Sprite _state2 = null;
	[SerializeField] protected int points = 0;
	
	private SpriteRenderer _spriteRenderer;

	public int Points { get {return points;} }
	
	private void Awake()
	{
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	public void ChangeSprite()
	{
		if(_spriteRenderer.sprite == _state1)
			_spriteRenderer.sprite = _state2;
		else
			_spriteRenderer.sprite = _state1;
	}
}
