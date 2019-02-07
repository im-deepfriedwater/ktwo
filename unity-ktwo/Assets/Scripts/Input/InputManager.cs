using System.Collections;
using UnityEngine;

public enum Buttons {
	Right,
	Left,
	Up,
	Down,
	A,
	B,
    Click
}

public enum Condition
{
	GreaterThan,
	LessThan
}

// Is responsible for telling behaviours if a certain button is pressed.
[System.Serializable]
public class InputAxisState
{
	public string axisName;
	public float offValue;
	public Buttons button;
	public Condition condition;

	public bool value
    {
		get
        {
			var val = Input.GetAxis(axisName);

			switch (condition)
            {
				case Condition.GreaterThan:
					return val > offValue;
				case Condition.LessThan:
					return val < offValue;
			}

			return false;
		}
	}
}

public class InputManager : MonoBehaviour
{
	public InputAxisState[] inputs;
	public InputState playerInputState;

	// Update is called once per frame
	void Update ()
    {
		foreach (var input in inputs)
        {
			playerInputState.SetButtonValue(input.button, input.value);
		}
	}
}
