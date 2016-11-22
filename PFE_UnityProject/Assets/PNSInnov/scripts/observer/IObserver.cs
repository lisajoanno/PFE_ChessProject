using System.Collections.Generic;
/// <summary>
///     The interface describing specifics methods for the Observer in the observer pattern
/// </summary>
public interface IObserver{
	void OnNotify(IList<EventTurn> eventNotify);
}
