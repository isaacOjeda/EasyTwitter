/*
 *  TweetActions.cs
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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using EasyTwitter.Entities;

namespace EasyTwitter
{
    public class TweetActions : TweetActionsCore
    {
        public TweetActions(OAuthTokens tokens)
            : base(tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
        }

        /// <summary>
        /// Documentation : https://dev.twitter.com/docs/api/1.1/post/statuses/update
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public TwitterStatus UpdateStatus(string status)
        {
            this.Method="update.json";

            this.AdditionalParameters.Add("status", status);
            TwitterResponse<string> twitterResponse = this.BeginRequest(HTTPVerb.POST);

            if (twitterResponse.Status == TwitterStatus.Success)
            {
                return twitterResponse.Status;
            }
            else
            {
                return TwitterStatus.GeneralError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfTweets"></param>
        /// <returns></returns>
        public TwitterResponse<Tweet> GetRetweets(int numberOfTweets)
        {
            return null;
        }
    }
}
