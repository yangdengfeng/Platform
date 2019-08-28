using ServiceStack;
using QZWebService.ServiceModel;
using ServiceStack.Logging;
using ServiceStack.FluentValidation;

namespace QZWebService.ServiceInterface
{
    public class MyServices : Service
    {
        public static ILog Log = LogManager.GetLogger("MyService");

        public object Any(Hello request)
        {
            Log.Debug("test with {0}".Fmt(request.Name));
            //throw new System.Exception("test");
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }

    public class HelloValidator : AbstractValidator<Hello>
    {
        public HelloValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Name.Length).GreaterThan(10).WithMessage("test");
        }
    }
}