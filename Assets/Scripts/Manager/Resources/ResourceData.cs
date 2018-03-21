using System;

public class ResourceData
{
    public ResourceData(string resourcePathIn,bool poolIn = false)
    {
        this.m_ResourceString = resourcePathIn;
        this.pool = poolIn;
    }

    public string ResourcePath
    {
        get
        {
            return this.m_ResourceString;
        }
    }

    public bool pool { get; private set; }

    private string m_ResourceString;
}
