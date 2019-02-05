using System;

namespace DynamicPipeline.Tests
{
	public class FooHandler : IHandler<string>
	{
		public IHandleResult Handle(IHandleContext<string> context)
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

			return result;
		}

		public void HandleError(IHandleContext<string> context)
		{
			Console.Out.WriteLine($"Foo handling errors: {string.Join(Environment.NewLine, context.Result.Errors)}");
		}
	}
}