using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	private Image _image;
	private GameManager _gameManager;
	
	private void Awake()
	{
		_image = gameObject.GetComponent<Image>();
		_gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		_image.color = Color.green;
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		_image.color = Color.white;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		_gameManager.ResumeGame();
	}
}
