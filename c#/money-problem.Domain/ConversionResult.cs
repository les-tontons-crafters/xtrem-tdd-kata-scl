namespace money_problem.Domain;


public class ConversionResult<T>
{
    private readonly T? failure;

    private readonly Money? success;

    public ConversionResult(Money success)
    {
        this.success = success;
    }

    public ConversionResult(T failure)
    {
        this.failure = failure;
    }

    public bool IsSuccess() => this.success != null;

    public bool IsFailure() => this.failure != null;

    public T GetFailureUnsafe() => this.failure!;

    public Money GetSuccessUnsafe() => this.success!;
}