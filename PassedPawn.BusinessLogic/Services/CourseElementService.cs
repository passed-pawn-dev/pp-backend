using PassedPawn.DataAccess.Entities.Courses;

namespace PassedPawn.BusinessLogic.Services;

public abstract class CourseElementService
{
    protected static int GetHighestOrderNumber(Lesson lesson)
    {
        return lesson.Elements.Any()
            ? lesson.Elements
                .Select(element => element.Order)
                .Max()
            : 0;
    }
    
    protected static List<string> GetInvalidOrderErrors(Lesson lesson)
    {
        return lesson.Elements
            .OrderBy(element => element.Order)
            .Select((element, index) => (element.Order, index))
            .Aggregate(new List<string>(), (acc, curr) =>
                curr.Order != curr.index + 1
                    ? [..acc, $"{curr.Order} order number is incorrect."]
                    : acc);
    }
    
    protected static void MoveOrderOnAdd(Lesson lesson, int newOrder)
    {
        foreach (var element in lesson.Elements)
        {
            if (element.Order < newOrder)
                continue;

            element.Order++;
        }
    }
    
    protected static void MoveOrderOnUpdate(Lesson lesson, int oldOrder, int newOrder)
    {
        if (oldOrder == newOrder)
            return;

        if (newOrder > oldOrder)
            DecrementOrders(lesson, oldOrder, newOrder);
        else
            IncrementOrders(lesson, newOrder, oldOrder);
    }

    private static void DecrementOrders(Lesson lesson, int start, int end)
    {
        foreach (var element in lesson.Elements)
            if (element.Order > start && element.Order <= end)
                element.Order--;
    }

    private static void IncrementOrders(Lesson lesson, int start, int end)
    {
        foreach (var element in lesson.Elements)
            if (element.Order >= start && element.Order < end)
                element.Order++;
    }
}
