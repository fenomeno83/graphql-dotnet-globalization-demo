using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class HttpContextExtensions
    {
       
       
        public static string GetCorrelation(this HttpContext context)
        {
            string item = context.GetRealCorrelation();

            if (string.IsNullOrWhiteSpace(item))
                return context.Request.Path;
            else
                return item;

        }

        public static string GetRealCorrelation(this HttpContext context)
        {
            return context.GetContextItem(ApiConstants.CorrelationKey);

        }

        public static string GetContextItem(this HttpContext context, string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                return null;

            context.Items.TryGetValue(itemName, out var value);

            if (value == null)
                return null;
            else
                return value.ToString();

        }

        public static void SetCorrelation(this HttpContext context, string correlationId)
        {
            context.SetContextItem(ApiConstants.CorrelationKey, correlationId);
        }

        public static void RemoveCorrelation(this HttpContext context)
        {
            context.RemoveContextItem(ApiConstants.CorrelationKey);
        }

        public static void SetContextItem(this HttpContext context, string itemName, string value)
        {
            if (!string.IsNullOrWhiteSpace(itemName) && !string.IsNullOrWhiteSpace(value))
                context.Items[itemName] = value;
        }

        public static void RemoveContextItem(this HttpContext context, string itemName)
        {
            if (!string.IsNullOrWhiteSpace(itemName) && context.Items != null)
                context.Items.Remove(itemName);
        }

        public static bool HasClaims(this HttpContext HttpContext, params string[] claims)
        {
            if (!claims.Any())
                return false;

            var userClaims = HttpContext.User?.Claims;

            if (!userClaims.Any())
                return false;

            foreach (string claim in claims)
            {
                if (!string.IsNullOrWhiteSpace(claim))
                    if (!userClaims.Any(f => !string.IsNullOrWhiteSpace(f?.Type) && f.Type.Trim().ToUpper() == claim.Trim().ToUpper()))
                        return false;
            }

            return true;
        }

        public static bool HasClaimWithValue(this HttpContext HttpContext, string claim, string value)
        {
            if (string.IsNullOrWhiteSpace(claim) || string.IsNullOrWhiteSpace(value))
                return false;

            var userClaim = HttpContext.GetClaimValue(claim);

            if (string.IsNullOrWhiteSpace(userClaim))
                return false;

            if (userClaim.Trim().ToUpper() == value.Trim().ToUpper())
                return true;
            else
                return false;

        }

        public static Claim GetClaim(this HttpContext HttpContext, string claim)
        {
            if (string.IsNullOrWhiteSpace(claim))
                return null;

            return HttpContext.User?.Claims?.FirstOrDefault(x => x.Type.Trim().ToUpper() == claim.Trim().ToUpper());

        }

        public static string GetClaimValue(this HttpContext HttpContext, string claim)
        {

            return HttpContext.GetClaim(claim)?.Value;

        }

        public static string GetClientId(this HttpContext HttpContext)
        {
            return HttpContext.GetClaimValue(AuthorizationConstants.ClientId);
        }

        public static string[] GetScopes(this HttpContext HttpContext)
        {
            var userScopes = HttpContext.GetClaimValue(AuthorizationConstants.Scope);

            if (userScopes == null)
                return null;

            var splittedUserScopes = userScopes.Split(" ");
            if (!splittedUserScopes.Any())
                return null;

            return splittedUserScopes;
        }

        public static void ValidateScopes(this HttpContext HttpContext, params string[] scopes)
        {
            if (!HasScopes(HttpContext, scopes))
                throw new System.Exception(Messages.UserNotAuthorized);

        }
        public static bool HasScopes(this HttpContext HttpContext, params string[] scopes)
        {
            if (!scopes.Any())
                return false;

            var splittedUserScopes = HttpContext.GetScopes();

            if (splittedUserScopes == null)
                return false;

            foreach (string scope in scopes)
            {
                if (!string.IsNullOrWhiteSpace(scope))
                    if (!splittedUserScopes.Any(f => !string.IsNullOrWhiteSpace(f) && f.Trim().ToUpper() == scope.Trim().ToUpper()))
                        return false;
            }

            return true;
        }

        public static string GetUsername(this HttpContext HttpContext)
        {
            return HttpContext?.User?.Identity?.Name;
        }

        public static string GetBodyString(this HttpContext HttpContext)
        {
            string bodyText;
            try
            {
                var bodyStream = new StreamReader(HttpContext.Request.Body);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                bodyText = bodyStream.BaseStream.Length == 0 ? null : bodyStream.ReadToEnd();

            }
            catch
            {
                bodyText = null;
            }

            return bodyText;
        }

        public static JToken GetBodyObject(this HttpContext HttpContext)
        {
            string body = HttpContext.GetBodyString();
            return string.IsNullOrWhiteSpace(body) ? null : JToken.Parse(body);
        }

        public static T GetBodyObject<T>(this HttpContext HttpContext)
        {

            string body = HttpContext.GetBodyString();

            return string.IsNullOrWhiteSpace(body) ? default(T) : JsonConvert.DeserializeObject<T>(body);

        }

    }

}
