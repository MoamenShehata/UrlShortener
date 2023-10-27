using System.Runtime.Serialization;

namespace Moamen.SiderProjects.Application.Exceptions
{
	[Serializable]
	public class UrlNotFoundException : Exception
	{
		public string Url { get; }

		public UrlNotFoundException(string url, string message) : base(message)
		{
			Url = url;
		}

		public UrlNotFoundException(string url, string message, Exception inner) : base(message, inner)
		{
			Url = url;
		}

		protected UrlNotFoundException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}
