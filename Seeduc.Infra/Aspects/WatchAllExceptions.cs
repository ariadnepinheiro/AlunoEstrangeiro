namespace Seeduc.Infra.Aspects
{
    using System;
    using log4net;
    using PostSharp.Aspects;
    using Seeduc.Infra.Configuration;

    [Serializable]
    internal sealed class WatchAllExceptions : OnMethodBoundaryAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            if (!ConfigurationManager.Instance.Section.Log.Enable)
            {
                return;
            }

            var log = LogManager.GetLogger(args.Instance.GetType());

            log.Error(args.Exception.Message, args.Exception);

            base.OnException(args);
        }
    }
}
