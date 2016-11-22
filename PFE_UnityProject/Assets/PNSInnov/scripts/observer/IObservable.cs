
using System.Collections.Generic;
/// <summary>
///     Interface for using the Observable of the Observer Pattern
/// </summary>
public interface IObservable{
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyAll();
	void NotifyAll(IList<EventTurn> events);
}