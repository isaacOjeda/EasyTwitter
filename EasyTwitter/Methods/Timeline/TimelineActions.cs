/*
 *  TimelineActions.cs
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

namespace EasyTwitter
{
    /// <summary>
    /// 
    /// </summary>
    public class TimelineActions:TimelineActionsCore
    {
        public TimelineActions(OAuthTokens tokens)
            : base(tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
        }
        /// <summary>
        /// Main GetHomeTimeline function, see more at
        /// https://dev.twitter.com/docs/api/1.1/get/statuses/home_timeline
        /// </summary>
        /// <param name="count"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <returns></returns>
        private TwitterResponse<List<Tweet>> GetHomeTimeline(int? count, decimal? since_id, decimal? max_id)
        {
            this.Method = "home_timeline.json?";

            if (count.HasValue)
                this.Method = this.Method + String.Format("count={0}&", count.Value);
            if (since_id.HasValue)
                this.Method = this.Method + String.Format("since_id={0}&", since_id.Value);
            if (max_id.HasValue)
                this.Method = this.Method + String.Format("max_id={0}&", max_id.Value);

            TwitterResponse<string> twitterResponse= this.BeginRequest();

            return Helpers.TweetParseHelper.ParseTweetsCollection(twitterResponse);
        }
        /// <summary>
        /// Get the first 25  home timeline tweets of the authenticated user
        /// </summary>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetHomeTimeline()
        {            
            return GetHomeTimeline(25,null,null);
        }
        /// <summary>
        /// Get 'count' number of home timeline tweets of the authenticated user
        /// </summary>
        /// <param name="count">Number of Tweets to display</param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetHomeTimeline(int count)
        {
            return GetHomeTimeline(count, null, null);
        }
        /// <summary>
        /// Get the first 25 home timeline tweets of the authenticated user
        /// since 'since_id' entered
        /// </summary>
        /// <param name="since_id">Tweet ID</param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetHomeTimeline(decimal since_id)
        {
            return GetHomeTimeline(25, since_id, null);
        }
        /// <summary>
        /// Get the 'count' number of home timeline tweets of the authenticated user    
        /// less than 'max_id' entered
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max_id"></param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetHomeTimeline(int count, decimal max_id)
        {
            return GetHomeTimeline(count, null, max_id);
        }
        /// <summary>
        /// Get tweets between since_id and max_id
        /// </summary>
        /// <param name="since_id">Since Tweet ID</param>
        /// <param name="max_id">Max Tweet ID</param>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetHomeTimeline(decimal since_id, decimal max_id)
        {
            return GetHomeTimeline(null, since_id, max_id);

        }



    }
}
