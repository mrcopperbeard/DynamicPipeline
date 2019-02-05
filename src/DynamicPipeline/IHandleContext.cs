namespace DynamicPipeline
{
	public interface IHandleContext<out T>
	{
		T Value { get; }

		IHandleResult Result { get; }
	}

	public class HandleContext<T> : IHandleContext<T>
	{
		public HandleContext(T value)
		{
			Value = value;
			Result = new HandleResult();
		}

		public T Value { get; }
		public IHandleResult Result { get; }
	}
}