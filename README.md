HTTP GRIP library for .NET
==========================

This library implements the Generic Realtime Intermediary Protocol (GRIP), used by Pushpin and Fanout.io to deliver realtime pushes to HTTP clients.

Copyright (c) RealCrowd, Inc. All rights reserved. See LICENSE in the project root for license information.

Background
----------

* Concept: http://blog.fanout.io/2013/02/10/http-grip-proxy-hold-technique/
* Implementation: http://blog.fanout.io/2013/04/09/an-http-reverse-proxy-for-realtime/
* Spec: https://fanout.io/docs/protocols.html#generic-realtime-intermediary-protocol-grip

Usage
-----

To subscribe an incoming HTTP request to a channel:

```c#
using System.Text;
using System.Net.Http;

public HttpResponseMessage Get()
{
  var instruct = new RealCrowd.Grip.Instruct();
  instruct.CreateResponseHold(
    "test-channel", // channel to subscribe to
    null,           // id of previous publish (optional)
    "text/plain",   // timeout content type
    "(no data)\n"); // timeout body
  var response = new HttpResponseMessage(HttpStatusCode.OK);
  response.Content = new StringContent(instruct.ToString(), Encoding.UTF8,
    "application/grip-instruct");
  return response;
}
```

To publish an HTTP response on a channel:

```c#
using System.Net.Http;

var publishControl = new RealCrowd.Grip.GripPublishControl("http://localhost:5561");
var responseFormat = new HttpResponseFormat(
  "text/plain",     // publish content type
  "hello world\n"); // publish body
await publishControl.PublishAsync(
  "test-channel",   // channel to publish on
  null,             // id of publish (optional)
  null,             // id of previous publish (optional)
  responseFormat);
```
