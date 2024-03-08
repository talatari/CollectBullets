using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.Codebase.Players.Joystick
{
    public class JoystickHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _joystickArea;
        [SerializeField] private Image _joystickBackground;
        [SerializeField] private Image _joystick;
        [SerializeField] private Color _inActiveBackgroundJoystickColor;
        [SerializeField] private Color _inActiveJoystickColor;
        [SerializeField] private Color _activeBackgroundJoystickColor;
        [SerializeField] private Color _activeJoystickColor;

        private Vector2 _joystickBackgroundStartPosition;
        private bool _joystickIsActive;

        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        protected Vector2 _inputVector;

        private void Start()
        {
            ClickEffect();

            _joystickBackgroundStartPosition = _joystickBackground.rectTransform.anchoredPosition;
        }

        private void ClickEffect()
        {
            if (_joystickIsActive == false)
            {
                _joystick.color = _activeJoystickColor;
                _joystickBackground.color = _activeBackgroundJoystickColor;
                _joystickIsActive = true;
            }
            else
            {
                _joystick.color = _inActiveJoystickColor;
                _joystickBackground.color = _inActiveBackgroundJoystickColor;
                _joystickIsActive = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _joystickBackground.rectTransform, eventData.position, null, out Vector2 joystickPosition))
            {
                Vector2 joystickBackgroundSizeDelta = _joystickBackground.rectTransform.sizeDelta;
                
                joystickPosition.x = joystickPosition.x * 2 / joystickBackgroundSizeDelta.x;
                joystickPosition.y = joystickPosition.y * 2 / joystickBackgroundSizeDelta.y;

                _inputVector = new Vector2(joystickPosition.x, joystickPosition.y);

                _inputVector = _inputVector.magnitude > 1.0f ? _inputVector.normalized : _inputVector;

                _joystick.rectTransform.anchoredPosition = new Vector2(
                    _inputVector.x * (joystickBackgroundSizeDelta.x / 2), 
                    _inputVector.y * (joystickBackgroundSizeDelta.y / 2));
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ClickEffect();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _joystickArea.rectTransform, eventData.position, null, out Vector2 joystickBackgroundPosition))
            {
                _joystickBackground.rectTransform.anchoredPosition = new Vector2(
                    joystickBackgroundPosition.x, joystickBackgroundPosition.y);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _joystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;
            
            ClickEffect();
            
            _inputVector = Vector2.zero;
            _joystick.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}