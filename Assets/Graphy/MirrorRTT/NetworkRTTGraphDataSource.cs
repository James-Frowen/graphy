using JamesFrowen.Graphy;
#if MIRAGE
using Mirage;
#elif MIRROR
using Mirror;
#endif

public class NetworkRTTGraphDataSource : GraphDataSource
{
#if MIRAGE

    public NetworkClient Client;

    private void Awake()
    {
        // find client if field isn't set
        // better if user sets client, but this makes it easier to set up for new people
        if (Client == null)
            Client = FindObjectOfType<NetworkClient>();
    }

    public override float GetNewValue()
    {
        if (!Client.Active)
            return 0;

        return (float)(Client.World.Time.Rtt * 1000f);
    }

#elif MIRROR

    public override float GetNewValue()
    {
        if (!Mirror.NetworkClient.active)
            return 0;

        return (float)(Mirror.NetworkTime.rtt * 1000f);
    }

#else

    private void Awake()
    {
        UnityEngine.Debug.LogError("NetworkRTTGraphDataSource only works with Mirage or Mirror. If you are using different networking make a copy and modify it to work with that");
    }

    public override float GetNewValue()
    {
        return 0;
    }

#endif
}
