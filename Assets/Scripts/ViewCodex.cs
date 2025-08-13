
using UnityEngine;
using UnityEngine.UI;

public class ViewCodex : MonoBehaviour
{
    public int index;
    Image image;
    Button button;
    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Open);
    }
   public void Open()
    {
        if (image.color.a != 1) return;

        GameController.instance.ShowCodexPiece(GameController.instance.codexMessages[index], GameController.instance.codexTitles[index], false  );
    }
}
