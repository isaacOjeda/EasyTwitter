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
        /// GetTweets the 'count' favorited tweets of the logged user
        /// Documentation: https://dev.twitter.com/docs/api/1.1/get/favorites/list
        /// </summary>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets(int count)
        {
            this.Method = String.Format("list.json?count={0}",count);

            TwitterResponse<string> twitterResponse = this.BeginRequest();

            return ParseTweetsCollection(twitterResponse);
        }

        /// <summary>
        /// GetTweets the first 50 favorited tweets of the logged user
        /// </summary>
        /// <returns></returns>
        public TwitterResponse<List<Tweet>> GetTweets()
        {
            return this.GetTweets(50);
        }

        #region FavoriteActions Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="twitterResponse"></param>
        /// <returns></returns>
        private TwitterResponse<List<Tweet>> ParseTweetsCollection(TwitterResponse<string> twitterResponse)
        {
            if (twitterResponse.Status == TwitterStatus.Success)
            {
                JArray json = JArray.Parse(twitterResponse.ObjectResponse);

                List<Tweet> listFavourited =
                    (
                        from tweets in json.AsEnumerable()
                        select new Tweet
                        {
                            Id = (decimal)tweets["id"],
                            Text = (string)tweets["text"],
                            CreatedAt=(string)tweets["created_at"],
                            User = new TwitterUser
                            {
                                Id = (int)tweets["user"]["id"],
                                Description = (string)tweets["user"]["description"],
                                FavouritesCount = (int)tweets["user"]["favourites_count"],
                                FollowersCount = (int)tweets["user"]["followers_count"],
                                Location = (string)tweets["user"]["location"],
                                Name = (string)tweets["user"]["name"],
                                ProfileImageUrl = (string)tweets["user"]["profile_image_url"],
                                ScreenName = (string)tweets["user"]["screen_name"]
                            },
                            Entities = new TweetEntities
                            {
                                Urls =
                                (
                                        from url in tweets["entities"]["urls"].AsEnumerable()
                                        select new EntitieUrl
                                        {
                                            DisplayUrl = (string)url["url"],
                                            ExpandedUrl = (string)url["display_url"],
                                            Url = (string)url["expanded_url"]
                                        }
                                ).ToList<EntitieUrl>()
                            }
                        }
                    ).ToList<Tweet>();
                return new TwitterResponse<List<Tweet>>
                {
                    ObjectResponse = listFavourited,
                    Status = TwitterStatus.Success
                };
            }
            else
            {
                return new TwitterResponse<List<Tweet>>
                {
                    ObjectResponse = null,
                    Status = TwitterStatus.GeneralError
                };
            }
        }

        #endregion
    }
}
