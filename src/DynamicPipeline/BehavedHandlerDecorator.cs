namespace DynamicPipeline
{
	internal class BehavedHandlerDecorator<T> : IHandler<T>
	{
		private readonly IHandler<T> _handler;

		private readonly IHandlerBehavior _behavior;

		public BehavedHandlerDecorator(
			IHandler<T> handler,
			IHandlerBehavior behavior)
		{
			_handler = handler;
			_behavior = behavior;
		}

		public IHandleResult Handle(IHandleContext<T> context)
		{
			if (context.Result.Success || _behavior.Mode == HandleMode.Anyway)
			{
				return _handler.Handle(context);
			}

			if (_behavior.Mode == HandleMode.OnError)
			{
				HandleError(context);
			}

			return new HandleResult();
		}

		public void HandleError(IHandleContext<T> context)
		{
			_handler.HandleError(context);
		}
	}
}