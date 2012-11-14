/*
 *  TwitterResponse.cs
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

namespace EasyTwitter
{

    public enum TwitterStatus
    {
       /// <summary>
        /// Success!
       /// </summary>
       Success=0,
       /// <summary>
       /// The request was invalid. 
       /// In API v1.1, a request without authentication is considered invalid and you will get this response.
       /// </summary>
       BadRequest = 2,
       /// <summary>
       /// The URI requested is invalid or the resource requested, such as a user, does not exists. 
       /// Also returned when the requested format is not supported by the requested method.
       /// </summary>
       FileNotFound = 1,
       /// <summary>
       /// Authentication credentials were missing or incorrect.
       /// </summary>
       Unauthorized=3,
       /// <summary>
       /// Returned by the Search API when an invalid format is specified in the request.
       /// </summary>
       NotAcceptable=4,
       /// <summary>
       /// 
       /// </summary>
       RateLimited=5,
       /// <summary>
       /// 
       /// </summary>
       TwitterIsDown=6,
       /// <summary>
       /// 
       /// </summary>
       TwitterIsOverloaded=7,
       /// <summary>
       /// 
       /// </summary>
       ConnectionFailure=8,                
       /// <summary>
       /// 
       /// </summary>
       GeneralError=9              
    }

    public class TwitterResponse<T>
    {
        public TwitterStatus Status { get; set; }
        public T ObjectResponse { get; set; }
    }
}
