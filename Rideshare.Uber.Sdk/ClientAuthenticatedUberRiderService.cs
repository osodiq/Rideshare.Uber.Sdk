﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rideshare.Uber.Sdk.Extensions;
using Rideshare.Uber.Sdk.Models;

namespace Rideshare.Uber.Sdk
{
    public class ClientAuthenticatedUberRiderService : BaseUberRiderService
    {
        /// <summary>
        /// Initialises a new <see cref="UberClient"/> with the required configurations
        /// </summary>
        /// <param name="tokenType">
        /// The token type - server or client
        /// </param>
        /// <param name="token">
        /// The token
        /// </param>
        /// <param name="baseUri">
        /// The base URI, defaults to production - https://api.uber.com
        /// </param>
        public ClientAuthenticatedUberRiderService(string clientToken, string baseUri = "https://api.uber.com")
            : base(AccessTokenType.Client, clientToken, baseUri)
        { }

        #region Requests

        /// <summary>
        /// Makes a pickup request.
        /// </summary>
        /// <param name="productId">
        /// The product ID.
        /// </param>
        /// <param name="startLatitude">
        /// The start location latitude.
        /// </param>
        /// <param name="startLongitude">
        /// The start location longitude.
        /// </param>
        /// <param name="endLatitude">
        /// The end location latitude.
        /// </param>
        /// <param name="endLongitude">
        /// The end location longitude.
        /// </param>
        /// <param name="surgeConfirmationId">
        /// The surge pricing confirmation ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Request"/>.
        /// </returns>
        public async Task<UberResponse<Request>> RequestAsync(string productId, float startLatitude, float startLongitude, float endLatitude, float endLongitude, string surgeConfirmationId = null)
        {
            var url = $"/{this._version}/requests";

            var postData = new Dictionary<string, string>
            {
                { "product_id", productId },
                { "start_latitude", startLatitude.ToString("0.00000") },
                { "start_longitude", startLongitude.ToString("0.00000") },
                { "end_latitude", endLatitude.ToString("0.00000") },
                { "end_longitude", endLongitude.ToString("0.00000") },
            };

            if (!string.IsNullOrWhiteSpace(surgeConfirmationId))
            {
                postData.Add("surge_confirmation_id", surgeConfirmationId);
            }

            var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            return await this.HttpClient.UberPostAsync<Request>(url, content);
        }

        /// <summary>
        /// Gets a request details.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="RequestDetails"/>.
        /// </returns>
        public async Task<UberResponse<RequestDetails>> GetRequestDetailsAsync(string requestId)
        {
            var url = $"/{this._version}/requests/{requestId}";

            return await this.HttpClient.UberGetAsync<RequestDetails>(url);
        }

        /// <summary>
        /// Gets the map for a given request.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="RequestMap"/>.
        /// </returns>
        public async Task<UberResponse<RequestMap>> GetRequestMapAsync(string requestId)
        {
            var url = $"/{this._version}/requests/{requestId}/map";

            return await this.HttpClient.UberGetAsync<RequestMap>(url);
        }

        /// <summary>
        /// Cancels a given request.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a boolean indicating if the Uber API returned a successful HTTP status.
        /// </returns>
        public async Task<UberResponse<bool>> CancelRequestAsync(string requestId)
        {
            var url = $"/{this._version}/requests/{requestId}";

            return await this.HttpClient.UberDeleteAsync(url);
        }

        #endregion

        #region Promotions

        /// <summary>
        /// Gets a promotion available to new users based on location.
        /// </summary>
        /// <param name="startLatitude">
        /// The start location latitude.
        /// </param>
        /// <param name="startLongitude">
        /// The start location longitude.
        /// </param>
        /// <param name="endLatitude">
        /// The end location latitude.
        /// </param>
        /// <param name="endLongitude">
        /// The end location longitude.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Promotion"/>.
        /// </returns>
        public async Task<UberResponse<Promotion>> GetPromotionAsync(float startLatitude, float startLongitude, float endLatitude, float endLongitude)
        {
            var url = $"/{this._version}/promotions?start_latitude={startLatitude}&start_longitude={startLongitude}&end_latitude={endLatitude}&end_longitude={endLongitude}";

            return await this.HttpClient.UberGetAsync<Promotion>(url);
        }

        #endregion

        #region Riders

        /// <summary>
        /// The User Profile endpoint returns information about the Uber user that has authorized with the application.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="UserProfile"/>.
        /// </returns>
        public async Task<UberResponse<UserProfile>> GetUserProfileAsync()
        {
            var url = $"/{this._version}/me";

            return await this.HttpClient.UberGetAsync<UserProfile>(url);
        }

        /// <summary>
        /// The User Promotion endpoint allows applying a promotion code to the user’s Uber account.
        /// </summary>
        /// <param name="promoCode">
        /// The promotion code to apply.
        /// </param>
        /// <returns>
        /// Returns a <see cref="PromotionApplied"/>.
        /// </returns>
        public async Task<UberResponse<PromotionApplied>> ApplyUserPromotionAsync(string promoCode)
        {
            var url = $"/{this._version}/me";

            var patchData = new Dictionary<string, string>
            {
                { "applied_promotion_codes", promoCode }
            };

            var content = new StringContent(JsonConvert.SerializeObject(patchData), Encoding.UTF8, "application/json");

            return await this.HttpClient.UberPatchAsync<PromotionApplied>(url, content);
        }

        /// <summary>
        /// Gets a list of the user's Uber activity.
        /// </summary>
        /// <param name="offset">
        /// The results offset.
        /// </param>
        /// <param name="limit">
        /// The results limit.
        /// </param>
        /// <returns>
        /// Returns a <see cref="UserActivity"/>.
        /// </returns>
        public async Task<UberResponse<UserActivity>> GetUserActivityAsync(int offset, int limit)
        {
            var url = $"/{this._version}/history?offset={offset}&limit={limit}";

            return await this.HttpClient.UberGetAsync<UserActivity>(url);
        }

        #endregion
    }
}