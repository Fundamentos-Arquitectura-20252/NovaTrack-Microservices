using MediatR;

namespace SharedKernel.Domain.Model
{
    public interface ICommand : IRequest<bool> { }
    public interface ICommand<out TResponse> : IRequest<TResponse> { }
}