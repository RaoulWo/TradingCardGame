namespace BusinessObjects;

public class RequestMap : IEquatable<RequestMap>
{
    public string Method { get; set; }
    public string Target { get; set; }

    public RequestMap(string method, string target)
    {
        Method = method;
        Target = target;
    }

    public override int GetHashCode()
    {
        int result = 17;
        result = -13 * result + (Method == null ? 0 : Method.GetHashCode());
        result = -13 * result + (Target == null ? 0 : Target.GetHashCode());
        return result;
    }

    public override bool Equals(object other)
    {
        return Equals(other as RequestMap);
    }

    public bool Equals(RequestMap other)
    {
        if (other == null) return false;
        if (other == this) return true;
        return Method == other.Method && Target == other.Target;
    }
}