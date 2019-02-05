using System.Collections.Generic;
using System.Linq;

namespace DynamicPipeline
{
	public interface IHandleResult
	{
		bool Success { get; }

		IList<string> Errors { get; }
	}

	public class HandleResult : IHandleResult
	{
		public HandleResult()
		{
			Errors = new List<string>();
		}

		public bool Success => !Errors.Any();

		public IList<string> Errors { get; }
	}
}