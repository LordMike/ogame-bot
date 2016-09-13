﻿using System;
using System.Net;
using System.Net.Http;
using ScraperClientLib.Engine;

namespace OgameBot.Engine
{
    public class OGameAutoLoginner : IInterventionHandler
    {
        private readonly OGameClient _client;

        public OGameAutoLoginner(OGameClient client)
        {
            _client = client;
        }

        public bool DoIntervention(ResponseDocument offendingTask)
        {
            if (offendingTask.StatusCode == HttpStatusCode.Redirect && offendingTask.ResponseMessage.Headers.Location != null)
            {
                Uri requestUri = offendingTask.RequestMessage.RequestUri;
                Uri responseRedirectLocation = offendingTask.ResponseMessage.Headers.Location;

                if (responseRedirectLocation.DnsSafeHost != requestUri.DnsSafeHost &&
                    requestUri.DnsSafeHost.EndsWith(responseRedirectLocation.DnsSafeHost))
                {
                    // All but the first part of the DNS name matches, we've been sent to the frontpage
                    return true;
                }
            }

            return false;
        }

        public InterventionResult Handle(ResponseDocument offendingTask)
        {
            // Build login request
            HttpRequestMessage loginReq = _client.PrepareLogin();

            return new InterventionResult(InterventionResultState.RetryCurrentTask, loginReq);
        }
    }
}
