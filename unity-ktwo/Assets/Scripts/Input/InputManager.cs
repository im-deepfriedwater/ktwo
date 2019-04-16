using System.Collections;
using UnityEngine;

public enum Buttons {
	Right,
	Left,
	Up,
	Down,
	A,
	B,
	C,
	D,
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
	public static InputManager instance; // Singleton
	public InputAxisState[] inputs;
	[HideInInspector]
	public InputState playerInputState; // Set at runtime.
	public bool isInitialized = false;

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	// This must be called after the client connects by the client.
	// Should get called in PlayerBehaviour.Start
	public void Initialize (GameObject player)
	{
		playerInputState = player.GetComponent<InputState>();
		isInitialized = true;
	}

	// Update is called once per frame
	void Update ()
    {
		if (!isInitialized) return;

		foreach (var input in inputs)
        {
			playerInputState.SetButtonValue(input.button, input.value);
		}
	}
}
