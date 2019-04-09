# InitialState<nolink/>.NET
InitialState<nolink/>.NET is a .NET class library that simplifies streaming event data to Initial State.  http://www.initialstate.com


## Example Usage

The following example shows you basic use of InitialState<nolink/>.NET.  This example shows you how to create an ISStream object, connect it to a data bucket, add some event data to the stream and then stream it to Initial State.

```csharp
ISStreamer stream = new ISStreamer(); // Create a new streamer
stream.ConnectBucket(api_key, bucket_key); // Connect the streamer to an event data bucket
stream.Eventsdata.Add(new ISEventData(itemKey, itemValue)) // Add event data to be streamed
stream.Stream(); // Stream the event data to Initial State
stream.Close(); // Close the streamer when finished
```

## Theory of Operation
InitialState<nolink/>.NET is a very simple to use library for connecting to an Initial State Data Bucket and streaming event data to.  Usage consists of the following steps.

* Create an ISStreamer object.
* Connect the ISStreamer to an existing data bucket or create a new data bucket.
* Add event data to the event data collection.
* Push the data to Initial State by calling one of the Stream methods.  The event data is cleared from the event data collection upon successful streaming.
* Continue adding event data and streaming or close the stream.


## InitialState<nolink/>.NET Objects

### ISStream
The ISStreamer object encapsulates an Initial State event data stream and is used to make a connection to an event data bucket and stream event data to it.

### ISEventData
The ISEventData object respresents a single event and its corresponding data.  When you create an ISEventData object you must specify a key and value pair, where the key is the name of the event data and the value is the value of the event data.  Optionally you can also specify a timestamp for the event data and the event data can be configured as to whether or not that timestamp should be used when it is streamed to Initial State.  If the event data is configured to not use the timestamp, then when the event data is streamed to Initial State, the timestamp will not be included and Initial State will automatically timestamp the event data with the time that it is received.


## Maintainer

* [David Wyban](https://github.com/dwyban)


## Disclaimer

This is not an officially supported Tektronix product. It is maintained by a small group of employees in their spare time. We lack the resources typical of most Tektronix products, so please bear with us! We will do our best to address your issues and answer any questions directly related to this extension in a timely manner.


## License

[Put license check badge here]


## Contributor License Agreement

Contributions to this project must be accompanied by a Contributor License Agreement. You (or your employer) retain the copyright to your contribution; this simply gives us permission to use and redistribute your contributions as part of the project.

You generally only need to submit a CLA once, so if you've already submitted one (even if it was for a different project), you probably don't need to do it again.
