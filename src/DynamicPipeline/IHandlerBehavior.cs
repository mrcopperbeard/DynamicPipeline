namespace DynamicPipeline
{
	public interface IHandlerBehavior
	{
		HandleMode Mode { get; }
	}

	public class HandlerBehavior : IHandlerBehavior
	{
		public HandleMode Mode { get; set; }
	}
}