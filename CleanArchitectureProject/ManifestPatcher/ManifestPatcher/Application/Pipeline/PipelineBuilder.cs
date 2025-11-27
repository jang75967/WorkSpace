using ManifestPatcher.Domain.Services;

namespace ManifestPatcher.Application.Pipeline
{
	public sealed class PipelineBuilder
	{
		private readonly List<IRebuildHandler> _handlers = new();

		public PipelineBuilder Use(IRebuildHandler handler)
		{
			_handlers.Add(handler);
			return this;
		}

		public PipelineBuilder Use(IManifestRebuildStep step)
		{
			return Use(new StepHandlerAdapter(step));
		}

		public Action<ManifestRebuildContext> Build()
		{
			Action<ManifestRebuildContext> app = _ => { };
			for (int i = _handlers.Count - 1; i >= 0; i--)
			{
				var next = app;
				var handler = _handlers[i];
				app = ctx => handler.Handle(ctx, () => next(ctx));
			}
			return app;
		}
	}
}

