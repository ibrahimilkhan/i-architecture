using Core.CrossCuttingConcerns.Exceptions.Types;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception)
    {
        switch (exception)
        {
            case BusinessException businessException:
                return HandleException(businessException);
            default:
                return HandleException(exception);
        }
    }

    protected abstract Task HandleException(BusinessException businessException);
    protected abstract Task HandleException(Exception exception);
}