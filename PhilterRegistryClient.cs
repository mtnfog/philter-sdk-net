﻿/*******************************************************************************
 * Copyright 2020 Mountain Fog, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License.  You may obtain a copy
 * of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
 * License for the specific language governing permissions and limitations under
 * the License.
 ******************************************************************************/

using Newtonsoft.Json;
using System.Collections.Generic;
using RestSharp;
using Philter.Model;

namespace Philter
{
    public class PhilterRegistryClient
    {

        private readonly RestClient client;

        public PhilterRegistryClient(string endpoint, bool ignoreSelfSignedCertificates)
        {

            this.client = new RestClient(endpoint);

            if (ignoreSelfSignedCertificates)
            {
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }

        }

        public List<string> Get()
        {

            var request = new RestRequest("api/profiles", Method.GET);
            request.AddHeader("accept", "application/json");

            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<List<string>>(response.Content);
            }
            else
            {
                throw new PhilterException("Unable to get filter profiles.", response.ErrorException);
            }

        }

        public string Get(string filterProfileName)
        {

            var request = new RestRequest("api/profiles/{filterProfileName}", Method.GET);
            request.AddParameter("filterProfileName", filterProfileName, ParameterType.UrlSegment);
            request.AddHeader("accept", "application/json");

            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                throw new PhilterException("Unable to get filter profile.", response.ErrorException);
            }

        }

        public void Save(string filterProfile)
        {

            var request = new RestRequest("api/profiles", Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", filterProfile, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new PhilterException("Unable to save filter profile.", response.ErrorException);
            }

        }

        public void Delete(string filterProfileName)
        {

            var request = new RestRequest("api/profiles/{filterProfileName}", Method.DELETE);
            request.AddParameter("filterProfileName", filterProfileName, ParameterType.UrlSegment);

            IRestResponse response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new PhilterException("Unable to delete filter profile.", response.ErrorException);
            }

        }

    }
}