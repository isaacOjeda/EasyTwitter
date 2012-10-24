//This OAuth section was inspired by twitterizer
// Modified by Isaac Ojeda

//-----------------------------------------------------------------------
// <copyright file="OAuthUtility.cs" company="Patrick 'Ricky' Smith">
//  This file is part of the Twitterizer library (http://www.twitterizer.net/)
// 
//  Copyright (c) 2010, Patrick "Ricky" Smith (ricky@digitally-born.com)
//  All rights reserved.
//  
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  - Redistributions of source code must retain the above copyright notice, this list 
//    of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice, this list 
//    of conditions and the following disclaimer in the documentation and/or other 
//    materials provided with the distribution.
//  - Neither the name of the Twitterizer nor the names of its contributors may be 
//    used to endorse or promote products derived from this software without specific 
//    prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <author>Ricky Smith</author>
// <summary>Provides simple methods to simplify OAuth interaction.</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EasyTwitter
{
    public class OAuthUtility
    {

        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <returns></returns>
        public static OAuthTokenResponse GetRequestToken()
        {
            string callbackAddress = Config.EasyTwitterConfigHelper.GetCallBackUrl;
            WebRequestBuilder builder = new WebRequestBuilder(
                new Uri("https://api.twitter.com/oauth/request_token"),
                HTTPVerb.POST,
                new OAuthTokens());

            if (!string.IsNullOrEmpty(callbackAddress))
            {
                builder.Parameters.Add("oauth_callback", callbackAddress);
            }

            string responseBody = null;

            try
            {
                HttpWebResponse webResponse = builder.ExecuteRequest();
                Stream responseStream = webResponse.GetResponseStream();
                if (responseStream != null) responseBody = new StreamReader(responseStream).ReadToEnd();
            }
            catch (WebException wex)
            {
                throw new ApplicationException(wex.Message, wex);
            }

            return new OAuthTokenResponse
            {
                Token = ParseQuerystringParameter("oauth_token", responseBody),
                TokenSecret = ParseQuerystringParameter("oauth_token_secret", responseBody),
                VerificationString = ParseQuerystringParameter("oauth_verifier", responseBody)
            };
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>
        /// An <see cref="OAuthTokenResponse"/> class containing access token information.
        /// </returns>
        /// 
        public static OAuthTokenResponse GetAccessToken()
        {
            return GetAccessToken(String.Empty, String.Empty);
        }
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="verifier">The pin number or verifier string.</param>
        /// <returns></returns>
        public static OAuthTokenResponse GetAccessToken(string requestToken, string verifier)
        {
            string denied = String.Empty;
            if (String.IsNullOrEmpty(requestToken) && String.IsNullOrEmpty(verifier))
            {
                //this is only for web...
                requestToken = System.Web.HttpContext.Current.Request.QueryString["oauth_token"] as string;
                verifier = System.Web.HttpContext.Current.Request.QueryString["oauth_verifier"] as string;
                denied = System.Web.HttpContext.Current.Request.QueryString["denied"] as string ?? String.Empty;
            }

            if (String.IsNullOrEmpty(requestToken) || String.IsNullOrEmpty(verifier))
                throw new ArgumentNullException("You need to prove the request token and the verifier code");

            WebRequestBuilder builder = new WebRequestBuilder(
                new Uri("https://api.twitter.com/oauth/access_token"),
                HTTPVerb.GET,
                new OAuthTokens());

            if (!string.IsNullOrEmpty(verifier))
            {
                builder.Parameters.Add("oauth_verifier", verifier);
            }

            builder.Parameters.Add("oauth_token", requestToken);

            string responseBody;

            try
            {
                if (!String.IsNullOrEmpty(denied))
                    throw new ApplicationException("User didn't log in ");

                HttpWebResponse webResponse = builder.ExecuteRequest();

                responseBody = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
            catch (ApplicationException aex)
            {
                return null;
            }
            catch (WebException wex)
            {
                throw new ApplicationException(wex.Message, wex);
            }

            OAuthTokenResponse response = new OAuthTokenResponse();
            response.Token = Regex.Match(responseBody, @"oauth_token=([^&]+)").Groups[1].Value;
            response.TokenSecret = Regex.Match(responseBody, @"oauth_token_secret=([^&]+)").Groups[1].Value;
            response.UserId = long.Parse(Regex.Match(responseBody, @"user_id=([^&]+)").Groups[1].Value, CultureInfo.CurrentCulture);
            response.ScreenName = Regex.Match(responseBody, @"screen_name=([^&]+)").Groups[1].Value;
            return response;
        }

        /// <summary>
        /// Builds the authorization URI.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <returns>A new <see cref="Uri"/> instance.</returns>
        public static Uri BuildAuthorizationUri(string requestToken)
        {
            return BuildAuthorizationUri(requestToken, false);
        }

        /// <summary>
        /// Builds the authorization URI.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="authenticate">if set to <c>true</c>, the authenticate url will be used. (See: "Sign in with Twitter")</param>
        /// <returns>A new <see cref="Uri"/> instance.</returns>
        public static Uri BuildAuthorizationUri(string requestToken, bool authenticate)
        {
            StringBuilder parameters = new StringBuilder("https://twitter.com/oauth/");

            if (authenticate)
            {
                parameters.Append("authenticate");
            }
            else
            {
                parameters.Append("authorize");
            }

            parameters.AppendFormat("?oauth_token={0}", requestToken);

            return new Uri(parameters.ToString());
        }

        /// <summary>
        /// Tries to the parse querystring parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="text">The text.</param>
        /// <returns>The value of the parameter or an empty string.</returns>
        /// <remarks></remarks>
        private static string ParseQuerystringParameter(string parameterName, string text)
        {
            Match expressionMatch = Regex.Match(text, string.Format(@"{0}=(?<value>[^&]+)", parameterName));

            if (!expressionMatch.Success)
            {
                return string.Empty;
            }

            return expressionMatch.Groups["value"].Value;
        }


    }
}
