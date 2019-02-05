using System;
using System.Threading.Tasks;

namespace DynamicPipeline.Tests
{
	public class FooHandler : IHandler<string>
	{
		public Task<IHandleResult> Handle(IHandleContext<string> context)
		{
			var result = new HandleResult();
			var value = context.Value;

			if (value.Contains("Foo"))
			{
				Console.Out.WriteLine($"Foo: {value}");
			}
			else
			{
				result.Errors.Add($"Failed in Foo: {value}");
			}

			return Task.FromResult((IHandleResult)result);
		}

		public Task HandleError(IHandleContext<string> context)
		{
			Console.Out.WriteLine($"Foo handling errors: {string.Join(Environment.NewLine, context.Result.Errors)}");

			return Task.CompletedTask;
		}
	}
}