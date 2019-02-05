using System.Threading.Tasks;

namespace DynamicPipeline
{
	public interface IHandler<in T>
	{
		Task<IHandleResult> Handle(IHandleContext<T> context);

		Task HandleError(IHandleContext<T> context);
	}
}