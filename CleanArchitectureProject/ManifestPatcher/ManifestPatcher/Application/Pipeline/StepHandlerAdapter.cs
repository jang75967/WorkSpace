using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Pipeline
{
	public class StepHandlerAdapter : IRebuildHandler
	{
		private readonly IManifestRebuildStep _step;

		public StepHandlerAdapter(IManifestRebuildStep step)
		{
			_step = step;
		}

		public void Handle(ManifestRebuildContext context, Action next)
		{
			_step.Execute(context);
			next();
		}
	}
}

