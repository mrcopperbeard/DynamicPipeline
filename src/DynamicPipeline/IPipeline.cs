using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Syntax;

namespace DynamicPipeline
{
	public interface IPipeline<in T>
	{
		void Configure(IDictionary<string, IHandlerBehavior> configuration);

		IHandleResult Execute(IHandleContext<T> context);
	}

	public class Pipeline<T> : IPipeline<T>
	{
		private readonly IResolutionRoot _kernel;

		private readonly IList<IHandler<T>> _pipeline;

		public Pipeline(IResolutionRoot kernel)
		{
			_kernel = kernel;
			_pipeline = new List<IHandler<T>>();
		}

		public void Configure(IDictionary<string, IHandlerBehavior> configuration)
		{
			_pipeline.Clear();

			foreach (var step in configuration)
			{
				var handler = _kernel.Get<IHandler<T>>(step.Key);

				_pipeline.Add(new BehavedHandlerDecorator<T>(handler, step.Value));
			}
		}

		public IHandleResult Execute(IHandleContext<T> context)
		{
			if (!_pipeline.Any())
			{
				throw new InvalidOperationException($"{nameof(Pipeline<T>)} has not been configured.");
			}

			foreach (var handler in _pipeline)
			{
				var handleResult = handler.Handle(context);

				foreach (var error in handleResult.Errors)
				{
					context.Result.Errors.Add(error);
				}
			}

			return context.Result;
		}
	}
}