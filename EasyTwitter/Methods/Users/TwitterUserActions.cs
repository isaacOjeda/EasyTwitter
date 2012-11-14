
/*
 *  TwitterUserActions.cs
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
using EasyTwitter.Core;


namespace EasyTwitter
{
    public  class TwitterUserActions: TwitterUserActionsCore
    {
        public TwitterUserActions(OAuthTokens tokens)
            : base(tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }
        }

        /// <summary>
        /// Get the principal information of the user
        /// https://dev.twitter.com/docs/api/1.1/get/users/show
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public  TwitterResponse<TwitterUser> GetUserInfo(string userName)
        {
            this.Method = String.Format("show.json?screen_name={0}",userName);

            TwitterResponse<string> twitterResponse = this.BeginRequest();

            return ParseUserInfo(twitterResponse);

        }

        /// <summary>
        /// Search Twitter users
        /// Documentation: https://dev.twitter.com/docs/api/1.1/get/users/search
        /// </summary>
        /// <param name="hint">Text to search</param>
        /// <param name="page">Number of page to display</param>
        /// <param name="count">Number of element to display in page</param>
        /// <returns>TwitterResponse with a list of twitter users object</returns>
        public TwitterResponse<List<TwitterUser>> Search(string hint,int? page,int? count)
        {
            if (String.IsNullOrEmpty(hint))
                throw new ApplicationException("Hint cannot be null or empty");            

            //Method name 
            this.Method = "search.json";
            //Parameters
            this.AdditionalParameters.Add("q", hint);
            if(page.HasValue)
                this.AdditionalParameters.Add("page",page.Value.ToString());
            if (count.HasValue)
                this.AdditionalParameters.Add("count", count.Value.ToString());
            //begin request
            TwitterResponse<string> twitterResponse = this.BeginRequest();

            return ParseListUsers(twitterResponse);
        }

        /// <summary>
        /// Search twitter users
        /// </summary>
        /// <param name="hint">Text to search</param>
        /// <returns>TwitterResponse with a list of twitter users object</returns>
        public TwitterResponse<List<TwitterUser>> Search(string hint)
        {
            return Search(hint, null, null);
        }



        #region TwitterUser Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twitterResponse"></param>
        /// <returns></returns>
        private TwitterResponse<List<TwitterUser>> ParseListUsers(TwitterResponse<string> twitterResponse)
        {
            if (twitterResponse.Status == TwitterStatus.Success)
            {
                JArray json = JArray.Parse(twitterResponse.ObjectResponse);

                List<TwitterUser> usersSearched =
                    (
                        from  user in json.AsEnumerable()
                        select new TwitterUser
                        {
                            Name = (string)user["name"],
                            Id = (int)user["id"],
                            Location = (string)user["location"],
                            Description = (string)user["description"],
                            ScreenName = (string)user["screen_name"],
                            FavouritesCount = (int)user["favourites_count"],
                            FollowersCount = (int)user["followers_count"],
                            ProfileImageUrl = (string)user["profile_image_url"],
                            Verified = (bool)user["verified"]
                        }
                    ).ToList<TwitterUser>();

                return new TwitterResponse<List<TwitterUser>>
                {
                    Status=TwitterStatus.Success,
                    ObjectResponse=usersSearched
                };
            }
            else
                return new TwitterResponse<List<TwitterUser>>
                {
                    Status = twitterResponse.Status,
                    ObjectResponse = null
                };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twitterResponse"></param>
        /// <returns></returns>
        private TwitterResponse<TwitterUser> ParseUserInfo(TwitterResponse<string> twitterResponse)
        {
            if (twitterResponse.Status == TwitterStatus.Success)
            {
                JObject json = JObject.Parse(twitterResponse.ObjectResponse);

                TwitterUser user = new TwitterUser()
                {
                    Name = (string)json["name"],
                    Id = (int)json["id"],
                    Location = (string)json["location"],
                    Description = (string)json["description"],
                    ScreenName = (string)json["screen_name"],
                    FavouritesCount = (int)json["favourites_count"],
                    FollowersCount = (int)json["followers_count"],
                    ProfileImageUrl = (string)json["profile_image_url"],
                    Verified=(bool)json["verified"]
                };

                return new TwitterResponse<TwitterUser>()
                {
                    Status = TwitterStatus.Success,
                    ObjectResponse = user
                };
            }
            else
                return new TwitterResponse<TwitterUser>()
                {
                    Status = twitterResponse.Status,
                    ObjectResponse = null
                };
        }
        
        #endregion


    }
}
