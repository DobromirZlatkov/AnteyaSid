namespace AnteyaSidOnContainers.Services.Catalog.API.Application.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AnteyaSidOnContainers.Services.Catalog.Services.Data.Contracts.Idempotency;
    using MediatR;

    /// <summary>
    /// Provides a base implementation for handling duplicate request and ensuring idempotent updates, in the cases where
    /// a requestid sent by client is used to detect duplicate requests.
    /// </summary>
    /// <typeparam name="T">Type of the command handler that performs the operation if request is not duplicated</typeparam>
    /// <typeparam name="R">Return value of the inner command handler</typeparam>
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IClientRequestService _clientRequestService;
       
        public IdentifiedCommandHandler(
            IMediator mediator,
            IClientRequestService clientRequestService
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _clientRequestService = clientRequestService ?? throw new ArgumentNullException(nameof(clientRequestService));
        }

        /// <summary>
        /// Creates the result value to return if a previous request was found
        /// </summary>
        /// <returns></returns>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }

        /// <summary>
        /// This method handles the command. It just ensures that no other request exists with the same ID, and if this is the case
        /// just enqueues the original inner command.
        /// </summary>
        /// <param name="message">IdentifiedCommand which contains both original command & request ID</param>
        /// <returns>Return value of inner command or default value if request same ID was found</returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _clientRequestService.DoExistAsync(message.Id);
            if (alreadyExists)
            {
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _clientRequestService.CreateRequestForCommandAsync<T>(message.Id);
                try
                {
                    // Send the embeded business command to mediator so it runs its related CommandHandler 
                    var result = await _mediator.Send(message.Command);
                    return result;
                }
                catch
                {
                    return default(R);
                }
            }
        }
    }
}
