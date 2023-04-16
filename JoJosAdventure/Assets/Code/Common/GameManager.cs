using UnityEngine;

namespace JoJosAdventure.Common
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Texture2D cursorTexture = null;

        private void Start()
        {
            this.SetCursorIcon();
        }

        private void SetCursorIcon()
        {
            Cursor.SetCursor(
                this.cursorTexture,
                new Vector2(this.cursorTexture.width / 2f, this.cursorTexture.height / 2f),
                CursorMode.Auto);
        }
    }
}