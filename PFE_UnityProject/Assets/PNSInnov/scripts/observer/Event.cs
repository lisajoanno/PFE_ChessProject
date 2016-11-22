using System;

public enum EventTurnType { None = 0, Timeout = 1, MovementDone = 2, EndTurn = 3}

/// <summary>
/// An Event class with all the informations on
/// the event that you receive
/// </summary>
public class EventTurn
{
	private EventTurnType type;             //type of the event
	public EventTurnType Type
    {
        get
        {
            return type;
        }
    }

    private String message;             //message which is sent/received with the event
    public String Message
    {
        get
        {
            return message;
        }
            
    }

	public EventTurn(EventTurnType type, String message)
    {
        this.type = type;
        this.message = message;
    }
}