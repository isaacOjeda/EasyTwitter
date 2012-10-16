
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

        #region TwitterUser Helpers
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
                    ProfileImageUrl = (string)json["profile_image_url"]
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
                    Status = TwitterStatus.GeneralError,
                    ObjectResponse = null
                };
        }
        #endregion


    }
}
