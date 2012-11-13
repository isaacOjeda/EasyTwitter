/*
 *  FavoriteAction.cs
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

using EasyTwitter.Entities;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;

namespace EasyTwitter
{
    public class FavoriteActions:FavoritesActionsCore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        public FavoriteActions(OAuthTokens tokens)
            : base(tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
        }

        /// <summary>
        /// Main GetTweets function see more at
        /// Documentation: https://dev.twitter.com/docs/api/1.1/get/favorites/list
        /// </summary>
        /// <param name="count"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <returns></returns>
        private TwitterResponse<List<Tweet>> GetTweets(int? count,decimal? since_id,decimal? max_id)
        {
            this.Method = "list.json";

            if (count.HasValue)
                this.AdditionalParameters.Add("count", count.Value.ToString());
            if (since_id.HasValue)
                this.AdditionalParameters.Add("since_id", since_id.Value.ToString());
            if (max_id.HasValue)
                this.AdditionalParameters.Add("max_id", max_id.Value.ToString());

            TwitterResponse<string> twitterResponse = this.BeginRequest();

            return Helpers.TweetParseHelper.ParseTweetsCollection(twitterResponse);
        }

        /// <summary>
        /// Get the first 50 favorited tweets of the logged user
        /// </summary>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets()
        {
            return this.GetTweets(50,null,null);
        }

        /// <summary>
        /// Get 'count' number of  favorited tweets of the logged user
        /// </summary>
        /// <param name="count">Number of favorited tweets to display</param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets(int count)
        {
            return this.GetTweets(count, null, null);
        }
        /// <summary>
        /// Get the first 50 favorited tweets of the logged user since
        /// the since_id entered
        /// </summary>
        /// <param name="since_id"></param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets(decimal since_id)
        {
            return this.GetTweets(50, since_id,null);
        }

        /// <summary>
        /// Get 'count'number of favorited tweets less than max_id tweet
        /// </summary>
        /// <param name="count">Number of favorited tweets to display</param>
        /// <param name="max_id"></param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets(int count,decimal max_id)
        {
            return this.GetTweets(count, null, max_id);
        }

        /// <summary>
        /// Get tweets between since_id and max_id
        /// </summary>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets(decimal since_id,decimal max_id)
        {
            return this.GetTweets(null,since_id,max_id);
        }

        /// <summary>
        /// Un-favorite a favorited tweet
        /// Documentation: https://dev.twitter.com/docs/api/1.1/post/favorites/destroy
        /// </summary>
        /// <param name="tweetId">Tweet id to un-favorite</param>
        /// <returns></returns>
        public TwitterStatus DestroyFavorite(string tweetId)
        {
            this.Method = String.Format("destroy.json");
            this.AdditionalParameters.Add("id", tweetId);
            TwitterResponse<string> twitterResponse = this.BeginRequest(HTTPVerb.POST);
            return twitterResponse.Status;
        }
    }
}
