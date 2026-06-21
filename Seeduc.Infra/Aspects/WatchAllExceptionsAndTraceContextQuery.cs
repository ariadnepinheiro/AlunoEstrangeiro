namespace Seeduc.Infra.Aspects
{
    using System;
    using System.Linq;
    using log4net;
    using PostSharp.Aspects;
    using Seeduc.Infra.Configuration;
    using Seeduc.Infra.Data;

    [Serializable]
    internal sealed class WatchAllExceptionsAndTraceContextQuery : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if (!ConfigurationManager.Instance.Section.Log.Enable)
            {
                return;
            }

            var message = args.Exception.Message;

            if (args.Exception is ContextQueryException
                && args.Arguments.Count(x => x is ContextQuery) == 1)
            {
                var contextQuery = args.Arguments.Single(x => x is ContextQuery) as ContextQuery;

                if (!ContextQuery.IsEmpty(contextQuery))
                {
                    message = string.Concat(message, contextQuery.ToPlainSql());
                }
            }

            var log = LogManager.GetLogger(args.Instance.GetType());

            log.Error(message, args.Exception);

            base.OnException(args);
        }
    }
}
