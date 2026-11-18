
using TMPro;
using UnityEngine;

public class CurrentObjective : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    private string[] objectives =
    {
        "Revisar caja del primer piso",
        "Revisar caja del patio",
        "Revisar caja del primer piso (nuevamente)",
        "ESCAPAR"
    };

    private int currentObjective = 0;

    private void Start()
    {
        ShowObjective();
    }

    public void NextObjective()
    {
        if (currentObjective < objectives.Length - 1)
        {
            currentObjective++;
            ShowObjective();
        }
    }

    private void ShowObjective()
    {
        objectiveText.text = objectives[currentObjective];
    }
}
