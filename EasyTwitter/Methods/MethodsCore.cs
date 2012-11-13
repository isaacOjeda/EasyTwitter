/*
 *  MethodsCore.cs
 *  
 *  Copyright 2012 Isaac Ojeda <isaacoq(at)gmail(dot)com>
 * 
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 * 
 *  This progam is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace EasyTwitter.Core
{
    public abstract class MethodsCore
    {
        /// <summary>
        /// 
        /// </summary>
        protected string BaseUri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected OAuthTokens Tokens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected HTTPVerb Verb { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, object> AdditionalParameters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUri"></param>
        public MethodsCore(string baseUri)
        {
            this.BaseUri = baseUri;
            this.AdditionalParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overrideVerb"></param>
        /// <returns></returns>
        protected abstract TwitterResponse<string> BeginRequest(HTTPVerb overrideVerb = HTTPVerb.GET);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="overrideVerb"></param>
        /// <returns></returns>
        protected virtual TwitterResponse<string> ExecuteRequest(HTTPVerb overrideVerb)
        {
            this.Verb = overrideVerb;
            if (String.IsNullOrEmpty(this.Method))
                throw new ArgumentNullException("Method");

            WebRequestBuilder builder = new WebRequestBuilder(
                new Uri(this.BaseUri + this.Method),
                this.Verb,
                this.Tokens);

            string responseBody = String.Empty;

            try
            {
                if (this.AdditionalParameters.Count > 0)
                {
                    foreach (var item in this.AdditionalParameters)
                        builder.Parameters.Add(item.Key, item.Value);
                    if(this.Verb==HTTPVerb.POST)
                        builder.Multipart = true;
                }

                HttpWebResponse webResponse = builder.ExecuteRequest();
                Stream responseStream = webResponse.GetResponseStream();
                if (responseStream != null) responseBody = new StreamReader(responseStream).ReadToEnd();
            }
            catch (WebException wex)
            {
                return new TwitterResponse<string>()
                {
                    Status = TwitterStatus.NotFound,
                    ObjectResponse = String.Empty
                };
            }

            return new TwitterResponse<string>()
            {
                Status = TwitterStatus.Success,
                ObjectResponse = responseBody
            };
        }
    }
}
