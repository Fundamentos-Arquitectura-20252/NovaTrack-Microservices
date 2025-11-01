using MediatR;

namespace SharedKernel.Domain.Model
{
    public interface IQuery<out TResponse> : IRequest<TResponse> { }
}
