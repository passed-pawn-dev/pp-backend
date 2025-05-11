namespace PassedPawn.BusinessLogic.Services.Contracts;

public interface IStripeService
{
    Task<string> CreateCourseIntentSecret(int userId, float amount, int courseId);
}