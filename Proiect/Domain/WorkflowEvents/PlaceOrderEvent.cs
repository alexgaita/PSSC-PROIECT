using CSharp.Choices;

namespace Domain.WorkflowEvents;

[AsChoice]
public static partial class PlaceOrderEvent
{
    public interface IPlaceOrderEvent { }
    
    public record OrderPlacedSucceededEvent : IPlaceOrderEvent
    {
       public int OrderId { get; }
       
       public OrderPlacedSucceededEvent(int orderId)
       {
           OrderId = orderId;
       }
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