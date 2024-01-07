using CSharp.Choices;

namespace Domain.WorkflowEvents;

[AsChoice]
public static partial class PlaceOrderEvent
{
    public interface IPlaceOrderEvent { }
    
   
    public record OrderPlacedSucceededEvent : IPlaceOrderEvent
    {
       
    }
    
    public record OrderPlacedFailedEvent : IPlaceOrderEvent
    {
        public string Reason { get; }

        public OrderPlacedFailedEvent(string reason)
        {
            Reason = reason;
        }
    }
    
    
    
}