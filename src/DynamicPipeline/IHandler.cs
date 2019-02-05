namespace DynamicPipeline
{
	public interface IHandler<in T>
	{
		IHandleResult Handle(IHandleContext<T> context);

		void HandleError(IHandleContext<T> context);
	}
}