using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	private Image _image;
	
	private void Awake()
	{
		_image = gameObject.GetComponent<Image>();
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
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
}
