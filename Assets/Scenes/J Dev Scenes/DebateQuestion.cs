using UnityEngine;

[CreateAssetMenu(fileName = "NewDebateQuestion", menuName = "Debate/Question")]
public class DebateQuestion : ScriptableObject
{
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex; // used if you want scoring based on correctness
}
