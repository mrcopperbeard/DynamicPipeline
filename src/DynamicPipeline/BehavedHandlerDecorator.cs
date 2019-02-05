using System.Threading.Tasks;

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

		public async Task<IHandleResult> Handle(IHandleContext<T> context)
		{
			if (context.Result.Success || _behavior.Mode == HandleMode.Anyway)
			{
				return await _handler.Handle(context).ConfigureAwait(false);
			}

			if (_behavior.Mode == HandleMode.OnError)
			{
				await HandleError(context).ConfigureAwait(false);
			}

			return new HandleResult();
		}

		public Task HandleError(IHandleContext<T> context)
		{
			return _handler.HandleError(context);
		}
	}
}