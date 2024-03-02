// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Orleans.Execution;

/// <summary>
/// Represents an Orleans <see cref="IOutgoingGrainCallFilter">grain call filter</see> that adds
/// the correct <see cref="ExecutionContext"/> values to the request context.
/// </summary>
public class ExecutionContextOutgoingCallFilter : IOutgoingGrainCallFilter
{
    readonly IExecutionContextManager _executionContextManager;
    readonly IRequestContextManager _requestContextManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecutionContextOutgoingCallFilter"/> class.
    /// </summary>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with execution context.</param>
    /// <param name="requestContextManager"><see cref="IRequestContextManager"/> for working with state in requests.</param>
    public ExecutionContextOutgoingCallFilter(
        IExecutionContextManager executionContextManager,
        IRequestContextManager requestContextManager)
    {
        _executionContextManager = executionContextManager;
        _requestContextManager = requestContextManager;
    }

    /// <inheritdoc/>
    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        if (_executionContextManager.IsInContext)
        {
            var executionContext = _executionContextManager.Current;
            _requestContextManager.Set(RequestContextKeys.MicroserviceId, executionContext.MicroserviceId);
            _requestContextManager.Set(RequestContextKeys.TenantId, executionContext.TenantId);
            _requestContextManager.Set(RequestContextKeys.CorrelationId, executionContext.CorrelationId);
        }
        else
        {
            _requestContextManager.Set(RequestContextKeys.MicroserviceId, ExecutionContextManager.GlobalMicroserviceId);
        }
        await context.Invoke();
    }
}
