using Neuro_SDK_Csharp.Messages.API;
using Neuro_SDK_Csharp.Messages.Outgoing;

namespace Neuro_SDK_Csharp.Websocket;

public class MessageQueue
{
    protected readonly List<OutgoingMessageHandler> Messages = new() { new Startup() };

    public virtual int Count
    {
        get
        {
            lock (Messages)
            {
                return Messages.Count;
            }
        }
    }

    public virtual void Enqueue(OutgoingMessageHandler message)
    {
        lock (Messages)
        {
            foreach (OutgoingMessageHandler existingMessage in Messages)
            {
                if (existingMessage.Merge(message)) return;
            }
            
            Messages.Add(message);
        }
    }
    
    /// <summary>
    /// Remove first element of queue 
    /// </summary>
    /// <returns>The first element of the queue that has been removed</returns>
    public virtual OutgoingMessageHandler? Dequeue()
    {
        lock (Messages)
        {
            if (Messages.Count == 0) return null;

            OutgoingMessageHandler message = Messages[0];
            Messages.RemoveAt(0);

            return message;
        }
    }
}