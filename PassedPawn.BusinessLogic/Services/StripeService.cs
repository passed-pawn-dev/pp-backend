using Microsoft.Extensions.Configuration;
using PassedPawn.BusinessLogic.Services.Contracts;
using Stripe;

namespace PassedPawn.BusinessLogic.Services;

public class StripeService : IStripeService
{
    public StripeService(IConfiguration config)
    {
        StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
    }

    public async Task<string> CreateCourseIntentSecret(int userId, float amount, int courseId)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Stripe uses cents
            Currency = "pln",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            },
            Metadata = new Dictionary<string, string>
            {
                { "courseId", courseId.ToString() },
                { "userId", userId.ToString() }
            }
        };

        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(options);

        return intent.ClientSecret;
    }
}
