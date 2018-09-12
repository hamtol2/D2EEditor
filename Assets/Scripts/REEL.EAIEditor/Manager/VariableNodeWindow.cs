using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace REEL.EAIEditor
{
	public class VariableNodeWindow : MonoBehaviour
	{
        [SerializeField] private Text noteTypeText;
        [SerializeField] private Text nodeIDText;
        [SerializeField] private InputField nodeTitleInput;
        [SerializeField] private InputField nodeNameInput;
        [SerializeField] private Dropdown nodeOperatorDropdown;
        [SerializeField] private InputField nodeValueInput;
        [SerializeField] private Text nextIDText;
    }
}