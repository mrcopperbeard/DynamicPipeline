using System;
using System.Threading.Tasks;

namespace DynamicPipeline.Tests
{
	public class BarHandler : IHandler<string>
	{
		public virtual Task<IHandleResult> Handle(IHandleContext<string> context)
		{
			var result = new HandleResult();
			var value = context.Value;

			if (value.Contains("Bar"))
			{
				Console.Out.WriteLine($"Bar: {value}");
			}
			else
			{
				result.Errors.Add($"Failed in Bar: {value}");
			}

			return Task.FromResult((IHandleResult)result);
		}

		public virtual Task HandleError(IHandleContext<string> context)
		{
			Console.Out.WriteLine($"Bar handling errors: {string.Join(Environment.NewLine, context.Result.Errors)}");

			return Task.CompletedTask;
		}
	}
}