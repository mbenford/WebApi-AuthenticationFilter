using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace WebApi.AuthenticationFilter
{
    /// <summary>
    /// Defines a filter that performs authentication.
    /// </summary>
    public abstract class AuthenticationFilterAttribute : FilterAttribute, IAuthenticationFilter
    {        
        /// <summary>
        /// Authenticates the request asynchronously.
        /// </summary>
        /// <param name="context">The authentication context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A Task that will perform the authentication challenge.</returns>
        public virtual Task OnAuthenticationAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {
                OnAuthentication(context);
            }
            catch(Exception ex)
            {
                return FromError(ex);
            }

            return Completed();
        }

        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The authentication context.</param>
        public virtual void OnAuthentication(HttpAuthenticationContext context)
        {
        }

        /// <summary>
        /// Adds an authentication challenge to the inner IHttpActionResult asynchronously.
        /// </summary>
        /// <param name="context">The authentication challenge context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A Task that will perform the authentication challenge.</returns>
        public virtual Task OnAuthenticationChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            try
            {
                OnAuthenticationChallenge(context);
            }
            catch(Exception ex)
            {
                return FromError(ex);
            }

            return Completed();
        }

        /// <summary>
        /// Adds an authentication challenge to the inner IHttpActionResult.
        /// </summary>
        /// <param name="context">The authentication challenge context.</param>
        public virtual void OnAuthenticationChallenge(HttpAuthenticationChallengeContext context)
        {
        }

        Task IAuthenticationFilter.AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            return AuthenticateAsync(context, cancellationToken);
        }

        Task IAuthenticationFilter.ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return ChallengeAsync(context, cancellationToken);
        }

        private async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            await OnAuthenticationAsync(context, cancellationToken);
        }

        private async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            await OnAuthenticationChallengeAsync(context, cancellationToken);
        }

        private static Task Completed()
        {
            return Task.FromResult(default(AsyncVoid));
        }

        private static Task<AsyncVoid> FromError(Exception exception)
        {
            var tcs = new TaskCompletionSource<AsyncVoid>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        private struct AsyncVoid
        {
        }
    }
}