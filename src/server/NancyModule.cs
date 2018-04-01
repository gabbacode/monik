﻿using System;
using System.Collections.Generic;
using Monik.Common;
using Nancy;
using Nancy.ModelBinding;

namespace Monik.Service
{
    public class MainNancyModule : NancyModule
    {
        public MainNancyModule(IRepository repo, ICacheLog cacheLog, ICacheKeepAlive cacheKeepAlive,
            ISourceInstanceCache sourceInstanceCache, IMonik monik)
        {
            Get["/sources"] = args =>
            {
                try
                {
                    List<Source> result = sourceInstanceCache.GetAllSources();
                    return Response.AsJson<Source[]>(result.ToArray());
                }
                catch (Exception ex)
                {
                    monik.ApplicationError($"Method /sources : {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            };

            Get["/instances"] = args =>
            {
                try
                {
                    List<Instance> result = sourceInstanceCache.GetAllInstances();
                    return Response.AsJson<Instance[]>(result.ToArray());
                }
                catch (Exception ex)
                {
                    monik.ApplicationError($"Method /instances : {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            };

            Get["/groups"] = args =>
            {
                try
                {
                    List<Group> result = sourceInstanceCache.GetAllGroups();
                    return Response.AsJson<Group[]>(result.ToArray());
                }
                catch (Exception ex)
                {
                    monik.ApplicationError($"Method /instances : {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            };

            Post["/logs5"] = args =>
            {
                try
                {
                    var filter = this.Bind<LogRequest>();

                    List<Log_> result = cacheLog.GetLogs5(filter);
                    return Response.AsJson<Log_[]>(result.ToArray());
                }
                catch (Exception ex)
                {
                    monik.ApplicationError($"Method /logs5 : {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            };

            Post["/keepalive2"] = args =>
            {
                var filter = this.Bind<KeepAliveRequest>();

                try
                {
                    List<KeepAlive_> result = cacheKeepAlive.GetKeepAlive2(filter);
                    return Response.AsJson<KeepAlive_[]>(result.ToArray());
                }
                catch (Exception ex)
                {
                    monik.ApplicationError($"Method /keepalive : {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            };

            Get["/keepalive-status"] = args =>
            {
                try
                {
                    var filter = new KeepAliveRequest();
                    List<KeepAlive_> kaResult = cacheKeepAlive.GetKeepAlive2(filter);
                    var result = new List<KeepAliveStatus>();

                    foreach (var ka in kaResult)
                    {
                        var inst = sourceInstanceCache.GetInstanceById(ka.InstanceID);

                        KeepAliveStatus status = new KeepAliveStatus()
                        {
                            SourceID = inst.SourceID,
                            InstanceID = inst.ID,
                            SourceName = inst.SourceRef().Name,
                            InstanceName = inst.Name,
                            DisplayName = inst.SourceRef().Name + "." + inst.Name,
                            Created = ka.Created,
                            Received = ka.Received,
                            StatusOK = (DateTime.UtcNow - ka.Created).TotalSeconds < 120 // in seconds
                                                                                         // TODO: use param or default value for delta seconds
                        };

                        result.Add(status);
                    }

                    return Response.AsJson<KeepAliveStatus[]>(result.ToArray());
                }
                catch (Exception ex)
                {
                    monik.ApplicationError($"Method /status : {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            };
        }
    }//end of class
}