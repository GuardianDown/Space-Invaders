using UnityEngine;

public class BunkerSprite : MonoBehaviour
{
	[SerializeField] private Sprite[] _squareStates = new Sprite[3];
	
	private SpriteRenderer _spriteRenderer;
	private int _index = 0;
	
	private void Awake()
	{
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	public void ChangeSprite()
	{
		if(_index < 3)
		{
			_spriteRenderer.sprite = _squareStates[_index];
			_index++;
		}
		else
			Destroy(gameObject);
	}
}
