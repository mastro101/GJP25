using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonGenericAction : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SetActiveTrue(GameObject toAble)
    {
        toAble.SetActive(true);
    }

    public void SetActiveFalse(GameObject toDisable)
    {
        toDisable.SetActive(false);
    }

    public void SetActiveButton(Button button)
    {
        button.Select();
    }
}