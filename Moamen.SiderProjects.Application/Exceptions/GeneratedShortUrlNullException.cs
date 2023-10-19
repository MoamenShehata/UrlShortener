using System.Runtime.Serialization;

namespace Moamen.SiderProjects.Application.Exceptions
{
	[Serializable]
	public class GeneratedShortUrlNullException : Exception
	{
		public GeneratedShortUrlNullException()
		{
		}

		public GeneratedShortUrlNullException(string message) : base(message)
		{
		}

		public GeneratedShortUrlNullException(string message, Exception inner) : base(message, inner)
		{
		}

		protected GeneratedShortUrlNullException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
