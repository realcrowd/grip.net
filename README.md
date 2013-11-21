HTTP GRIP library for .NET
==========================

This library implements the Generic Realtime Intermediary Protocol (GRIP),
used by Pushpin and Fanout.io to deliver realtime pushes to HTTP clients.

To subscribe a client:

  async Task<HttpResponseMessage> GetAsync(...)
  {
    var instruct = new Grip.Instruct();
    instruct.CreateResponseHold(
      "test-channel", // channel to subscribe to
      null,           // id of previous publish (optional)
      "text/plain",   // timeout content type
      "(no data)\n"); // timeout body
    var response = new HttpResponseMessage(HttpStatusCode.OK);
    response.Content = new StringContent(instruct.ToString(), Encoding.UTF8,
      "grip-instruct");
    return response;
  }

To publish:

  var publishControl = new Grip.GripPublishControl("http://localhost:5561");
  var responseFormat = new HttpResponseFormat();
  responseFormat.Create(
    "text/plain",     // publish content type
    "hello world\n"); // publish body
  await publishControl.PublishAsync(
    "test-channel",   // channel to publish to
    null,             // id of publish (optional)
    null,             // id of previous publish (optional)
    responseFormat);
