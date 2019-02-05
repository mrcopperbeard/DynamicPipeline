using System;

namespace DynamicPipeline.Tests
{
	public class BarHandler : IHandler<string>
	{
		public virtual IHandleResult Handle(IHandleContext<string> context)
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

			return result;
		}

		public virtual void HandleError(IHandleContext<string> context)
		{
			Console.Out.WriteLine($"Bar handling errors: {string.Join(Environment.NewLine, context.Result.Errors)}");
		}
	}
}